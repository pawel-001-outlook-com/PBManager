using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;
using PBManager.Services.Contracts;
using Unity.Attributes;

namespace PBManager.Services.Helpers
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        private IProjectRepository _projectRepository;
        private ICashflowRepository _cashflowRepository;

        public ProjectService(){}


        [InjectionConstructor]
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Project> GetAllProjects()
        {
            return _unitOfWork.projects.GetAllProjects();
        }


        public IEnumerable<Project> GetAllActiveProjects()
        {
            return _unitOfWork.projects.GetAllActiveProjects();
        }


        public IEnumerable<Project> GetAllProjectsAndUser(int userId)
        {
            return _unitOfWork.projects.GetAllProjectsAndUser(userId);
        }


        public void Add(Project project)
        {
            // project.Enabled = true;
            project.StartDate = DateTime.Today;

            var nameExists = (_unitOfWork.projects.GetProjectByName(project.Name)) != null;

            if (!nameExists)
            {
                _unitOfWork.projects.Add(project);
                _unitOfWork.Complete();
            }
            else
                throw new Exception($"Already exists a project with the name {project.Name}");
        }


        public void Update(Project project)
        {
            // project.Enabled = true;
            var quantity = (_unitOfWork.projects.GetAllProjects(project.Name)).Count(p => !p.Id.Equals(project.Id));

            if (quantity.Equals(0))
            {
                _unitOfWork.projects.Update(project);
                _unitOfWork.Complete();
            }
            else
                throw new Exception($"Already exists a project with the name {project.Name}");
        }


        public void Delete(int id)
        {
            var project = GetProjectById(id);
            // project.Enabled = false;


            _unitOfWork.projects.Delete(project);
            _unitOfWork.Complete();
        }


        public Project GetProjectById(int id)
        {
            var project = _unitOfWork.projects.GetProjectById(id);

            if (project != null)
                return project;
            else
                throw new Exception("project does not exist");
        }




        public int GetTotalCount()
        {
            return _unitOfWork.projects.GetTotalCount();
        }


        public int GetFilteredCount(string searchValue)
        {
            return _unitOfWork.projects.GetFilteredCount(searchValue);
        }



        public List<Project> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length)
        {
            List<Project> a = _unitOfWork.projects.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length);
            return a;
        }


    }
}
