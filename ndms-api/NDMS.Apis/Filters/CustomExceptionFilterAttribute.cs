using System.Net;
using System.Web.Http.Filters;
using System.Net.Http;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using NDMS.Helpers;
using NDMS.Logger;
using NDMS.Apis.Models;

namespace NDMS.Apis.Filters
{
    /// <summary>
    /// Exception filter which takes care about the exception handling for whole API's. We don't want to
    /// add try catch in all Web Api controllers
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        #region Field(s)
        private const string UnauthorizedErrorMessage = "You don’t have sufficient permission to perform this operation. Kindly contact your system administrator for more details.";
        private const string UnhandledErrorMessage = "Something went wrong!!! Please contact your system administrator with below identifier - {0}";
        private const string ExceptionSeperator = "*************************************************************************************************";
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Handles all unhandled exceptions
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        private ApiResponse<bool> HandleSystemException(HttpActionExecutedContext actionExecutedContext)
        {
            ApiResponse<bool> response;
            Guid exceptionIdentifier = Guid.NewGuid();

            // Log the exception
            LogManager.Instance.Log(ComposeExceptionLog(actionExecutedContext, exceptionIdentifier),
                actionExecutedContext.Exception, LogLevelEnum.Error);

            response = new ApiResponse<bool> { HasError = true };
            response.Errors.Add(string.Format(UnhandledErrorMessage, exceptionIdentifier));
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
            return response;
        }

        /// <summary>
        /// Handles the particular business exception thrown business layer
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        private static ApiResponse<bool> HandleBusinessException(HttpActionExecutedContext actionExecutedContext)
        {
            var businessException = actionExecutedContext.Exception as NDMSBusinessException;

            ApiResponse<bool> response = new ApiResponse<bool> { HasError = true };
            response.Errors.AddRange(businessException.Errors);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
            return response;
        }

        /// <summary>
        /// Compose the log message for the exception
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="exceptionIdentifier"></param>
        /// <returns></returns>
        private static string ComposeExceptionLog(HttpActionExecutedContext actionExecutedContext, 
            Guid exceptionIdentifier)
        {
            var arguments = actionExecutedContext.ActionContext.ActionArguments;
            var sbErrorLog = new StringBuilder();
            sbErrorLog.AppendLine(ExceptionSeperator);
            sbErrorLog.AppendLine(exceptionIdentifier.ToString());
            sbErrorLog.AppendLine("Error occurred for request- " + 
                actionExecutedContext.Request.RequestUri.ToString());
            sbErrorLog.AppendLine("Arguments - " + JsonConvert.SerializeObject(arguments));
            sbErrorLog.AppendLine("Exception- " + actionExecutedContext.Exception.Message);
            sbErrorLog.AppendLine("StackTrace- " + actionExecutedContext.Exception.ToString());
            sbErrorLog.AppendLine(ExceptionSeperator);
            return sbErrorLog.ToString();
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Will be invoked when an exception is thrown
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //string exceptionMessage = actionExecutedContext.Exception.Message;
            ApiResponse<bool> response = null;

            if (actionExecutedContext.Exception is NDMSBusinessException)
            {
                response = HandleBusinessException(actionExecutedContext);
            }
            else
            {
                response = HandleSystemException(actionExecutedContext);
            }
        }
        #endregion
    }
}
