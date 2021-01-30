using coreApp.Areas.Procurement.Interfaces;
using Module.Core;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.Filters
{

    public class OfficeInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "officeId");

                tblOffice office = Cache.Get_Tables().Offices.Where(x => x.OfficeId == int.Parse(v)).SingleOrDefault();
                if (office == null)
                {
                    throw new Exception(ModuleConstants.OFFICE_ID_NOT_FOUND);
                }
                else
                {
                    filterContext.Controller.ViewBag.Office = office;
                    ((IOfficeController)filterContext.Controller).Office = office;
                }


                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                filterContext.HttpContext.Session["ApplicationErrorMessage"] = coreProcs.ShowErrors(e);
                filterContext.Result = new RedirectResult("/Error");
            }
        }

    }
}