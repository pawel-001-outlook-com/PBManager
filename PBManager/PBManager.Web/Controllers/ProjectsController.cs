using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
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
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;


        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult New()
        {
            var projectViewModel = new ProjectViewModel();
            projectViewModel.UserId = UserDataHelper.GetUserId(HttpContext);
            return PartialView(projectViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ProjectViewModel, Project>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var project = mapper.Map<ProjectViewModel, Project>(projectViewModel);

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

            return View();
        }


        public ActionResult Edit(int? id)
        {
            try
            {
                var project = _projectService.GetProjectById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Project, ProjectViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var projectViewModel = mapper.Map<Project, ProjectViewModel>(project);


                return PartialView(projectViewModel);
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
            if (ModelState.IsValid && id.Equals(projectViewModel.Id) &&
                projectViewModel.UserId.Equals(UserDataHelper.GetUserId(HttpContext)))
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ProjectViewModel, Project>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var project = mapper.Map<ProjectViewModel, Project>(projectViewModel);

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
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }
                catch (NotFoundException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }

            return View(projectViewModel);
        }

        public ActionResult View(int? id)
        {
            try
            {
                var project = _projectService.GetProjectById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Project, ProjectViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var projectViewModel = mapper.Map<Project, ProjectViewModel>(project);

                return PartialView(projectViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }


        public ActionResult Delete(int? id)
        {
            try
            {
                var project = _projectService.GetProjectById(id.GetValueOrDefault());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Project, ProjectViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var projectViewModel = mapper.Map<Project, ProjectViewModel>(project);

                return PartialView(projectViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
            }
        }


        public ActionResult ProjectData()
        {
            var draw = Convert.ToInt32(Request.QueryString["draw"]);
            var start = Convert.ToInt32(Request.QueryString["start"]);
            var length = Convert.ToInt32(Request.QueryString["length"]);

            var sortColumnIndex = Convert.ToInt32(Request.QueryString["order[0][column]"]);
            var sortColumnName = Request.QueryString["columns[" + sortColumnIndex + "][data]"];

            var sortDirection = Request.QueryString["order[0][dir]"];

            var searchValue = Request.QueryString["search[value]"];

            using (var db = new DataContext())
            {
                db.Configuration.LazyLoadingEnabled = true;

                var repository = new ProjectRepository(db);

                var recordsTotal = _projectService.GetTotalCount();
                var recordsFiltered = _projectService.GetFilteredCount(searchValue);

                var data = _projectService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                    length);

                var listObject = new List<object>();
                foreach (var p in data)
                {
                    var pr = new
                    {
                        p.Id,
                        p.Name,
                        StartDate = p.StartDate.ToShortDateString(),
                        TotalExpenses = p.Cashflows != null ? p.Cashflows.Sum(cf => cf.Value).ToString() : "",
                        p.Budget,
                        BudgetConsumed = p.Cashflows != null
                            ? Convert.ToString(p.Cashflows.Sum(c => c.Value) / p.Budget)
                            : ""
                    };
                    listObject.Add(pr);
                }


                var response = new
                {
                    draw,
                    recordsTotal,
                    recordsFiltered,
                    data = listObject
                };

                return Content(JsonConvert.SerializeObject(response), "application/json");
            }
        }
    }
}