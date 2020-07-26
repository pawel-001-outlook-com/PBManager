using System.Collections.Generic;
using System.Web.Mvc;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface ISubcategoryService
    {
        IEnumerable<Subcategory> GetAll();
        Subcategory GetById(int id);
        void Add(Subcategory subcategory);
        void Update(Subcategory subcategory);
        void Delete(int id);
        void Delete(ICollection<Subcategory> subcategories);
        // IEnumerable<SelectListItem> GetCategoryAndSubcategories(string categoryId);
        IEnumerable<Subcategory> GetSubcategoriesAndUser(int userId);
        IEnumerable<SelectListItem> GetCategoryAndSubcategories(string categoryId);
    }
}