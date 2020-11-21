using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PBManager.Core.Models;
using PBManager.DAL;
using PBManager.DAL.Repositories;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using PBManager.Services.Exensions;
using PBManager.Services.Helpers;

namespace PBManager.Web.Controllers
{
    public class CashflowsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICashflowService _cashflowService;
        private readonly ICategoryService _categoryService;
        private readonly IProjectService _projectService;
        private readonly ISubcategoryService _subcategoryService;


        public CashflowsController(IAccountService accountService,
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


        public ActionResult New()
        {
            var cashflowViewModel = new CashflowViewModel();

            var userId = UserDataHelper.GetUserId(HttpContext);

            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            var accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            var projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;


            return PartialView(cashflowViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CashflowViewModel cashflowViewModel)
        {
            if (ModelState.IsValid)
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CashflowViewModel, Cashflow>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var cashflow = mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                    _cashflowService.Add(cashflow);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("MovementValidation", e.Message);
                    return View(cashflowViewModel);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }

            var userId = (int) Session["CurrentUserId"];

            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            var accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            var projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;

            return View(cashflowViewModel);
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                var cashflow = _cashflowService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cashflow, CashflowViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(cashflow);

                return PartialView(cashflowViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                var cashflow = _cashflowService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cashflow, CashflowViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(cashflow);

                var userId = UserDataHelper.GetUserId(HttpContext);

                var accountsSourceList = _accountService.GetByUser(userId);
                var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

                var accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
                var projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
                var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


                TempData["accountsSelectList"] = accountsSelectList;
                TempData["projectsSelectList"] = projectsSelectList;
                TempData["categoriesSelectList"] = categoriesSelectList;

                return PartialView(cashflowViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CashflowViewModel cashflowViewModel)
        {
            if (ModelState.IsValid && id.Equals(cashflowViewModel.Id))
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CashflowViewModel, Cashflow>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var cashflow = mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                    _cashflowService.Update(cashflow);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("MovementValidation", e.Message);
                    return View(cashflowViewModel);
                }
                catch (NotFoundException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }
                catch (DbUpdateException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }

            var userId = UserDataHelper.GetUserId(HttpContext);

            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            var accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            var projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            TempData["accountsSelectList"] = accountsSelectList;
            TempData["projectsSelectList"] = projectsSelectList;
            TempData["categoriesSelectList"] = categoriesSelectList;

            return View(cashflowViewModel);
        }


        public ActionResult View(int? id)
        {
            try
            {
                var cashflow = _cashflowService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cashflow, CashflowViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(cashflow);

                return PartialView(cashflowViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }


        [HttpGet]
        public ActionResult GetCategoryAndSubcategories(string categoryId)
        {
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                var subcategories = _subcategoryService.GetCategoryAndSubcategories(categoryId);
                return Json(subcategories, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        public JsonResult CashflowData()
        {
            var draw = Convert.ToInt32(Request.QueryString["draw"]);
            var start = Convert.ToInt32(Request.QueryString["start"]);
            var length = Convert.ToInt32(Request.QueryString["length"]);


            // get the column index of datatable to sort on
            var sortColumnIndex = Convert.ToInt32(Request.QueryString["order[0][column]"]);
            var sortColumnName = Request.QueryString["columns[" + sortColumnIndex + "][data]"];

            // get direction of sort
            var sortDirection = Request.QueryString["order[0][dir]"];

            //// get the search string
            var searchValue = Request.QueryString["search[value]"];


            using (var db = new DataContext())
            {
                // prevent Linq from DB calls to retrieve related entities
                db.Configuration.LazyLoadingEnabled = false;

                var repository = new CashflowRepository(db);

                var recordsTotal = _cashflowService.GetTotalCount();
                var recordsFiltered = _accountService.GetFilteredCount(searchValue);

                var data = repository.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length);


                var listObject = new List<object>();
                foreach (var c in data)
                {
                    var cf = new
                    {
                        c.Id,
                        AccountingDate = c.AccountingDate.ToShortDateString(),
                        c.Name,
                        c.Value,
                        Account = c.Account != null ? c.Account.Name : "",
                        Category = c.Category != null ? c.Category.Name : "",
                        Subcategory = c.Subcategory != null ? c.Subcategory.Name : "",
                        Project = c.Project != null ? c.Project.Name : ""
                    };
                    listObject.Add(cf);
                }

                var response = new
                {
                    draw,
                    recordsTotal,
                    recordsFiltered,
                    data = listObject
                };

                var jsonResponse = Json(response, JsonRequestBehavior.AllowGet);

                return jsonResponse;
            }
        }
    }
}