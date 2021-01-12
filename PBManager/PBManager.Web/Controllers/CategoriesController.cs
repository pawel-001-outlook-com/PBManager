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
    public class CategoriesController : BaseController
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult View(int id)
        {
            Category category = _categoryService.GetById(id);

            CategoryViewModel categoryViewModel = Mapper.Map<Category, CategoryViewModel>(category);

            return PartialView(categoryViewModel);
        }


        public ActionResult New()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.UserId = (int)UserDataHelper.GetUserId(HttpContext);
            return PartialView(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CategoryViewModel categoryViewModel)
        {
            Category category = Mapper.Map<CategoryViewModel, Category>(categoryViewModel);

            _categoryService.Add(category);
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            Category category = _categoryService.GetById(id);

            CategoryViewModel categoryViewModel = Mapper.Map<Category, CategoryViewModel>(category);

            return PartialView(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid && Id.Equals(categoryViewModel.Id))
            {
                Category category = Mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                _categoryService.Update(category);
                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryViewModel);
            }
        }


        public ActionResult Delete(int id)
        {
            Category category = _categoryService.GetById(id);

            CategoryViewModel categoryViewModel = Mapper.Map<Category, CategoryViewModel>(category);

            return PartialView(categoryViewModel);
        }


        public JsonResult CategoryData(string userId = null)
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


                var recordsTotal = _categoryService.GetTotalCount(userIdInt);
                var recordsFiltered = _categoryService.GetFilteredCount(searchValue, userIdInt);

                var data = _categoryService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length, userId);


                List<object> listObject = new List<object>();
                foreach (var c in data)
                {
                    var cf = new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        NoOfSubcategories = c.Subcategories != null ? c.Subcategories.Count.ToString() : ""
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
