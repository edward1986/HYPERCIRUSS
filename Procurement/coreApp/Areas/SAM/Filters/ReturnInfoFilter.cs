using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class ReturnInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "returnId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        List<tblARE> rets = context.tblAREs.Where(x => x._AREType == (int)AREType.Return).OrderBy(x => x._To_Name).ThenBy(x => x._ARENo).ToList();
                        tblARE ret = rets.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (ret == null)
                        {
                            throw new Exception("Return Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.AREs = rets;
                            filterContext.Controller.ViewBag.ARE = ret;
                            ((IAREController)filterContext.Controller).ARE = ret;
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