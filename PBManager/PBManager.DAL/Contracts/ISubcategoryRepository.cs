using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface ISubcategoryRepository
    {
        IEnumerable<Subcategory> GetSubcategories();
        IEnumerable<Subcategory> GetSubcategoriesByName(string name, int baseCategoryId);
        Subcategory GetSubcategoryById(int id);

        Subcategory GetSubcategoryByName(string name, int baseCategoryId);
        void Update(Subcategory subcategory);

        void Add(Subcategory subcategory);
        void Delete(ICollection<Subcategory> subcategories);
        void Delete(Subcategory subcategory);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);
    }
}