using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;
using PBManager.Services.Contracts;
using Unity.Attributes;


namespace PBManager.Services.Helpers
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;


        public CategoryService(){}

        // [InjectionConstructor]
        // public CategoryService(ICategoryRepository categoryRepository, ISubcategoryService subcategoryService)
        // {
        //     _categoryRepository = categoryRepository;
        //     _subcategoryService = subcategoryService;
        // }

        [InjectionConstructor]
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Category> GetAll()
        {
            return _unitOfWork.categories.GetCategories();
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
            else
                throw new Exception($"Already exists a category with name {category.Name}");
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
                else
                    throw new Exception($"Already exists a {category.Type} category with name {category.Name}");
        }


        public Category GetById(int id)
        {
            var category = _unitOfWork.categories.GetCategoryById(id);

            if (category != null)
                return category;
            else
                throw new Exception("This category not exists");
        }


        public Category GetByName(string name, string type)
        {
            var category = _unitOfWork.categories.GetCategoryByName(name, type);

            if (category != null)
                return category;
            else
                throw new Exception("This category not exists");
        }


        public void Remove(int id)
        {
            var category = GetById(id);
                _unitOfWork.subcategories.Delete(category.Subcategories);
                _unitOfWork.categories.Delete(category);
                _unitOfWork.Complete();
        }

    }


}
