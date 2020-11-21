using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name, string type);
        ICollection<Category> GetCategoriesByName(string name);

        void Update(Category category);

        IEnumerable<Category> GetCategoriesAndUser(int userId);
        void Add(Category category);
        void Delete(Category category);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);

        List<Category> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start,
            int length);
    }
}