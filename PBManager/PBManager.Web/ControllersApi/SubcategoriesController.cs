using PBManager.Services.Contracts;
using System.Web.Http;

namespace PBManager.Web.ControllersApi
{
    public class SubcategoriesController : ApiController
    {
        private ISubcategoryService _subcategoryService;

        public SubcategoriesController(ISubcategoryService subcategoryService)
        {

            _subcategoryService = subcategoryService;
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _subcategoryService.Delete(id);
            return Ok();
        }
    }
}
