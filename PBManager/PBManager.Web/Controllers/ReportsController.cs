using PBManager.Core.Models;
using PBManager.Dto.Dtos;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PBManager.Controllers
{
    public class ReportsController : Controller
    {
        private ICashflowService _cashflowService;
        private IAccountService _accountService;
        private ICategoryService _categoryService;
        private ISubcategoryService _subcategoryService;
        private IProjectService _projectService;

        public ReportsController(
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
        public ActionResult GetReportData(ReportFormDataRequestDto reportFormDataRequestDto)
        {
            var userId = UserDataHelper.GetUserId(HttpContext);

            List<Cashflow> res = new List<Cashflow>();

            if (reportFormDataRequestDto.accountId != null)
            {
                if (reportFormDataRequestDto.categoryId == null)
                {
                    res = _cashflowService.GetAll(userId, "")
                        .Where(a => reportFormDataRequestDto.accountId != null
                            ? a.AccountId.ToString().Equals(reportFormDataRequestDto.accountId)
                            : true)
                        .Where(a => a.AccountingDate >= Convert.ToDateTime(reportFormDataRequestDto.datefrom)
                                    && a.AccountingDate <= Convert.ToDateTime(reportFormDataRequestDto.dateto)

                        )
                        .OrderByDescending(a => a.AccountingDate)
                        .Take(1000)
                        .ToList();
                }

            }

            if (reportFormDataRequestDto.categoryId != null)
            {
                res = _cashflowService.GetAll(userId, "")
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
                    .Where(a => a.AccountingDate >= Convert.ToDateTime(reportFormDataRequestDto.datefrom)
                                && a.AccountingDate <= Convert.ToDateTime(reportFormDataRequestDto.dateto)

                    )
                    .OrderByDescending(a => a.AccountingDate)
                    .Take(1000)
                    .ToList();
            }

            if (reportFormDataRequestDto.subcategoryId != null)
            {
                res = _cashflowService.GetAll(userId, "")
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
                    .Where(a => a.AccountingDate >= Convert.ToDateTime(reportFormDataRequestDto.datefrom)
                                && a.AccountingDate <= Convert.ToDateTime(reportFormDataRequestDto.dateto)

                    )
                    .OrderByDescending(a => a.AccountingDate)
                    .Take(1000)
                    .ToList();
            }



            if (reportFormDataRequestDto.accountId == null)
            {
                if (reportFormDataRequestDto.categoryId == null)
                {

                    if (reportFormDataRequestDto.categoryId == null)
                    {
                        res = _cashflowService.GetAll(userId, "")
                            .Where(a => a.AccountingDate >= Convert.ToDateTime(reportFormDataRequestDto.datefrom)
                                        && a.AccountingDate <= Convert.ToDateTime(reportFormDataRequestDto.dateto)

                            )
                            .OrderByDescending(a => a.AccountingDate)
                            .Take(1000)
                            .ToList();
                    }


                }

                if (reportFormDataRequestDto.categoryId != null)
                {
                    res = _cashflowService.GetAll(userId, "")
                        .Where(a => a.CategoryId != null
                            ? a.CategoryId.ToString().Equals(reportFormDataRequestDto.categoryId)
                            : false)
                        .Where(a => reportFormDataRequestDto.projectId != null
                            ? (a.ProjectId != null
                                ? a.ProjectId.ToString().Equals(reportFormDataRequestDto.projectId)
                                : false)
                            : true)
                        .Where(a => a.AccountingDate >= Convert.ToDateTime(reportFormDataRequestDto.datefrom)
                                    && a.AccountingDate <= Convert.ToDateTime(reportFormDataRequestDto.dateto)

                        )
                        .OrderByDescending(a => a.AccountingDate)
                        .Take(1000)
                        .ToList();
                }

                if (reportFormDataRequestDto.subcategoryId != null)
                {
                    res = _cashflowService.GetAll(userId, "")
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
                        .Where(a => a.AccountingDate >= Convert.ToDateTime(reportFormDataRequestDto.datefrom)
                                    && a.AccountingDate <= Convert.ToDateTime(reportFormDataRequestDto.dateto)

                        )
                        .OrderByDescending(a => a.AccountingDate)
                        .Take(1000)
                        .ToList();
                }
            }

            ReportViewModel rvm = new ReportViewModel();
            rvm.Cashflows = res;
            return PartialView("_ReportTable", rvm);
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