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
using System.Data;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class POController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }
        
        public ActionResult Index()
        {
            POModels model = new POModels(Year);
            return View(model);
        }

        public ActionResult Details(int? id, bool isaob = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isaob)
                {
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == id).SingleOrDefault();
                    if (aob == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/AOB/_AOB.cshtml", aob);
                    }
                }
                else
                {
                    tblPO po = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                    if (po == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_PO", po);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblPO model = new tblPO
            {
                Year = Year
            };
            return PartialView("_PO", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblPO model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = Cache.Get().userAccess;

                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        if (context.tblPOs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblPO po = new tblPO
                        {
                            Description = model.Description,
                            Year = Year,
                            CreatedBy_UserId = access.user.Id,
                            CreateDate = DateTime.Now,
                            Form_PlaceOfDelivery = model.Form_PlaceOfDelivery,
                            Form_DateOfDelivery = model.Form_DateOfDelivery,
                            Form_PaymentTerm = model.Form_PaymentTerm,
                            Form_DeliveryTerm = model.Form_DeliveryTerm
                        };

                        context.tblPOs.InsertOnSubmit(po);
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
                tblPO po = context.tblPOs.Where(x => x.Id == id).Single();
                if (po == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_PO", po);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPO model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblPO po = context.tblPOs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (po == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (po.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        if (context.tblPOs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        po.Description = model.Description;
                        po.Form_PlaceOfDelivery = model.Form_PlaceOfDelivery;
                        po.Form_DateOfDelivery = model.Form_DateOfDelivery;
                        po.Form_PaymentTerm = model.Form_PaymentTerm;
                        po.Form_DeliveryTerm = model.Form_DeliveryTerm;

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
                    tblPO po = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                    if (po == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (po.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblPOs.DeleteOnSubmit(po);
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
                    tblPO po = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                    if (po == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var poModel = new POModel(id);

                    if (po.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (string.IsNullOrEmpty(po.AOBIds))
                    {
                        AddError("Document has no item");
                    }
                    else
                    {
                        var _conflicts = poModel.ItemsInConflictWithOtherPOs();
                        if (_conflicts.Any())
                        {
                            AddError("There are items that conflicts with other items in other POs");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        po.SubmittedBy_UserId = access.user.Id;
                        po.SubmitDate = DateTime.Now;

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
                    tblAOB aob = context.tblAOBs.Where(x => x.Id == id).SingleOrDefault();
                    if (aob == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var referringPOs = aob.ReferringPOs();

                    if (referringPOs.Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted PO");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        foreach (tblPO po in referringPOs)
                        {
                            po.RemoveAOBId(id);
                        }

                        //remove approval
                        aob.ApproveDate = null;
                        aob.ApprovedBy_UserId = null;

                        aob.ReturnedBy_UserId = access.user.Id;
                        aob.ReturnDate = DateTime.Now;
                        aob.ReturnMessage = returnMessage;

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

        class PrintPOItem
        {
            public tblPPMPItem Item { get; set; }
            public tblAOB_Bid Bid { get; set; }
        }

        public ActionResult Print(int id, bool dlWord = false)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPO po = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                if (po == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                POModel poModel = new POModel(po.Id);
                List<SupplierPO> supplierPOs = Common.GetSupplierPOs(poModel);
                if (!supplierPOs.Any())
                {
                    throw new Exception("No record to print");
                }

                string fn = string.Format("po-{0}", po.Year.ToString());
                asposeWordsTemplateReport obj = new asposeWordsTemplateReport(CustomizeDoc_Aspose, new ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 });

                string finalFilename = "", newFilename = "";
                int i = 0;

                foreach (SupplierPO model in supplierPOs)
                {
                    if (i == 0)
                    {
                        newFilename = obj.GetReference("procurement-po", model, true);
                    }
                    else
                    {

                        asposeWordsTemplateReport obj2 = new asposeWordsTemplateReport(CustomizeDoc_Aspose, new ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 })
                        {
                            customWorkingFilename = "po-tmp"
                        };

                        string f2 = obj2.GetReference("procurement-po", model, true, "out2");
                        obj.mergeFiles(newFilename, f2, true);
                    }

                    i++;
                }

                finalFilename = newFilename;
                obj.dlWord = dlWord;
                if (!dlWord)
                {
                    finalFilename = newFilename.Replace(".docx", ".pdf");
                    Document doc = new Document(newFilename);
                    doc.Save(finalFilename, SaveFormat.Pdf);

                    System.IO.File.Delete(newFilename);
                }

                return obj.DownloadFile(finalFilename, fn);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                SupplierPO supplierPO = (SupplierPO)data;
                POModel model = new POModel(supplierPO.PO.Id);
                
                string[] fields = new string[] {
                    "doctype", "supplier", "address", "tin", "pono", "date", "mop", "placeofdelivery", "dateofdelivery", "deliveryterm", "paymentterm"
                };

                tblSupplier supplier = supplierPO.Supplier;

                string[] fieldValues = new string[] {
                    model.po.HasBeenSubmitted ? "" : "[Draft]",
                    supplier.CompanyName,
                    supplier.Address,
                    supplier.TIN + " [" + supplier.VAT_Desc + "]",
                    supplierPO.PONo,
                    model.po.HasBeenSubmitted ? model.po.SubmitDate.Value.ToShortDateString() : "",
                    "",
                    model.po.Form_PlaceOfDelivery,
                    model.po.Form_DateOfDelivery == null ? "" : model.po.Form_DateOfDelivery.Value.ToShortDateString(),
                    model.po.Form_DeliveryTerm,
                    model.po.Form_PaymentTerm
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                List<PrintPOItem> items = supplierPO.Bids.Join(model.ConsolidatedItems, a => a.ItemId, b => b.ItemId, (a, b) => new PrintPOItem
                {
                    Item = b,
                    Bid = a
                }).ToList();


                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                for (int i = 1; i <= items.Count; i++)
                {
                    //char ii = 'a';
                    PrintPOItem item = items[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];
                    int cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Item.ItemUnit));
                    //r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Item.ItemName + Environment.NewLine + item.Item.ItemDescription));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Bid.ItemBid));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Item.Qty.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Bid.UnitBid == null ? "" : item.Bid.UnitBid.Value.ToCurrencyString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Bid.TotalBid == null ? "" : item.Bid.TotalBid.Value.ToCurrencyString()));
                    
                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                r.Cells[0].FirstParagraph.AppendChild(new Run(wordDoc, "TOTAL"));
                r.Cells[0].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                r.Cells[0].CellFormat.HorizontalMerge = CellMerge.First;
                r.Cells[1].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[2].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[3].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[4].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, items.Sum(x => x.Bid.TotalBid ?? 0).ToCurrencyString()));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
                
            }
        }
    }
}