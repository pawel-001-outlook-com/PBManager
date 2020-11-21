using System.Web.Mvc;
using PBManager.Web.Filters;

namespace PBManager.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MainExceptionFilter());
        }
    }
}