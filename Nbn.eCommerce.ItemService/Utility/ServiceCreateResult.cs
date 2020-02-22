using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Describes the result of a service operation that changes the state of resources
    /// </summary>
    /// <typeparam name="TKey">The object type of the identifier for the resource that was created</typeparam>
    public class ServiceCreateResult<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCreateResult{TKey}"/> class.
        /// </summary>
        /// <param name="resultCode">Indicates success or failure of operation</param>
        /// <param name="identifier">Identifier of newly created resource</param>
        /// <param name="validationErrors">Validation errors that occurred during the execution of the request.</param>
        public ServiceCreateResult(ServiceCreateResultCode resultCode, TKey identifier = default(TKey), IEnumerable<ValidationResult> validationErrors = null)
        {
            if (validationErrors == null)
            {
                validationErrors = new List<ValidationResult>();
            }

            this.ValidationErrors = validationErrors;
            this.ResultCode = resultCode;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets or sets the operation result code
        /// </summary>
        public ServiceCreateResultCode ResultCode { get; protected set; }

        /// <summary>
        /// Gets or sets the list of errors that resulted from the operation.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationErrors { get; protected set; }

        /// <summary>
        /// Gets or sets Identifier
        /// </summary>
        public TKey Identifier { get; protected set; }
    }
}
