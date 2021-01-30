using coreApp.Areas.Procurement.Interfaces;
using coreApp.DAL;
using coreApp.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.Filters
{

    public class YearInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "year");
                int year = DateTime.Today.Year;
                
                if (v != null && v != "-1")
                {
                    year = int.Parse(v);
                }

                filterContext.Controller.ViewBag.Year = year;
                ((IYearController)filterContext.Controller).Year = year;

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