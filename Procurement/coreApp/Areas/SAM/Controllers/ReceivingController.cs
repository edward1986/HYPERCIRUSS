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
    [UserAccessAuthorize(allowedAccess: "sam_receiving")]
    public class ReceivingController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                var model = context.tblReceipts.Where(x => x.ReceivedDate.Year == Year && x.WarehouseId == warehouse.Id).ToList();
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

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblReceipt model = new tblReceipt
            {
                ReceivedDate = DateTime.Today
            };

            return PartialView("_Receipt", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string SearchPONo, tblReceipt model, FormCollection _model)
        {

            if (_model.Get("selectedDepartment") == "-1")
            {
                ModelState.AddModelError("Department", "Office/Department selection is incomplete");
            }
            else
            {
                _model.Set("DepartmentId", _model.Get("selectedDepartment").Split(',')[1]);

                model.DepartmentId = Convert.ToInt16(_model.Get("selectedDepartment").Split(',')[1]);
                model._OfficeId = Convert.ToInt16(_model.Get("searchOffice").Split(',')[1]);
            }

            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    POModel poModel = Common.GetPOModel(SearchPONo, warehouse.Id, importProcurementPO: true);
                    if (poModel == null)
                    {
                        AddError("P.O. No. not found");
                    }
                    else if (poModel.IsComplete)
                    {
                        AddError("P.O. has been completely delivered");
                    }

                    //if (OfficeId != -1 && model.DepartmentId == -1)
                    //{
                    //    AddError("Office/Department selection is incomplete");
                    //}

                    if (ModelState.IsValid)
                    {
                        tblReceipt item = new tblReceipt
                        {
                            InvoiceNo = model.InvoiceNo,
                            InvoiceDate = model.InvoiceDate,
                            DRNo = model.DRNo,
                            ReceivedBy_UserId = Cache.Get().userAccess.user.Id,
                            POId = poModel.PO.Id,
                            ReceivedDate = model.ReceivedDate,
                            DepartmentId = model.DepartmentId,
                            FundCluster = model.FundCluster,
                            WarehouseId = warehouse.Id
                        };

                        item.UpdateFields(true);

                        context.tblReceipts.InsertOnSubmit(item);
                        context.SubmitChanges();

                        res.Data = item.Id;
                        res.Remarks = "Record was successfully created";                        
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
        public ActionResult Edit(tblReceipt model, FormCollection _model)
        {
            if (_model.Get("selectedDepartment") == "-1")
            {
                ModelState.AddModelError("Department", "Office/Department selection is incomplete");
            }
            else
            {
                _model.Set("DepartmentId", _model.Get("selectedDepartment").Split(',')[1]);

                model.DepartmentId = Convert.ToInt16(_model.Get("selectedDepartment").Split(',')[1]);
                model._OfficeId = Convert.ToInt16(_model.Get("searchOffice").Split(',')[1]);
            }

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
                                       
                    if (ModelState.IsValid)
                    {
                        item.ReceivedDate = model.ReceivedDate;
                        item.InvoiceNo = model.InvoiceNo;
                        item.InvoiceDate = model.InvoiceDate;
                        item.DRNo = model.DRNo;
                        item.DepartmentId = model.DepartmentId;
                        item.FundCluster = model.FundCluster;

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
        public ActionResult Delete(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblReceipt item = context.tblReceipts.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (context.tblReceiptItems.Where(x => x.ReceiptId == id)
                        .Join(context.tblInventoryItems, a => a.Id, b => b.ReceiptItemId, (a, b) => b)
                        .Any())
                    {
                        AddError("Receipt items have already been tagged");
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblReceipts.DeleteOnSubmit(item);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";
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
            queryResult model = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    tblReceipt receipt = context.tblReceipts.Where(x => x.Id == id).SingleOrDefault();
                    if (receipt == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    
                    if (receipt.HasBeenSubmitted)
                    {
                        AddError("Request has already been submitted");
                    }

                    if (ModelState.IsValid)
                    {
                        receipt.SubmittedBy_UserId = Cache.Get().userAccess.user.Id;
                        receipt.DateSubmitted = DateTime.Now;

                        context.SubmitChanges();

                        model.Remarks = "Record was successfully submitted";
                    }
                    else
                    {
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }
            }
            catch (Exception e)
            {
                model.IsSuccessful = false;
                model.Err = coreProcs.ShowErrors(e);
            }

            return Json(model);
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

                if (!receipt.HasBeenSubmitted)
                {
                    throw new Exception("Please submit this item first before printing Request for Inspection");
                }

                string fn = string.Format("rfi-{0}", Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-rfi", fn, receipt, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                tblReceipt receipt = (tblReceipt)data;
                
                tblPO po = receipt.GetPO();
                POModel pOModel = new POModel(po);
                
                //bool isCompleteDelivery = receipt.IsCompleteDelivery();

                tblWarehouse warehouse = DBProcs.get_WarehouseById(po.WarehouseId);

                string[] fields = new string[] {
                    "rfino", "date", "warehouse"
                };

                string[] fieldValues = new string[] {
                    receipt.RFINo,
                    receipt.ReceivedDate.ToString("MMMM dd, yyyy"),
                    warehouse == null ? "" : warehouse.Description
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Aspose.Words.Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int cellIndex;

                r = t.Rows[t.Rows.Count - 1];

                cellIndex = 0;

                r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, po.PRNo ?? ""));

                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, receipt.InvoiceNo));

                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, po.PONo));

                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, pOModel.Amount.ToString("#,##0.00")));

                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, po._SupplierName));

               
                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}