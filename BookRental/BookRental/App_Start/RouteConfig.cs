using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookRental
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "B1",
                url: "{Book}/{RM}/{year}/{month}",
                defaults: new { controller = "Book", action = "ReleaseM" }
                );

            routes.MapRoute(
                name: "B2",
                url: "{Book}/{RYA}/{year}/{author}",
                defaults: new { controller = "Book", action = "ReleaseYA" },
                constraints: new { year = @"\d{4}" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
