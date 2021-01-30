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
    [PRInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_office_ppmp")]
    public class OfficePRItemsController : ProcurementBaseController, IPRController
    {
        public tblPR PR { get; set; }

        public ActionResult Index(bool? fromCPR)
        {
            Session["FromCPR"] = null;
            if (fromCPR != null)
            {
                Session["FromCPR"] = fromCPR.Value;
            }

            if (!PR.HasBeenSubmitted)
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = PR.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = PR.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.PPMPs = context.tblPPMPs
                        .Where(x => x.Year == PR.Year && x.OfficeId == PR.OfficeId)
                        .ToList()
                        .Where(x => x.HasBeenIncludedInSubmittedAPP())
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = PR.ContainsPPMP(x.Id), Data = new System.Collections.Hashtable { { "ppmp", x } } })
                        .ToList();
                    
                }
            }

            return View(PR);
        }

        public ActionResult MainList_OfficePR()
        {
            return PartialView(PR);
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
                    if (PR.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ids == null)
                    {
                        AddError("No item selected");
                    }
                    else if (ids.Length > 1)
                    {
                        AddError("Cannot have multiple PPMPs");
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
                        PRModel model = new PRModel(PR.Id);
                        model.ImportItems(ids, category_ids, period_ids);

                        res.Remarks = "PPMP was successfully selected";
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

        [HttpPost]
        public ActionResult DeleteItems(string[] ids)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        List<tblPPMPItem> items = context.tblPPMPItems.Where(x => x.PRId == PR.Id && ids.Contains(x.ItemId.ToString())).ToList();

                        context.tblPPMPItems.DeleteAllOnSubmit(items);
                        context.SubmitChanges();

                        res.Remarks = "Selected items were successfully deleted";
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

        class objMQ
        {
            public int id { get; set; }
            public int month { get; set; }
            public int value { get; set; }
        }

        [HttpPost]
        public ActionResult ApplyMQ(string data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        objMQ[] d = Newtonsoft.Json.JsonConvert.DeserializeObject<objMQ[]>(data);

                        foreach(objMQ _d in d)
                        {
                            tblPPMPItem item = context.tblPPMPItems.Where(x => x.Id == _d.id).SingleOrDefault();
                            if (item != null)
                            {
                                if (_d.month == 1) item.M1 = _d.value;
                                if (_d.month == 2) item.M2 = _d.value;
                                if (_d.month == 3) item.M3 = _d.value;
                                if (_d.month == 4) item.M4 = _d.value;
                                if (_d.month == 5) item.M5 = _d.value;
                                if (_d.month == 6) item.M6 = _d.value;
                                if (_d.month == 7) item.M7 = _d.value;
                                if (_d.month == 8) item.M8 = _d.value;
                                if (_d.month == 9) item.M9 = _d.value;
                                if (_d.month == 10) item.M10 = _d.value;
                                if (_d.month == 11) item.M11 = _d.value;
                                if (_d.month == 12) item.M12 = _d.value;
                            }
                        }

                        context.SubmitChanges();

                        res.Remarks = "Item quantities were successfully updated";
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