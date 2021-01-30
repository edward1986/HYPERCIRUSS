using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using Module.Core;

namespace coreApp.Areas.Procurement.Filters
{

    public class CompanyPRInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "prId");
                
                using(procurementDataContext context = new procurementDataContext())
                {
                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == int.Parse(v)).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.PR = pr;
                        ((ICompPRController)filterContext.Controller).PR = pr;
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