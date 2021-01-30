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
using Module.Core;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using reports.Aspose;
using Aspose.Words;

namespace coreApp.Areas.SAM.Controllers
{
    [UserAccessAuthorize(allowedAccess: "sam_monitoring")]
    public class EquipmentController : SAMBaseController
    {
        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                List<InventoryItemModel> model =
                    context.tblReceipts.Where(x => x.WarehouseId == warehouse.Id)
                    .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                    .Join(context.tblInventoryItems, a => a.Id, b => b.ReceiptItemId, (a, b) => b)
                    .Select(x => new InventoryItemModel(x))
                    .ToList()
                    .OrderBy(x => x.Tag.POItem.ItemName)
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
                tblInventoryItem item = context.tblInventoryItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }

                return PartialView("_Inventory", new InventoryItemModel(item));
            }
        }

        public ActionResult History(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            InventoryItemModel inventoryItemModel = new InventoryItemModel(id.Value);
            
            return PartialView("_History", inventoryItemModel.Logs);
        }

        public ActionResult Print(bool dlWord = false)
        {
            using (samDataContext context = new samDataContext())
            {
                List<InventoryItemModel> inventory = context.tblReceipts.Where(x => x.WarehouseId == warehouse.Id)
                    .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                    .Join(context.tblInventoryItems, a => a.Id, b => b.ReceiptItemId, (a, b) => b)
                    .Select(x => new InventoryItemModel(x))
                    .ToList()
                    .OrderBy(x => x.Tag.POItem.ItemName)
                    .ToList();

                string fn = string.Format("equipment-{0}", DateTime.Now.ToString("yyyyMMddhhmmss"));

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-equipmentlist", fn, inventory, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                List<InventoryItemModel> items = (List<InventoryItemModel>)data;

                string[] fields = new string[] {
                    "asof", "warehouse"
                };

                string[] fieldValues = new string[] {
                    DateTime.Now.ToString("MM/dd/yyyy"),
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
                    InventoryItemModel e = items[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.PropertyNo));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Tag.POItem.ItemName));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Tag.POItem._Category));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.Barcode ?? ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.SerialNo ?? ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.ModelNo ?? ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.Brand ?? ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.Color ?? ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.LastIssuedTo ?? ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.LatestInspection == null ? "" : e.LatestInspection.Status_Desc));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item._IsSemiExpendable == true ? "Semi-Expendable" : ""));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.Remarks ?? ""));
                }

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 9);
            }
        }
    }
}