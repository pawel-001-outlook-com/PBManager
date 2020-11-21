using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions; // using System.Web.Mvc;

namespace PBManager.Web.ControllersApi
{
    [AllowAnonymous]
    public class CashflowsController : ApiController
    {
        private readonly ICashflowService _cashflowService;

        public CashflowsController(ICashflowService cashflowService)
        {
            _cashflowService = cashflowService;
        }


        [HttpDelete]
        // DELETE: api/Cashflow/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _cashflowService.Delete(id);
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