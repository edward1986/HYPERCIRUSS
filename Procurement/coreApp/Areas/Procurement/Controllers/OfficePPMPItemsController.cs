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
    [PPMPInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_office_ppmp,procurement_access_app")]
    public class OfficePPMPItemsController : DepartmentPPMPItemsController
    {
        public override ActionResult Index(bool? fromAPP, bool? fromPR)
        {
            Session["FromAPP"] = null;
            if (fromAPP != null)
            {
                Session["FromAPP"] = fromAPP.Value;
            }

            Session["FromPR"] = null;
            if (fromPR != null)
            {
                Session["FromPR"] = fromPR.Value;
            }

            if (!PPMP.HasBeenSubmitted) {
                using (procurementDataContext context = new procurementDataContext())
                {

                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .ToList()
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = PPMP.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = PPMP.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.PPMPs = context.tblPPMPs
                        .Where(x => x.OfficeId == PPMP.OfficeId && x.Year == PPMP.Year)
                        .ToList()
                        .Where(x => x.IsDepartmentPPMP && x.HasBeenSubmitted)
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = PPMP.ContainsPPMP(x.Id), Data = new System.Collections.Hashtable { { "ppmp", x } } })
                        .ToList();
                }
            }

            return View(PPMP);
        }

        public ActionResult MainList_Office()
        {
            return PartialView(PPMP);
        }

        [HttpPost]
        public ActionResult ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (PPMP.HasBeenSubmitted)
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
                        PPMPModel model = new PPMPModel(PPMP.Id);
                        int n = model.ImportItems(ids, category_ids, period_ids);
                        
                        res.Remarks = n + " selected PPMP items were successfully imported";
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