﻿using System.Web.Mvc;
using System.Web.Routing;

namespace PBManager.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Accounts",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "PBManager.Web.Controllers" }
            );


        }
    }
}
