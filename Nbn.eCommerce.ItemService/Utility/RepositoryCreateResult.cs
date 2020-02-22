using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Standard response object from create requests against a repository
    /// </summary>
    /// <typeparam name="TKey">The object type of the unique identifier for the resource managed by the repository</typeparam>
    public class RepositoryCreateResult<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCreateResult{TKey}"/> class.
        /// </summary>
        /// <param name="resultCode">Enum describing possible expected results of the operation</param>
        /// <param name="identifier">The identifier assigned to the new resource that was created</param>
        public RepositoryCreateResult(RepositoryCreateResultCode resultCode, TKey identifier)
        {
            this.ResultCode = resultCode;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets or sets the ResultCode.
        /// </summary>
        public RepositoryCreateResultCode ResultCode { get; protected set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public TKey Identifier { get; protected set; }
    }
}
