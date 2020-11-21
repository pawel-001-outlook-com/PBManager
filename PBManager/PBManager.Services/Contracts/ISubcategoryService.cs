using System.Collections.Generic;
using System.Web.Mvc;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface ISubcategoryService
    {
        Subcategory GetById(int id);
        void Add(Subcategory subcategory);
        void Update(Subcategory subcategory);
        void Delete(int id);


        IEnumerable<SelectListItem> GetCategoryAndSubcategories(string categoryId);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);
    }
}