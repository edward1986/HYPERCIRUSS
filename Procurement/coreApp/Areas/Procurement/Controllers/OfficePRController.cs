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
using Module.DB;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_office_ppmp")]
    public class OfficePRController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

            var model = new OfficePRModel(Year, employee);
            return View(model);
        }

        public ActionResult Details(int? id, bool isppmp = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;
            ViewBag.IsPPMP = isppmp;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isppmp)
                {
                    tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                    if (ppmp == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/OfficePPMP/_OfficePPMP.cshtml", ppmp);
                    }
                }
                else
                {
                    tblPR pr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_OfficePR", pr);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblPR model = new tblPR
            {
                Year = Year
            };
            return PartialView("_OfficePR", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblPR model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

                using (procurementDataContext context = new procurementDataContext())
                {
                    

                    if (ModelState.IsValid)
                    {
                        if (context.tblPRs.Any(x => x.Year == Year && x.OfficeId == employee.Office.OfficeId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblPR pr = new tblPR
                        {
                            Description = model.Description,
                            Year = Year,
                            OfficeId = employee.Office.OfficeId,
                            CreateDate = DateTime.Now,
                            CreatedBy_UserId = coreApp.Cache.Get().userAccess.user.Id,
                            Form_PRNo = model.Form_PRNo,
                            Form_Purpose = model.Form_Purpose
                        };

                        context.tblPRs.InsertOnSubmit(pr);
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
                tblPR pr = context.tblPRs.Where(x => x.Id == id).Single();
                if (pr == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_OfficePR", pr);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPR model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

                using (procurementDataContext context = new procurementDataContext())
                {
                    tblPR pr = context.tblPRs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        
                        if (context.tblPRs.Any(x => x.Id != model.Id && x.Year == Year && x.OfficeId == employee.Office.OfficeId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        pr.Description = model.Description;
                        pr.Form_PRNo = model.Form_PRNo;
                        pr.Form_Purpose = model.Form_Purpose;

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
                    tblPR pr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        List<tblPPMPItem> items = context.tblPPMPItems
                            .Where(x => x.PRId == id)
                            .ToList();

                        context.tblPPMPItems.DeleteAllOnSubmit(items);
                        context.tblPRs.DeleteOnSubmit(pr);
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
                    UserAccess access = coreApp.Cache.Get().userAccess;

                    tblPR pr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    PRModel prModel = new PRModel(id);

                    if (pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (!prModel.Items.Any())
                    {
                        AddError("Document has no item");
                    }
                    else
                    {
                        var _conflicts = prModel.ItemsInConflictWithOtherPRs();
                        if (_conflicts.Any())
                        {
                            AddError("There are items that conflicts with other items in other PRs");
                        }
                    }
                    
                    
                    if (ModelState.IsValid)
                    {                        

                        pr.SubmittedBy_UserId = access.user.Id;
                        pr.SubmitDate = DateTime.Now;

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

        public ActionResult MOP(int id)
        {
            PRModel model = new PRModel(id);

            ViewBag.PRId = id;
            ViewBag.PRDescription = model.pr.Description;
            ViewBag.PRYear = Year;
            ViewBag.BC = new Breadcrumb { Description = "Office PRs", Link = Url.Action("Index", "OfficePR", new { area = "Procurement" }) };
            ViewBag.ControllerName = "OfficePR";

            return View(model.Categories);
        }

        [HttpPost]
        public ActionResult MOP_Update(int id, bool isClear, List<tblMOP> data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblPR pr = context.tblPRs.Where(x => x.Id == id).Single();

                    if (!pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        var tmp = context.tblMOPs.Where(x => x.PRId == id && x.Type == (int)MOPType.OfficePR);
                        context.tblMOPs.DeleteAllOnSubmit(tmp);

                        context.SubmitChanges();

                        if (!isClear && data != null)
                        {
                            data.ForEach(x => x.Type = (int)MOPType.OfficePR);

                            context.tblMOPs.InsertAllOnSubmit(data);
                            context.SubmitChanges();

                        }

                        if (isClear)
                        {
                            res.Remarks = "Item entries were successfully cleared";
                        }
                        else
                        {
                            res.Remarks = "Items were successfully updated";
                        }

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
                tblPR pr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                if (pr == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("pr-{0}", pr.Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose, new reports.Aspose.ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 }).Get("procurement-pr", fn, pr, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPR pr = (tblPR)data;
                PRModel model = new PRModel(pr.Id);

                List<tblPPMPItem> items = model.Items.OrderBy(x => x.ItemName).ToList();


                string[] fields = new string[] {
                    "title", "agency-name", "pr-no", "date", "office-section", "agency-code", "purpose"
                };

                string[] fieldValues = new string[] {
                    "PURCHASE REQUEST",
                    FixedSettings.AgencyName,
                    pr.Form_PRNo,
                    pr.HasBeenSubmitted ? pr.SubmitDate.Value.ToShortDateString() : "",
                    pr.Office.Office,
                    pr.Office.RCC,
                    pr.Form_Purpose
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);
                
                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                for (int i = 1; i <= items.Count; i++)
                {
                    tblPPMPItem item = items[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];
                    int cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemUnit));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemName));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Qty.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemPrice.ToCurrencyString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Amount.ToCurrencyString()));

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
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, items.Sum(x => x.Amount).ToCurrencyString()));


                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);



            }
        }
    }
}