using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using PBManager.Dto.ViewModels;
using PBManager.Services.Helpers;
using PBManager.Services.Exceptions;
using System.Net;
using System.IO;
using System.Linq;
using AutoMapper;
using PBManager.Core.Models;
using PBManager.Services.Contracts;
using PBManager.Services.Exensions;

namespace PBManager.Controllers
{
    public class ReportsController : Controller
    {
        private ICashflowService _cashflowService;
        private IAccountService _accountService;
        private ICategoryService _categoryService;
        private ISubcategoryService _subcategoryService;
        private IProjectService _projectService;

        public ReportsController(IAccountService accountService,
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


            var accountsSourceList = _accountService.GetByUser(userId);
            var projectsSourceList = _projectService.GetAllProjectsAndUser(userId);
            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);

            SelectList accountsSelectList = new SelectList(accountsSourceList, "Id", "Name", new { });
            SelectList projectsSelectList = new SelectList(projectsSourceList, "Id", "Name", new { });
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });


            ViewBag.accountsSelectList = accountsSelectList;
            ViewBag.projectsSelectList = projectsSelectList;
            ViewBag.categoriesSelectList = categoriesSelectList;

            var reportViewModel = new ReportViewModel
            {
                UserId = userId
                // Accounts = _accountService.GetAll(),
                // Categories = _categoryService.GetAll(),
                // Subcategories = _subcategoryService.GetAll(),
                // Projects = _projectService.GetAllProjects()
            };

            return View(reportViewModel);
        }


        [HttpPost]
        public ActionResult Report(ReportViewModel reportViewModel)
        {
            try
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

                reportViewModel.Cashflows = _cashflowService.GetAll(reportViewModel);

                return View(reportViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
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