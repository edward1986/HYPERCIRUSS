using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class AREInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "areId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        List<tblARE> ares = context.tblAREs.Where(x => x._AREType == (int)AREType.PAR).OrderBy(x => x._To_Name).ThenBy(x => x._ARENo).ToList();
                        tblARE are = ares.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (are == null)
                        {
                            throw new Exception("PAR Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.AREs = ares;
                            filterContext.Controller.ViewBag.ARE = are;
                            ((IAREController)filterContext.Controller).ARE = are;
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