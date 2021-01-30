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
    [CPRInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class ConsolidatedPRItemsController : ProcurementBaseController, ICPRController
    {
        public tblConsolidatedPR CPR { get; set; }

        public ActionResult Index(bool? fromRFQ)
        {
            Session["fromRFQ"] = null;
            if (fromRFQ != null)
            {
                Session["fromRFQ"] = fromRFQ.Value;
            }

            if (!CPR.HasBeenSubmitted) {
                using (procurementDataContext context = new procurementDataContext())
                {

                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = CPR.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = CPR.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.Funds = context.tblFunds
                     .OrderBy(x => x.Fund)
                     .Select(x => new SelectListItem { Text = x.Fund, Value = x.Id.ToString(), Selected = CPR.ContainsFund(x.Id) })
                     .ToList();

                    ViewBag.OfficePRs = context.tblPRs
                        .Where(x => x.Year == CPR.Year)
                        .ToList()
                        .Where(x => x.IsApproved)
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = CPR.ContainsPR(x.Id), Data = new System.Collections.Hashtable { { "pr", x } } })
                        .ToList();

                    ViewBag.CompanyPRs = context.tblCompanyPRs
                        .Where(x => x.Year == CPR.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted)
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = CPR.ContainsCompanyPR(x.Id), Data = new System.Collections.Hashtable { { "companyPR", x } } })
                        .ToList();
                }
            }

            return View(CPR);
        }

        public ActionResult MainList_ConsolidatedPR()
        {
            return PartialView(CPR);
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
        public ActionResult ImportItems(int[] ids, int[] cids, int[] category_ids, int[] period_ids, int[] fund_ids)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (CPR.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ids == null && cids == null)
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

                    if (ModelState.IsValid)
                    {
                        CPRModel model = new CPRModel(CPR.Id);
                        model.ImportItems(ids, cids, category_ids, period_ids, fund_ids);

                        res.Remarks = "Selected PRs were successfully consolidated";
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