using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using PBManager.DAL;

namespace PBManager.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetAnnualData([FromBody] string year)
        {
            var dc = new DataContext();
            var total = dc.Cashflows
                .Where(c => c.AccountingDate.Year.ToString() == year)
                .Select(c => c.Value)
                .Sum();

            return Json(total);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetAnnualDataList([FromBody] string year)
        {
            var dc = new DataContext();
            var total = dc.Cashflows
                .Where(c => c.AccountingDate.Year.ToString() == year)
                .Select(c => c.Value)
                .Sum();

            var newItem = new {listOfValues = total};
            var listObject = new List<object>();
            listObject.Add(newItem);

            return Json(listObject);
        }
    }
}