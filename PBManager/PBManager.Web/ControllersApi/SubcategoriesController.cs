using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;

namespace PBManager.Web.ControllersApi
{
    public class SubcategoriesController : ApiController
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoriesController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _subcategoryService.Delete(id);
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