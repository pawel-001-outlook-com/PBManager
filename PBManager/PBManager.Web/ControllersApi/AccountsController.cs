using PBManager.Services.Contracts;
using PBManager.Web.Filters;
using System.Web.Http;

namespace PBManager.Web.ControllersApi
{
    [Authorize]
    public class AccountsController : ApiController
    {
        private IAccountService _accountService;

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
        [WebApiValidateAntiForgeryToken]
        public IHttpActionResult DeleteAccount(int id)
        {
            _accountService.Remove(id);
            return Ok();
        }
    }
}


