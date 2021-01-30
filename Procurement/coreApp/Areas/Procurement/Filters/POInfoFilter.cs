using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using Module.Core;

namespace coreApp.Areas.Procurement.Filters
{

    public class POInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "poId");
                
                using(procurementDataContext context = new procurementDataContext())
                {
                    tblPO po = context.tblPOs.Where(x => x.Id == int.Parse(v)).SingleOrDefault();
                    if (po == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.PO = po;
                        ((IPOController)filterContext.Controller).PO = po;
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