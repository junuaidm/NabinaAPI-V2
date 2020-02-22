using Nbn.eCommerce.ItemService.ResponseObjects;
using Nbn.eCommerce.ItemService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Service
{
    public interface IDivisionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctok"></param>
        /// <returns></returns>
        Task<ServiceGetResult<List<GetDivisionResponse>>> GetAllDivision(CancellationToken ctok);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ctok"></param>
        /// <returns></returns>
        Task<ServiceGetResult<GetDivisionResponse>> GetDivisionById(int divisionId, CancellationToken ctok);

    }
}
