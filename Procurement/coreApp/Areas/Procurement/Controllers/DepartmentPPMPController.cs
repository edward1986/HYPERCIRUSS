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
using reports.Aspose;
using reports;
using Aspose.Words;
using coreLib.Extensions;
using Aspose.Words.Tables;
using System.Drawing;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_department_ppmp")]
    public class DepartmentPPMPController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

            var model = new DepartmentPPMPModel(Year, employee);
            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }
            
            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                if (ppmp == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_DepartmentPPMP", ppmp);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblPPMP model = new tblPPMP
            {
                Year = Year
            };
            return PartialView("_DepartmentPPMP", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblPPMP model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = coreApp.Cache.Get().userAccess;
                tblEmployee employee = access.employee;

                using(procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        if (context.tblPPMPs.Any(x => x.Year == Year && x.DepartmentId == employee.DepartmentId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblPPMP ppmp = new tblPPMP
                        {
                            Description = model.Description,
                            OfficeId = employee.Office.OfficeId,
                            DepartmentId = employee.DepartmentId,
                            Year = Year,
                            CreatedBy_UserId = access.user.Id,
                            CreateDate = DateTime.Now
                        };

                        context.tblPPMPs.InsertOnSubmit(ppmp);
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
                tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).Single();
                if (ppmp == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_DepartmentPPMP", ppmp);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPPMP model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = coreApp.Cache.Get().userAccess;
                tblEmployee employee = access.employee;

                using (procurementDataContext context = new procurementDataContext())
                {
                    tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (ppmp == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ppmp.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        
                        if (context.tblPPMPs.Any(x => x.Id != model.Id && x.Year == Year && x.DepartmentId == employee.DepartmentId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        ppmp.Description = model.Description;
                        
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
                    tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                    if (ppmp == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ppmp.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        List<tblPPMPItem> items = context.tblPPMPItems
                            .Where(x => x.APPId == null && x.PRId == null && x.CPRId == null && x.PPMPId == id)
                            .ToList();

                        context.tblPPMPItems.DeleteAllOnSubmit(items);
                        context.tblPPMPs.DeleteOnSubmit(ppmp);
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
                    tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                    if (ppmp == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ppmp.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    var items = context.tblPPMPItems.Where(x => x.APPId == null && x.PRId == null && x.CPRId == null && x.PPMPId == id);
                    if (!items.Any())
                    {
                        AddError("Document has no item");
                    }

                    tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

                    var model = new DepartmentPPMPModel(Year, employee);
                    var ppmpModel = new PPMPModel(id);

                    double availableFunds = model.utility.Balance(id);
                    double totalAmount = ppmpModel.TotalAmount();
                    double balance = availableFunds - totalAmount;

                    if (balance < 0)
                    {
                        AddError("Total amount exceeds available funds");
                    }
                    
                    if (ModelState.IsValid)
                    {
                        UserAccess access = coreApp.Cache.Get().userAccess;

                        ppmp.SubmittedBy_UserId = access.user.Id;
                        ppmp.SubmitDate = DateTime.Now;

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

        public ActionResult Print(int id, bool dlWord = false)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                if (ppmp == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string unit = ppmp.Office.Office + "-" + ppmp.Department.Department;
                string fn = string.Format("ppmp-{0}-{1}", unit.ToLower(), ppmp.Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose, new reports.Aspose.ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 }).Get("procurement-ppmp", fn, ppmp, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                string[] fields = new string[] {
                    "year", "enduser", "funds", "description"
                };

                tblPPMP ppmp = (tblPPMP)data;
                PPMPModel model = new PPMPModel(ppmp.Id);
                
                string[] fieldValues = new string[] {
                    ppmp.Year.ToString(),
                    ppmp.Office.Office + (ppmp.DepartmentId != null ? " | " + ppmp.Department.Department : ""),
                    ppmp.Fund == null ? "" : ppmp.Fund.Fund,
                    ppmp.Description == null ? "" : ppmp.Description
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int total_qty = 0;
                double total_budget = 0;
                int[] total = new int[12];

                var catIds = model.Items.Select(x => x.ItemCategoryId).Distinct();
                List<tblCategory> Categories = context.tblCategories.Where(x => catIds.Contains(x.Id)).ToList();
                

                int cout = 1;
                for (int a = 1; a <= Categories.Count; a++)
                {
                    tblCategory cat = Categories[a - 1];

                    if (a > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    for (int b = 0; b <= r.Cells.Count - 1; b++)
                    {
                        r.Cells[b].CellFormat.HorizontalMerge = b == 0 ? CellMerge.First : CellMerge.Previous;
                    }

                    r.Cells[0].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    r.Cells[0].FirstParagraph.AppendChild(new Run(wordDoc, cat.Category));

                    foreach (Cell cell in r.Cells)
                    {
                        cell.CellFormat.Shading.BackgroundPatternColor = Color.LightGray;
                    }

                    List<tblPPMPItem> Items = model.Items.Where(x => x.ItemCategoryId == cat.Id).OrderBy(x => x.Item.Name).ToList();
                    for (int i = 1; i <= Items.Count; i++)
                    {
                        tblPPMPItem e = Items[i - 1];

                        t.Rows.Add(row.Clone(true));
                        r = t.Rows[t.Rows.Count - 1];

                        int cellIndex = 0;

                        //Item No.
                        r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, cout.ToString() + "."));

                        //Description
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.ItemName));

                        //Unit
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.ItemUnit));

                        //Qty
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Qty.ToString()));

                        //Unit Price
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.ItemPrice.ToCurrencyString()));

                        //Estimated Budget
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Amount.ToCurrencyString()));

                        //Mode of Procurement
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M1 == null || e.M1 == 0 ? "" : e.M1.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M2 == null || e.M2 == 0 ? "" : e.M2.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M3 == null || e.M3 == 0 ? "" : e.M3.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M4 == null || e.M4 == 0 ? "" : e.M4.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M5 == null || e.M5 == 0 ? "" : e.M5.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M6 == null || e.M6 == 0 ? "" : e.M6.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M7 == null || e.M7 == 0 ? "" : e.M7.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M8 == null || e.M8 == 0 ? "" : e.M8.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M9 == null || e.M9 == 0 ? "" : e.M9.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M10 == null || e.M10 == 0 ? "" : e.M10.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M11 == null || e.M11 == 0 ? "" : e.M11.Value.ToString()));
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.M12 == null || e.M12 == 0 ? "" : e.M12.Value.ToString()));

                        total_qty += e.Qty;
                        total_budget += e.Amount;

                        total[0] += e.M1 == null ? 0 : e.M1.Value;
                        total[1] += e.M2 == null ? 0 : e.M2.Value;
                        total[2] += e.M3 == null ? 0 : e.M3.Value;
                        total[3] += e.M4 == null ? 0 : e.M4.Value;
                        total[4] += e.M5 == null ? 0 : e.M5.Value;
                        total[5] += e.M6 == null ? 0 : e.M6.Value;
                        total[6] += e.M7 == null ? 0 : e.M7.Value;
                        total[7] += e.M8 == null ? 0 : e.M8.Value;
                        total[8] += e.M9 == null ? 0 : e.M9.Value;
                        total[9] += e.M10 == null ? 0 : e.M10.Value;
                        total[10] += e.M11 == null ? 0 : e.M11.Value;
                        total[11] += e.M12 == null ? 0 : e.M12.Value;

                        cout++;
                    }

                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                foreach (Cell cell in r.Cells)
                {
                    cell.CellFormat.Shading.BackgroundPatternColor = Color.LightGray;
                }

                //Total
                r.Cells[1].FirstParagraph.AppendChild(new Run(wordDoc, "Total"));
                r.Cells[1].FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;

                r.Cells[3].FirstParagraph.AppendChild(new Run(wordDoc, total_qty.ToString()));
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, total_budget.ToCurrencyString()));

                r.Cells[7].FirstParagraph.AppendChild(new Run(wordDoc, total[0] == 0 ? "" : total[0].ToString()));
                r.Cells[8].FirstParagraph.AppendChild(new Run(wordDoc, total[1] == 0 ? "" : total[1].ToString()));
                r.Cells[9].FirstParagraph.AppendChild(new Run(wordDoc, total[2] == 0 ? "" : total[2].ToString()));
                r.Cells[10].FirstParagraph.AppendChild(new Run(wordDoc, total[3] == 0 ? "" : total[3].ToString()));
                r.Cells[11].FirstParagraph.AppendChild(new Run(wordDoc, total[4] == 0 ? "" : total[4].ToString()));
                r.Cells[12].FirstParagraph.AppendChild(new Run(wordDoc, total[5] == 0 ? "" : total[5].ToString()));
                r.Cells[13].FirstParagraph.AppendChild(new Run(wordDoc, total[6] == 0 ? "" : total[6].ToString()));
                r.Cells[14].FirstParagraph.AppendChild(new Run(wordDoc, total[7] == 0 ? "" : total[7].ToString()));
                r.Cells[15].FirstParagraph.AppendChild(new Run(wordDoc, total[8] == 0 ? "" : total[8].ToString()));
                r.Cells[16].FirstParagraph.AppendChild(new Run(wordDoc, total[9] == 0 ? "" : total[9].ToString()));
                r.Cells[17].FirstParagraph.AppendChild(new Run(wordDoc, total[10] == 0 ? "" : total[10].ToString()));
                r.Cells[18].FirstParagraph.AppendChild(new Run(wordDoc, total[11] == 0 ? "" : total[11].ToString()));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}