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
    [POInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class POItemsController : ProcurementBaseController, IPOController
    {
        public tblPO PO { get; set; }

        public ActionResult Index()
        {

            if (!PO.HasBeenSubmitted)
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = PO.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = PO.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.AOBs = context.tblAOBs
                        .Where(x => x.Year == PO.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted)
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = PO.ContainsAOB(x.Id), Data = new System.Collections.Hashtable { { "aob", x } } })
                        .ToList();
                    
                }
            }

            return View(PO);
        }

        public ActionResult MainList_PO()
        {
            return PartialView(PO);
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
                    if (PO.HasBeenSubmitted)
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
                        POModel model = new POModel(PO.Id);
                        model.ImportItems(ids, category_ids, period_ids);

                        res.Remarks = "AOB was successfully selected";
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