using System;
using System.Linq;
using System.Web.Mvc;
using coreApp.Interfaces;
using Module.DB;

namespace coreApp.Filters
{

    public class PositionInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "positionId");

                if (v != null)
                {
                    tblPosition position = Cache.Get_Tables().Positions.Where(x => x.PositionId == int.Parse(v.ToString())).SingleOrDefault();
                    if (position == null)
                    {
                        throw new Exception("Position Id not found");
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.Positions = Cache.Get_Tables().Positions.OrderBy(x => x.Position).ToList();
                        ((IPositionController)filterContext.Controller).position = position;
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