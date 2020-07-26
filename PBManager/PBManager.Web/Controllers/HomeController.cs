using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Helpers;

namespace PBManager.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private AccountService _as = new AccountService();

        public ActionResult Index()
        {
            var dvm = new DashboardViewModel();
            return View(dvm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}