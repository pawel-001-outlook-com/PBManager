using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Attributes;


namespace PBManager.Services.Helpers
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;


        public CategoryService() { }

        [InjectionConstructor]
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Category> GetCategoriesAndUser(int userId)
        {
            return _unitOfWork.categories.GetCategoriesAndUser(userId);
        }


        public void Add(Category category)
        {

            var nameExists = _unitOfWork.categories.GetCategoryByName(category.Name, category.Type) != null;

            if (!nameExists)
            {
                _unitOfWork.categories.Add(category);
                _unitOfWork.Complete();
            }
        }


        public void Update(Category category)
        {
            var currentCategory = GetById(category.Id);

            var quantity = (_unitOfWork.categories.GetCategoriesByName(category.Name)).Count(c => !c.Id.Equals(currentCategory.Id));

            if (quantity.Equals(0))
            {
                _unitOfWork.categories.Update(category);
                _unitOfWork.Complete();
            }

        }


        public Category GetById(int id)
        {
            var category = _unitOfWork.categories.GetCategoryById(id);

            if (category != null)
                return category;
            throw new Exception("no account by Id");
        }


        public void Remove(int id)
        {
            var category = GetById(id);

            _unitOfWork.subcategories.Delete(category.Subcategories);
            _unitOfWork.categories.Delete(category);
            _unitOfWork.Complete();
        }


        public int GetTotalCount(int userId)
        {
            return _unitOfWork.categories.GetTotalCount(userId);
        }


        public int GetFilteredCount(string searchValue, int userId)
        {
            return _unitOfWork.categories.GetFilteredCount(searchValue, userId);
        }


        public List<Category> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId)
        {
            List<Category> a = _unitOfWork.categories.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length, userId);
            return a;
        }


        public IEnumerable<Category> GetCategoriesByAccount(string accountId, int userId)
        {
            try
            {
                int accountIdInt = Convert.ToInt32(accountId);
                if (accountId != null)
                {
                    List<Category> list = _unitOfWork.categories.GetCategoriesByAccount(accountIdInt, userId);
                    return list;
                }
                else
                {
                    return new List<Category>();

                }
            }
            catch
            {
                return new List<Category>();
            }
        }

    }


}
