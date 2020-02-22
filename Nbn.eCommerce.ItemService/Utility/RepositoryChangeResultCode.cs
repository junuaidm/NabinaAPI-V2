using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Result code describing possible results from a repository request that changes resource state.
    /// </summary>
    public enum RepositoryChangeResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Version Conflict
        /// </summary>
        VersionConflict
    }
}
