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
    public class SubcategoriesController : BaseController
    {
        private ISubcategoryService _subcategoryService;
        private ICategoryService _categoryService;

        public SubcategoriesController(ICategoryService categoryService, ISubcategoryService subcategoryService)
        {
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult View(int id)
        {
            Subcategory subcategory = _subcategoryService.GetById(id);

            SubcategoryViewModel subcategoryViewModel = Mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

            return PartialView(subcategoryViewModel);
        }


        public ActionResult Edit(int id)
        {
            Subcategory subcategory = _subcategoryService.GetById(id);

            SubcategoryViewModel subcategoryViewModel = Mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

            int userId = (int)UserDataHelper.GetUserId(HttpContext);

            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
            TempData["categoriesSelectList"] = categoriesSelectList;

            return PartialView(subcategoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SubcategoryViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid && id.Equals(subcategoryViewModel.Id))
            {
                Subcategory subcategory = Mapper.Map<SubcategoryViewModel, Subcategory>(subcategoryViewModel);

                _subcategoryService.Update(subcategory);
                return RedirectToAction("Index");
            }
            else
            {
                return PartialView(subcategoryViewModel);
            }
        }


        public ActionResult Delete(int id)
        {
            Subcategory subcategory = _subcategoryService.GetById(id);

            SubcategoryViewModel subcategoryViewModel = Mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

            return PartialView(subcategoryViewModel);
        }


        public ActionResult New(string categoryId = null)
        {
            SubcategoryViewModel subcategoryViewModel = new SubcategoryViewModel();

            int userId = (int)UserDataHelper.GetUserId(HttpContext);
            if (categoryId != null) subcategoryViewModel.CategoryId = Convert.ToInt32(categoryId);


            var categoriesSourceList = _categoryService.GetCategoriesAndUser(userId);
            SelectList categoriesSelectList = new SelectList(categoriesSourceList, "Id", "Name", new { });
            TempData["categoriesSelectList"] = categoriesSelectList;

            return PartialView(subcategoryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(SubcategoryViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Subcategory subcategory = Mapper.Map<SubcategoryViewModel, Subcategory>(subcategoryViewModel);

                _subcategoryService.Add(subcategory);
                return RedirectToAction("Index");

            }
            else
            {
                return View(subcategoryViewModel);
            }
        }


        public JsonResult SubcategoryData(string userId)
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


                var recordsTotal = _subcategoryService.GetTotalCount(userIdInt);
                var recordsFiltered = _subcategoryService.GetFilteredCount(searchValue, userIdInt);

                var data = _subcategoryService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                    length, userId);


                List<object> listObject = new List<object>();
                foreach (var c in data)
                {
                    var cf = new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Category = c.Category != null ? c.Category.Name : ""
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