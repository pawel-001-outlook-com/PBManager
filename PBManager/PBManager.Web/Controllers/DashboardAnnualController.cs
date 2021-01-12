using Newtonsoft.Json;
using PBManager.Core.Consts;
using PBManager.Dto.Dtos;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace PBManager.Web.Controllers
{
    public class DashboardAnnualController : BaseController
    {
        private ICashflowService _cashflowService;
        private IAccountService _accountService;
        private ICategoryService _categoryService;
        private ISubcategoryService _subcategoryService;
        private IProjectService _projectService;

        public DashboardAnnualController(
            IAccountService accountService,
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


        public ActionResult Index()
        {
            int userId = UserDataHelper.GetUserId(HttpContext);
            string userName = UserDataHelper.GetUserName(HttpContext);


            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });

            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;
            ViewBag.UserId = userId;

            return View();
        }


        [HttpPost]
        public JsonResult GetReportData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var userId = UserDataHelper.GetUserId(HttpContext);

            var cashflows = _cashflowService.GetAll(userId, "");

            var res = new[]
            {
                new
                {
                    categoryId = 0,
                    categoryName = "",
                    categorySum = 0.0d,
                    categoryColor = ""
                }
            }.ToList();

            var resIsDirty = false;

            if (reportFormDataRequestDto.accountId != null)
            {
                if (reportFormDataRequestDto.categoryId == null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Category != null ? a.Category.Id : 0),
                            Name = (a.Category != null ? a.Category.Name : "null")
                        })
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.categoryId != null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.subcategoryId != null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => a.SubcategoryId != null
                            ? a.SubcategoryId.ToString().Equals(reportFormDataRequestDto.subcategoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }
            }


            if (reportFormDataRequestDto.accountId == null)
            {
                if (reportFormDataRequestDto.categoryId == null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))

                        .GroupBy(a => new
                        {
                            Id = (a.Category != null ? a.Category.Id : 0),
                            Name = (a.Category != null ? a.Category.Name : "null")
                        })
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.categoryId != null)
                {
                    res = cashflows
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.subcategoryId != null)
                {
                    res = cashflows
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => a.SubcategoryId != null
                            ? a.SubcategoryId.ToString().Equals(reportFormDataRequestDto.subcategoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }
            }



            var cashflowSum = cashflows.Sum(c => c.Value);

            List<object> listCategories = new List<object>();
            int i = 0;
            while (i < res.Count)
            {
                var item = new
                {
                    res[i].categoryId,
                    res[i].categoryName,
                    categorySum = Math.Round((res[i].categorySum / cashflowSum), 2),
                    categoryColor = PieChartColor.PieColors.ElementAt(i).Value
                };
                listCategories.Add(item);
                i++;
            }

            if (i == 0 && resIsDirty == false)
            {
                var item = new
                {
                    categoryId = "no data",
                    categoryName = "no data",
                    categorySum = 1,
                    categoryColor = "rgba(220,220,220,0)"
                };
                listCategories.Add(item);
            }


            JsonResult jr = Json(listCategories);

            return jr;
        }

        [HttpPost]
        public JsonResult GetProgressBarData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            var cookieValue = authCookie.Value;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieValue);
            string userDataString = ticket.UserData;
            Dictionary<string, string> userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);

            var userId = userData["CurrentUserId"] != null ? Convert.ToInt32(userData["CurrentUserId"]) : 0;

            var cashflows = _cashflowService.GetAll(userId, "");

            var resIsDirty = false;

            var res = new[]
            {
                new
                {
                    categoryId = 0,
                    categoryName = "",
                    categorySum = 0.0d,
                    categoryColor = ""
                }
            }.ToList();

            if (reportFormDataRequestDto.accountId != null)
            {
                if (reportFormDataRequestDto.categoryId == null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Category != null ? a.Category.Id : 0),
                            Name = (a.Category != null ? a.Category.Name : "null")
                        })
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.categoryId != null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.subcategoryId != null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => a.SubcategoryId != null
                            ? a.SubcategoryId.ToString().Equals(reportFormDataRequestDto.subcategoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }
            }


            if (reportFormDataRequestDto.accountId == null)
            {
                if (reportFormDataRequestDto.categoryId == null)
                {
                    res = cashflows
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))

                        .GroupBy(a => new
                        {
                            Id = (a.Category != null ? a.Category.Id : 0),
                            Name = (a.Category != null ? a.Category.Name : "null")
                        })
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.categoryId != null)
                {
                    res = cashflows
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }

                if (reportFormDataRequestDto.subcategoryId != null)
                {
                    res = cashflows
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => a.SubcategoryId != null
                            ? a.SubcategoryId.ToString().Equals(reportFormDataRequestDto.subcategoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate
                            .ToShortDateString()
                            .Substring(0, 4)
                            .Equals(reportFormDataRequestDto.month))
                        .GroupBy(a => new
                        {
                            Id = (a.Subcategory != null ? a.Subcategory.Id : 0),
                            Name = (a.Subcategory != null ? a.Subcategory.Name : "null")
                        }
                        )
                        .Select(a => new
                        {
                            categoryId = a.Key.Id,
                            categoryName = a.Key.Name,
                            categorySum = a.Sum(c => c.Value),
                            categoryColor = ""
                        })
                        .OrderByDescending(a => a.categorySum)
                        .ToList();
                }
            }


            List<object> listCategories = new List<object>();
            int i = 0;
            while (i < res.Count())
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
                resIsDirty = true;
            }

            if (i == 0 & resIsDirty == false)
            {
                var item = new
                {
                    categoryId = "null",
                    categoryName = "null",
                    categorySum = 0,
                    categoryColor = PieChartColor.PieColors.ElementAt(i).Value
                };
                listCategories.Add(item);
            }


            JsonResult jr = Json(listCategories);

            return jr;
        }


        [HttpPost]
        public ActionResult GetExpensesData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            var cookieValue = authCookie.Value;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieValue);
            string userDataString = ticket.UserData;
            Dictionary<string, string> userData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(userDataString);
            var userId = userData["CurrentUserId"] != null ? Convert.ToInt32(userData["CurrentUserId"]) : 0;


            var data = new List<double>();
            var labels = new List<string>();

            var cashflows = _cashflowService.GetAll(userId, "");


            var z = cashflows
                .Where(a => reportFormDataRequestDto.accountId != null
                    ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                    : true)
                .Where(a => reportFormDataRequestDto.categoryId != null
                    ? (a.CategoryId != null
                        ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                        : false)
                    : true)
                .Where(a => reportFormDataRequestDto.subcategoryId != null
                    ? (a.SubcategoryId != null
                        ? a.SubcategoryId.ToString().Equals(reportFormDataRequestDto.subcategoryId)
                        : false)
                    : true)
                .Where(a => reportFormDataRequestDto.projectId != null
                    ? (a.ProjectId != null ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId) : false)
                    : true)
                .Where(a => a.AccountingDate
                    .ToShortDateString()
                    .Substring(0, 4)
                    .Equals(reportFormDataRequestDto.month))
                .GroupBy(a => a.AccountingDate.Date.Month)
                .Select(a => new
                {
                    date = a.Key,
                    value = a.Sum(b => b.Value)
                })
                .OrderBy(a => a.date)
                .ToList();


            if (z.Count > 0)
            {
                int cdate = 1;
                int endDate = 12;
                int i = 0;

                if (z.Count > 0)
                {
                    while (cdate <= endDate)
                    {
                        if (!cdate.Equals(z[i].date))
                        {
                            while (!cdate.Equals(z[i].date) && cdate <= endDate)
                            {
                                labels.Add(reportFormDataRequestDto.month + "-"
                                                                          + (Convert.ToString(cdate).Length < 2
                                                                            ? "0" + Convert.ToString(cdate)
                                                                            : Convert.ToString(cdate))
                                                                          );
                                data.Add(0);
                                cdate++;
                            }
                        }

                        if (cdate.Equals(z[i].date))
                        {
                            labels.Add(reportFormDataRequestDto.month + "-"
                                                                      + (Convert.ToString(cdate).Length < 2
                                                                          ? "0" + Convert.ToString(cdate)
                                                                          : Convert.ToString(cdate))
                            );
                            data.Add(z[i].value);
                        }

                        if (i < z.Count - 1)
                        {
                            i++;

                        }

                        cdate++;

                    }
                }

                if (z.Count == 0)
                {
                    while (cdate <= endDate)
                    {
                        labels.Add(reportFormDataRequestDto.month + "-"
                                                                  + (Convert.ToString(cdate).Length < 2
                                                                      ? "0" + Convert.ToString(cdate)
                                                                      : Convert.ToString(cdate))
                        );
                        data.Add(0);
                        cdate++;
                    }
                }

                var response = new
                {
                    labels,
                    data
                };

                string jr = JsonConvert.SerializeObject(response);

                return Content(jr, "application/json");
            }
            else
            {
                {

                    int cdate = 1;
                    int endDate = 12;
                    int i = 0;

                    // if (z.Count > 0)
                    {
                        while (cdate <= endDate)
                        {
                            labels.Add(reportFormDataRequestDto.month + "-"
                                                                      + (Convert.ToString(cdate).Length < 2
                                                                          ? "0" + Convert.ToString(cdate)
                                                                          : Convert.ToString(cdate))
                            );
                            data.Add(0);
                            cdate++;
                        }
                    }

                    var response = new
                    {
                        labels,
                        data
                    };

                    string jr = JsonConvert.SerializeObject(response);

                    return Content(jr, "application/json");

                }
            }
        }


        [HttpGet]
        public ActionResult GetCategoryAndSubcategories(string categoryId)
        {
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                IEnumerable<SelectListItem> subcategories = _subcategoryService.GetCategoryAndSubcategories(categoryId);
                return Json(subcategories, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }

}