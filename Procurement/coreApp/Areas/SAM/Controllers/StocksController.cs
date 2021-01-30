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
using coreApp.Areas.SAM.Enums;
using reports.Aspose;
using Aspose.Words;

namespace coreApp.Areas.SAM.Controllers
{
    [UserAccessAuthorize(allowedAccess: "sam_monitoring")]
    public class StocksController : SAMBaseController
    {
        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {

                List<StockModel> model = context.tblReceipts.Where(x => x.WarehouseId == warehouse.Id)
                    .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                    .Join(context.tblPOItems, a => a.POItemId, b => b.Id, (a, b) => b)
                    .Where(x => x._CategoryType == (int)CategoryType.Supply)
                    .Select(x => x.Procurement_ItemId)
                    .Distinct()
                    .ToList()
                    .Select(x => new StockModel(x))
                    .OrderBy(x => x.Item.Name)
                    .ToList();

                return View(model);
            }
        }

        public ActionResult Print(bool dlWord = false)
        {
            using (samDataContext context = new samDataContext())
            {
                List<StockModel> stocks = context.tblReceipts.Where(x => x.WarehouseId == warehouse.Id)
                    .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                    .Join(context.tblPOItems, a => a.POItemId, b => b.Id, (a, b) => b)
                    .Where(x => x._CategoryType == (int)CategoryType.Supply)
                    .Select(x => x.Procurement_ItemId)
                    .Distinct()
                    .ToList()
                    .Select(x => new StockModel(x))
                    .OrderBy(x => x.Item.Name)
                    .ToList();

                string fn = string.Format("stocklist-{0}", DateTime.Now.ToString("yyyyMMddhhmmss"));

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-stocklist", fn, stocks, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                List<StockModel> items = (List<StockModel>)data;
                                
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
                    StockModel e = items[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.StockNo));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.Name));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Item.Unit.Unit));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Balance().ToString("#,##0.#")));
                }

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}