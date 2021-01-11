using PBManager.Services.Contracts;
using System.Web.Http;

namespace PBManager.Web.ControllersApi
{
    public class ProjectsController : ApiController
    {
        private IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {

            _projectService = projectService;
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _projectService.Delete(id);
            return Ok();
        }
    }
}
