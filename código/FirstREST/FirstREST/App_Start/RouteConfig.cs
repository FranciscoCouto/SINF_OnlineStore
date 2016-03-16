using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FirstREST
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "HomePost",
                "Home/POST/{action}",
                new
                {
                    controller = "Home",
                    action = "Index"
                }
                );

            routes.MapRoute(
                "Home",
                "Home/{op}/{op_dois}",
                new
                {
                    controller = "Home",
                    action = "Index",
                    op = UrlParameter.Optional,
                    op_dois = UrlParameter.Optional
                }
                );
        }

     
    }
}