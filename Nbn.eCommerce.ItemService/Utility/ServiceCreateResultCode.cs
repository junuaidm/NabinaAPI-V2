using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Service Create Result Codes
    /// </summary>
    public enum ServiceCreateResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Resource already exists
        /// </summary>
        ResourceAlreadyExists,

        /// <summary>
        /// Validation errors
        /// </summary>
        ValidationErrors,

        /// <summary>
        /// Internal error
        /// </summary>
        InternalError
    }
}
