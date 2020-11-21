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
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult View(int? id)
        {
            try
            {
                var category = _categoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var categoryViewModel = mapper.Map<Category, CategoryViewModel>(category);

                return PartialView(categoryViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }


        public ActionResult New()
        {
            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.UserId = UserDataHelper.GetUserId(HttpContext);
            return PartialView(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CategoryViewModel, Category>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                    _categoryService.Add(category);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("CategoryValidation", e.Message);
                    return View(categoryViewModel);
                }
                catch (DbUpdateException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }

            return View(categoryViewModel);
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                var category = _categoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var categoryViewModel = mapper.Map<Category, CategoryViewModel>(category);

                return PartialView(categoryViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid && Id.Equals(categoryViewModel.Id))
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CategoryViewModel, Category>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                    _categoryService.Update(category);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("CategoryValidation", e.Message);
                    return View(categoryViewModel);
                }
                catch (NotFoundException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }
                catch (DbUpdateException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }

            return View(categoryViewModel);
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                var category = _categoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var categoryViewModel = mapper.Map<Category, CategoryViewModel>(category);

                return PartialView(categoryViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }

        public JsonResult CategoryData()
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

                var repository = new CategoryRepository(db);

                var recordsTotal = _categoryService.GetTotalCount();
                var recordsFiltered = _categoryService.GetFilteredCount(searchValue);

                var data = repository.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length);


                var listObject = new List<object>();
                foreach (var c in data)
                {
                    var cf = new
                    {
                        c.Id,
                        c.Name,
                        NoOfSubcategories = c.Subcategories != null ? c.Subcategories.Count.ToString() : ""
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