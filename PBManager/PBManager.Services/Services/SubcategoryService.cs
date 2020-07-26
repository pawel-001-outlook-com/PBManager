using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PBManager.Core.Models;
using PBManager.DAL;
using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;
using PBManager.Services.Contracts;

namespace PBManager.Services.Helpers
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubcategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Subcategory> GetAll()
        {
            return _unitOfWork.subcategories.GetSubcategories();
        }


        public Subcategory GetById(int id)
        {
            var subcategory = _unitOfWork.subcategories.GetSubcategoryById(id);

            if (subcategory != null)
                return subcategory;
            else
                throw new Exception("This subcategory not exist");
        }


        public void Add(Subcategory subcategory)
        {
            var nameExists = (_unitOfWork.subcategories.GetSubcategoryByName(subcategory.Name, subcategory.CategoryId)) != null;

            if (!nameExists)
            {
                _unitOfWork.subcategories.Add(subcategory);
                _unitOfWork.Complete();
            }
            else
                throw new Exception($"Already exists a subcategory {subcategory.Name} in this category");
        }


        public void Update(Subcategory subcategory)
        {
            var currentSubcategory = GetById(subcategory.Id);

            if (true)
            {
                var quantity = (_unitOfWork.subcategories
                    .GetSubcategoriesByName(subcategory.Name, subcategory.CategoryId))
                    .Count(s => !s.Id.Equals(currentSubcategory.Id));

                if (quantity.Equals(0))
                {
                    _unitOfWork.subcategories.Update(subcategory);
                    _unitOfWork.Complete();
                }
                else
                    throw new Exception($"Already exists a subcategory {subcategory.Name} in this category");
            }
        }


        public void Delete(int id)
        {
            var subcategory = GetById(id);

                _unitOfWork.subcategories.Delete(subcategory);
                _unitOfWork.Complete();
        }


        public void Delete(ICollection<Subcategory> subcategories)
        {
            subcategories.ToList();

            _unitOfWork.subcategories.Delete(subcategories);

        }


        public IEnumerable<SelectListItem> GetCategoryAndSubcategories(string categoryId)
        {
            if (!String.IsNullOrWhiteSpace(categoryId))
            {
                    var categoryIdAsInt = Int32.Parse(categoryId);
                    IEnumerable<Subcategory> subcategories = _unitOfWork.subcategories.GetSubcategories()
                        .OrderBy(n => n.Name)
                        .Where(n => n.CategoryId == categoryIdAsInt)
                        .ToList();
                    return new SelectList(subcategories, "Id", "Name");
            }
            return null;
        }


        public IEnumerable<Subcategory> GetSubcategoriesAndUser(int userId)
        {
            return _unitOfWork.subcategories.GetSubcategoriesAndUser(userId);
        }


    }
}
