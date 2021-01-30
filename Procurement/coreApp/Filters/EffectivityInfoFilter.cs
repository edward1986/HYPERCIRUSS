using coreApp.Areas.Procurement.DAL;
using coreApp.Interfaces;
using Module.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Filters
{

    public class EffectivityInfoFilterAttribute : ActionFilterAttribute
    {
        string tableName = "";

        public EffectivityInfoFilterAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string v = coreProcs.getRouteParam(filterContext.HttpContext.Request, "dt").Trim();

                string effectivity = ModuleConstants.DEFAULT_EFFECTIVITY;
                List<DateTime> dates = new List<DateTime>();

                if (tableName == "Settings_Effectivities")
                {
                    using (procurementDataContext context = new procurementDataContext())
                    {
                        dates = context.tblSettings_Effectivities.Select(x => x.DateEffective).Distinct().OrderByDescending(x => x).ToList();
                    }
                }
                                
                List<DateTime> _dates = dates.Where(x => x <= DateTime.Today).ToList();
                if (_dates.Count > 0)
                {
                    effectivity = _dates.First().ToString("MM-dd-yyyy");
                }

                filterContext.Controller.ViewBag.Effectivities = dates;

                if (v.ToUpper() == ModuleConstants.DEFAULT_EFFECTIVITY)
                {
                    effectivity = ModuleConstants.DEFAULT_EFFECTIVITY;
                }
                else if (v != null)
                {
                    DateTime dt;

                    if (DateTime.TryParse(v, out dt))
                    {
                        effectivity = dt.ToString("MM-dd-yyyy");
                    }
                }

                ((IEffectivityController)filterContext.Controller).effectivity = effectivity;
                filterContext.Controller.ViewBag.Effectivity = effectivity;

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