using Microsoft.Extensions.Logging;
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
    public class DivisionService : IDivisionService
    {
        /// <summary>
        /// Logger handler
        /// </summary>
        private readonly ILogger<DivisionService> logger;

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
        public DivisionService(IBuildingMaterialRepository buildingMaterial, IKitchenRepository kitchen, ILogger<DivisionService> logger)
        {
            this.logger = logger;
            this.buildingMaterial = buildingMaterial;
            this.kitchen = kitchen;
        }

        public async Task<ServiceGetResult<List<GetDivisionResponse>>> GetAllDivision(CancellationToken ctok)
        {
            try
            {
                var serviceCode = ServiceGetResultCode.NotFound;
                List<GetDivisionResponse> response = null;

                var buildingDivisions = await this.buildingMaterial.GetAllDivision(ctok);
                if (buildingDivisions != null && buildingDivisions.Any())
                {
                    response = buildingDivisions.Select(item => new GetDivisionResponse
                    {
                        DivisionId = item.CostCentreId,
                        DivisionName = item.CostCentreName,
                    }).ToList();
                }

                var kitchenDivision = await this.kitchen.GetAllDivision(ctok);

                if (kitchenDivision != null && kitchenDivision.Any())
                {
                    if (response == null)
                    {
                        response = kitchenDivision.Select(item => new GetDivisionResponse
                        {
                            DivisionId = item.CostCentreId,
                            DivisionName = item.CostCentreName,
                        }).ToList();
                    }
                    else
                    {
                        response.AddRange(kitchenDivision.Select(item => new GetDivisionResponse
                        {
                            DivisionId = item.CostCentreId,
                            DivisionName = item.CostCentreName,
                        }).ToList());
                    }

                    serviceCode = ServiceGetResultCode.Success;

                }

                return new ServiceGetResult<List<GetDivisionResponse>>(serviceCode, response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceGetResult<GetDivisionResponse>> GetDivisionById(int divisionId, CancellationToken ctok)
        {
            try
            {
                var serviceCode = ServiceGetResultCode.NotFound;
                GetDivisionResponse response = null;

                var buildingDivisions = await this.buildingMaterial.GetDivisionById(divisionId, ctok);
                if (buildingDivisions != null)
                {
                    response = new GetDivisionResponse
                    {
                        DivisionId = buildingDivisions.CostCentreId,
                        DivisionName = buildingDivisions.CostCentreName,
                    };
                    serviceCode = ServiceGetResultCode.Success;

                }

                if (response == null)
                {
                    var kitchenDivision = await this.kitchen.GetDivisionById(divisionId,ctok);
                    if (kitchenDivision != null )
                    {

                        response = new GetDivisionResponse
                        {
                            DivisionId = kitchenDivision.CostCentreId,
                            DivisionName = kitchenDivision.CostCentreName,
                        };
                        serviceCode = ServiceGetResultCode.Success;
                    }
                }

                return new ServiceGetResult<GetDivisionResponse>(serviceCode, response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
