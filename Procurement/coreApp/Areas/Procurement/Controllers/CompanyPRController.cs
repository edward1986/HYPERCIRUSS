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
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class CompanyPRController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            var model = new CompanyPRModels(Year);
            return View(model);
        }

        public ActionResult Details(int? id, bool isapr = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;
            ViewBag.IsAPR = isapr;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isapr)
                {
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (apr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/CompanyAPR/_CompanyAPR.cshtml", apr);
                    }
                }
                else
                {
                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_CompanyPR", pr);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblCompanyPR model = new tblCompanyPR
            {
                Year = Year
            };
            return PartialView("_CompanyPR", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblCompanyPR model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
               
                using (procurementDataContext context = new procurementDataContext())
                {
                    
                    if (ModelState.IsValid)
                    {
                        if (context.tblCompanyPRs.Any(x => x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }


                        tblCompanyPR pr = new tblCompanyPR
                        {
                            Description = model.Description,
                            Year = Year,
                            CreateDate = DateTime.Now,
                            CreatedBy_UserId = coreApp.Cache.Get().userAccess.user.Id,
                            Form_PRNo = model.Form_PRNo,
                            Form_Purpose = model.Form_Purpose
                        };

                        context.tblCompanyPRs.InsertOnSubmit(pr);
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
                tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).Single();
                if (pr == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_CompanyPR", pr);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCompanyPR model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
               
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == model.Id).SingleOrDefault();
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
                        
                        if (context.tblCompanyPRs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
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
                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).SingleOrDefault();
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
                        context.tblCompanyPRs.DeleteOnSubmit(pr);
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

                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    CompanyPRModel companyPRModel = new CompanyPRModel(id);

                    if (pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (!companyPRModel.Items.Any())
                    {
                        AddError("Document has no item");
                    }
                    
                    if (companyPRModel.aprModel.apr.InSubmittedCompanyPR(id))
                    {
                        AddError("An Agency PR containing the same APR has already been submitted");
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

        [HttpPost]
        public ActionResult Return(int id, string returnMessage)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (apr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (!apr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    List<tblCompanyPR> referringAgencyPRs = apr.ReferringAgencyPRs();

                    if (referringAgencyPRs.Any(x => x.APRId != id && x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted APR");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;
                        
                        foreach (tblCompanyPR pr in referringAgencyPRs)
                        {
                            pr.RemoveAPRId();
                        }

                        apr.ReturnedBy_UserId = access.user.Id;
                        apr.ReturnDate = DateTime.Now;
                        apr.ReturnMessage = returnMessage;

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
            CompanyPRModel model = new CompanyPRModel(id);

            ViewBag.PRId = id;
            ViewBag.PRDescription = model.pr.Description;
            ViewBag.PRYear = Year;
            ViewBag.BC = new Breadcrumb { Description = "Agency PRs (For Out-Of-Stock Items)", Link = Url.Action("Index", "CompanyPR", new { area = "Procurement" }) };
            ViewBag.ControllerName = "CompanyPR";

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
                    tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).Single();
                    
                    if (!pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        var tmp = context.tblMOPs.Where(x => x.PRId == id && x.Type == (int)MOPType.AgencyPR);
                        context.tblMOPs.DeleteAllOnSubmit(tmp);

                        context.SubmitChanges();

                        if (!isClear && data != null)
                        {
                            data.ForEach(x => x.Type = (int)MOPType.AgencyPR);

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
                tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == id).SingleOrDefault();
                if (pr == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("agency-pr-{0}", pr.Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose, new reports.Aspose.ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 }).Get("procurement-pr", fn, pr, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblCompanyPR pr = (tblCompanyPR)data;
                CompanyPRModel model = new CompanyPRModel(pr.Id);

                List<tblPPMPItem> items = model.Items.OrderBy(x => x.ItemName).ToList();


                string[] fields = new string[] {
                    "title", "agency-name", "pr-no", "date", "office-section", "agency-code", "purpose"
                };

                string[] fieldValues = new string[] {
                    "PURCHASE REQUEST",
                    FixedSettings.AgencyName,
                    pr.Form_PRNo,
                    pr.HasBeenSubmitted ? pr.SubmitDate.Value.ToShortDateString() : "",
                    "",
                    FixedSettings.AgencyCode,
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
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.OOS.Value.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.DBMPrice != null ? item.DBMPrice.Value.ToCurrencyString() : item.ItemPrice.ToCurrencyString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.OOSAmount.ToCurrencyString()));

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
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, items.Sum(x => x.OOSAmount).ToCurrencyString()));


                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);



            }
        }
    }
}