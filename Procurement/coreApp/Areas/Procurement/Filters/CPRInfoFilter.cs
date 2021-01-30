using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using Module.Core;

namespace coreApp.Areas.Procurement.Filters
{

    public class CPRInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "cprId");
                
                using(procurementDataContext context = new procurementDataContext())
                {
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == int.Parse(v)).SingleOrDefault();
                    if (cpr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.CPR = cpr;
                        ((ICPRController)filterContext.Controller).CPR = cpr;
                    }
                }

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