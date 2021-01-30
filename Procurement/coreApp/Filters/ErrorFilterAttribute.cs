using System.Web.Mvc;

namespace coreApp.Filters
{
    public class ErrorFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                filterContext.HttpContext.Session["ApplicationErrorMessage"] = coreProcs.ShowErrors(filterContext.Exception);
                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    filterContext.HttpContext.Session["error_backurl"] = filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri;
                }
                filterContext.RequestContext.HttpContext.Response.Redirect("/Error");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

    }
}