using System.Web.Mvc;

namespace PBManager.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult FileNotFound()
        {
            return View("Error404");
        }

        public ActionResult AccessForbidden()
        {
            return View("Error403");
        }

        public ActionResult NotValidAuthentication()
        {
            return View("Error401");
        }

        public ActionResult InternalEror()
        {
            return View("Error500");
        }
    }
}