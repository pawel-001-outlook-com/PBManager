using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PBManager.Core.Models;

namespace PBManager.DAL.Repositories
{
    public class SubcategoryRepository
    {
        public IEnumerable<Subcategory> GetSubcategories()
        {
            using (DataContext context = new DataContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                    .ToList();
            }
        }

        public IEnumerable<Subcategory> GetSubcategoriesByName(string name, int baseCategoryId)
        {
            using (DataContext context = new DataContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                    .Where(s => s.Name.Equals(name) && s.Category.Id.Equals(baseCategoryId)).ToList();
            }
        }

        public Subcategory GetSubcategoryById(int id)
        {
            using (DataContext context = new DataContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                    .Include(s => s.Cashflows)
                    .FirstOrDefault(s => s.Id.Equals(id));
            }
        }

        public async Task<Subcategory> GetSubcategoryByName(string name)
        {
            using (DataContext context = new DataContext())
            {
                return context.Subcategories.Where(s => s.Name.Equals(name)).FirstOrDefault();
            }
        }

        public Subcategory GetSubcategoryByName(string name, int baseCategoryId)
        {
            using (DataContext context = new DataContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                    .Where(s => s.Category.Id.Equals(baseCategoryId) && s.Name.Equals(name)).
                    FirstOrDefault();
            }
        }

        public void Insert(Subcategory subcategory)
        {
            using (DataContext context = new DataContext())
            {
                context.Subcategories.Add(subcategory);
                context.SaveChanges();
            }
        }

        public void Update(Subcategory subcategory)
        {
            using (DataContext context = new DataContext())
            {
                context.Entry(subcategory).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Update(ICollection<Subcategory> subcategories)
        {
            using (DataContext context = new DataContext())
            {
                foreach (var subcategory in subcategories)
                    context.Entry(subcategory).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
