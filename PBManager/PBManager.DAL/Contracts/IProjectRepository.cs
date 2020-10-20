using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAllProjects();
        IEnumerable<Project> GetAllProjects(string name);
        IEnumerable<Project> GetAllActiveProjects();
        Project GetProjectByName(string name);
        Project GetProjectById(int id);
        // void Insert(Project project);
        void Update(Project project);
        IEnumerable<Project> GetAllProjectsAndUser(int userId);
        void Remove(Project project);
        void Add(Project project);
        void Delete(Project project);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);

        List<Project> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length);
    }
}