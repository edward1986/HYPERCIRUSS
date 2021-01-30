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
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_transactions")]
    public class RISController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {            
            using (samDataContext context = new samDataContext())
            {
                var model = context.tblRIs.Where(x => x.RequisitionDate.Year == Year && x.WarehouseId == warehouse.Id).ToList();
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
                tblRI item = context.tblRIs.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_RIS", item);
                }
            }
        }

        public ActionResult Create()
        {
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblRI model = new tblRI
            {
                RequisitionDate = DateTime.Today
            };

            return PartialView("_RIS", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblRI model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using(samDataContext context = new samDataContext())
                {
                    using (procurementDataContext _context = new procurementDataContext())
                    {                        

                        if (ModelState.IsValid)
                        {
                            tblRI item = new tblRI
                            {
                                RequisitionDate = model.RequisitionDate,
                                EmployeeId = model.EmployeeId,
                                Purpose = model.Purpose,
                                Fund = model.Fund,
                                WarehouseId = warehouse.Id
                            };

                            item.UpdateFields();

                            context.tblRIs.InsertOnSubmit(item);
                            context.SubmitChanges();

                            item.UpdateFields();
                            item.UpdateRISNo();

                            res.Remarks = "Record was successfully created";
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
                tblRI item = context.tblRIs.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_RIS", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblRI model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblRI item = context.tblRIs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        item.RequisitionDate = model.RequisitionDate;
                        item.EmployeeId = model.EmployeeId;
                        item.Purpose = model.Purpose;
                        item.Fund = model.Fund;

                        item.UpdateFields();

                        context.SubmitChanges();

                        item.UpdateRISNo();

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
                    tblRI item = context.tblRIs.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    
                    if (ModelState.IsValid)
                    {
                        context.tblRIs.DeleteOnSubmit(item);
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

        public ActionResult Print(int id, bool dlWord = false)
        {
            using (samDataContext context = new samDataContext())
            {
                tblRI rIS = context.tblRIs.Where(x => x.Id == id).SingleOrDefault();
                if (rIS == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("ris-{0}", Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-ris", fn, rIS, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                tblRI rIS = (tblRI)data;
                List<tblRISItem> items = context.tblRISItems.Where(x => x.RISId == rIS.Id).ToList();

                tblWarehouse warehouse = DBProcs.get_WarehouseById(rIS.WarehouseId);

                string[] fields = new string[] {
                    "agency", "department", "office", "agency-code", "rcc", "fund", "risno", "purpose", "employee-name", "position", "risdate", "warehouse"
                };

                string[] fieldValues = new string[] {
                    FixedSettings.AgencyName.ToUpper(),
                    rIS._Department,
                    rIS._Office,
                    FixedSettings.AgencyCode,
                    rIS._RCC,
                    rIS.Fund,
                    rIS._RISNo,
                    rIS.Purpose,
                    rIS._EmployeeName,
                    rIS._Position,
                    rIS.RequisitionDate.ToShortDateString(),
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
                    tblRISItem e = items[i - 1];
                    
                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._Unit));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._ItemName));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Requested_Qty.ToString()));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, "Yes"));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Approved_Qty.ToString()));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Remarks ?? ""));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, (e._UnitCost ?? 0).ToString("#,##0.00")));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Amount.ToString("#,##0.00")));
                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                cellIndex = 0;

                r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, "***NOTHING FOLLOWS***"));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));                
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                t.Rows.Add(row.Clone(true));


                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}