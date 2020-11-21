﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using PBManager.DAL;

namespace PBManager.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
                ReferenceLoopHandling.Ignore;

            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            if (Request.IsAuthenticated)
            {
                string[] roles = null;
                var rolesList = new List<string>();
                var userName = Context.User.Identity.Name;

                using (var dataContext = new DataContext())
                {
                    var user = dataContext.Users.FirstOrDefault(u => u.UserName.Equals(userName));
                    var userRoles = dataContext.Users
                        .Include(e => e.Roles)
                        .Where(e => e.Id.Equals(user.Id))
                        .Select(u => u.Roles.Select(r => r.RoleName))
                        .ToList();


                    foreach (var roleName in userRoles)
                    foreach (var rn in roleName)
                        rolesList.Add(rn);

                    roles = rolesList.ToArray();
                }

                IIdentity userIdentity = new GenericIdentity(userName);
                IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);
                Context.User = newUserObj;
            }
        }
    }
}