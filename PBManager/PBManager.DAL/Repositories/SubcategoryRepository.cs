using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;

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


        public Subcategory GetSubcategoryByName(string name)
        {
            return _dataContext.Subcategories
                .Where(s => s.Name.Equals(name))
                .FirstOrDefault();
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


        public void Update(ICollection<Subcategory> subcategories)
        {
            foreach (var subcategory in subcategories)
            {
                _dataContext.Entry(subcategory).State = EntityState.Modified;
            }
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


        public IEnumerable<Subcategory> GetSubcategoriesAndUser(int userId)
        {
                return _dataContext.Subcategories
                    .Include(s => s.Category)
                    .Include(s => s.Cashflows)
                    .Where(s => s.Category.UserID == userId)
                    .ToList();
        }
        
    }
}
