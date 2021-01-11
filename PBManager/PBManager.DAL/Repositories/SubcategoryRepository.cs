using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PBManager.DAL.Repositories
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly DataContext _dataContext;

        public SubcategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IEnumerable<Subcategory> GetSubcategories()
        {
            return _dataContext.Subcategories
                .AsNoTracking()
                    .Include(s => s.Category)
                    .ToList();
        }


        public IEnumerable<Subcategory> GetSubcategoriesByName(string name, int baseCategoryId)
        {
            return _dataContext.Subcategories
                .Include(s => s.Category)
                .Where(s => s.Name.Equals(name) && s.Category.Id.Equals(baseCategoryId))
                .ToList();
        }


        public Subcategory GetSubcategoryById(int id)
        {
            return _dataContext.Subcategories.AsNoTracking()
                .Include(s => s.Category)
                .Include(s => s.Cashflows)
                .FirstOrDefault(s => s.Id.Equals(id));
        }


        public Subcategory GetSubcategoryByName(string name, int baseCategoryId)
        {
            return _dataContext.Subcategories
                    .Include(s => s.Category)
                    .Where(s => s.Category.Id.Equals(baseCategoryId) && s.Name.Equals(name)).
                    FirstOrDefault();
        }


        public void Add(Subcategory subcategory)
        {
            _dataContext.Subcategories.Add(subcategory);
        }


        public void Update(Subcategory subcategory)
        {
            _dataContext.Entry(subcategory).State = EntityState.Modified;
        }


        public void Delete(Subcategory subcategory)
        {
            _dataContext.Entry(subcategory).State = EntityState.Deleted;
        }


        public void Delete(ICollection<Subcategory> subcategories)
        {
            Subcategory s;
            List<Subcategory> l = subcategories.ToList();
            int n = l.Count;
            for (int i = 0; i < n; i++)
            {
                s = l[i];
                _dataContext.Entry(s).State = EntityState.Deleted;
            }
        }


        public int GetTotalCount(int userId)
        {
            int totalCount = _dataContext.Subcategories
                .Include(a => a.Category)
                .Where(a => a.Category.UserID.Equals(userId))
                .Count();
            return totalCount;
        }

        public int GetFilteredCount(string searchValue, int userId)
        {
            IQueryable<Subcategory> query = _dataContext.Subcategories;

            return GetFilteredQuery(_dataContext.Subcategories, searchValue, userId).Count();
        }


        protected IQueryable<Subcategory> GetFilteredQuery(IQueryable<Subcategory> queryable, string searchValue, int userId)
        {
            return queryable
                .Include(a => a.Category)
                .Where(a => a.Category.UserID.Equals(userId))
                .Where(x =>
                x.Name.Contains(searchValue));
        }

        public List<Subcategory> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length, string userId)
        {
            List<Subcategory> result;
            int userIdInt = Convert.ToInt32(userId);
            if (sortDirection == "asc")
            {
                result =
                    _dataContext.Subcategories
                        .Include(a => a.Category)
                        .Where(a => a.Category.UserID.Equals(userIdInt))
                        .Where(a => a.Name.Contains(searchValue)
                                || a.Category.Name.Equals(searchValue)
                        )
                        .ToList<Subcategory>()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))//Sort by sortColumn
                        .Skip(start)
                        .Take(length)
                        .ToList<Subcategory>();
            }
            else
            {
                result =
                    _dataContext.Subcategories
                        .Include(a => a.Category)
                        .Where(a => a.Category.UserID.Equals(userIdInt))
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Category.Name.Equals(searchValue)
                        )
                        .ToList<Subcategory>()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList<Subcategory>();
            }

            return result;
        }

    }
}
