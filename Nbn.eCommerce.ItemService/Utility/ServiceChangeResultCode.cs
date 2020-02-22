using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Service Change Result Codes
    /// </summary>
    public enum ServiceChangeResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Validation errors
        /// </summary>
        ValidationErrors,

        /// <summary>
        /// Not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Version conflict
        /// </summary>
        VersionConflict,

        /// <summary>
        /// Internal error
        /// </summary>
        InternalError
    }
}
