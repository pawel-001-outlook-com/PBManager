using PBManager.Services.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PBManager.Web.Filters
{
    public class MainExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {

            ViewResult v = new ViewResult();
            v.ViewName = "Error500";
            Guid guid = Guid.NewGuid();
            string g = guid.ToString();
            v.ViewBag.Guid = g;
            filterContext.Result = v;
            filterContext.ExceptionHandled = true;
            var statusCode = new HttpException(null, filterContext.Exception).GetHttpCode();
            filterContext.HttpContext.Response.StatusCode = statusCode;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { g },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            }

            string s = "Guid: " + g
                                + ", User Id: " + UserDataHelper.GetUserId(filterContext.HttpContext).ToString()
                                + ", User name: " + UserDataHelper.GetUserName(filterContext.HttpContext).ToString()
                                + ", Date: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
                                + ", Message: " + filterContext.Exception.Message
                                + ", Type: " + filterContext.Exception.GetType().ToString()
                                + ", Source: " + filterContext.Exception.Source
                                + ", Stack trace: " + filterContext.Exception.StackTrace;

            StreamWriter sw = File.AppendText(filterContext.RequestContext.HttpContext.Request.PhysicalApplicationPath + "\\ErrorLog.txt");
            sw.WriteLine(s);
            sw.Close();

        }
    }
}
