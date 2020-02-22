

namespace Nbn.eCommerce.ItemService.Utility
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// This holds the common functionality for the controllers. This
    /// could also be moved into a base class.
    /// </summary>
    public static class ControllerCommonFunctionality
    {
        /// <summary>
        /// This is used to return the ServiceGetResultCode for the
        /// associated ServiceGetResult.ResultCode.
        /// </summary>
        /// <typeparam name="T">This is the ServiceGetResult type</typeparam>
        /// <typeparam name="U">This is the response object that is held by this</typeparam>
        /// <param name="result">This is ServiceResultCode corresponding to the
        /// result code</param>
        /// <param name="controllerBase">The Web API controller</param>
        /// <returns>This is the IActionResult that corresponds to the result code</returns>
        public static IActionResult ToApiGetResponse<T, U>(this T result, ControllerBase controllerBase)
                where T : ServiceGetResult<U>
                where U : class
        {
            IActionResult response = null;

            switch (result.ResultCode)
            {
                case ServiceGetResultCode.Success:
                    response = controllerBase.Ok(result.Resource);
                    break;

                case ServiceGetResultCode.NotFound:
                    response = controllerBase.NotFound();
                    break;

                case ServiceGetResultCode.ValidationErrors:
                    response = controllerBase.BadRequest(result.ValidationErrors);
                    break;

                case ServiceGetResultCode.NotAuthorized:
                    response = controllerBase.Unauthorized();
                    break;

                default:
                    response = controllerBase.BadRequest("Unknown error occurred");
                    break;
            }

            return response;
        }

        /// <summary>
        /// This is used to retrieve the response for a service change result.
        /// </summary>
        /// <typeparam name="T">This is the service result type</typeparam>
        /// <param name="result">This is the result</param>
        /// <param name="controllerBase">Controller base</param>
        /// <returns>The corresponding action</returns>
        public static IActionResult ToApiChangeResponse<T>(this T result, ControllerBase controllerBase)
            where T : ServiceChangeResult
        {
            IActionResult response;

            switch (result.ResultCode)
            {
                case ServiceChangeResultCode.Success:
                    response = controllerBase.NoContent();
                    break;

                case ServiceChangeResultCode.NotFound:
                    response = controllerBase.NotFound();
                    break;

                case ServiceChangeResultCode.ValidationErrors:
                    response = controllerBase.BadRequest(result.ValidationErrors);
                    break;

                case ServiceChangeResultCode.VersionConflict:
                    response = controllerBase.Conflict();
                    break;

                default:
                    throw new InvalidCastException($"Unexpected {typeof(T).Name} value of {result.ResultCode} received.");
            }

            return response;
        }

        /// <summary>
        /// This is used to return the ServiceCreateResultCode for the
        /// associated ServiceCreateResult.ResultCode.
        /// </summary>
        /// <typeparam name="T">This is the ServiceCreateResult type</typeparam>
        /// <typeparam name="U">This is the response object that is held by this</typeparam>
        /// <param name="result">This is ServiceResultCode corresponding to the
        /// result code</param>
        /// <param name="controllerBase">The Web API controller</param>
        /// <returns>This is the IActionResult that corresponds to the result code</returns>
        public static IActionResult ToApiCreateResponse<T, U>(this T result, ControllerBase controllerBase)
                where T : ServiceCreateResult<U>
                where U : class
        {
            IActionResult response;

            switch (result.ResultCode)
            {
                case ServiceCreateResultCode.Success:
                    response = controllerBase.Created(string.Empty, result.Identifier);
                    break;

                case ServiceCreateResultCode.ResourceAlreadyExists:
                    response = controllerBase.StatusCode((int)HttpStatusCode.PreconditionFailed, result.Identifier);
                    break;

                case ServiceCreateResultCode.ValidationErrors:
                    response = controllerBase.BadRequest(result.ValidationErrors);
                    break;

                default:
                    throw new InvalidCastException($"Unexpected {typeof(T).Name} value of {result.ResultCode} received.");
            }

            return response;
        }
    }
}
