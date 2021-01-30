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
using coreApp.Areas.Procurement.Enums;
using reports.Aspose;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Drawing;
using coreLib.Extensions;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class AOBController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            AOBModels model = new AOBModels(Year);
            return View(model);
        }

        public ActionResult Details(int? id, bool isrfq = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isrfq)
                {
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).SingleOrDefault();
                    if (rfq == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/RFQ/_RFQ.cshtml", rfq);
                    }
                }
                else
                {
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == id).SingleOrDefault();
                    if (aob == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_AOB", aob);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblAOB model = new tblAOB
            {
                Year = Year
            };
            return PartialView("_AOB", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblAOB model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = Cache.Get().userAccess;

                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        if (context.tblAOBs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblAOB aob = new tblAOB
                        {
                            Description = model.Description,
                            Year = Year,
                            CreatedBy_UserId = access.user.Id,
                            CreateDate = DateTime.Now,
                            Form_OpeningDate = model.Form_OpeningDate
                        };

                        context.tblAOBs.InsertOnSubmit(aob);
                        context.SubmitChanges();

                        res.Remarks = "Document was successfully created";
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblAOB aob = context.tblAOBs.Where(x => x.Id == id).Single();
                if (aob == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_AOB", aob);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAOB model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (aob == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (aob.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        if (context.tblAOBs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        aob.Description = model.Description;
                        aob.Form_OpeningDate = model.Form_OpeningDate;

                        context.SubmitChanges();

                        res.Remarks = "Document was successfully updated";
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
        public ActionResult Delete(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == id).SingleOrDefault();
                    if (aob == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (aob.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblAOBs.DeleteOnSubmit(aob);
                        context.SubmitChanges();

                        res.Remarks = "Document was successfully deleted";
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
        public ActionResult Submit(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == id).SingleOrDefault();
                    if (aob == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var aobModel = new AOBModel(id);

                    if (aob.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (string.IsNullOrEmpty(aob.RFQIds))
                    {
                        AddError("Document has no item");
                    }
                    else
                    {
                        var _conflicts = aobModel.ItemsInConflictWithOtherAOBs();
                        if (_conflicts.Any())
                        {
                            AddError("There are items that conflicts with other items in other AOBs");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        aob.SubmittedBy_UserId = access.user.Id;
                        aob.SubmitDate = DateTime.Now;

                        context.SubmitChanges();

                        res.Remarks = "Document was successfully submitted";
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
        public ActionResult Return(int id, string returnMessage)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).SingleOrDefault();
                    if (rfq == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var referringAOBs = rfq.ReferringAOBs();

                    if (referringAOBs.Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted AOB");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        foreach (tblAOB aob in referringAOBs)
                        {
                            aob.RemoveRFQId(id);
                        }

                        //remove approval
                        rfq.ApproveDate = null;
                        rfq.ApprovedBy_UserId = null;

                        rfq.ReturnedBy_UserId = access.user.Id;
                        rfq.ReturnDate = DateTime.Now;
                        rfq.ReturnMessage = returnMessage;

                        context.SubmitChanges();

                        res.Remarks = "Document was successfully returned";
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

        public ActionResult Bids(int id)
        {
            AOBModel model = new AOBModel(id);

            ViewBag.AOBId = id;
            ViewBag.Description = model.aob.Description;
            ViewBag.Year = Year;
            ViewBag.BC = new Breadcrumb { Description = "Abstract of Bids", Link = Url.Action("Index", "AOB", new { area = "Procurement" }) };
            ViewBag.ControllerName = "AOB";

            return View(model);
        }

        [HttpPost]
        public ActionResult Bids_Update(int id, bool isClear, List<tblAOB_Bid> data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == id).Single();

                    //if (cpr.HasBeenIncludedInSubmittedAgencyPR())
                    //{
                    //    AddError("Document has already been included in a submitted Agency PR");
                    //}

                    //if (!aob.HasBeenSubmitted)
                    //{
                    //    AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    //}

                    if (ModelState.IsValid)
                    {
                        var tmp = context.tblAOB_Bids.Where(x => x.AOBId == id);
                        context.tblAOB_Bids.DeleteAllOnSubmit(tmp);

                        context.SubmitChanges();

                        if (!isClear && data != null)
                        {
                            context.tblAOB_Bids.InsertAllOnSubmit(data);
                            context.SubmitChanges();
                        }

                        if (isClear)
                        {
                            res.Remarks = "Item entries were successfully cleared";
                        }
                        else
                        {
                            res.Remarks = "Items were successfully updated";
                        }

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

        public ActionResult Print(int id, bool dlWord = false)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAOB aob = context.tblAOBs.Where(x => x.Id == id).SingleOrDefault();
                if (aob == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("aob-{0}", aob.Year.ToString());
                asposeWordsTemplateReport obj = new asposeWordsTemplateReport(CustomizeDoc_Aspose, new ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 });

                return obj.Get("procurement-aob", fn, aob, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAOB aob = (tblAOB)data;
                AOBModel model = new AOBModel(aob.Id);

                List<tblPPMPItem> items = model.ConsolidatedItems.OrderBy(x => x.ItemName).ToList();
                List<tblSupplier> suppliers = model.rfqModel.rfq.Suppliers().OrderBy(x => x.CompanyName).ToList();

                string[] fields = new string[] {
                    "aobno", "description", "rfqno", "openingdate", "date",
                    "Supplier1", "Supplier2", "Supplier3"
                };

                string Supplier1 = "";
                string Supplier2 = "";
                string Supplier3 = "";

                if (suppliers.Count == 1)
                {
                    Supplier1 = suppliers[0].CompanyName;
                }

                switch (suppliers.Count)
                {
                    case 1:
                        Supplier1 = suppliers[0].CompanyName;
                        break;

                    case 2:
                        Supplier1 = suppliers[0].CompanyName;
                        Supplier2 = suppliers[1].CompanyName;
                        break;

                    case 3:
                        Supplier1 = suppliers[0].CompanyName;
                        Supplier2 = suppliers[1].CompanyName;
                        Supplier3 = suppliers[2].CompanyName;
                        break;
                }

                string[] fieldValues = new string[] {
                    aob.Id.ToString("000000"),
                    aob.Description.ToString(),
                    int.Parse(aob.RFQIds).ToString("000000"),
                    aob.Form_OpeningDate == null ? "" : aob.Form_OpeningDate.Value.ToShortDateString(),
                    DateTime.Now.ToString(),
                    Supplier1.ToString(),
                    Supplier2.ToString(),
                    Supplier3.ToString()


                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Table t = (Table)_t;

                Row r = null;
                Row row = (Row)t.Rows[t.Rows.Count - 1].Clone(true);

                row = (Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int[] cprIds = model.rfqModel.rfq.CPRIds.Split(',').Select(x => int.Parse(x)).ToArray();

                List<tblMOP> mops = context.tblMOPs.Where(x => cprIds.Contains(x.PRId)).ToList();
                List<tblCategory> categories = context.tblCategories.ToList();

                int i = 1;
                foreach (int categoryId in items.Select(x => x.ItemCategoryId).Distinct())
                {
                    string allowPartial = "";

                    tblCategory cat = categories.Where(x => x.Id == categoryId).Single();
                    tblMOP mop = mops.Where(x => x.CategoryId == categoryId).SingleOrDefault();

                    if (mop != null)
                    {
                        if (mop.AllowPartial == true)
                        {
                            allowPartial = " [Allow Partial]";
                        }
                    }

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    r.Cells[0].FirstParagraph.AppendChild(new Run(wordDoc, cat.Category + allowPartial));
                    r.Cells[0].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    r.Cells[0].CellFormat.HorizontalMerge = CellMerge.First;
                    r.Cells[1].CellFormat.HorizontalMerge = CellMerge.Previous;
                    r.Cells[1].CellFormat.HorizontalMerge = CellMerge.Previous;
                    r.Cells[2].CellFormat.HorizontalMerge = CellMerge.Previous;
                    r.Cells[3].CellFormat.HorizontalMerge = CellMerge.Previous;
                    r.Cells[4].CellFormat.HorizontalMerge = CellMerge.Previous;

                    foreach (tblPPMPItem item in items.Where(x => x.ItemCategoryId == categoryId))
                    {
                        char ii = 'a';

                        t.Rows.Add(row.Clone(true));

                        r = t.Rows[t.Rows.Count - 1];
                        int cellIndex = 0;

                        r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemName + Environment.NewLine + item.ItemDescription));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Qty.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemUnit));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.OOSAmount.ToCurrencyString()));

                        int xi = 0;

                        foreach (tblSupplier supplier in suppliers)
                        {
                            xi += 1;

                            tblAOB_Bid bid = model.getBid(supplier.Id, item.ItemId);
                            if (bid == null) continue;
                            if (bid.UnitBid == null && bid.TotalBid == null) continue;

                            r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                            r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, bid.ItemBid ?? ""));
                            r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, bid.UnitBid == null ? "" : bid.UnitBid.ToCurrencyString()));
                            r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, bid.TotalBid == null ? "" : bid.TotalBid.ToCurrencyString()));
                            r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, bid.Remarks ?? ""));

                            if (bid.IsWinner)
                            {
                                int xCounter = 1;
                                int cIndex = (xi - 1) * 4 + 6;

                                foreach (Cell c in r.Cells)
                                {

                                    if (xCounter >= cIndex && xCounter <= cIndex + 3)
                                    {
                                        c.CellFormat.Shading.BackgroundPatternColor = Color.LightSkyBlue;
                                    }

                                    xCounter += 1;

                                }
                            }

                            ii++;
                        }

                        
                        r = t.Rows[t.Rows.Count - 1];
                        cellIndex = 0;

                        i++;
                    }
                }

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 8);

            }
        }
    }
}