using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    public class CategoriesController : Controller
    {
        // private CategoryService _categoryService = new CategoryService();
        private ICategoryService _categoryService;


        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        

        // GET: Categories
        public ActionResult Index()
        {
            int userId = (int) Session["CurrentUserId"];
            IEnumerable<Category> categories = _categoryService.GetCategoriesAndUser(userId);
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            CategoryViewModel categoryViewModel;

            foreach (Category c in categories)
            {
                categoryViewModel = mapper.Map<Category, CategoryViewModel>(c);
                categoryViewModels.Add(categoryViewModel);
            }

            return View(categoryViewModels);
        }


        public ActionResult View(int? id)
        {
            try
            {
                Category category = _categoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                CategoryViewModel categoryViewModel = mapper.Map<Category, CategoryViewModel>(category);

                return View(categoryViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }


        public ActionResult New()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.UserId = (int)Session["CurrentUserId"];
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Category category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
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
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(categoryViewModel);
            }
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                Category category = _categoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                CategoryViewModel categoryViewModel = mapper.Map<Category, CategoryViewModel>(category);


                return View(categoryViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid && Id.Equals(categoryViewModel.Id))
            {
                try
                {

                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Category category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
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
                return View(categoryViewModel);
            }
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                Category category = _categoryService.GetById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                CategoryViewModel categoryViewModel = mapper.Map<Category, CategoryViewModel>(category);

                return View(categoryViewModel);
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
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                Category category = mapper.Map<CategoryViewModel, Category>(categoryViewModel);

                _categoryService.Remove(id);
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

    }
}