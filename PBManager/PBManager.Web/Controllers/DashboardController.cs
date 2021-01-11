using PBManager.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace PBManager.Web.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetAnnualData([FromBody]string year)
        {
            DataContext dc = new DataContext();
            var total = dc.Cashflows
                .Where(c => c.AccountingDate.Year.ToString() == year)
                .Select(c => c.Value)
                .Sum();

            return Json(total);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetAnnualDataList([FromBody]string year)
        {
            DataContext dc = new DataContext();
            var total = dc.Cashflows
                .Where(c => c.AccountingDate.Year.ToString() == year)
                .Select(c => c.Value)
                .Sum();

            var newItem = new { listOfValues = total };
            List<object> listObject = new List<object>();
            listObject.Add(newItem);

            return Json(listObject);
        }
    }

}