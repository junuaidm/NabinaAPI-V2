using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Resource describing the result of a service request that reads resource state.
    /// </summary>
    /// <typeparam name="TResponse">Type of service result being returned.</typeparam>
    public class ServiceGetResult<TResponse>
        where TResponse : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceGetResult{TResponse}"/> class.
        /// </summary>
        /// <param name="resultCode">Enumeration describing the result of the request</param>
        /// <param name="resource">The TResponse resource returned if successful</param>
        /// <param name="validationErrors">A collection of validation errors that occurred during the request execution.</param>
        public ServiceGetResult(ServiceGetResultCode resultCode, TResponse resource = null, IEnumerable<ValidationResult> validationErrors = null)
        {
            // We should have some object reference if this is a get request and
            // we intend to indicate a successful read.
            if (resultCode == ServiceGetResultCode.Success && resource == null)
            {
                throw new ArgumentException(paramName: nameof(resource), message: $"Argument should not be 'null' when intended result code is '{resultCode.ToString()}'");
            }

            // We should not have a resource reference if the intended response is 'Not Found'
            if (resultCode == ServiceGetResultCode.NotFound && resource != null)
            {
                throw new ArgumentException(paramName: nameof(resource), message: $"Argument should be 'null' when intended result code is '{resultCode.ToString()}'");
            }

            // We should not have a resource reference if the intended response is 'Validation Errors'
            if (resultCode == ServiceGetResultCode.ValidationErrors && resource != null)
            {
                throw new ArgumentException(paramName: nameof(resource), message: $"Argument should be 'null' when intended result code is '{resultCode.ToString()}'");
            }

            if (resultCode == ServiceGetResultCode.ValidationErrors && validationErrors == null)
            {
                throw new ArgumentException(paramName: nameof(validationErrors), message: $"Argument should not be 'null' when intended result code is '{resultCode.ToString()}'");
            }

            this.ResultCode = resultCode;
            this.Resource = resource;
            this.ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public ServiceGetResultCode ResultCode { get; protected set; }

        /// <summary>
        /// Gets or sets the returned resource
        /// </summary>
        public TResponse Resource { get; protected set; }

        /// <summary>
        /// Gets or sets the list of errors that resulted from the operation.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationErrors { get; protected set; }
    }
}
