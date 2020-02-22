using Nbn.eCommerce.ItemService.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Repository
{
    public interface IKitchenRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disisionId"></param>
        /// <param name="ctok"></param>
        /// <returns></returns>
        Task<IList<ItemType>> GetItemTypes(int disisionId, CancellationToken ctok);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctok"></param>
        /// <returns></returns>
        Task<IList<CostCentre>> GetAllDivision(CancellationToken ctok);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ctok"></param>
        /// <returns></returns>
        Task<CostCentre> GetDivisionById(int divisionId, CancellationToken ctok);
    }
}
