using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Enumeration describing the expected results from a service get request.
    /// </summary>
    public enum ServiceGetResultCode
    {
        /// <summary>
        /// Request was successful
        /// </summary>
        Success = 1,

        /// <summary>
        /// The request failed due to validation errors
        /// </summary>
        ValidationErrors,

        /// <summary>
        /// The requested resource was not found
        /// </summary>
        NotFound,

        /// <summary>
        /// The requested resource was not found
        /// </summary>
        NotAuthorized
    }
}
