using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategoriesAndUser(int userId);
        void Add(Category category);
        void Update(Category category);
        Category GetById(int id);

        void Remove(int id);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);
    }
}