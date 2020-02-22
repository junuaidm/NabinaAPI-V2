using Nbn.eCommerce.ItemService.ResponseObjects;
using Nbn.eCommerce.ItemService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Service
{
    public interface IItemsService
    {
        /// <summary>
        /// This returns List of Categories
        /// </summary>
        /// <param name="disisionId">disision Id</param>
        /// <param name="dbName">Datebase Name</param>
        /// <param name="ctok">Cancellation token</param>
        /// <returns>An array of courier information objects</returns>
        Task<ServiceGetResult<List<GetItemCategoryResponse>>> GetCategoriesByDivision(int disisionId, string dbName, CancellationToken ctok);
    }
}
