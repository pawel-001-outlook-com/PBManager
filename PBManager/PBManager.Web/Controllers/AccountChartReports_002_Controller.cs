// using System;
// using System.Collections.Generic;
// using System.Data.Entity.SqlServer;
// using System.Globalization;
// using System.Linq;
// using System.Net;
// using System.Web;
// using System.Web.Mvc;
// using PBManager.Dto.ViewModels;
// using PBManager.Services.Contracts;
// using PBManager.Services.Exceptions;
//
// namespace PBManager.Web.Controllers
// {
//     public class AccountChartReportsController : Controller
//     {
//         private ICashflowService _cashflowService;
//         private IAccountService _accountService;
//         private ICategoryService _categoryService;
//         private ISubcategoryService _subcategoryService;
//         private IProjectService _projectService;
//
//         public AccountChartReportsController(IAccountService accountService,
//             ICategoryService categoryService,
//             ISubcategoryService subcategoryService,
//             IProjectService projectService,
//             ICashflowService cashflowService)
//         {
//             _accountService = accountService;
//             _categoryService = categoryService;
//             _subcategoryService = subcategoryService;
//             _projectService = projectService;
//             _cashflowService = cashflowService;
//         }
//
//         public ActionResult Index()
//         {
//             int userId = (int)Session["CurrentUserId"];
//             string userName = (string)Session["CurrentUserName"];
//
//
//             var accountsSourceList = _accountService.GetByUser(userId);
//             var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
//             var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
//
//             SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
//             SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
//             SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
//
//
//             ViewBag.accountsSelectList = accountsSelectList;
//             ViewBag.projectsSelectList = projectsSelectList;
//             ViewBag.categoriesSelectList = categoriesSelectList;
//
//             var reportViewModel = new AccountChartReportViewModel
//             {
//                 UserId = userId
//             };
//
//             return View(reportViewModel);
//         }
//
//         [HttpPost]
//         public ActionResult Report(AccountChartReportViewModel accountChartReportViewModel)
//         {
//             try
//             {
//                 int userId = (int)Session["CurrentUserId"];
//
//                 var accountsSourceList = _accountService.GetByUser(userId);
//                 var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
//                 var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
//
//                 SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
//                 SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
//                 SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
//
//
//                 ViewBag.accountsSelectList = accountsSelectList;
//                 ViewBag.projectsSelectList = projectsSelectList;
//                 ViewBag.categoriesSelectList = categoriesSelectList;
//
//                 // var viewModel = GetReportViewModel();
//                 accountChartReportViewModel.Cashflows = _cashflowService.GetAll(accountChartReportViewModel);
//
//                 // return View(accountChartReportViewModel);
//                 return null;
//             }
//             catch (NotFoundException e)
//             {
//                 TempData["ErrorMessage"] = e.Message;
//                 return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
//             }
//         }
//
//         [HttpPost]
//         public ActionResult AccountChartReport(AccountChartReportViewModel acReportViewModel)
//         {
//             DateTime startDate = acReportViewModel.StartDate;
//             DateTime endDate = acReportViewModel.EndDate;
//
//             int userId = (int)Session["CurrentUserId"];
//
//             var accountsSourceList = _accountService.GetByUser(userId);
//             var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
//             var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
//
//             SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
//             SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
//             SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
//
//
//             ViewBag.accountsSelectList = accountsSelectList;
//             ViewBag.projectsSelectList = projectsSelectList;
//             ViewBag.categoriesSelectList = categoriesSelectList;
//
//
//
//
//             try
//             {
//                 var viewModel = new AccountChartReportViewModel
//                 {
//                     UserId = userId
//                 };
//
//
//                 viewModel.Cashflows = _cashflowService.GetAll(acReportViewModel);
//
//                 // var z = viewModel.Cashflows
//                 //     .GroupBy(a => a.AccountingDate.Date)
//                 //     .Select(a => new
//                 //     {
//                 //         date = a.Key,
//                 //         value = a.Sum(b => b.Value)
//                 //     })
//                 //     .OrderBy(a => a.date)
//                 //     .ToList();
//
//
//
//                 CultureInfo ciCurr = CultureInfo.CurrentCulture;
//                 // int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstDay DayOfWeek.Monday);
//
//                 // SqlFunctions.DatePart("week", DateTime.Now);
//
//
//                 var z = viewModel.Cashflows
//                     .GroupBy(a => a.AccountingDate.Year + "-" + SqlFunctions.DatePart("week", a.AccountingDate)
//             // new { a.AccountingDate.Year,
//             // SqlFunctions.DatePart("week", DateTime.Now)}
//             )
//                     .Select(a => new
//                     {
//                         // year = a.Key.Year,
//                         // month = a.Key.Month,
//                         week = a.Key,
//                         value = a.Sum(b => b.Value)
//                     })
//                     .OrderBy(a => a.week)
//                     .ToList();
//
//
//
//
//                 var data = new List<double>();
//                 var labels = new List<string>();
//
//                 foreach (var item in z)
//                 {
//                     labels.Add(item.week);
//                     data.Add(item.value);
//
//                 }
//
//
//                 //  MIESIAC
//
//
//                 // var z = viewModel.Cashflows
//                 //     .GroupBy(a => new{ a.AccountingDate.Year, a.AccountingDate.Month})
//                 //     .Select(a => new
//                 //     {
//                 //         year = a.Key.Year,
//                 //         month = a.Key.Month,
//                 //         value = a.Sum(b => b.Value)
//                 //     })
//                 //     .OrderBy(a => a.month)
//                 //     .ToList();
//                 //
//                 //
//                 //
//                 //
//                 // var data = new List<double>();
//                 // var labels = new List<string>();
//                 //
//                 // foreach (var item in z)
//                 // {
//                 //     labels.Add(item.year.ToString() + "-" + item.month.ToString());
//                 //     data.Add(item.value);
//                 //
//                 // }
//
//
//                 // .MIESIEC
//
//
//                 DateTime cdate = startDate;
//                 DateTime ndate = startDate.AddDays(1);
//                 int i = 0;
//
//                 // if (z.Count > 0)
//                 // {
//                 //     while (cdate <= endDate)
//                 //     {
//                 //         if (!cdate.Equals(z[i].date))
//                 //         {
//                 //             while (!cdate.Equals(z[i].date) && cdate <= endDate)
//                 //             {
//                 //                 labels.Add(Convert.ToDateTime(cdate).Date.ToString("yyyy-MM-dd"));
//                 //                 data.Add(0);
//                 //                 cdate = cdate.AddDays(1);
//                 //             }
//                 //         }
//                 //
//                 //         if (cdate.Equals(z[i].date))
//                 //         {
//                 //             labels.Add(Convert.ToDateTime(z[i].date).Date.ToString("yyyy-MM-dd"));
//                 //             data.Add(z[i].value);
//                 //         }
//                 //
//                 //         if (i < z.Count - 1)
//                 //         {
//                 //             i++;
//                 //
//                 //         }
//                 //         
//                 //         cdate = cdate.AddDays(1);
//                 //
//                 //     }
//                 // }
//                 //
//                 // if (z.Count == 0 || z.Count == null)
//                 // {
//                 //     while (cdate <= endDate)
//                 //     {
//                 //         labels.Add(Convert.ToDateTime(cdate).Date.ToString("yyyy-MM-dd"));
//                 //         data.Add(0);
//                 //         cdate = cdate.AddDays(1);
//                 //     }
//                 // }
//
//                 ViewBag.LABELS = labels;
//                 ViewBag.DATA = data;
//
//
//                 // var pieChartPreData = viewModel.Cashflows
//                 //     .GroupBy(a => new {a.Category.Id , a.Category.Name })
//                 //     .Select(a => new
//                 //     {
//                 //         categoryId = a.Key.Id,
//                 //         categoryName = a.Key.Name,
//                 //         categorySum = a.Sum(c => c.Value)
//                 //     })
//                 //     .OrderBy(a => a.categorySum)
//                 //     .ToList();
//                 //
//                 //
//                 // var pcCategoryId = new List<int>();
//                 // var pcCategoryValue = new List<double>();
//                 // var pcCategoryLabels = new List<string>();
//                 //
//                 // foreach(var car in pieChartPreData)
//                 // {
//                 //     pcCategoryId.Add(car.categoryId);
//                 //     pcCategoryValue.Add(car.categorySum);
//                 //     pcCategoryLabels.Add(car.categoryName);
//                 // }
//                 //
//                 // ViewBag.PIECHARTID = pcCategoryId;
//                 // ViewBag.PIECHARTLABELS = pcCategoryLabels;
//                 // ViewBag.PIECHARTDATA = pcCategoryValue;
//
//                 //
//                 // var bcdata = viewModel.Cashflows
//                 //     .GroupBy(a => new { a.Project.Id, a.Project.Name })
//                 //     .Select(a => new
//                 //     {
//                 //         projectid = a.Key.Id,
//                 //         projectname = a.Key.Name,
//                 //         projectvalue = a.Sum(c => c.Value)
//                 //     })
//                 //     .OrderBy(a => a.projectvalue)
//                 //     .ToList();
//                 //
//                 // var bcProjectId = new List<int>();
//                 // var bcProjectValue = new List<double>();
//                 // var bcProjectName = new List<string>();
//                 //
//                 // foreach (var project in bcdata)
//                 // {
//                 //     bcProjectId.Add(project.projectid);
//                 //     bcProjectValue.Add(project.projectvalue);
//                 //     bcProjectName.Add(project.projectname);
//                 // }
//                 //
//                 // ViewBag.BARCHARTID =     bcProjectId;
//                 // ViewBag.BARCHARTLABELS = bcProjectName;
//                 // ViewBag.BARCHARTDATA =   bcProjectValue;
//
//
//                 return View(viewModel);
//
//
//
//             }
//             catch (NotFoundException e)
//             {
//                 TempData["ErrorMessage"] = e.Message;
//                 return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
//             }
//         }
//
//
//
//
//
//
//         [HttpGet]
//         public ActionResult GetCategoryAndSubcategories(string categoryId)
//         {
//             if (!string.IsNullOrWhiteSpace(categoryId))
//             {
//                 IEnumerable<SelectListItem> subcategories = _subcategoryService.GetCategoryAndSubcategories(categoryId);
//                 return Json(subcategories, JsonRequestBehavior.AllowGet);
//             }
//             return null;
//         }
//
//
//         // private ReportViewModel GetReportViewModel()
//         // {
//         //     return new ReportViewModel
//         //     {
//         //
//         //         Accounts = _accountService.GetAll(),
//         //         Categories = _categoryService.GetAll(),
//         //         Subcategories = _subcategoryService.GetAll(),
//         //         Projects = _projectService.GetAllProjects()
//         //     };
//         // }
//
//
//
//
//     }
// }