using AutoMapper;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PBManager.Web.Controllers
{
    public class CashflowsController : BaseController
    {
        private ICashflowService _cashflowService;
        private IAccountService _accountService;
        private ICategoryService _categoryService;
        private ISubcategoryService _subcategoryService;
        private IProjectService _projectService;


        public CashflowsController(
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
            return View();
        }


        public ActionResult New(string accountId = null, string projectId = null, string categoryId = null, string subcategoryId = null)
        {
            CashflowViewModel cashflowViewModel = new CashflowViewModel();
            if (accountId != null) cashflowViewModel.AccountId = Convert.ToInt32(accountId);
            if (projectId != null) cashflowViewModel.ProjectId = Convert.ToInt32(projectId);
            if (categoryId != null) cashflowViewModel.CategoryId = Convert.ToInt32(categoryId);
            if (categoryId != null) cashflowViewModel.SubcategoryId = Convert.ToInt32(subcategoryId);

            int userId = (int)UserDataHelper.GetUserId(HttpContext);

            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;

            if (subcategoryId != null)
            {
                var subcategoriesSourceList = new List<Subcategory>();
                subcategoriesSourceList.Add(_subcategoryService.GetById(Convert.ToInt32(subcategoryId)));
                SelectList subcategoriesSelectList = new SelectList(subcategoriesSourceList, "Id", "Name", new { });
                TempData["subcategoriesSelectList"] = subcategoriesSelectList;
            }



            return PartialView(cashflowViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CashflowViewModel cashflowViewModel)
        {
            if (ModelState.IsValid)
            {
                Cashflow cashflow = Mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                _cashflowService.Add(cashflow);
                return RedirectToAction("Index");
            }
            else
            {
                int userId = (int)UserDataHelper.GetUserId(HttpContext);

                var accountsSourceList = _accountService.GetByUser(userId);
                var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

                SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
                SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
                SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


                TempData["accountsSelectList"] = accountsSelectList;
                TempData["projectsSelectList"] = projectsSelectList;
                TempData["categoriesSelectList"] = categoriesSelectList;

                return View(cashflowViewModel);
            }
        }


        public ActionResult Delete(int id)
        {
            Cashflow cashflow = _cashflowService.GetById(id);

            CashflowViewModel cashflowViewModel = Mapper.Map<Cashflow, CashflowViewModel>(cashflow);

            return PartialView(cashflowViewModel);
        }


        public ActionResult Edit(int id)
        {
            Cashflow cashflow = _cashflowService.GetById(id);

            CashflowViewModel cashflowViewModel = Mapper.Map<Cashflow, CashflowViewModel>(cashflow);

            int userId = (int)UserDataHelper.GetUserId(HttpContext);

            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            if (cashflow.Category != null)
            {
                if (cashflow.Category.Subcategories != null)
                {
                    var subcategoriesSourceList = new List<Subcategory>();

                    foreach (var subcategory in cashflow.Category.Subcategories)
                    {
                        subcategoriesSourceList.Add(_subcategoryService.GetById(Convert.ToInt32(subcategory.Id)));
                    }

                    SelectList subcategoriesSelectList = new SelectList(subcategoriesSourceList, "Id", "Name", new { });
                    TempData["subcategoriesSelectList"] = subcategoriesSelectList;
                }
            }

            SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;

            return PartialView(cashflowViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CashflowViewModel cashflowViewModel)
        {
            if (ModelState.IsValid && id.Equals(cashflowViewModel.Id))
            {
                Cashflow cashflow = Mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                _cashflowService.Update(cashflow);
                return RedirectToAction("Index");
            }
            else
            {
                int userId = (int)UserDataHelper.GetUserId(HttpContext);

                var accountsSourceList = _accountService.GetByUser(userId);
                var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

                SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
                SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
                SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


                TempData["accountsSelectList"] = accountsSelectList;
                TempData["projectsSelectList"] = projectsSelectList;
                TempData["categoriesSelectList"] = categoriesSelectList;

                return View(cashflowViewModel);
            }
        }


        public ActionResult View(int id)
        {
            Cashflow cashflow = _cashflowService.GetById(id);

            CashflowViewModel cashflowViewModel = Mapper.Map<Cashflow, CashflowViewModel>(cashflow);

            return PartialView(cashflowViewModel);
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


        public JsonResult CashflowData(string userId = null)
        {
            int userIdInt = Convert.ToInt32(userId);

            if (userIdInt.Equals(UserDataHelper.GetUserId(HttpContext)))
            {

                int draw = Convert.ToInt32(Request.QueryString["draw"]);
                int start = Convert.ToInt32(Request.QueryString["start"]);
                int length = Convert.ToInt32(Request.QueryString["length"]);

                var sortColumnIndex = Convert.ToInt32(Request.QueryString["order[0][column]"]);
                var sortColumnName = Request.QueryString["columns[" + sortColumnIndex + "][data]"];

                var sortDirection = Request.QueryString["order[0][dir]"];

                var searchValue = Request.QueryString["search[value]"];

                var recordsTotal = _cashflowService.GetTotalCount(userIdInt);
                var recordsFiltered = _cashflowService.GetFilteredCount(searchValue, userIdInt);

                var data = _cashflowService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                    length, userId);


                List<object> listObject = new List<object>();
                foreach (var c in data)
                {
                    var cf = new
                    {
                        Id = c.Id,
                        AccountingDate = c.AccountingDate.ToShortDateString(),
                        Name = c.Name,
                        Value = c.Value.ToString("F4"),
                        Account = c.Account != null ? c.Account.Name : "",
                        Category = c.Category != null ? c.Category.Name : "",
                        Subcategory = c.Subcategory != null ? c.Subcategory.Name : "",
                        Project = c.Project != null ? c.Project.Name : ""
                    };
                    listObject.Add(cf);
                }

                var response = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsFiltered,
                    data = listObject
                };

                var jsonResponse = Json(response, JsonRequestBehavior.AllowGet);

                return jsonResponse;
            }
            else
            {
                throw new Exception("Bad User Id");
            }
        }

    }

}
