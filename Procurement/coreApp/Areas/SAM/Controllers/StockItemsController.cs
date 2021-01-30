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
using coreLib.Enums;
using reports.Aspose;
using Aspose.Words;
using Aspose.Words.Tables;

namespace coreApp.Areas.SAM.Controllers
{
    [UserAccessAuthorize(allowedAccess: "sam_monitoring")]
    public class StockItemsController : SAMBaseController
    {
        public ActionResult Index(int itemId, string startDate, string endDate)
        {
            PeriodModel pm = new PeriodModel(PeriodModelInitType.ThisYear);
            DateTime tmp;

            if (DateTime.TryParse(startDate, out tmp))
            {
                pm.StartDate = tmp;
            }

            if (DateTime.TryParse(endDate, out tmp))
            {
                pm.EndDate = tmp;
            }

            using (procurementDataContext context = new procurementDataContext())
            {
                tblItem item = context.tblItems.Where(x => x.Id == itemId).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }

                ViewBag.PeriodModel = pm;
                ViewBag.ItemId = itemId;

                StockModel model = new StockModel(item.Id, pm);

                return View(model);
            }
        }

        public ActionResult Print(int itemId, string startDate, string endDate, bool dlWord = false)
        {
            using (samDataContext context = new samDataContext())
            {
                StockModel model = new StockModel(itemId, new PeriodModel(startDate, endDate));

                string fn = string.Format("stockcard-{0}-{1}-{2}", itemId, startDate, endDate);

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-stockcard", fn, model, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                StockModel model = (StockModel)data;
                List<StockEntryInfo> items = model.Current_Entries();
                
                string[] fields = new string[] {
                    "itemname", "stockcardno", "unit", "period", "prevbalance", "currentbalance", "nextbalance", "overallbalance", "warehouse"
                };

                string[] fieldValues = new string[] {
                    model.Item.Name.ToUpper(),
                    model.Item.Id.ToString("00000"),
                    model.Item.Unit.Unit,
                    coreLib.Procs.friendlyPeriod(model.periodModel.StartDate, model.periodModel.EndDate, true),
                    model.Previous_Balance().ToString("#,##0.#"),
                    model.Current_Balance().ToString("#,##0.#"),
                    model.Next_Balance().ToString("#,##0.#"),
                    model.Balance().ToString("#,##0.#"),
                    warehouse == null ? "" : warehouse.Description
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Aspose.Words.Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                for (int i = 1; i <= items.Count; i++)
                {
                    StockEntryInfo e = items[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    int cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Date.ToString("MM/dd/yyy")));

                    if (e.IsReceipt)
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Receipt.InvoiceNo ?? ""));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Receipt.DRNo ?? ""));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Receipt.ReceivedBy));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Qty.ToString()));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                    }
                    else
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Issuance._RISNo));                        
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Issuance._EmployeeName + Environment.NewLine + "(" + e.Issuance._Office + ")"));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Qty.ToString()));
                    }
                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                r.Cells[0].FirstParagraph.AppendChild(new Run(wordDoc, "Total"));
                r.Cells[0].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                r.Cells[0].CellFormat.HorizontalMerge = CellMerge.First;
                r.Cells[1].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[2].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[3].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[4].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, model.Current_DR().ToString("#,##0.#")));
                r.Cells[6].FirstParagraph.AppendChild(new Run(wordDoc, model.Current_CR().ToString("#,##0.#")));

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                r.Cells[0].FirstParagraph.AppendChild(new Run(wordDoc, "Period Balance"));
                r.Cells[0].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                r.Cells[0].CellFormat.HorizontalMerge = CellMerge.First;
                r.Cells[1].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[2].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[3].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[4].CellFormat.HorizontalMerge = CellMerge.Previous;
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, model.Current_Balance().ToString("#,##0.#")));
                r.Cells[6].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}