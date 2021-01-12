using PBManager.Core.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PBManager.Services.Contracts
{
    public interface ISubcategoryService
    {
        Subcategory GetById(int id);
        void Add(Subcategory subcategory);
        void Update(Subcategory subcategory);
        void Delete(int id);


        IEnumerable<SelectListItem> GetCategoryAndSubcategories(string categoryId);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);

        List<Subcategory> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId);
    }
}