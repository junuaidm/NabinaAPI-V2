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
    public class ServiceChangeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceChangeResult"/> class.
        /// </summary>
        /// <param name="resultCode">Inidcates success or failure of operation</param>
        /// <param name="validationErrors">Validation errors that occurred during the execution of the request.</param>
        public ServiceChangeResult(ServiceChangeResultCode resultCode, IEnumerable<ValidationResult> validationErrors = null)
        {
            // If the intended result code is 'ValidationErrors' then
            // a list of validation errors should be provided.
            if (resultCode == ServiceChangeResultCode.ValidationErrors)
            {
                Guard.AgainstNullOrEmptyEnumerable(value: validationErrors, name: nameof(validationErrors));
            }

            // Conversely If the intended result code is Succes then
            // a list of validation errors should NOT be provided.
            if (resultCode == ServiceChangeResultCode.Success)
            {
                validationErrors = new List<ValidationResult>();
            }

            if (validationErrors == null)
            {
                validationErrors = new List<ValidationResult>();
            }

            this.ValidationErrors = validationErrors;
            this.ResultCode = resultCode;
        }

        /// <summary>
        /// Gets or sets the operation result code
        /// </summary>
        public ServiceChangeResultCode ResultCode { get; protected set; }

        /// <summary>
        /// Gets or sets the list of errors that resulted from the operation.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationErrors { get; protected set; }
    }
}
