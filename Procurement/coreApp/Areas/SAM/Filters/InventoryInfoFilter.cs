using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class InventoryInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "inventoryId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        tblInventoryItem inventoryItem = context.tblInventoryItems.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (inventoryItem == null)
                        {
                            throw new Exception("Inventory Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.InventoryItem = inventoryItem;
                            ((IInventoryItemController)filterContext.Controller).InventoryItem = inventoryItem;
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