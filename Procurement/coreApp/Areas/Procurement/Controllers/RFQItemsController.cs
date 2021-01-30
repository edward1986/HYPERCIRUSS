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
    [RFQInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class RFQItemsController : ProcurementBaseController, IRFQController
    {
        public tblRFQ RFQ { get; set; }

        public ActionResult Index(bool? fromAOB)
        {
            Session["FromAOB"] = null;
            if (fromAOB != null)
            {
                Session["FromAOB"] = fromAOB.Value;
            }

            if (!RFQ.HasBeenSubmitted) {
                using (procurementDataContext context = new procurementDataContext())
                {
                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = RFQ.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = RFQ.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.CPRs = context.tblConsolidatedPRs
                        .Where(x => x.Year == RFQ.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted)
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = RFQ.ContainsCPR(x.Id), Data = new System.Collections.Hashtable { { "cpr", x } } })
                        .ToList();
                }
            }

            return View(RFQ);
        }

        public ActionResult MainList_RFQ()
        {
            return PartialView(RFQ);
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
        public ActionResult ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (RFQ.HasBeenSubmitted)
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

                    if (ModelState.IsValid)
                    {
                        RFQModel model = new RFQModel(RFQ.Id);
                        model.ImportItems(ids, category_ids, period_ids);

                        res.Remarks = "Selected CPRs were successfully consolidated";
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