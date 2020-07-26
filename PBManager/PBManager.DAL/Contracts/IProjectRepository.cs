using System.Collections.Generic;
using PBManager.Core.Models;

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
    }
}