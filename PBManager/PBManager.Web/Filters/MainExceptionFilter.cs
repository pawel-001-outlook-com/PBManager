using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using PBManager.Services.Helpers;

namespace PBManager.Web.Filters
{
    public class MainExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var v = new ViewResult();
            v.ViewName = "Error500";
            var guid = Guid.NewGuid();
            var g = guid.ToString();
            v.ViewBag.Guid = g;
            filterContext.Result = v;
            filterContext.ExceptionHandled = true;
            var statusCode = new HttpException(null, filterContext.Exception).GetHttpCode();
            filterContext.HttpContext.Response.StatusCode = statusCode;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // ContentResult cr = new ContentResult();
                // cr.Content = g;
                // cr.ContentType = "text/plain";
                // filterContext.Result = cr;

                filterContext.Result = new JsonResult
                {
                    Data = new {g},
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();

                // Added this line
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            var s = "Guid: " + g
                             + ", User Id: " + UserDataHelper.GetUserId(filterContext.HttpContext)
                             + ", User name: " + UserDataHelper.GetUserName(filterContext.HttpContext)
                             + ", Date: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                 CultureInfo.InvariantCulture)
                             + ", Message: " + filterContext.Exception.Message
                             + ", Type: " + filterContext.Exception.GetType()
                             + ", Source: " + filterContext.Exception.Source
                             + ", Stack trace: " + filterContext.Exception.StackTrace;

            var sw = File.AppendText(filterContext.RequestContext.HttpContext.Request.PhysicalApplicationPath +
                                     "\\ErrorLog.txt");
            sw.WriteLine(s);
            sw.Close();
        }
    }
}