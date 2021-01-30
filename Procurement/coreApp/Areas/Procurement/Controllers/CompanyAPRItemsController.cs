using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.Areas.Procurement.DAL;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using System.Collections.Generic;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;

namespace coreApp.Areas.Procurement.Controllers
{
    [APRInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class CompanyAPRItemsController : ProcurementBaseController, IAPRController
    {
        public tblAPR APR { get; set; }

        public ActionResult Index(bool? fromPR)
        {
            Session["FromPR"] = null;
            if (fromPR != null)
            {
                Session["FromPR"] = fromPR.Value;
            }

            if (!APR.HasBeenSubmitted) {
                using (procurementDataContext context = new procurementDataContext())
                {
                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = APR.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = APR.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.Funds = context.tblFunds
                      .OrderBy(x => x.Fund)
                      .Select(x => new SelectListItem { Text = x.Fund, Value = x.Id.ToString(), Selected = APR.ContainsFund(x.Id) })
                      .ToList();

                    ViewBag.APPs = context.tblAPPs
                        .Where(x => x.Year == APR.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted && !x.HasBeenIncludedInSubmittedAPR())
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = APR.ContainsAPP(x.Id), Data = new System.Collections.Hashtable { { "app", x } } })
                        .ToList();
                }
            }

            return View(APR);
        }

        public ActionResult MainList_Company()
        {
            return PartialView("MainList_Company_APR", APR);
        }

        public ActionResult ItemList(int year)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblItems.Where(x => x.Year == year).OrderBy(x => x.Name).ToList();
                return PartialView(model);
            }
        }

        [HttpPost]
        public ActionResult ImportItems(int[] ids, int[] category_ids, int[] period_ids, int[] fund_ids)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (APR.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ids == null)
                    {
                        AddError("No item selected");
                    }

                    if (category_ids == null)
                    {
                        AddError("No category selected");
                    }

                    if (period_ids == null)
                    {
                        AddError("No month selected");
                    }

                    if (fund_ids == null)
                    {
                        AddError("No fund selected");
                    }

                    if (ModelState.IsValid)
                    {
                        APRModel model = new APRModel(APR.Id);
                        model.ImportItems(ids, category_ids, period_ids, fund_ids);

                        res.Remarks = "Selected APPs were successfully consolidated";
                        TempData["GlobalMessage"] = res.Remarks;
                    }
                    else
                    {
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }

            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }
        
    }
}