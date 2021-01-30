using coreApp.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class OfficeInfoFilterAttribute : ActionFilterAttribute
    {
        bool allowNull;

        public OfficeInfoFilterAttribute(bool allowNull = false)
        {
            this.allowNull = allowNull;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                filterContext.Controller.ViewBag.Offices = Cache.Get_Tables().Offices.OrderBy(x => x.Office).ToList();

                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "officeId");

                if (v != null)
                {
                    tblOffice office = Cache.Get_Tables().Offices.Where(x => x.OfficeId == int.Parse(v.ToString())).SingleOrDefault();
                    
                    if (office == null && !allowNull)
                    {
                        throw new Exception("Office Id not found");
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.Office = office;
                        ((IOfficeController)filterContext.Controller).office = office;
                    }
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