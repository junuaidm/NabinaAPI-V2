using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nbn.eCommerce.ItemService.ResponseObjects;
using Nbn.eCommerce.ItemService.Service;
using Nbn.eCommerce.ItemService.Utility;

namespace Nbn.eCommerce.ItemService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<DivisionsController> logger;

        /// <summary>
        /// division service
        /// </summary>
        private readonly IDivisionService divisionService;

        public DivisionsController(ILogger<DivisionsController> logger, IDivisionService divisionService)
        {
            this.logger = logger;
            this.divisionService = divisionService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ctok = default(CancellationToken))
        {
            IActionResult response;

            try
            {
                ////this.logger.LogInformation($"Request received to Get categories by division :{disisionId} in DB :{dbName}");
                var result = await this.divisionService.GetAllDivision(ctok);
                response = result.ToApiGetResponse<ServiceGetResult<List<GetDivisionResponse>>, List<GetDivisionResponse>>(this);

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CATCH {MethodFileLine}", MethodInfo.GetInfo());
                response = this.StatusCode((int)HttpStatusCode.InternalServerError, ex.ToValidationResults());
            }

            return response;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ctok = default(CancellationToken))
        {
            IActionResult response;

            try
            {
                ////this.logger.LogInformation($"Request received to Get categories by division :{disisionId} in DB :{dbName}");
                var result = await this.divisionService.GetDivisionById(id, ctok);
                response = result.ToApiGetResponse<ServiceGetResult<GetDivisionResponse>, GetDivisionResponse>(this);

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