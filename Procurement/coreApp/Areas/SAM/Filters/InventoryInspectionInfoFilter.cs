using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class InventoryInspectionInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "inspectionId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        tblInventoryInspection inspection = context.tblInventoryInspections.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (inspection == null)
                        {
                            throw new Exception("Inspection Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.InventoryInspection = inspection;
                            ((IInventoryInspectionController)filterContext.Controller).InventoryInspection = inspection;
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