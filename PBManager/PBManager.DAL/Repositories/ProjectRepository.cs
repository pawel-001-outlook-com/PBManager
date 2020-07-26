using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;

namespace PBManager.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _dataContext;

        public ProjectRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        
        // public IEnumerable<Project> GetAllProjects()
        // {
        //     return _dataContext.Projects
        //             .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
        //             .ToList();
        // }
        

        public IEnumerable<Project> GetAllProjects(string name)
        {
                    return _dataContext.Projects
                        .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                        .Where(p => p.Name.Equals(name))
                        .ToList();
        }


        // public IEnumerable<Project> GetAllActiveProjects()
        // {
        //         return _dataContext.Projects
        //             .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
        //             .ToList();
        // }


        public Project GetProjectByName(string name)
        {
                return _dataContext.Projects
                    .Include(p => p.Cashflows)
                    .SingleOrDefault(p => p.Name.Equals(name));
        }


        public Project GetProjectById(int id)
        {
                return _dataContext.Projects
                    .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                    .SingleOrDefault(p => p.Id.Equals(id));
        }


        public IEnumerable<Project> GetAllProjectsAndUser(int userId)
        {
                return _dataContext.Projects
                    .Where(p => p.UserId == userId)
                    .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                    .ToList();
        }


        // public void Remove(Project project){
        //         _dataContext.Entry(project).State = EntityState.Deleted;
        //         _dataContext.SaveChanges();
        // }




        public void Add(Project project)
        {
            _dataContext.Projects.Add(project);
        }

        public void Update(Project project)
        {
            _dataContext.Entry(project).State = EntityState.Modified;
        }

        public void Delete(Project project)
        {
            _dataContext.Entry(project).State = EntityState.Deleted;
        }
    }
}
