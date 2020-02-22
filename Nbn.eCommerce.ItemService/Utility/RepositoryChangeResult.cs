
namespace Nbn.eCommerce.ItemService.Utility
{
    using System;

    /// <summary>
    /// Resource describing the result of a repository request that changes resource state.
    /// </summary>
    public class RepositoryChangeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryChangeResult"/> class.
        /// </summary>
        /// <param name="resultCode">Enum describing the possible expected results of the operation</param>
        public RepositoryChangeResult(RepositoryChangeResultCode resultCode)
        {
            this.ResultCode = resultCode;
        }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public RepositoryChangeResultCode ResultCode { get; protected set; }
    }
}
