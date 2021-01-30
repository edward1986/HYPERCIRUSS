using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class ReceiptItemInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "receiptItemId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        tblReceiptItem receiptItem = context.tblReceiptItems.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (receiptItem == null)
                        {
                            throw new Exception("Receipt Item Id not found");
                        }
                        else
                        {
                            //filterContext.Controller.ViewBag.ReceiptItems = context.tblReceiptItems.ToList();
                            filterContext.Controller.ViewBag.ReceiptItem = receiptItem;
                            ((IReceiptItemController)filterContext.Controller).ReceiptItem = receiptItem;
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