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
using Aspose.Words.Tables;
using Aspose.Words;
using System.Drawing;
using reports.Aspose;
using coreLib.Extensions;
using coreApp.Areas.Procurement.Enums;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class ConsolidatedPRController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            ConsolidatedPRModel model = new ConsolidatedPRModel(Year);
            return View(model);
        }

        public ActionResult Details(int? id, bool isofficepr = false, bool iscompanypr = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isofficepr)
                {
                    tblPR opr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (opr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/OfficePR/_OfficePR.cshtml", opr);
                    }
                }
                else if (iscompanypr)
                {
                    tblCompanyPR companypr = context.tblCompanyPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (companypr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/CompanyPR/_CompanyPR.cshtml", companypr);
                    }
                }
                else
                {
                    tblConsolidatedPR pr = context.tblConsolidatedPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_ConsolidatedPR", pr);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblConsolidatedPR model = new tblConsolidatedPR
            {
                Year = Year
            };
            return PartialView("_ConsolidatedPR", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblConsolidatedPR model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = Cache.Get().userAccess;

                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        if (context.tblConsolidatedPRs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }
                        
                        tblConsolidatedPR cpr = new tblConsolidatedPR
                        {
                            Description = model.Description,
                            Year = Year,
                            Form_PRNo = model.Form_PRNo,
                            Form_Purpose = model.Form_Purpose,
                            CreatedBy_UserId = access.user.Id,
                            CreateDate = DateTime.Now
                        };

                        context.tblConsolidatedPRs.InsertOnSubmit(cpr);
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
                tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == id).Single();
                if (cpr == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_ConsolidatedPR", cpr);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblConsolidatedPR model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (cpr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (cpr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        if (context.tblConsolidatedPRs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        cpr.Description = model.Description;
                        cpr.Form_PRNo = model.Form_PRNo;
                        cpr.Form_Purpose = model.Form_Purpose;

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
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (cpr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (cpr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        List<tblPPMPItem> items = context.tblPPMPItems
                           .Where(x => x.CPRId == id)
                           .ToList();

                        context.tblPPMPItems.DeleteAllOnSubmit(items);
                        context.tblConsolidatedPRs.DeleteOnSubmit(cpr);
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
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (cpr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var prModel = new CPRModel(id);

                    if (cpr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (string.IsNullOrEmpty(cpr.PRIds) && string.IsNullOrEmpty(cpr.CompanyPRIds))
                    {
                        AddError("Document has no item");
                    }
                    else if (!cpr.MOPSet())
                    {
                        AddError("Modes of procurement has not been set");
                    }
                    else
                    {
                        var _conflicts = prModel.ItemsInConflictWithOtherCPRs();
                        if (_conflicts.Any())
                        {
                            AddError("There are items that conflicts with other items in other Consolidated PRs");
                        }
                    }
                    
                    
                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        cpr.SubmittedBy_UserId = access.user.Id;
                        cpr.SubmitDate = DateTime.Now;

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
                    tblPR opr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (opr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var referringCPRs = opr.ReferringCPRs();

                    if (referringCPRs.Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted Consolidated PR");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;
                        
                        foreach (tblConsolidatedPR cpr in referringCPRs)
                        {
                            cpr.RemovePRId(opr.Id);
                        }

                        //remove approval
                        opr.ApproveDate = null;
                        opr.ApprovedBy_UserId = null;

                        opr.ReturnedBy_UserId = access.user.Id;
                        opr.ReturnDate = DateTime.Now;
                        opr.ReturnMessage = returnMessage;

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

        [HttpPost]
        public ActionResult ReturnCompanyPR(int id, string returnMessage)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var referringCPRs = pr.ReferringCPRs();

                    if (referringCPRs.Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted Consolidated PR");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;
                        
                        foreach (tblConsolidatedPR cpr in referringCPRs)
                        {
                            cpr.RemovePRId(pr.Id);
                        }

                        //remove approval
                        pr.SubmitDate = null;
                        pr.SubmittedBy_UserId = null;
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

        public ActionResult MOP(int id)
        {
            CPRModel model = new CPRModel(id);

            ViewBag.PRId = id;
            ViewBag.PRDescription = model.cpr.Description;
            ViewBag.PRYear = Year;
            ViewBag.BC = new Breadcrumb { Description = "Consolidated PRs", Link = Url.Action("Index", "ConsolidatedPR", new { area = "Procurement" }) };
            ViewBag.ControllerName = "ConsolidatedPR";

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
                    tblConsolidatedPR cpr = context.tblConsolidatedPRs.Where(x => x.Id == id).Single();

                    //if (cpr.HasBeenIncludedInSubmittedAgencyPR())
                    //{
                    //    AddError("Document has already been included in a submitted Agency PR");
                    //}

                    if (cpr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (data == null)
                    {
                        AddError("Document has no item");
                    }

                    if (ModelState.IsValid)
                    {
                        var tmp = context.tblMOPs.Where(x => x.PRId == id && x.Type == (int)MOPType.ConsolidatedPR);
                        context.tblMOPs.DeleteAllOnSubmit(tmp);

                        context.SubmitChanges();

                        if (!isClear && data != null)
                        {
                            data.ForEach(x => x.Type = (int)MOPType.ConsolidatedPR);

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
                tblConsolidatedPR pr = context.tblConsolidatedPRs.Where(x => x.Id == id).SingleOrDefault();
                if (pr == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("consolidated-pr-{0}", pr.Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose, new reports.Aspose.ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 }).Get("procurement-pr", fn, pr, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblConsolidatedPR cpr = (tblConsolidatedPR)data;
                CPRModel model = new CPRModel(cpr.Id);

                List<tblPPMPItem> items = model.Items.OrderBy(x => x.ItemName).ToList();


                string[] fields = new string[] {
                    "title", "agency-name", "pr-no", "date", "office-section", "agency-code", "purpose"
                };

                string[] fieldValues = new string[] {
                    "CONSOLIDATED PURCHASE REQUEST",
                    FixedSettings.AgencyName,
                    cpr.Form_PRNo,
                    cpr.HasBeenSubmitted ? cpr.SubmitDate.Value.ToShortDateString() : "",
                    "",
                    FixedSettings.AgencyCode,
                    cpr.Form_Purpose
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

                    Double price = 0;
                    if (item.DBMPrice != null)
                    {
                        price = item.DBMPrice.Value;
                    }
                    else if (item.CPR_NewPrice != null)
                    {
                        price = item.CPR_NewPrice.Value;
                    }
                    else
                    {
                        price = item.ItemPrice;
                    }

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, price.ToCurrencyString()));
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