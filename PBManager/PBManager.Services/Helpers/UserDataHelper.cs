using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

namespace PBManager.Services.Helpers
{

    public static class UserDataHelper
    {
        public static string GetUserName(HttpContextBase httpContext)
        {
            Dictionary<string, string> userData = GetUserData(httpContext);

            return userData["CurrentUserName"];
        }


        public static int GetUserId(HttpContextBase httpContext)
        {
            Dictionary<string, string> userData = GetUserData(httpContext);

            return Convert.ToInt32(userData["CurrentUserId"]);
        }

        public static int GetUserId(HttpContext httpContext)
        {
            Dictionary<string, string> userData = GetUserData(httpContext);

            return Convert.ToInt32(userData["CurrentUserId"]);
        }



        private static Dictionary<string, string> GetUserData(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) throw new ArgumentNullException();
            var cookieValue = authCookie.Value;

            if (String.IsNullOrWhiteSpace(cookieValue)) throw new ArgumentNullException();
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieValue);

            string userDataString = ticket.UserData;

            Dictionary<string, string> userData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            return userData;
        }


        private static Dictionary<string, string> GetUserData(HttpContext httpContext)
        {
            var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) throw new ArgumentNullException();
            var cookieValue = authCookie.Value;

            if (String.IsNullOrWhiteSpace(cookieValue)) throw new ArgumentNullException();
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieValue);

            string userDataString = ticket.UserData;

            Dictionary<string, string> userData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            return userData;
        }
    }
}

