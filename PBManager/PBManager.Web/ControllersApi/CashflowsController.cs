using PBManager.Services.Contracts;
using System.Web.Http;

namespace PBManager.Web.ControllersApi
{

    [AllowAnonymous]
    public class CashflowsController : ApiController
    {
        private ICashflowService _cashflowService;

        public CashflowsController(ICashflowService cashflowService)
        {

            _cashflowService = cashflowService;
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _cashflowService.Delete(id);
            return Ok();
        }
    }
}
