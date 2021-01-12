using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.Services.Contracts
{
    public interface ICategoryService
    {

        IEnumerable<Category> GetCategoriesAndUser(int userId);
        void Add(Category category);
        void Update(Category category);
        Category GetById(int id);

        void Remove(int id);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);

        List<Category> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId);

        IEnumerable<Category> GetCategoriesByAccount(string accountId, int userId);
    }
}