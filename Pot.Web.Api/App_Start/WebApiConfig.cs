namespace Pot.Web.Api
{
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public const string DefaultApi = "DefaultApi";

        public static void RegisterWebApi(this HttpConfiguration config)
        {
            // Web API configuration and services
//            AutomapperConfig.Initialize();
            UnityConfig.RegisterUnityComponents(config);

            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: DefaultApi,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
