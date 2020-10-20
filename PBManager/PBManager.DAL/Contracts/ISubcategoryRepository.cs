using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface ISubcategoryRepository
    {
        IEnumerable<Subcategory> GetSubcategories();
        IEnumerable<Subcategory> GetSubcategoriesByName(string name, int baseCategoryId);
        Subcategory GetSubcategoryById(int id);
        Subcategory GetSubcategoryByName(string name);
        Subcategory GetSubcategoryByName(string name, int baseCategoryId);
        // void Insert(Subcategory subcategory);
        void Update(Subcategory subcategory);
        void Update(ICollection<Subcategory> subcategories);
        // void Remove(Subcategory subcategory);
        // void Remove(ICollection<Subcategory> subcategories);
        IEnumerable<Subcategory> GetSubcategoriesAndUser(int userId);
        void Add(Subcategory subcategory);
        void Delete(ICollection<Subcategory> subcategories);
        void Delete(Subcategory subcategory);
    }
}