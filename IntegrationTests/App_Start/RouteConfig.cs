using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IntegrationTests
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "API_Save",
                url: "API/{ModelName}",
                defaults: new { controller = "APITest", action = "Save" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
            );

            routes.MapRoute(
                name: "API_Save2",
                url: "API/{ModelName}/{ID}",
                defaults: new { controller = "APITest", action = "Save" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
            );

            routes.MapRoute(
                name: "API_Delete",
                url: "API/{ModelName}/{ID}",
                defaults: new { controller = "APITest", action = "Delete" },
                constraints: new { httpMethod = new HttpMethodConstraint("DELETE") }
            );

            routes.MapRoute(
                name: "API_Any",
                url: "API/{ModelName}/{ID}",
                defaults: new { controller = "APITest", action = "Any" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                name: "API_All",
                url: "API/{ModelName}/",
                defaults: new { controller = "APITest", action = "All" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}