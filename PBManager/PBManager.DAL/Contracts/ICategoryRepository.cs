using PBManager.Core.Models;
using System.Collections.Generic;

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
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);
        List<Category> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length, string userId);
        List<Category> GetCategoriesByAccount(int accountIdInt, int userId);
    }
}