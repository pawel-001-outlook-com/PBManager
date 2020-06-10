using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PBManager.Core.Models;

namespace PBManager.DAL.Repositories
{
    public class ProjectRepository
    {
        public IEnumerable<Project> GetAllProjects()
        {
            using (DataContext context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows)
                    .Include("Movements.Category")
                    .Where(p => p.Enabled).ToList();
            }
        }
        
        public IEnumerable<Project> GetAllProjects(string name)
        {
                using (DataContext context = new DataContext())
                {
                    return context.Projects
                        .Include(p => p.Cashflows)
                        .Include("Movements.Category")
                        .Where(p => p.Name.Equals(name) && p.Enabled).ToList();

                }
        }

        public IEnumerable<Project> GetAllActiveProjects()
        {
            using (DataContext context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows)
                    .Include("Cashflow.Category")
                    .ToList();
            }
        }

        public Project GetProjectByName(string name)
        {
            using (DataContext context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows)
                    .SingleOrDefault(p => p.Name.Equals(name));
            }
        }

        public Project GetProjectById(int id)
        {
            using (DataContext context = new DataContext())
            {
                return context.Projects
                    .Include(p => p.Cashflows)
                    .Include("Cashflows.Category")
                    .SingleOrDefault(p => p.Id.Equals(id));
            }
        }

        public void Insert(Project project)
        {
            using (DataContext context = new DataContext())
            {
                context.Projects.Add(project);
                context.SaveChanges();
            }
        }

        public void Update(Project project)
        {
            using (DataContext context = new DataContext())
            {
                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
