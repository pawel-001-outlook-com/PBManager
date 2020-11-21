﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;
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


        public Subcategory GetById(int id)
        {
            var subcategory = _unitOfWork.subcategories.GetSubcategoryById(id);

            if (subcategory != null)
                return subcategory;
            throw new Exception("This subcategory not exist");
        }


        public void Add(Subcategory subcategory)
        {
            var nameExists = _unitOfWork.subcategories.GetSubcategoryByName(subcategory.Name, subcategory.CategoryId) !=
                             null;

            if (!nameExists)
            {
                _unitOfWork.subcategories.Add(subcategory);
                _unitOfWork.Complete();
            }
            else
            {
                throw new Exception($"Already exists a subcategory {subcategory.Name} in this category");
            }
        }


        public void Update(Subcategory subcategory)
        {
            var currentSubcategory = GetById(subcategory.Id);

            if (true)
            {
                var quantity = _unitOfWork.subcategories
                    .GetSubcategoriesByName(subcategory.Name, subcategory.CategoryId)
                    .Count(s => !s.Id.Equals(currentSubcategory.Id));

                if (quantity.Equals(0))
                {
                    _unitOfWork.subcategories.Update(subcategory);
                    _unitOfWork.Complete();
                }
                else
                {
                    throw new Exception($"Already exists a subcategory {subcategory.Name} in this category");
                }
            }
            else
            {
                throw new Exception("This subcategory was generated by other routine and cannot be changed");
            }
        }


        public void Delete(int id)
        {
            var subcategory = GetById(id);

            _unitOfWork.subcategories.Delete(subcategory);
            _unitOfWork.Complete();
        }


        public IEnumerable<SelectListItem> GetCategoryAndSubcategories(string categoryId)
        {
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                var categoryIdAsInt = int.Parse(categoryId);
                IEnumerable<Subcategory> subcategories = _unitOfWork.subcategories.GetSubcategories()
                    .OrderBy(n => n.Name)
                    .Where(n => n.CategoryId == categoryIdAsInt)
                    .ToList();
                return new SelectList(subcategories, "Id", "Name");
            }

            return null;
        }


        public int GetTotalCount()
        {
            return _unitOfWork.subcategories.GetTotalCount();
        }

        public int GetFilteredCount(string searchValue)
        {
            return _unitOfWork.subcategories.GetFilteredCount(searchValue);
        }
    }
}