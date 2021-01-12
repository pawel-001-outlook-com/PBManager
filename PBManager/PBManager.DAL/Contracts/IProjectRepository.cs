using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.DAL.Contracts
{
    public interface IProjectRepository
    {

        IEnumerable<Project> GetAllProjects(string name);

        Project GetProjectByName(string name);
        Project GetProjectById(int id);

        void Update(Project project);
        IEnumerable<Project> GetAllProjectsAndUser(int userId);

        void Add(Project project);
        void Delete(Project project);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);

        List<Project> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId);
    }
}