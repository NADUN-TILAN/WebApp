using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable CORS (Allow requests from specific origins)
            //var cors = new EnableCorsAttribute("*", "*", "*"); // Adjust as needed
            //config.EnableCors(cors);

            // Enable attribute-based routing
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
