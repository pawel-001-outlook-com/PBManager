using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;

namespace PBManager.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public Category GetCategoryById(int id)
        {
            var category = _dataContext.Categories
                .AsNoTracking()
                .Include(c => c.Subcategories)
                .Include(c => c.Cashflows)
                .ToList()
                .SingleOrDefault(c => c.Id.Equals(id));

            if (category != null)
                category.Subcategories = category.Subcategories.ToList();

            return category;
        }


        public Category GetCategoryByName(string name, string type)
        {
            return _dataContext.Categories
                .AsNoTracking()
                .Include(c => c.Subcategories)
                .Include(c => c.Cashflows)
                .SingleOrDefault(c => c.Name.Equals(name) && c.Type.Equals(type));
        }


        public ICollection<Category> GetCategoriesByName(string name)
        {
            return _dataContext.Categories
                .AsNoTracking()
                .Include(c => c.Subcategories)
                .Where(c => c.Name.Equals(name))
                .ToList();
        }


        public IEnumerable<Category> GetCategoriesAndUser(int userId)
        {
            var categories = _dataContext.Categories
                .AsNoTracking()
                .Where(c => c.UserID == userId)
                .Include(c => c.Subcategories)
                .Include(c => c.Cashflows)
                .ToList();

            foreach (var category in categories)
                category.Subcategories = category.Subcategories.ToList();

            return categories;
        }


        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _dataContext.Entry(category).State = EntityState.Modified;
        }

        public void Delete(Category category)
        {
            _dataContext.Entry(category).State = EntityState.Deleted;
        }


        public int GetTotalCount()
        {
            var totalCount = _dataContext.Categories.Count();
            return totalCount;
        }

        public int GetFilteredCount(string searchValue)
        {
            IQueryable<Category> query = _dataContext.Categories;

            return GetFilteredQuery(_dataContext.Categories, searchValue).Count();
        }


        public List<Category> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length)
        {
            List<Category> result;
            if (sortDirection == "asc")
                result =
                    _dataContext.Categories
                        .Include(a => a.Subcategories)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Subcategories.Any(s => s.Name.Contains(searchValue))
                        )
                        .ToList()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)) //Sort by sortColumn
                        .Skip(start)
                        .Take(length)
                        .ToList();
            else
                result =
                    _dataContext.Categories
                        .Include(a => a.Subcategories)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Subcategories.Any(s => s.Name.Contains(searchValue))
                        )
                        .ToList()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList();

            return result;
        }


        protected IQueryable<Category> GetFilteredQuery(IQueryable<Category> queryable, string searchValue)
        {
            return queryable.Where(x =>
                x.Name.Contains(searchValue));
        }
    }
}