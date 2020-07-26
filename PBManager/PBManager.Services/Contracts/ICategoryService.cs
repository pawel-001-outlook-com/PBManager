using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        IEnumerable<Category> GetCategoriesAndUser(int userId);
        void Add(Category category);
        void Update(Category category);
        Category GetById(int id);
        Category GetByName(string name, string type);
        // double CategoryBalance(IEnumerable<Cashflow> cashflows);

        void Remove(int id);
    }
}