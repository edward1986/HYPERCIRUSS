using coreApp.Areas.SAM.Interfaces;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Filters
{

    public class ReceiptInfoFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "receiptId");

                if (v != null)
                {
                    using (samDataContext context = new samDataContext())
                    {
                        tblReceipt receipt = context.tblReceipts.Where(x => x.Id == int.Parse(v.ToString())).SingleOrDefault();
                        if (receipt == null)
                        {
                            throw new Exception("Receipt Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.Receipts = context.tblReceipts.OrderBy(x => x._PONo).ToList();
                            filterContext.Controller.ViewBag.Receipt = receipt;
                            ((IReceiptController)filterContext.Controller).Receipt = receipt;
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