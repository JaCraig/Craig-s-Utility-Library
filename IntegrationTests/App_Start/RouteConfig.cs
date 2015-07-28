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
"Default",
"{controller}/{action}/{id}",
new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}