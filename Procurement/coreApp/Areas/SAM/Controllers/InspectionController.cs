using System.Linq;
using System.Web.Mvc;
using System;
using coreLib.Objects;
using System.Data;
using Module.Core;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using reports.Aspose;
using System.Collections.Generic;
using Aspose.Words;

namespace coreApp.Areas.SAM.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_inspection")]
    public class InspectionController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                var model = context.tblReceipts.Where(x => x.ReceivedDate.Year == Year && x.WarehouseId == warehouse.Id)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .ToList();

                return View(model);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (samDataContext context = new samDataContext())
            {
                tblReceipt item = context.tblReceipts.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Receipt", item);
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (samDataContext context = new samDataContext())
            {
                tblReceipt item = context.tblReceipts.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Receipt", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblReceipt model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    tblReceipt item = context.tblReceipts.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (model.InspectionDate == null)
                    {
                        AddError("The field Inspection Date is required");
                    }

                    if (ModelState.IsValid)
                    {
                        item.InspectionDate = model.InspectionDate;
                        item.InspectionRemarks = model.InspectionRemarks;

                        item.UpdateFields(false);

                        context.SubmitChanges();

                        res.Remarks = "Record was successfully updated";
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
        public ActionResult Return(int id, string message)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblReceipt receipt = context.tblReceipts.Where(x => x.Id == id).SingleOrDefault();
                    if (receipt == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        if (!receipt.HasBeenSubmitted)
                        {
                            AddError("Request has not been submitted");
                        }

                        if (receipt.IsInspected)
                        {
                            AddError("Request has already been inspected");
                        }

                        if (ModelState.IsValid)
                        {
                            receipt.DateReturned = DateTime.Now;
                            receipt.ReturnedBy_UserId = Cache.Get().userAccess.user.Id;
                            receipt.ReturnMessage = message;

                            context.SubmitChanges();

                            res.Remarks = "Record was successfully returned";

                        }
                        else
                        {
                            throw new Exception(coreProcs.ShowErrors(ModelState));
                        }
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
            using (samDataContext context = new samDataContext())
            {
                tblReceipt receipt = context.tblReceipts.Where(x => x.Id == id).SingleOrDefault();
                if (receipt == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("iar-{0}", Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-iar", fn, receipt, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                tblReceipt receipt = (tblReceipt)data;
                List<tblReceiptItem> items = context.tblReceiptItems.Where(x => x.ReceiptId == receipt.Id).ToList();

                tblPO po = receipt.GetPO();
                tblWarehouse warehouse = DBProcs.get_WarehouseById(po.WarehouseId);

                bool isCompleteDelivery = receipt.IsCompleteDelivery();

                string[] fields = new string[] {
                    "agency", "fund", "supplier", "iarno", "iar-date", "pono", "pono-date", "invoiceno", "invoiceno-date", "office-department", "rcc", "complete", "partial", "date-received", "date-inspected", "warehouse"
                };

                string[] fieldValues = new string[] {
                    FixedSettings.AgencyName.ToUpper(),
                    receipt.FundCluster,
                    receipt._SupplierName,
                    receipt.IARNo,
                    receipt.InspectionDate == null ? "" : receipt.InspectionDate.Value.ToShortDateString(),
                    po.PONo,
                    po.PODate.ToShortDateString(),
                    receipt.InvoiceNo,
                    receipt.InvoiceDate == null ? "" : receipt.InvoiceDate.Value.ToShortDateString(),
                    receipt.Office_Department,
                    receipt._RCC,
                    isCompleteDelivery ? "[X]" : "[  ]",
                    isCompleteDelivery || !items.Any() ? "[  ]" : "[X]",
                    receipt.ReceivedDate.ToShortDateString(),
                    receipt.InspectionDate == null ? "" : receipt.InspectionDate.Value.ToShortDateString(),
                    warehouse == null ? "" : warehouse.Description
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Aspose.Words.Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int cellIndex;

                for (int i = 1; i <= items.Count; i++)
                {
                    tblReceiptItem e = items[i - 1];
                    tblPOItem pOItem = e.GetPOItem();

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, pOItem.StockNo));
                    
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, pOItem.ItemName));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, pOItem._Unit));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Qty.ToString()));
                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                cellIndex = 0;

                r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, "***NOTHING FOLLOWS***"));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                t.Rows.Add(row.Clone(true));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}