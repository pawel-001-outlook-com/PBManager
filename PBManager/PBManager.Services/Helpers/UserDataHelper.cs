using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace PBManager.Services.Helpers
{
    public static class UserDataHelper
    {
        public static string GetUserName(HttpContextBase httpContext)
        {
            var userData = GetUserData(httpContext);

            return userData["CurrentUserName"];
        }


        public static int GetUserId(HttpContextBase httpContext)
        {
            var userData = GetUserData(httpContext);

            return Convert.ToInt32(userData["CurrentUserId"]);
        }


        private static Dictionary<string, string> GetUserData(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) throw new ArgumentNullException();
            var cookieValue = authCookie.Value;

            if (string.IsNullOrWhiteSpace(cookieValue)) throw new ArgumentNullException();
            var ticket = FormsAuthentication.Decrypt(cookieValue);

            var userDataString = ticket.UserData;

            var userData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            return userData;
        }
    }
}