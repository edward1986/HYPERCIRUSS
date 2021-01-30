using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class PTRInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "ptrId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        List<tblARE> ptrs = context.tblAREs.Where(x => x._AREType == (int)AREType.PTR).OrderBy(x => x._To_Name).ThenBy(x => x._ARENo).ToList();
                        tblARE ptr = ptrs.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (ptr == null)
                        {
                            throw new Exception("PTR Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.AREs = ptrs;
                            filterContext.Controller.ViewBag.ARE = ptr;
                            ((IAREController)filterContext.Controller).ARE = ptr;
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