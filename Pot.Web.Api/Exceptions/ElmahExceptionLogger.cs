// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElmahExceptionLogger.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the ElmahExceptionLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api
{
    using System;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.ExceptionHandling;

    using Elmah;

    /// <summary>
    /// The Elmah exception logger.
    /// </summary>
    public class ElmahExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public override void Log(ExceptionLoggerContext context)
        {
            if (context == null)
            {
                return;
            }

            // Retrieve the current HttpContext instance for this request.
            HttpContext httpContext = GetHttpContext(context.Request);

            if (httpContext == null)
            {
                return;
            }

            // Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
            Exception exceptionToRaise = new HttpUnhandledException(context.Exception.Message, context.Exception);

            // Send the exception to ELMAH (for logging, mailing, filtering, etc.).
            ErrorSignal signal = ErrorSignal.FromContext(httpContext);
            signal.Raise(exceptionToRaise, httpContext);
        }

        /// <summary>
        /// The get http context.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="HttpContext"/>.
        /// </returns>
        private static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;
            if (!request.Properties.TryGetValue("MS_HttpContext", out value))
            {
                return null;
            }

            var context = value as HttpContextBase;
            if (context == null)
            {
                return null;
            }

            return context.ApplicationInstance.Context;
        }
    }
}