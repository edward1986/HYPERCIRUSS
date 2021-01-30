using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class WarehouseFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                tblWarehouse warehouse = null;

                UserAccess access = coreApp.Cache.Get().userAccess;
                if (access.employee != null)
                {
                    warehouse = DBProcs.get_Warehouse(access.employee);
                    if (warehouse != null)
                    {
                        ((ISAMController)filterContext.Controller).warehouse = warehouse;
                    }
                }

                if (warehouse == null)
                {
                    throw new Exception("User must be an employee with Warehouse field defined");
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