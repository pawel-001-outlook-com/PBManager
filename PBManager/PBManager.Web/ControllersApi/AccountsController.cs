using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;

namespace PBManager.Web.ControllersApi
{
    public class AccountsController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IHttpActionResult GetAccount(int id)
        {
            return Ok();
        }


        [HttpDelete]
        // [Route("api/accounts/{id}")]
        public IHttpActionResult DeleteAccount(int id)
        {
            try
            {
                _accountService.Remove(id);
                return Ok();
            }
            catch (NotFoundException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch (DbUpdateException e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}