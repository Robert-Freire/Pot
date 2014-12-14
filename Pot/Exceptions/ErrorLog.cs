
namespace Pot.Web.Api
{
    using System;
    using System.Web;

    using Elmah;

    public static class ErrorLog
    {
      public static void LogError(Exception ex, string contextualMessage)
        {
            try
            {
                // log error to Elmah
                if (contextualMessage != null)
                {
                    // log exception with contextual information that's visible when 
                    // clicking on the error in the Elmah log
                    var annotatedException = new Exception(contextualMessage, ex);
                    ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
                }
                else
                {
                    ErrorSignal.FromCurrentContext().Raise(ex, HttpContext.Current);
                }
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                // uh oh! just keep going
            }
        }

        public static void LogError(Exception ex)
        {
            LogError(ex, null);
        }
    }
}