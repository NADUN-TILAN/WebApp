using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Correct usage of EnableCorsAttribute
            var cors = new EnableCorsAttribute("*", "*", "*"); // Allow all origins, headers, and methods
            config.EnableCors(cors); // No need to cast it

            // Web API attribute routing
            config.MapHttpAttributeRoutes();

            // Define a default Web API route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
