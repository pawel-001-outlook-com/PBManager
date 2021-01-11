using AutoMapper;
using PBManager.Core.Models;
using PBManager.Dto.Dtos;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using PBManager.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PBManager.Web.ControllersApi
{
    public class CategoriesController : ApiController
    {
        private ICategoryService _categoryService;
        private IAccountService _accountService;

        public CategoriesController(ICategoryService categoryService, IAccountService accountService)
        {

            _categoryService = categoryService;
            _accountService = accountService;
        }

        [HttpGet]
        [WebApiValidateAntiForgeryToken]
        public IHttpActionResult Get(string accountId, int userId)
        {
            var a = _accountService.GetById(Convert.ToInt32(accountId)).User.Id.Equals(userId);
            if (_accountService.GetById(Convert.ToInt32(accountId)).User.Id.Equals(userId)) ;
            if (true)
            {
                if (!string.IsNullOrWhiteSpace(accountId))
                {
                    IEnumerable<Category> items = _categoryService
                                                    .GetCategoriesByAccount(accountId, UserDataHelper.GetUserId(HttpContext.Current));
                    return Json(items.Select(Mapper.Map<Category, CategoryDto>));
                }
                return BadRequest();
            }
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _categoryService.Remove(id);
            return Ok();
        }
    }
}
