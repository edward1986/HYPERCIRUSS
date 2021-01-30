using System;
using System.Web.Mvc;

namespace coreApp.Filters
{
    public class RateSettingsFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string controller = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

                if (FixedSettings.RatesBySalaryGrade && controller == "Rates")
                {
                    throw new Exception("Position > Rates module is disabled. Rates are determined by salary grades");
                }

                if (!FixedSettings.RatesBySalaryGrade && controller == "SalaryGrades")
                {
                    throw new Exception("SalaryGrades module is disabled. Rates are determined by Position > Rates");
                }

                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                filterContext.HttpContext.Session["ApplicationErrorMessage"] = coreProcs.ShowErrors(e);
                filterContext.Result = new RedirectResult("/Error");
            }
        }

    }
}