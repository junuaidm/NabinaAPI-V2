using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Result code describing possible results from a repository request that creates a new resource.
    /// </summary>
    public enum RepositoryCreateResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Resource already exists
        /// </summary>
        ResourceAlreadyExists
    }
}
