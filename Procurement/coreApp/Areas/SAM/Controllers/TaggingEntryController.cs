using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using coreApp.Areas.SAM.Filters;
using coreApp.Areas.SAM.Interfaces;
using System;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using Module.Core;
using coreLib.Objects;
using reports.Aspose;
using System.IO;
using System.Drawing;
using Aspose.BarCode;
using Aspose.Words;

namespace coreApp.Areas.SAM.Controllers
{
    [YearInfoFilter]
    [ReceiptItemInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_tagging")]
    public class TaggingEntryController : SAMBaseController, IYearController, IReceiptItemController
    {
        public int Year { get; set; }
        public tblReceiptItem ReceiptItem { get; set; }

        public ActionResult Index(bool showCompleted = false)
        {
            using (samDataContext context = new samDataContext())
            {
                ViewBag.ShowCompleted = showCompleted;
                List<tblInventoryItem> model = context.tblInventoryItems.Where(x => x.ReceiptItemId == ReceiptItem.Id).ToList();

                List<TagItemModel> tagitems = Common.GetTaggingModels(Year).Where(x => x.Receipt.WarehouseId == warehouse.Id && x.ReceiptItem.Qty > 0)
                    .Where(x => (showCompleted && x.IsComplete) || (!showCompleted && !x.IsComplete))
                    .ToList();

                ViewBag.TagItems = tagitems;

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
                tblInventoryItem item = context.tblInventoryItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Entry", item);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblInventoryItem model = new tblInventoryItem
            {
                PropertyNo = ReceiptItem.GeneratePropertyNo(),
                ReceiptItemId = ReceiptItem.Id
            };
            return PartialView("_Entry", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblInventoryItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    if (context.tblInventoryItems.Where(x => x.ReceiptItemId == ReceiptItem.Id).Count() >= ReceiptItem.Qty)
                    {
                        AddError("Items exceeded received qty");
                    }

                    if (context.tblInventoryItems.Any(x => x.PropertyNo == model.PropertyNo))
                    {
                        AddError("Property No. already exists");
                    }

                    if (ModelState.IsValid)
                    {
                        tblPOItem pOItem = context.tblPOItems.Where(x => x.Id == ReceiptItem.POItemId).Single();

                        tblInventoryItem item = new tblInventoryItem
                        {
                            ReceiptItemId = ReceiptItem.Id,
                            PropertyNo = model.PropertyNo,
                            Barcode = model.Barcode,
                            SerialNo = model.SerialNo,
                            ModelNo = model.ModelNo,
                            Brand = model.Brand,
                            Color = model.Color,
                            Remarks = model.Remarks
                        };

                        item.UpdateFields();

                        context.tblInventoryItems.InsertOnSubmit(item);
                        context.SubmitChanges();

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
                tblInventoryItem item = context.tblInventoryItems.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Entry", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblInventoryItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    tblInventoryItem item = context.tblInventoryItems.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        item.Barcode = model.Barcode;
                        item.SerialNo = model.SerialNo;
                        item.ModelNo = model.ModelNo;
                        item.Brand = model.Brand;
                        item.Color = model.Color;
                        item.Remarks = model.Remarks;

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
                    tblInventoryItem item = context.tblInventoryItems.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblInventoryItems.DeleteOnSubmit(item);
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
        public ActionResult Generate()
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                res.Data = ReceiptItem.GeneratePropertyNo();
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
                tblInventoryItem inventoryItem = context.tblInventoryItems.Where(x => x.Id == id).SingleOrDefault();
                if (inventoryItem == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("sam-tag-{0}", Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-tag", fn, inventoryItem, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (Procurement.DAL.procurementDataContext pcontext = new Procurement.DAL.procurementDataContext())
            {
                using (samDataContext context = new samDataContext())
                {
                    tblInventoryItem inventoryItem = (tblInventoryItem)data;
                    InventoryItemModel model = new InventoryItemModel(inventoryItem);

                    Procurement.DAL.tblItem pItem = pcontext.tblItems.Where(x => x.Id == model.Tag.POItem.Procurement_ItemId).Single();
                    tblReceiptItem receiptItem = context.tblReceiptItems.Where(x => x.Id == model.Item.ReceiptItemId).Single();
                    tblReceipt receipt = context.tblReceipts.Where(x => x.Id == receiptItem.ReceiptId).Single();

                    var par = context.tblAREItems.Where(x => x.InventoryItemId == inventoryItem.Id)
                                    .Join(context.tblAREs, a => a.AREId, b => b.Id, (a, b) => new { a = a, b = b._ARENo, c = b.AREDate })
                                    .OrderBy(x => x.c)
                                    .FirstOrDefault();

                    tblWarehouse warehouse = DBProcs.get_WarehouseById(model.Tag.Receipt.WarehouseId);


                    string[] fields = new string[] {
                        "agency", "property-type", "description", "brand", "serial-no", "par-no", "date-acquired", "supplier", "unit-cost", "warehouse"
                    };

                    string[] fieldValues = new string[] {
                        FixedSettings.AgencyName,
                        pItem.Category.Category,
                        model.Tag.POItem.ItemName,
                        inventoryItem.Brand,
                        inventoryItem.SerialNo,
                        par == null ? "" : par.b,
                        receipt.ReceivedDate.ToShortDateString(),
                        receipt._SupplierName,
                        model.Tag.POItem.UnitCost.ToString("#,##0.00"),
                        warehouse == null ? "" : warehouse.Description
                    };

                    wordDoc.MailMerge.Execute(fields, fieldValues);
                    DocumentBuilder builder = new DocumentBuilder(wordDoc);

                    if (wordDoc.Range.Bookmarks["companyLogo"] != null)
                    {
                        string path = Path.Combine(Procs.getServerPath(), "Templates", "config", "logo.png");
                        builder.MoveToBookmark("companyLogo");
                        builder.InsertImage(path, 30, 30);
                    }

                    if (wordDoc.Range.Bookmarks["qr"] != null)
                    {
                        string path = Path.Combine(Procs.getServerPath(), "Temp", string.Format("qr-{0}-{1}.jpg", inventoryItem.PropertyNo, DateTime.Now.ToString("yyyyMMddhhmmss")));
                        GenerateQRCodeImage(model, path);

                        builder.MoveToBookmark("qr");
                        builder.InsertImage(path, 50, 40);

                        System.IO.File.Delete(path);
                    }

                    if (wordDoc.Range.Bookmarks["bar"] != null)
                    {
                        string path = Path.Combine(Procs.getServerPath(), "Temp", string.Format("bar-{0}-{1}.jpg", inventoryItem.PropertyNo, DateTime.Now.ToString("yyyyMMddhhmmss")));
                        GenerateBarCodeImage(model, path);

                        builder.MoveToBookmark("bar");
                        builder.InsertImage(path, 300, 50);

                        System.IO.File.Delete(path);
                    }
                }
            }
        }

        private void GenerateBarCodeImage(InventoryItemModel model, string path)
        {
            BarCodeBuilder builder = new BarCodeBuilder();

            builder.EncodeType = Aspose.BarCode.Generation.EncodeTypes.Code39Standard;
            builder.CodeText = model.Item.PropertyNo;
            builder.Save(path, BarCodeImageFormat.Jpeg);
        }

        private void GenerateQRCodeImage(InventoryItemModel model, string path)
        {
            BarCodeBuilder builder = new BarCodeBuilder();

            builder.EncodeType = Aspose.BarCode.Generation.EncodeTypes.QR;
            builder.QREncodeMode = QREncodeMode.Auto;
            builder.QREncodeType = QREncodeType.Auto;
            builder.QRErrorLevel = QRErrorLevel.LevelH;
            builder.CodeLocation = CodeLocation.None;

            builder.CodeText = string.Format("Item Name: {0}\nProperty No.: {1}\nSerial No.: {2}\nModel No.: {3}\nBrand: {4}\nColor: {5}", model.Tag.POItem.ItemName, model.Item.PropertyNo, model.Item.SerialNo, model.Item.ModelNo, model.Item.Brand, model.Item.Color);
            builder.Save(path, BarCodeImageFormat.Jpeg);
        }
    }
}