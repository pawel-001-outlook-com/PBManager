using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;

namespace PBManager.Web.ControllersApi
{
    public class ProjectsController : ApiController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _projectService.Delete(id);
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