using System.Web.Mvc;
using PBManager.Web.Filters;

namespace PBManager.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    // [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            return View();
        }
    }
}