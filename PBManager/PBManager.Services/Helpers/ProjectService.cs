using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.DAL.Repositories;

namespace PBManager.Services.Helpers
{
    class ProjectService
    {
        private ProjectRepository _repository = new ProjectRepository();
        private CashflowRepository _movementRepository = new CashflowRepository();

        public IEnumerable<Project> GetAllProjects()
        {
            return _repository.GetAllProjects();
        }

        public IEnumerable<Project> GetAllActiveProjects()
        {
            return _repository.GetAllActiveProjects();
        }

        public void Add(Project project)
        {
            project.Enabled = true;
            project.StartDate = DateTime.Today;

            var nameExists = (_repository.GetProjectByName(project.Name)) != null;

            if (!nameExists)
                _repository.Insert(project);
            else
                throw new Exception($"Already exists a project with the name {project.Name}");
        }

        public void Update(Project project)
        {
            project.Enabled = true;
            var quantity = (_repository.GetAllProjects(project.Name)).Count(p => !p.Id.Equals(project.Id));

            if (quantity.Equals(0))
                _repository.Update(project);
            else
                throw new Exception($"Already exists a project with the name {project.Name}");
        }

        public void Delete(int id)
        {
            var project = GetProjectById(id);
            project.Enabled = false;

            _repository.Update(project);
        }

        public Project GetProjectById(int id)
        {
            var project = _repository.GetProjectById(id);

            if (project != null)
                return project;
            else
                throw new Exception("This project not exists");
        }


    }
}
