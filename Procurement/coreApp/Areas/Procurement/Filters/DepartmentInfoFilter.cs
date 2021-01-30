using coreApp.Areas.Procurement.Interfaces;
using Module.Core;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.Filters
{

    public class DepartmentInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "deptId");

                tblDepartment  dept = Cache.Get_Tables().Departments.Where(x => x.DepartmentId == int.Parse(v)).SingleOrDefault();
                if (dept == null)
                {
                    throw new Exception(ModuleConstants.DEPARTMENT_ID_NOT_FOUND);
                }
                else
                {
                    filterContext.Controller.ViewBag.Department = dept;
                    ((IDepartmentController)filterContext.Controller).Department = dept;
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