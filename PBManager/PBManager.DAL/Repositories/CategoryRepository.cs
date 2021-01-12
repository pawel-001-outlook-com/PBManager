using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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


        public int GetTotalCount(int userId)
        {
            int totalCount = _dataContext.Categories
                .Where(a => a.UserID.Equals(userId))
                .Count();
            return totalCount;
        }

        public int GetFilteredCount(string searchValue, int userId)
        {
            IQueryable<Category> query = _dataContext.Categories;

            return GetFilteredQuery(_dataContext.Categories, searchValue, userId).Count();
        }


        protected IQueryable<Category> GetFilteredQuery(IQueryable<Category> queryable, string searchValue, int userId)
        {
            return queryable
                .Where(a => a.UserID.Equals(userId))
                .Where(x =>
                x.Name.Contains(searchValue));
        }


        public List<Category> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length, string userId)
        {
            List<Category> result;

            int userIdInt = Convert.ToInt32(userId);

            if (sortDirection == "asc")
            {
                result =
                    _dataContext.Categories
                        .Where(u => u.UserID.Equals(userIdInt))
                        .Include(a => a.Subcategories)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Subcategories.Any(s => s.Name.Contains(searchValue))
                                    )
                        .ToList<Category>()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList<Category>();
            }
            else
            {
                result =
                    _dataContext.Categories
                        .Where(u => u.UserID.Equals(userIdInt))
                        .Include(a => a.Subcategories)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Subcategories.Any(s => s.Name.Contains(searchValue))
                        )
                        .ToList<Category>()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList<Category>();
            }

            return result;
        }

        public List<Category> GetCategoriesByAccount(int accountIdInt, int userId)
        {
            var result = _dataContext
                .Categories
                .Include(c => c.Cashflows.Select(cf => cf.Account))
                .Where(c => c.Cashflows.Any(cf => cf.Account.UserId.Equals(userId)))
                .ToList<Category>();

            return result;
        }
    }
}
