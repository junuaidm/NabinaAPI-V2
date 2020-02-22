using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;
using Nbn.eCommerce.ItemService.Common;
using Nbn.eCommerce.ItemService.Repository.Entities;
using Nbn.eCommerce.ItemService.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Repository
{
    public class KitchenRepository : IKitchenRepository
    {

        /// <summary>
        /// A database connection
        /// </summary>
        private readonly IDbConnection dbConnection;

        /// <summary>
        /// Log handler
        /// </summary>
        private readonly ILogger<KitchenRepository> logger;

        private readonly IConfiguration config;



        public KitchenRepository(IConfiguration configuration, ILogger<KitchenRepository> logger)
        {
            this.logger = logger;
            this.config = configuration;
            this.dbConnection = new SqlConnection(this.config.GetConnectionString(DatabaseNameConstant.NbmKm));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disisionId"></param>
        /// <param name="ctok"></param>
        /// <returns></returns>
        public async Task<IList<ItemType>> GetItemTypes(int disisionId, CancellationToken ctok)
        {

            var parameter = new { CostCentreID = disisionId };

            var sql = @"SELECT 
	                        ItemTypeID,
	                        ItemTypeName
                        FROM ItemType
                        WHERE CostCentreID = @CostCentreID AND ItemParentType =0
                        ORDER BY ItemTypeName";

            // Sql query to read given Item type from database
            var result = await this.dbConnection.QueryAsyncWithRetry<ItemType>(new CommandDefinition(commandText: sql, parameters: parameter, cancellationToken: ctok));
            return result.ToList();

        }

        /// <inheritdoc />
        public async Task<IList<CostCentre>> GetAllDivision(CancellationToken ctok)
        {
            var sql = @"SELECT 
	                    CostCentreID,
	                    CostCentreName
                    FROM CostCentre
                    WHERE CostCentreActive =1";

            // Sql query to read given Item type from database
            var result = await this.dbConnection.QueryAsyncWithRetry<CostCentre>(new CommandDefinition(commandText: sql, cancellationToken: ctok));
            return result.ToList();
        }

        /// <inheritdoc />
        public async Task<CostCentre> GetDivisionById(int divisionId, CancellationToken ctok)
        {
            var parameter = new { CostCentreID = divisionId };
            var sql = @"SELECT 
	                    CostCentreID,
	                    CostCentreName
                    FROM CostCentre
                    WHERE CostCentreActive =1
                    AND CostCentreID = @CostCentreID";

            // Sql query to read given Item type from database
            var result = await this.dbConnection.QuerySingleAsync<CostCentre>(new CommandDefinition(commandText: sql, parameters: parameter, cancellationToken: ctok));
            return result;
        }
    }
}
