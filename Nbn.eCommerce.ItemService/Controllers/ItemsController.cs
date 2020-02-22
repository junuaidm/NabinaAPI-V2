using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nbn.eCommerce.ItemService.ResponseObjects;
using Nbn.eCommerce.ItemService.Security;
using Nbn.eCommerce.ItemService.Service;
using Nbn.eCommerce.ItemService.Utility;

namespace Nbn.eCommerce.ItemService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors("unrestricted-cors-policy")]
    public class ItemsController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ItemsController> logger;

        /// <summary>
        /// item service
        /// </summary>
        private readonly IItemsService itemsService;

        public ItemsController(ILogger<ItemsController> logger, IItemsService itemsService)
        {
            this.logger = logger;
            this.itemsService = itemsService;
        }

        /// <summary>
        /// Get List of categories by division.
        /// </summary>
        /// <param name="disisionId">disision Id</param>
        /// <param name="dbName">Datebase Name</param>
        /// <param name="ctok">Cancellation token.</param>
        /// <returns>resturns list of categories.</returns>
        [Route("division/categories/{disisionId}/{dbName}")]
        [HttpGet]
        public async Task<IActionResult> GetCategoriesByDivision([FromRoute] int disisionId, [FromRoute] string dbName,  CancellationToken ctok = default(CancellationToken))
        {
            IActionResult response;

            // Check for null and maximum length for courier code
            List<ValidationResult> errors = new List<ValidationResult>();
            errors = Validate.ThatRequiredValueIsPresent(dbName, nameof(dbName), errors);
            ////errors = Validate.ThatMaxLengthRequirementIsMet(value: courierCode, length: 1, name: nameof(courierCode), validationResults: errors);

            if (errors.Count > 0)
            {
                return this.BadRequest(errors);
            }

            try
            {
                this.logger.LogInformation($"Request received to Get categories by division :{disisionId} in DB :{dbName}");
                var result = await this.itemsService.GetCategoriesByDivision(disisionId, dbName, ctok);
                response = result.ToApiGetResponse<ServiceGetResult<List<GetItemCategoryResponse>>, List<GetItemCategoryResponse>>(this);

                this.logger.LogInformation($"Successfully processed request to get categories by division :{disisionId} in DB :{dbName}.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CATCH {MethodFileLine}", MethodInfo.GetInfo());
                response = this.StatusCode((int)HttpStatusCode.InternalServerError, ex.ToValidationResults());
            }

            return response;
        }

    }
}