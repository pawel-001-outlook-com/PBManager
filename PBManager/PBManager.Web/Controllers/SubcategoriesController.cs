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
    public class SubcategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoriesController(ICategoryService categoryService, ISubcategoryService subcategoryService)
        {
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult View(int? id)
        {
            try
            {
                var subcategory = _subcategoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Subcategory, SubcategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

                return PartialView(subcategoryViewModel);
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
                var subcategory = _subcategoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Subcategory, SubcategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

                var userId = (int) Session["CurrentUserId"];

                var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
                var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
                TempData["categoriesSelectList"] = categoriesSelectList;

                return PartialView(subcategoryViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SubcategoryViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid && id.Equals(subcategoryViewModel.Id))
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SubcategoryViewModel, Subcategory>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var subcategory = mapper.Map<SubcategoryViewModel, Subcategory>(subcategoryViewModel);

                    _subcategoryService.Update(subcategory);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);
                    return PartialView(subcategoryViewModel);
                }
                catch (NotFoundException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }
                catch (DbUpdateException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }

            return PartialView(subcategoryViewModel);
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                var subcategory = _subcategoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Subcategory, SubcategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

                return PartialView(subcategoryViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }


        public ActionResult New()
        {
            var subcategoryViewModel = new SubcategoryViewModel();

            var userId = UserDataHelper.GetUserId(HttpContext);

            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
            var categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
            TempData["categoriesSelectList"] = categoriesSelectList;

            return PartialView(subcategoryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(SubcategoryViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid)
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SubcategoryViewModel, Subcategory>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var subcategory = mapper.Map<SubcategoryViewModel, Subcategory>(subcategoryViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
                    _subcategoryService.Add(subcategory);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);
                    return View(subcategoryViewModel);
                }
                catch (DbUpdateException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }

            return View(subcategoryViewModel);
        }


        public JsonResult SubcategoryData()
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

                var repository = new SubcategoryRepository(db);

                var recordsTotal = _subcategoryService.GetTotalCount();
                var recordsFiltered = _subcategoryService.GetFilteredCount(searchValue);

                var data = repository.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length);


                var listObject = new List<object>();
                foreach (var c in data)
                {
                    var cf = new
                    {
                        c.Id,
                        c.Name,
                        Category = c.Category != null ? c.Category.Name : ""
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