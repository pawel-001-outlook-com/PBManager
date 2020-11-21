using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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


        public IEnumerable<Project> GetAllProjects(string name)
        {
            using (var context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                    .Where(p => p.Name.Equals(name))
                    .ToList();
            }
        }


        public Project GetProjectByName(string name)
        {
            using (var context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows)
                    .SingleOrDefault(p => p.Name.Equals(name));
            }
        }

        public Project GetProjectById(int id)
        {
            using (var context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                    .SingleOrDefault(p => p.Id.Equals(id));
            }
        }


        public IEnumerable<Project> GetAllProjectsAndUser(int userId)
        {
            using (var context = new DataContext())
            {
                return context.Projects
                    .Where(p => p.UserId == userId)
                    .Include(p => p.Cashflows.Select(cf => cf.Subcategory))
                    .ToList();
            }
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


        public int GetFilteredCount(string searchValue)
        {
            return _dataContext.Projects
                .Where(a => a.Name.Contains(searchValue)
                )
                .Count();
        }

        public int GetTotalCount()
        {
            var totalCount = _dataContext.Projects.Count();
            return totalCount;
        }

        public List<Project> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length)
        {
            List<Project> result;
            if (sortDirection == "asc")
                result =
                    _dataContext.Projects
                        .Where(a => a.Name.Contains(searchValue)
                        )
                        .Include(a => a.Cashflows)
                        .ToList()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)) //Sort by sortColumn
                        .Skip(start)
                        .Take(length)
                        .ToList();
            else
                result =
                    _dataContext.Projects
                        .Where(a => a.Name.Contains(searchValue)
                        )
                        .Include(a => a.Cashflows)
                        .ToList()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList();

            return result;
        }
    }
}