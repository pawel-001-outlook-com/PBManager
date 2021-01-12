using PBManager.Web.Filters;
using System.Web.Mvc;

namespace PBManager.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class AdminHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}