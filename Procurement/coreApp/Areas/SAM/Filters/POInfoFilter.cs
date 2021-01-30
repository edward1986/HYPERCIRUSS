using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class POInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "poId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        tblPO po = context.tblPOs.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (po == null)
                        {
                            throw new Exception("P.O. Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.POs = context.tblPOs.OrderBy(x => x.PONo).ToList();
                            filterContext.Controller.ViewBag.PO = po;
                            ((IPOController)filterContext.Controller).PO = po;
                        }
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