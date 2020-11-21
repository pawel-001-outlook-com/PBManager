using System.Web.Mvc;

namespace PBManager.Web.Filters
{
    public class AdminFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.IsInRole("Admin") == false)
                filterContext.Result = new HttpStatusCodeResult(403);
        }
    }
}