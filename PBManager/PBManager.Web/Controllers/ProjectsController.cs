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
    public class ProjectsController : Controller
    {
        private IProjectService _projectService;


        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        public ActionResult Index()
        {
            int userId = (int)Session["CurrentUserId"];
            IEnumerable<Project> projects = _projectService.GetAllProjectsAndUser(userId);
            List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            ProjectViewModel projectViewModel;

            foreach (Project c in projects)
            {
                projectViewModel = mapper.Map<Project, ProjectViewModel>(c);
                projectViewModels.Add((ProjectViewModel) projectViewModel);
            }

            return View(projectViewModels);
        }


        public ActionResult New()
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            projectViewModel.UserId = (int)Session["CurrentUserId"];
            return View(projectViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProjectViewModel, Project>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Project project = mapper.Map<ProjectViewModel, Project>(projectViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
                    _projectService.Add(project);
                    return RedirectToAction("Index");

                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("ProjectValidation", e.Message);
                    return View(projectViewModel);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View();
            }
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                Project project = _projectService.GetProjectById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                ProjectViewModel projectViewModel = mapper.Map<Project, ProjectViewModel>(project);


                return View(projectViewModel);


                // return View(_projectService.GetProjectById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid && id.Equals(projectViewModel.Id))
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProjectViewModel, Project>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Project project = mapper.Map<ProjectViewModel, Project>(projectViewModel);

                    // category.UserID = (int) Session["CurrentUserId"];
                    _projectService.Update(project);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("ProjectValidation", e.Message);
                    return View(projectViewModel);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
                catch (NotFoundException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
                }
            }
            else
            {
                return View(projectViewModel);
            }
        }

        public ActionResult View(int? id)
        {
            try
            {
                Project project = _projectService.GetProjectById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                ProjectViewModel projectViewModel = mapper.Map<Project, ProjectViewModel>(project);

                return View(projectViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                Project project = _projectService.GetProjectById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                ProjectViewModel projectViewModel = mapper.Map<Project, ProjectViewModel>(project);

                return View(projectViewModel);

                // return View(_projectService.GetProjectById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ProjectViewModel projectViewModel)
        {
            try
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProjectViewModel, Project>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                Project project = mapper.Map<ProjectViewModel, Project>(projectViewModel);

                _projectService.Delete(id);
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