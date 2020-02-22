

namespace Nbn.eCommerce.ItemService.Utility
{
    using System;

    /// <summary>
    /// Resource describing the result of a repository request that reads resource state.
    /// </summary>
    /// <typeparam name="TResource">Type of repository result being returned.</typeparam>
    public class RepositoryGetResult<TResource>
        where TResource : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryGetResult{TResource}"/> class.
        /// </summary>
        /// <param name="resultCode">A result code enumeration describing the possible results of the operation</param>
        /// <param name="resource">The resource being returned if the query was successsful.</param>
        public RepositoryGetResult(RepositoryGetResultCode resultCode, TResource resource = null)
        {
            // We should have some object reference if this is a get request and
            // we intend to indicate a successful read.
            if (resultCode == RepositoryGetResultCode.Success && resource == null)
            {
                throw new ArgumentException(paramName: nameof(resource), message: $"Argument should not be 'null' when intended result code is '{resultCode.ToString()}'");
            }

            // We should not have a resource reference if the intended response is 'Not Found'
            if (resultCode == RepositoryGetResultCode.NotFound && resource != null)
            {
                throw new ArgumentException(paramName: nameof(resource), message: $"Argument should be 'null' when intended result code is '{resultCode.ToString()}'");
            }

            this.ResultCode = resultCode;
            this.Resource = resource;
        }

        /// <summary>
        /// Gets or sets the ResultCode
        /// </summary>
        public RepositoryGetResultCode ResultCode { get; protected set; }

        /// <summary>
        /// Gets or sets the Resource
        /// </summary>
        public TResource Resource { get; protected set; }
    }
}
