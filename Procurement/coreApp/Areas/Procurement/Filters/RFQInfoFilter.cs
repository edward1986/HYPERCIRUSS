using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using Module.Core;

namespace coreApp.Areas.Procurement.Filters
{

    public class RFQInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "rfqId");
                
                using(procurementDataContext context = new procurementDataContext())
                {
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == int.Parse(v)).SingleOrDefault();
                    if (rfq == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.RFQ = rfq;
                        ((IRFQController)filterContext.Controller).RFQ = rfq;
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