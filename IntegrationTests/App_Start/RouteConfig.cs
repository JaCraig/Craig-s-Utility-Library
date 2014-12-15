using System.Web.Mvc;
using System.Web.Routing;

namespace IntegrationTests
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Utilities.IoC.Manager.Bootstrapper.Resolve<Ironman.Core.API.Manager.Manager>().RegisterRoutes(routes, "APITest");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}