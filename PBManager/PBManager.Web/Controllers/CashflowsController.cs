using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using PBManager.Services.Exensions;
using PBManager.Services.Helpers;

namespace PBManager.Web.Controllers
{
    public class CashflowsController : Controller
    {
        private ICashflowService _cashflowService;
        private IAccountService _accountService ;
        private ICategoryService _categoryService;
        private ISubcategoryService _subcategoryService;
        private IProjectService _projectService;


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
            int userId = (int)Session["CurrentUserId"];
            string userName = (string)Session["CurrentUserName"];

            IEnumerable<Cashflow> cashflows = _cashflowService.GetAll(userId, userName);
            List<CashflowViewModel> cashflowViewModels = new List<CashflowViewModel>();

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Cashflow, CashflowViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            CashflowViewModel cashflowViewModel;

            foreach (Cashflow c in cashflows)
            {
                cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(c);
                cashflowViewModels.Add(cashflowViewModel);
            }

            return View(cashflowViewModels);
        }

        public ActionResult New()
        {
            CashflowViewModel cashflowViewModel = new CashflowViewModel();
            // cashflowViewModel.UserId = (int)Session["CurrentUserId"];

            int userId = (int)Session["CurrentUserId"];

            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            ViewBag.accountsSelectList = accountsSelectList;
            ViewBag.projectsSelectList = projectsSelectList;
            ViewBag.categoriesSelectList = categoriesSelectList;


            return View(cashflowViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CashflowViewModel cashflowViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<CashflowViewModel, Cashflow>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Cashflow cashflow = mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
                    _cashflowService.Add(cashflow);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("cashflow validation", e.Message);
                    return View(cashflowViewModel);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                int userId = (int)Session["CurrentUserId"];

                var accountsSourceList = _accountService.GetByUser(userId);
                var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

                SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
                SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
                SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


                ViewBag.accountsSelectList = accountsSelectList;
                ViewBag.projectsSelectList = projectsSelectList;
                ViewBag.categoriesSelectList = categoriesSelectList;

                return View(cashflowViewModel);
            }
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                Cashflow cashflow = _cashflowService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Cashflow, CashflowViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                CashflowViewModel cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(cashflow);

                return View(cashflowViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CashflowViewModel cashflowViewModel)
        {
            try
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<CashflowViewModel, Cashflow>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                Cashflow cashflow = mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                _cashflowService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
            catch (DbUpdateException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                Cashflow cashflow = _cashflowService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Cashflow, CashflowViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                CashflowViewModel cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(cashflow);

                int userId = (int) Session["CurrentUserId"];

                var accountsSourceList = _accountService.GetByUser(userId);
                var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

                SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
                SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
                SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


                ViewBag.accountsSelectList = accountsSelectList;
                ViewBag.projectsSelectList = projectsSelectList;
                ViewBag.categoriesSelectList = categoriesSelectList;

                return View(cashflowViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CashflowViewModel cashflowViewModel)
        {
            if (ModelState.IsValid && id.Equals(cashflowViewModel.Id))
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<CashflowViewModel, Cashflow>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Cashflow cashflow = mapper.Map<CashflowViewModel, Cashflow>(cashflowViewModel);

                    _cashflowService.Update(cashflow);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("cashflow validation", e.Message);
                    return View(cashflowViewModel);
                }
                catch (NotFoundException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                int userId = (int) Session["CurrentUserId"];

                var accountsSourceList = _accountService.GetByUser(userId);
                var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

                SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
                SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
                SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


                ViewBag.accountsSelectList = accountsSelectList;
                ViewBag.projectsSelectList = projectsSelectList;
                ViewBag.categoriesSelectList = categoriesSelectList;


                return View(cashflowViewModel);
            }
        }


        public ActionResult View(int? id)
        {
            try
            {
                Cashflow cashflow = _cashflowService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Cashflow, CashflowViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                CashflowViewModel cashflowViewModel = mapper.Map<Cashflow, CashflowViewModel>(cashflow);

                return View(cashflowViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }


        // private CashflowViewModel GetViewModel(Cashflow cashflow)
        // {
        //     return new CashflowViewModel
        //     {
        //         Cashflow = cashflow,
        //         Accounts = _accountService.GetAll((int) Session["CurrentUserId"]),
        //         Categories = _categoryService.GetAll(),
        //         Subcategories = _subcategoryService.GetAll(),
        //         Projects = _projectService.GetAllProjectsAndUser((int) Session["CurrentUserId"])
        //     };
        // }


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