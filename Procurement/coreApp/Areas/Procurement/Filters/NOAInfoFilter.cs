using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using Module.Core;

namespace coreApp.Areas.Procurement.Filters
{

    public class NOAInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "noaId");
                
                using(procurementDataContext context = new procurementDataContext())
                {
                    tblNOA noa = context.tblNOAs.Where(x => x.Id == int.Parse(v)).SingleOrDefault();
                    if (noa == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.NOA = noa;
                        ((INOAController)filterContext.Controller).NOA = noa;
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