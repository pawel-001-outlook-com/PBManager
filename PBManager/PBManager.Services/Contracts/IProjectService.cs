using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.Services.Contracts
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAllProjectsAndUser(int userId);
        void Add(Project project);
        void Update(Project project);
        void Delete(int id);
        Project GetProjectById(int id);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);
        List<Project> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId);
    }
}