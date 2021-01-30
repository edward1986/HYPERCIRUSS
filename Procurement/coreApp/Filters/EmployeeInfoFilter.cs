using coreApp.DAL;
using coreApp.Interfaces;
using Module.Core;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class EmployeeInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "employeeId");
                
                if (v != null)
                {
                    using (dalDataContext context = new dalDataContext())
                    {
                        tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == int.Parse(v)).SingleOrDefault();
                        if (employee == null)
                        {
                            throw new Exception(ModuleConstants.EMPLOYEE_ID_NOT_FOUND);
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.Employee = employee;
                            ((IEmployeeController)filterContext.Controller).employee = employee;
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