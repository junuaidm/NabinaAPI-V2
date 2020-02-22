using Microsoft.Extensions.Logging;
using Nbn.eCommerce.ItemService.Common;
using Nbn.eCommerce.ItemService.Repository;
using Nbn.eCommerce.ItemService.ResponseObjects;
using Nbn.eCommerce.ItemService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Service
{
    public class ItemsService : IItemsService
    {

        /// <summary>
        /// Logger handler
        /// </summary>
        private readonly ILogger<ItemsService> logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly IBuildingMaterialRepository buildingMaterial;

        /// <summary>
        /// 
        /// </summary>
        private readonly IKitchenRepository kitchen;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingMaterial"></param>
        /// <param name="kitchen"></param>
        /// <param name="logger"></param>
        public ItemsService(IBuildingMaterialRepository buildingMaterial, IKitchenRepository kitchen, ILogger<ItemsService> logger)
        {
            this.logger = logger;
            this.buildingMaterial = buildingMaterial;
            this.kitchen = kitchen;
        }

        /// <summary>
        /// This Get List of Categories
        /// </summary>
        /// <param name="disisionId">disision Id</param>
        /// <param name="dbName">Datebase Name</param>
        /// <param name="ctok">Cancellation token</param>
        /// <returns>An array of courier information objects</returns>
        public async Task<ServiceGetResult<List<GetItemCategoryResponse>>> GetCategoriesByDivision(int disisionId, string dbName, CancellationToken ctok)
        {
            try
            {
                var serviceCode = ServiceGetResultCode.NotFound;
                List<GetItemCategoryResponse> response = null;
                if (dbName.Equals(DatabaseNameConstant.Nbm))
                {

                    var result = await this.buildingMaterial.GetItemTypes(disisionId, ctok);
                    if (result != null && result.Any())
                    {
                        response = result.Select(item => new GetItemCategoryResponse
                        {
                             CatagoryId = item.ItemTypeId,
                             CatagoryName = item.ItemTypeName,
                        }).ToList();
                    }

                    serviceCode = ServiceGetResultCode.Success;
                }
                else
                {
                    var result = await this.kitchen.GetItemTypes(disisionId, ctok);
                    if (result != null && result.Any())
                    {
                        response = result.Select(item => new GetItemCategoryResponse
                        {
                            CatagoryId = item.ItemTypeId,
                            CatagoryName = item.ItemTypeName,
                        }).ToList();
                    }
                    serviceCode = ServiceGetResultCode.Success;

                }

                return new ServiceGetResult<List<GetItemCategoryResponse>>(serviceCode, response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
