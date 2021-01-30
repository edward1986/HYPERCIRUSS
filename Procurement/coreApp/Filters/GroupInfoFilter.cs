using coreApp.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class GroupInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "groupId");

                if (v != null)
                {
                    tblGroup group = Cache.Get_Tables().Groups.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                    if (group == null)
                    {
                        throw new Exception("Group Id not found");
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.Groups = Cache.Get_Tables().Groups.OrderBy(x => x.GroupName).ToList();
                        ((IGroupController)filterContext.Controller).group = group;
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