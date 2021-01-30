using coreApp.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class CampusInfoFilterAttribute : ActionFilterAttribute
    {
        bool allowNull;

        public CampusInfoFilterAttribute(bool allowNull = false)
        {
            this.allowNull = allowNull;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                filterContext.Controller.ViewBag.Campuses = Cache.Get_Tables().Campuses.OrderBy(x => x.Campus).ToList();

                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "campusID");

                if (v != null)
                {
                    tblCampus campus = Cache.Get_Tables().Campuses.Where(x => x.CampusID == int.Parse(v.ToString())).SingleOrDefault();
                    
                    if (campus == null && !allowNull)
                    {
                        throw new Exception("Campus Id not found");
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.Campus = campus;
                        ((ICampusController)filterContext.Controller).campus = campus;
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