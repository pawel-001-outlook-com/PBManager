using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using PBManager.Core.Consts;
using PBManager.DAL;
using PBManager.Dto.Dtos;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;

namespace PBManager.Web.Controllers
{
    public class DashboardMainController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICashflowService _cashflowService;
        private readonly ICategoryService _categoryService;
        private readonly IProjectService _projectService;
        private ISubcategoryService _subcategoryService;

        public DashboardMainController(IAccountService accountService,
            ICategoryService categoryService,
            ISubcategoryService subcategoryService,
            IProjectService projectService,
            ICashflowService cashflowService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _projectService = projectService;
            _cashflowService = cashflowService;
        }


        // GET: Dashboard
        public ActionResult Index()
        {
            var userId = UserDataHelper.GetUserId(HttpContext);
            var userName = UserDataHelper.GetUserName(HttpContext);


            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            var accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            var projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });

            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;

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

        [System.Web.Mvc.HttpPost]
        public JsonResult GetReportData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var userId = UserDataHelper.GetUserId(HttpContext);

            var cashflows = _cashflowService.GetAll(userId, "");

            var res = cashflows
                .Where(a => a.Category != null)
                .GroupBy(a => new {a.Category.Id, a.Category.Name})
                .Select(a => new
                {
                    categoryId = a.Key.Id,
                    categoryName = a.Key.Name,
                    categorySum = a.Sum(c => c.Value),
                    categoryColor = ""
                })
                .OrderByDescending(a => a.categorySum)
                .ToList();

            var cashflowSum = cashflows.Sum(c => c.Value);

            var listCategories = new List<object>();
            var i = 0;
            while (i < res.Count)
            {
                var item = new
                {
                    res[i].categoryId,
                    res[i].categoryName,
                    categorySum = Math.Round(res[i].categorySum / cashflowSum, 2),
                    categoryColor = PieChartColor.PieColors.ElementAt(i).Value
                };
                listCategories.Add(item);
                i++;
            }

            var jr = Json(listCategories);

            return jr;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetProgressBarData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            var cookieValue = authCookie.Value;
            var ticket = FormsAuthentication.Decrypt(cookieValue);
            var userDataString = ticket.UserData;
            var userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            var userId = userData["CurrentUserId"] != null ? Convert.ToInt32(userData["CurrentUserId"]) : 0;

            var cashflows = _cashflowService.GetAll(userId, "");

            var res = cashflows
                .Where(a => a.Category != null)
                .GroupBy(a => new {a.Category.Id, a.Category.Name})
                .Select(a => new
                {
                    categoryId = a.Key.Id,
                    categoryName = a.Key.Name,
                    categorySum = a.Sum(c => c.Value),
                    categoryColor = ""
                })
                .OrderByDescending(a => a.categorySum)
                .ToList();


            var listCategories = new List<object>();
            var i = 0;
            while (i < res.Count)
            {
                var item = new
                {
                    res[i].categoryId,
                    res[i].categoryName,
                    res[i].categorySum,
                    categoryColor = PieChartColor.PieColors.ElementAt(i).Value
                };
                listCategories.Add(item);
                i++;
            }


            // var c = PieChartColor.PieColors.ElementAt(1);
            var jr = Json(listCategories);

            return jr;
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetExpensesData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            var cookieValue = authCookie.Value;
            var ticket = FormsAuthentication.Decrypt(cookieValue);
            var userDataString = ticket.UserData;
            var userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);
            var userId = userData["CurrentUserId"] != null ? Convert.ToInt32(userData["CurrentUserId"]) : 0;


            var data = new List<double>();
            var labels = new List<string>();

            var cashflows = _cashflowService.GetAll(userId, "");


            var z = cashflows
                .Where(c => c.AccountingDate.Month.ToString().Equals(reportFormDataRequestDto.month))
                .GroupBy(a => a.AccountingDate.Date)
                .Select(a => new
                {
                    date = a.Key,
                    value = a.Sum(b => b.Value)
                })
                .OrderBy(a => a.date)
                .ToList();


            var cdate = z.First().date;
            var endDate = z.Last().date;
            var i = 0;

            if (z.Count > 0)
                while (cdate <= endDate)
                {
                    if (!cdate.Equals(z[i].date))
                        while (!cdate.Equals(z[i].date) && cdate <= endDate)
                        {
                            labels.Add(Convert.ToDateTime(cdate).Date.ToString("yyyy-MM-dd"));
                            data.Add(0);
                            cdate = cdate.AddDays(1);
                        }

                    if (cdate.Equals(z[i].date))
                    {
                        labels.Add(Convert.ToDateTime(z[i].date).Date.ToString("yyyy-MM-dd"));
                        data.Add(z[i].value);
                    }

                    if (i < z.Count - 1) i++;

                    cdate = cdate.AddDays(1);
                }

            if (z.Count == 0 || z.Count == null)
                while (cdate <= endDate)
                {
                    labels.Add(Convert.ToDateTime(cdate).Date.ToString("yyyy-MM-dd"));
                    data.Add(0);
                    cdate = cdate.AddDays(1);
                }

            var response = new
            {
                labels, data
            };

            var jr = JsonConvert.SerializeObject(response);

            return Content(jr, "application/json");
        }
    }
}