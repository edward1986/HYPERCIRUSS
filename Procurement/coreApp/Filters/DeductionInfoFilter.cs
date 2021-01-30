using coreApp.DAL;
using coreApp.Interfaces;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Filters
{
    public class DeductionInfoFilterAttribute : ActionFilterAttribute
    {
        bool allowNull;

        public DeductionInfoFilterAttribute(bool allowNull = false)
        {
            this.allowNull = allowNull;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                using (hr2017DataContext context = new hr2017DataContext())
                {

                    filterContext.Controller.ViewBag.Deductions = context.TblPayrollDeductions.OrderBy(x => x.DeductionName).ToList();

                    string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "DeductionID");

                    if (v != null)
                    {
                        TblPayrollDeduction Deduction = context.TblPayrollDeductions.Where(x => x.DeductionID == int.Parse(v.ToString())).SingleOrDefault();
                        if (Deduction == null && !allowNull)
                        {
                            throw new Exception("Deduction Id not found");
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.Deduction = Deduction;
                            ((IDeductionController)filterContext.Controller).deduction = Deduction;
                        }
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