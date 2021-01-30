using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class RISInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "risId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        List<tblRI> ris = context.tblRIs.ToList();
                        tblRI ri = ris.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (ri == null)
                        {
                            throw new Exception("RIS Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.RIS = ris;
                            filterContext.Controller.ViewBag.RI = ri;
                            ((IRISController)filterContext.Controller).RIS = ri;
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