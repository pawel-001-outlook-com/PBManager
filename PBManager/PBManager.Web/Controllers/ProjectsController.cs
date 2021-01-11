using AutoMapper;
using Newtonsoft.Json;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PBManager.Web.Controllers
{
    public class ProjectsController : BaseController
    {
        private IProjectService _projectService;


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
            ProjectViewModel projectViewModel = new ProjectViewModel();
            projectViewModel.UserId = (int)UserDataHelper.GetUserId(HttpContext);
            return PartialView(projectViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                Project project = Mapper.Map<ProjectViewModel, Project>(projectViewModel);

                _projectService.Add(project);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            Project project = _projectService.GetProjectById(id);

            ProjectViewModel projectViewModel = Mapper.Map<Project, ProjectViewModel>(project);


            return PartialView(projectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid && id.Equals(projectViewModel.Id) && projectViewModel.UserId.Equals(UserDataHelper.GetUserId(HttpContext)))
            {
                Project project = Mapper.Map<ProjectViewModel, Project>(projectViewModel);

                _projectService.Update(project);
                return RedirectToAction("Index");
            }
            else
            {
                return View(projectViewModel);
            }
        }

        public ActionResult View(int id)
        {
            Project project = _projectService.GetProjectById(id);

            ProjectViewModel projectViewModel = Mapper.Map<Project, ProjectViewModel>(project);

            return PartialView(projectViewModel);
        }


        public ActionResult Delete(int id)
        {
            Project project = _projectService.GetProjectById(id);

            ProjectViewModel projectViewModel = Mapper.Map<Project, ProjectViewModel>(project);

            return PartialView(projectViewModel);
        }


        public ActionResult ProjectData(string userId)
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

                var recordsTotal = _projectService.GetTotalCount(userIdInt);
                var recordsFiltered = _projectService.GetFilteredCount(searchValue, userIdInt);

                var data = _projectService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                    length, userId);

                List<object> listObject = new List<object>();
                foreach (var p in data)
                {
                    var pr = new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        StartDate = p.StartDate.ToShortDateString(),
                        TotalExpenses = p.Cashflows != null ? p.Cashflows.Sum(cf => cf.Value).ToString("F4") : "",
                        Budget = p.Budget.ToString("F4"),
                        BudgetConsumed = p.Cashflows != null
                            ? Convert.ToInt32(p.Cashflows.Sum(c => c.Value) / (p.Budget == 0 ? -1 : p.Budget) * 100)
                                .ToString("0")
                            : ""
                    };
                    listObject.Add(pr);
                }



                var response = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsFiltered,
                    data = listObject
                };

                return Content(JsonConvert.SerializeObject(response), "application/json");
            }
            else
            {
                throw new Exception("Bad User Id");
            }
        }
    }


}
