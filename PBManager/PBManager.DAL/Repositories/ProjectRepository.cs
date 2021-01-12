using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PBManager.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _dataContext;

        public ProjectRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IEnumerable<Project> GetAllProjects(string name)
        {
            return _dataContext.Projects
                .AsNoTracking()
                .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                .Where(p => p.Name.Equals(name))
                .ToList();

        }


        public Project GetProjectByName(string name)
        {
            return _dataContext.Projects
                .AsNoTracking()
                .Include(p => p.Cashflows)
                .SingleOrDefault(p => p.Name.Equals(name));
        }

        public Project GetProjectById(int id)
        {
            return _dataContext.Projects
                .AsNoTracking()
                .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                .SingleOrDefault(p => p.Id.Equals(id));
        }


        public IEnumerable<Project> GetAllProjectsAndUser(int userId)
        {
            return _dataContext.Projects
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                .ToList();
        }


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



        public int GetFilteredCount(string searchValue, int userId)
        {
            return _dataContext.Projects
                .Where(a => a.UserId.Equals(userId))
                .Where(a => a.Name.Contains(searchValue)
                )
                .Count();
        }

        public int GetTotalCount(int userId)
        {
            int totalCount = _dataContext.Projects
                .Where(a => a.UserId.Equals(userId)).
                Count();
            return totalCount;
        }

        public List<Project> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId)
        {
            List<Project> result;
            int userIdInt = Convert.ToInt32(userId);
            if (sortDirection == "asc")
            {
                result =
                    _dataContext.Projects
                        .Where(a => a.UserId.Equals(userIdInt))
                        .Where(a => a.Name.Contains(searchValue))
                        .Include(a => a.Cashflows)
                        .ToList<Project>()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)) //Sort by sortColumn
                        .Skip(start)
                        .Take(length)
                        .ToList<Project>();
            }
            else
            {
                result =
                    _dataContext.Projects
                        .Where(a => a.UserId.Equals(userIdInt))
                        .Where(a => a.Name.Contains(searchValue))
                        .Include(a => a.Cashflows)
                        .ToList<Project>()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList<Project>();
            }

            return result;
        }
    }
}
