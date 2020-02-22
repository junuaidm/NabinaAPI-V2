namespace Nbn.eCommerce.ItemService.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Polly;

    public static class DapperExtension
    {
        /// <summary>
        /// Policy handler.
        /// </summary>
        private static IAsyncPolicy asyncPolicy = null;

        /// <summary>
        /// Function that caller can provide to hook the OnRetry function
        /// for logging purposes, etc.
        /// </summary>
        private static Action<Exception, TimeSpan, int> retryHook;

        /// <summary>
        /// Initializes database retry policy.
        /// </summary>
        /// <param name="retryCount">How many times Polly needs to retry incase of recoverable exception.</param>
        /// <param name="maxRetryDelayInSeconds">What is the delay in between each retry incase of recoverable exception.</param>
        /// <param name="onRetry">Optional function that caller can provide to hook the OnRetry function for logging purposes, etc.</param>
        public static void Init(int retryCount, int maxRetryDelayInSeconds, Action<Exception, TimeSpan, int> onRetry = null)
        {
            if (asyncPolicy == null)
            {
                if (retryCount < 1)
                {
                    throw new ArgumentException(message: "Value must be greater than 0", paramName: nameof(retryCount));
                }

                if (maxRetryDelayInSeconds < 1)
                {
                    throw new ArgumentException(message: "Value must be greater than 0", paramName: nameof(maxRetryDelayInSeconds));
                }

                retryHook = onRetry;

                asyncPolicy = Policy
                                .Handle<SqlException>(exception => TransientFaultHandling.IsTransient(exception))
                                .WaitAndRetryAsync(
                                    retryCount: retryCount,
                                    sleepDurationProvider: attempt =>
                                    {
                                        // This gives each thread its own version on the random number generator.
                                        // Random is not threadsafe, so this is required.
                                        var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

                                        // Convert everything to milliseconds to provide more variability in calculated delays
                                        int maxDelay = maxRetryDelayInSeconds * 1000;
                                        int delay = Convert.ToInt32(1000 * Math.Pow(2, attempt));

                                        // This implements exponential backoff with a maximum retry delay
                                        // The probability of waiting longer increases up to some maximum value

                                        // Minimum retry delay will be 100 ms.
                                        // First retry will delay randomly between 100 and 2000 ms
                                        // Second retry will delay randomly between 100 and 4000 ms
                                        // Third retry will delay randomly between 100 and 8000 ms.

                                        // Max retry delay limits the maximum retry delay that could possibly occur.
                                        return TimeSpan.FromMilliseconds(random.Value.Next(100, (delay < maxDelay) ? delay : maxDelay));
                                    },
                                    (exception, timeSpan, retries, context) =>
                                    {
                                        // Will call if available, otherwise quietly pass over.
                                        retryHook?.Invoke(exception, timeSpan, retries);
                                        if (retryCount != retries)
                                        {
                                            return;
                                        }
                                    });
            }
        }

        /// <summary>
        /// Re-Initializes database retry policy.
        /// </summary>
        /// <param name="retryCount">How many times Polly needs to retry incase of recoverable exception.</param>
        /// <param name="maxRetryDelayInSeconds">What is the delay in between each retry incase of recoverable exception.</param>
        /// <param name="onRetry">Optional function that caller can provide to hook the OnRetry function for logging purposes, etc.</param>
        public static void ReInit(int retryCount, int maxRetryDelayInSeconds, Action<Exception, TimeSpan, int> onRetry = null)
        {
            asyncPolicy = null;
            Init(retryCount, maxRetryDelayInSeconds, onRetry);
        }

        /// <summary>
        /// Executes given command asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the return object.</typeparam>
        /// <param name="dbConnection">The database connection to execute on.</param>
        /// <param name="commandDefinition">The command to execute.</param>
        /// <returns>Returns List Of Objects.</returns>
        public static async Task<T> QueryFirstOrDefaultAsyncWithRetry<T>(this IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            CheckInitialization();

            return await asyncPolicy.ExecuteAsync(async () =>
            {
                return await dbConnection.QueryFirstOrDefaultAsync<T>(commandDefinition);
            });
        }

        /// <summary>
        /// Executes given command with retires and returns result set.
        /// </summary>
        /// <typeparam name="T">type of the return object.</typeparam>
        /// <param name="dbConnection">database connection.</param>
        /// <param name="commandDefinition">command definition.</param>
        /// <returns>result set.</returns>
        public static async Task<IEnumerable<T>> QueryAsyncWithRetry<T>(this IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            CheckInitialization();

            return await asyncPolicy.ExecuteAsync(async () =>
            {
                return await dbConnection.QueryAsync<T>(commandDefinition);
            });
        }

        /// <summary>
        /// Executes given single row command asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the return object.</typeparam>
        /// <param name="dbConnection">The database connection to execute on.</param>
        /// <param name="commandDefinition">The command to execute.</param>
        /// <returns>Returns Single Object or Null.</returns>
        public static async Task<T> QuerySingleOrDefaultAsyncWithRetry<T>(this IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            CheckInitialization();

            return await asyncPolicy.ExecuteAsync(async () =>
            {
                return await dbConnection.QuerySingleOrDefaultAsync<T>(commandDefinition);
            });
        }

        /// <summary>
        /// Executes given command asynchronously.
        /// </summary>
        /// <param name="dbConnection">The connection to execute on.</param>
        /// <param name="commandDefinition">The command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public static async Task<int> ExecuteAsyncWithRetry(this IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            CheckInitialization();

            return await asyncPolicy.ExecuteAsync(async () =>
            {
                return await dbConnection.ExecuteAsync(commandDefinition);
            });
        }

        /// <summary>
        /// Executes given commands asynchronously.
        /// </summary>
        /// <param name="dbConnection">The connection to execute on.</param>
        /// <param name="commandDefinition">The command to execute.</param>
        /// <param name="ignoreAffectedRowsCount">Ignore sql command affected rows count.</param>
        /// <returns>True/False.</returns>
        public static async Task<bool> ExecuteAsyncWithTransactionAndRetry(this IDbConnection dbConnection, CommandDefinition[] commandDefinition, bool ignoreAffectedRowsCount = false)
        {
            bool rtnValue = true;
            CheckInitialization();

            return await asyncPolicy.ExecuteAsync(async () =>
            {
                // Open database connection if it is not already opened
                bool connectionOpen = false;
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                    connectionOpen = true;
                }

                try
                {
                    // Begin transaction
                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            // Loop through each command and execute
                            foreach (CommandDefinition cmddefinition in commandDefinition)
                            {
                                int affectedRowsCnt = await dbConnection.ExecuteAsync(new CommandDefinition(cmddefinition.CommandText, cmddefinition.Parameters, transaction, cmddefinition.CommandTimeout, cancellationToken: cmddefinition.CancellationToken));
                                if (affectedRowsCnt <= 0 && !ignoreAffectedRowsCount)
                                {
                                    // If any command failed then return false
                                    rtnValue = false;
                                    break;
                                }
                            }

                            // Commit transaction if all commands execution is successful
                            if (rtnValue)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                            }

                            // Close database connection if it is Opened in this function
                            if (connectionOpen)
                            {
                                dbConnection.Close();
                            }
                        }
                        catch
                        {
                            // Rollback transaction
                            transaction.Rollback();

                            // Close database connection if it is Opened in this function
                            if (connectionOpen)
                            {
                                dbConnection.Close();
                            }

                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }

                return rtnValue;
            });
        }

        /// <summary>
        /// Common function for validating that retry policy has been initialized.
        /// </summary>
        private static void CheckInitialization()
        {
            if (asyncPolicy == null)
            {
                throw new InvalidOperationException("The retry policy has not been initialized.  Did you forget to call DapperExtensionPolicy.Init?");
            }
        }
    }
}
