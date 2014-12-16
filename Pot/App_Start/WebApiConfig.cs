namespace Pot.Web.Api
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using NWL.Web.Api;

    public static class WebApiConfig
    {
        public const string DefaultApi = "DefaultApi";

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
//            AutomapperConfig.Initialize();
            UnityConfig.RegisterComponents(config);

            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: DefaultApi,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
