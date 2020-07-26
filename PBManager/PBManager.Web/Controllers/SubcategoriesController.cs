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
    public class SubcategoriesController : Controller
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
            int userId = (int)Session["CurrentUserId"];
            IEnumerable<Subcategory> subcategories = _subcategoryService.GetSubcategoriesAndUser(userId);
            List<SubcategoryViewModel> subcategoryViewModels = new List<SubcategoryViewModel>();

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Subcategory, SubcategoryViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            SubcategoryViewModel subcategoryViewModel;

            foreach (Subcategory c in subcategories)
            {
                subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(c);
                subcategoryViewModels.Add(subcategoryViewModel);
            }

            return View(subcategoryViewModels);
        }


        public ActionResult View(int? id)
        {
            try
            {

                Subcategory subcategory = _subcategoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Subcategory, SubcategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                SubcategoryViewModel subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

                return View(subcategoryViewModel);


            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                Subcategory subcategory = _subcategoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Subcategory, SubcategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                SubcategoryViewModel subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

                int userId = (int)Session["CurrentUserId"];
                var categories = _categoryService.GetCategoriesAndUser(userId);
                ViewBag.CATEGORIES = categories;

                return View(subcategoryViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SubcategoryViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid && id.Equals(subcategoryViewModel.Id))
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<SubcategoryViewModel, Subcategory>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Subcategory subcategory = mapper.Map<SubcategoryViewModel, Subcategory>(subcategoryViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
                    _subcategoryService.Update(subcategory);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);
                    return View(subcategoryViewModel);
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
                return View(subcategoryViewModel);
            }
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                Subcategory subcategory = _subcategoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Subcategory, SubcategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                SubcategoryViewModel subcategoryViewModel = mapper.Map<Subcategory, SubcategoryViewModel>(subcategory);

                return View(subcategoryViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CategoryViewModel categoryViewModel)
        {
            try
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<SubcategoryViewModel, Subcategory>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                Category category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                _subcategoryService.Delete(id);
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


        public ActionResult New()
        {
            SubcategoryViewModel subcategoryViewModel = new SubcategoryViewModel();
            // subcategoryViewModel.UserId = (int)Session["CurrentUserId"];

            int userId = (int)Session["CurrentUserId"];

            var categories = _categoryService.GetCategoriesAndUser(userId);
            ViewBag.CATEGORIES = categories;

            return View(subcategoryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(SubcategoryViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<SubcategoryViewModel, Subcategory>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Subcategory subcategory = mapper.Map<SubcategoryViewModel, Subcategory>(subcategoryViewModel);

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
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(subcategoryViewModel);
            }
        }


    }
}