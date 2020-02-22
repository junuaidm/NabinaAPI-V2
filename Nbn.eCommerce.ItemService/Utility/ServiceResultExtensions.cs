
namespace Nbn.eCommerce.ItemService.Utility
{
    using System;

    /// <summary>
    /// ServiceResultExtensions class
    /// </summary>
    public static class ServiceResultExtensions
    {
        /// <summary>
        /// Converts to ServiceChangeResult
        /// </summary>
        /// <param name="rcr">RepositoryChangeResult handle</param>
        /// <returns>ServiceChangeResult handle</returns>
        public static ServiceChangeResult ToServiceChangeResult(this RepositoryChangeResult rcr)
        {

            switch (rcr.ResultCode)
            {
                case RepositoryChangeResultCode.NotFound:
                    return new ServiceChangeResult(ServiceChangeResultCode.NotFound);
                case RepositoryChangeResultCode.Success:
                    return new ServiceChangeResult(ServiceChangeResultCode.Success);
                case RepositoryChangeResultCode.VersionConflict:
                    return new ServiceChangeResult(ServiceChangeResultCode.VersionConflict);
                default:
                    throw new Exception(message: $"Unexpected result code of {rcr.ResultCode} was returned on enumeration {typeof(RepositoryChangeResultCode).Name}");
            }
        }

        /// <summary>
        /// Converts to ServiceGetResult
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="rgr">RepositoryGetResult handle</param>
        /// <returns>ServiceGetResult handle</returns>
        public static ServiceGetResult<T> ToServiceGetResult<T>(this RepositoryGetResult<T> rgr)
            where T : class
        {

            switch (rgr.ResultCode)
            {
                case RepositoryGetResultCode.NotFound:
                    return new ServiceGetResult<T>(ServiceGetResultCode.NotFound);
                case RepositoryGetResultCode.Success:
                    return new ServiceGetResult<T>(ServiceGetResultCode.Success, resource: rgr.Resource);
                default:
                    throw new Exception(message: $"Unexpected result code of {rgr.ResultCode} was returned on enumeration {typeof(RepositoryGetResultCode).Name}");
            }
        }

        /// <summary>
        /// Converts to Service Create Result
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="rcr">repository create result</param>
        /// <returns>ServiceCreateResult handle</returns>
        public static ServiceCreateResult<T> ToServiceCreateResult<T>(this RepositoryCreateResult<T> rcr)
            where T : class
        {

            switch (rcr.ResultCode)
            {
                case RepositoryCreateResultCode.ResourceAlreadyExists:
                    return new ServiceCreateResult<T>(ServiceCreateResultCode.ResourceAlreadyExists);
                case RepositoryCreateResultCode.Success:
                    return new ServiceCreateResult<T>(resultCode: ServiceCreateResultCode.Success, identifier: rcr.Identifier);
                default:
                    throw new Exception(message: $"Unexpected result code of {rcr.ResultCode} was returned on enumeration {typeof(RepositoryCreateResultCode).Name}");
            }
        }
    }
}
