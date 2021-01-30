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
using Aspose.Words.Tables;
using Aspose.Words;
using System.Drawing;
using reports.Aspose;
using coreLib.Extensions;
using coreApp.Areas.Procurement.Enums;
using System.IO;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class RFQController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        private int printParam_SupplierId = -1;

        public ActionResult Index()
        {
            RFQModels model = new RFQModels(Year);
            return View(model);
        }

        public ActionResult Details(int? id, bool isconsolidatedpr = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isconsolidatedpr)
                {
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (cpr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/ConsolidatedPR/_ConsolidatedPR.cshtml", cpr);
                    }
                }
                else
                {
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).SingleOrDefault();
                    if (rfq == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_RFQ", rfq);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblRFQ model = new tblRFQ
            {
                Year = Year
            };
            return PartialView("_RFQ", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblRFQ model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = Cache.Get().userAccess;

                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        if (context.tblRFQs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblRFQ rfq = new tblRFQ
                        {
                            Description = model.Description,
                            Year = Year,
                            CreatedBy_UserId = access.user.Id,
                            CreateDate = DateTime.Now,
                            Form_PHILGEPS = model.Form_PHILGEPS,
                            Form_Deadline = model.Form_Deadline,
                            Form_Canvasser = model.Form_Canvasser,
                            SupplierIds = model.SupplierIds
                        };

                        context.tblRFQs.InsertOnSubmit(rfq);
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
                tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).Single();
                if (rfq == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_RFQ", rfq);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblRFQ model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (rfq == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (rfq.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        if (context.tblRFQs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        rfq.Description = model.Description;
                        rfq.Form_PHILGEPS = model.Form_PHILGEPS;
                        rfq.Form_Deadline = model.Form_Deadline;
                        rfq.Form_Canvasser = model.Form_Canvasser;
                        rfq.SupplierIds = model.SupplierIds;

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
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).SingleOrDefault();
                    if (rfq == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (rfq.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblRFQs.DeleteOnSubmit(rfq);
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
                    tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).SingleOrDefault();
                    if (rfq == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var rfqModel = new RFQModel(id);

                    if (rfq.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (string.IsNullOrEmpty(rfq.CPRIds))
                    {
                        AddError("Document has no item");
                    }
                    else
                    {
                        var _conflicts = rfqModel.ItemsInConflictWithOtherRFQs();
                        if (_conflicts.Any())
                        {
                            AddError("There are items that conflicts with other items in other Consolidated PRs");
                        }
                    }
                    
                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        rfq.SubmittedBy_UserId = access.user.Id;
                        rfq.SubmitDate = DateTime.Now;

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
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (cpr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var referringRFQs = cpr.ReferringRFQs();

                    if (referringRFQs.Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted RFQ");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;
                        
                        foreach (tblRFQ rfq in referringRFQs)
                        {
                            rfq.RemoveCPRId(id);
                        }

                        //remove approval
                        cpr.ApproveDate = null;
                        cpr.ApprovedBy_UserId = null;

                        cpr.ReturnedBy_UserId = access.user.Id;
                        cpr.ReturnDate = DateTime.Now;
                        cpr.ReturnMessage = returnMessage;

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
        
        public ActionResult Print(int id, bool dlWord = false, int supplierId = -1)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblRFQ rfq = context.tblRFQs.Where(x => x.Id == id).SingleOrDefault();
                if (rfq == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                if (string.IsNullOrEmpty(rfq.SupplierIds))
                {
                    throw new Exception("No supplier specified");
                }

                string fn = string.Format("rfq-{0}", rfq.Year.ToString());
                asposeWordsTemplateReport obj = new asposeWordsTemplateReport(CustomizeDoc_Aspose, new ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 });

                printParam_SupplierId = supplierId;

                return obj.Get("procurement-rfq", fn, rfq, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblRFQ rfq = (tblRFQ)data;
                RFQModel model = new RFQModel(rfq.Id);

                List<tblPPMPItem> items = model.ConsolidatedItems.OrderBy(x => x.ItemName).ToList();

                tblSupplier supplier = rfq.Suppliers().Where(x => x.Id == printParam_SupplierId).Single();

                string[] fields = new string[] {
                    "date", "rfqno", "prno", "philgeps", "company", "company-address", "company-telephone", "deadline", "canvasser"
                };

                string[] fieldValues = new string[] {
                    DateTime.Today.ToShortDateString(),
                    rfq.Id.ToString("000000"),
                    rfq.CPRIds,
                    rfq.Form_PHILGEPS,
                    supplier.CompanyName,
                    supplier.Address,
                    supplier.TelephoneNo,
                    rfq.Form_Deadline == null ? "" : rfq.Form_Deadline.Value.ToShortDateString(),
                    rfq.Form_Canvasser
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int[] cprIds = model.rfq.CPRIds.Split(',').Select(x => int.Parse(x)).ToArray();

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
                        t.Rows.Add(row.Clone(true));

                        r = t.Rows[t.Rows.Count - 1];
                        int cellIndex = 0;

                        r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemName + Environment.NewLine + item.ItemDescription));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Qty.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.OOSAmount.ToCurrencyString()));

                        Double price = 0;
                        if (item.DBMPrice != null)
                        {
                            price = item.DBMPrice.Value;
                        }
                        else if (item.CPR_NewPrice != null)
                        {
                            price = item.CPR_NewPrice.Value;
                        }
                        else
                        {
                            price = item.ItemPrice;
                        }

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                        i++;
                    }
                }
                

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                r.Cells[0].FirstParagraph.AppendChild(new Run(wordDoc, "TOTAL"));
                r.Cells[0].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                r.Cells[0].CellFormat.HorizontalMerge = CellMerge.First;
                r.Cells[1].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[2].CellFormat.HorizontalMerge = CellMerge.Previous;             
                r.Cells[3].FirstParagraph.AppendChild(new Run(wordDoc, items.Sum(x => x.Amount).ToCurrencyString()));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);

            }
        }
    }
}