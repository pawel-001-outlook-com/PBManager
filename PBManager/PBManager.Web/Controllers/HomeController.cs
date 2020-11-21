using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using PBManager.Dto.ViewModels;
using PBManager.Services.Helpers;

namespace PBManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private AccountService _as = new AccountService();

        public ActionResult Index()
        {
            // int o = 0;
            // var i = 1 / o;

            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return RedirectToAction("Login", "Users");
            var cookieValue = authCookie.Value;

            if (string.IsNullOrWhiteSpace(cookieValue)) return RedirectToAction("Login", "Users");
            var ticket = FormsAuthentication.Decrypt(cookieValue);

            var userDataString = ticket.UserData;

            var userData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            ViewBag.UserName = UserDataHelper.GetUserName(HttpContext);
            var dvm = new DashboardViewModel();
            return View(dvm);
        }
    }
}