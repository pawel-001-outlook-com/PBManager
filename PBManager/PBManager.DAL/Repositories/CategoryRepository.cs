using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



        // public IEnumerable<Category> GetCategories()
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         var categories = context.Categories
        //             .Include(c => c.Subcategories)
        //             .Include(c => c.Cashflows)
        //             .ToList();
        //
        //         foreach (var category in categories)
        //             category.Subcategories = category.Subcategories.ToList();
        //
        //         return categories;
        //     }
        // }

        public IEnumerable<Category> GetCategories()
        {
            var categories = _dataContext.Categories
                .AsNoTracking()
                .Include(c => c.Subcategories)
                .Include(c => c.Cashflows)
                .ToList();

            foreach (var category in categories)
                category.Subcategories = category.Subcategories.ToList();

            return categories;
        }



        // public Category GetCategoryById(int id)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         var category = context.Categories
        //             .Include(c => c.Subcategories)
        //             .Include(c => c.Cashflows)
        //             .ToList()
        //             .SingleOrDefault(c => c.Id.Equals(id));
        //
        //         if (category != null)
        //             category.Subcategories = category.Subcategories.ToList();
        //
        //         return category;
        //     }
        // }


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




        // public Category GetCategoryByName(string name, string type)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         return context.Categories
        //             .Include(c => c.Subcategories)
        //             .Include(c => c.Cashflows)
        //             .SingleOrDefault(c => c.Name.Equals(name) && c.Type.Equals(type));
        //     }
        // }


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

        // public void Insert(Category category)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Categories.Add(category);
        //         context.SaveChanges();
        //     }
        // }

        // public void Update(Category category)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Entry(category).State = EntityState.Modified;
        //         context.SaveChanges();
        //     }
        // }

        // public void Remove(Category category)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Entry(category).State = EntityState.Deleted;
        //         context.SaveChanges();
        //     }
        // }

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

    }
}
