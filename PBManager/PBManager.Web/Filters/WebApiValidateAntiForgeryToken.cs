using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace PBManager.Web.Filters
{
    public class WebApiValidateAntiForgeryToken : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> tokenHeaders = filterContext.Request
                                                                .Headers
                                                                .GetValues("RequestVerificationToken");
            if (tokenHeaders != null && tokenHeaders.Count() > 0)
            {
                string[] tokens = tokenHeaders.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }

                System.Web.Helpers.AntiForgery.Validate(cookieToken, formToken);
            }


        }
    }
}