using coreApp.DAL;
using coreApp.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class MyFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                tblEmployee employee = Cache.Get().userAccess.employee;

                filterContext.Controller.ViewBag.Employee = employee;
                ((IMyController)filterContext.Controller).employee = employee;

                base.OnActionExecuting(filterContext);
            }
            catch(Exception e)
            {
                filterContext.HttpContext.Session["ApplicationErrorMessage"] = coreProcs.ShowErrors(e);
                filterContext.Result = new RedirectResult("/Error");
            }
        }

    }
}