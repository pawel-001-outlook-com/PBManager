using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAllProjectsAndUser(int userId);
        void Add(Project project);
        void Update(Project project);
        void Delete(int id);
        Project GetProjectById(int id);
    }
}