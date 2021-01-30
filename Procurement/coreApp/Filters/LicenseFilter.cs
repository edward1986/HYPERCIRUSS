using coreApp.DAL;
using coreApp.Interfaces;
using Module.Core;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class LicenseFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (!MvcApplication.ApplicationLicense.IsValid)
            {
                filterContext.HttpContext.Response.Redirect("/License");
            }
        }

    }
}