using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name, string type);
        ICollection<Category> GetCategoriesByName(string name);
        // void Insert(Category category);
        void Update(Category category);
        // void Remove(Category category);
        IEnumerable<Category> GetCategoriesAndUser(int userId);
        void Add(Category category);
        void Delete(Category category);
    }
}