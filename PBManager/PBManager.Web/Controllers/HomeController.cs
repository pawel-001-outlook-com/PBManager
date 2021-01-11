using Newtonsoft.Json;
using PBManager.Dto.ViewModels;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace PBManager.Web.Controllers
{

    public class HomeController : BaseController
    {
        private AccountService _as = new AccountService();

        public ActionResult Index()
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return RedirectToAction("Login", "Users");
            var cookieValue = authCookie.Value;

            if (String.IsNullOrWhiteSpace(cookieValue)) return RedirectToAction("Login", "Users");
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieValue);

            string userDataString = ticket.UserData;

            Dictionary<string, string> userData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            TempData["UserName"] = UserDataHelper.GetUserName(HttpContext);
            var dvm = new DashboardViewModel();
            return View(dvm);
        }


    }
}