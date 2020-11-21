using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;

namespace PBManager.Web.ControllersApi
{
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _categoryService.Remove(id);
                return Ok();
            }
            catch (NotFoundException e)
            {
                return Content(HttpStatusCode.NotFound, "");
            }
            catch (DbUpdateException e)
            {
                return Content(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}