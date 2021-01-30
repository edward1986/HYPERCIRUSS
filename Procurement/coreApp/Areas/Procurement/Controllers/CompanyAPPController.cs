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
using Module.DB;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class CompanyAPPController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            CompanyAPPModel model = new CompanyAPPModel(Year);
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
                    tblAPP app = context.tblAPPs.Where(x => x.Id == id).SingleOrDefault();
                    if (app == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_CompanyAPP", app);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblAPP model = new tblAPP
            {
                Year = Year
            };
            return PartialView("_CompanyAPP", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblAPP model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                tblEmployee employee = Cache.Get().userAccess.employee;

                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        if (context.tblPRs.Any(x => x.Id != model.Id && x.Year == Year && x.OfficeId == employee.Office.OfficeId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        if (context.tblAPPs.Any(x => x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblAPP app = new tblAPP
                        {
                            Description = model.Description,
                            Year = Year
                        };

                        context.tblAPPs.InsertOnSubmit(app);
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
                tblAPP app = context.tblAPPs.Where(x => x.Id == id).Single();
                if (app == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_CompanyAPP", app);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAPP model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAPP app = context.tblAPPs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (app == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (app.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        if (context.tblAPPs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        app.Description = model.Description;

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
                    tblAPP app = context.tblAPPs.Where(x => x.Id == id).SingleOrDefault();
                    if (app == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (app.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        List<tblPPMPItem> items = context.tblPPMPItems
                            .Where(x => x.APPId == id)
                            .ToList();

                        context.tblPPMPItems.DeleteAllOnSubmit(items);
                        context.tblAPPs.DeleteOnSubmit(app);
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
                    tblAPP app = context.tblAPPs.Where(x => x.Id == id).SingleOrDefault();
                    if (app == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    var appModel = new APPModel(id);

                    if (app.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (string.IsNullOrEmpty(app.PPMPIds))
                    {
                        AddError("Document has no item");
                    }
                    else
                    {
                        var _conflicts = appModel.ItemsInConflictWithOtherAPPs();
                        if (_conflicts.Any())
                        {
                            AddError("There are items that conflicts with other items in other APPs");
                        }
                    }

                    var model = new CompanyAPPModel(Year);

                    double availableFunds = model.utility.Balance(id);
                    double totalAmount = appModel.TotalAmount();
                    double balance = availableFunds - totalAmount;

                    if (balance < 0)
                    {
                        AddError("Total amount exceeds available funds");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        app.SubmittedBy_UserId = access.user.Id;
                        app.SubmitDate = DateTime.Now;

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
                    tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                    if (ppmp == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ppmp.IsDepartmentPPMP)
                    {
                        AddError("Document is not an Office PPMP");
                    }

                    if (ppmp.ReferringAPPs().Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted APP");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = Cache.Get().userAccess;

                        //remove from apps
                        foreach (tblAPP app in ppmp.ReferringAPPs())
                        {
                            app.RemovePPMPId(ppmp.Id);
                        }

                        //remove approval
                        ppmp.ApproveDate = null;
                        ppmp.ApprovedBy_UserId = null;

                        ppmp.ReturnedBy_UserId = access.user.Id;
                        ppmp.ReturnDate = DateTime.Now;
                        ppmp.ReturnMessage = returnMessage;

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

        public ActionResult Print(int id, bool dlWord = false)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPP app = context.tblAPPs.Where(x => x.Id == id).SingleOrDefault();
                if (app == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("app-{0}", app.Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose, new ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 }).Get("procurement-app", fn, app, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPP app = (tblAPP)data;
                List<tblAppItem> item = context.tblAppItems.Where(x => x.AppId == app.Id).ToList().OrderBy(x => x.getPPMP.Office.ShortName).ToList();


                string[] fields;
                string[] fieldValues;

                double xTotal = (double)app._TotalAmount;
                double xMOOE = (double)item.Sum(x => x.MOOE);
                double xCO = (double)item.Sum(x => x.CO);
               
                fields = new string[] {
                         "title",
                         "xtotal",
                         "xMOOE",
                         "xCO"
                    };

                fieldValues = new string[] {
                        app.Description,
                       xTotal.ToString("#,##0.00"),
                       xMOOE.ToString("#,##0.00"),
                       xCO.ToString("#,##0.00")
                    };
                //}

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Table t = (Table)_t;

                Row r = null;
                Row row = (Row)t.Rows[t.Rows.Count - 2].Clone(true);

                for (int i = 1; i <= item.Count; i++)
                {
                    tblAppItem xitem = item[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));

                    }

                    r = t.Rows[t.Rows.Count - 2];

                    int cellIndex = 0;

                    //Code (PAP)
                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.PPMPNumber.ToUpper()));

                    //Procurement Program/Project
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.getPPMP.Description.ToUpper()));

                    //PMO/End-User
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.getPPMP.Office.ShortName.ToUpper()));

                    //Mode of Procurement
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.MOP_desc.ToUpper()));

                    //Advertisement/Posting of IB/REI

                    if (xitem.Advertisement != null)
                    {
                        DateTime Advertisement = (DateTime)xitem.Advertisement;
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, Advertisement.ToString("MM/dd/yyyy")));
                    }
                    else
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.Advertisement.ToString()));
                    }

                    //Submission/ Opening of Bids
                    if (xitem.Submission != null)
                    {
                        DateTime Submission = (DateTime)xitem.Submission;
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, Submission.ToString("MM/dd/yyyy")));
                    }
                    else
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.Submission.ToString()));
                    }

                    //Notice of Award

                    if (xitem.NoticeOfAward != null)
                    {
                        DateTime NoticeOfAward = (DateTime)xitem.NoticeOfAward;
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, NoticeOfAward.ToString("MM/dd/yyyy")));
                    }
                    else
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.NoticeOfAward.ToString()));
                    }

                    //Contract Signing
                    if (xitem.ContractSigning != null)
                    {
                        DateTime ContractSigning = (DateTime)xitem.ContractSigning;
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ContractSigning.ToString("MM/dd/yyyy")));
                    }
                    else
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.ContractSigning.ToString()));
                    }


                    //Source of Funds

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.getPPMP.Fund.Fund));

                    //Total
                    double total = (double)xitem.Total;
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, total.ToString("#,##0.00")));

                    //MOOE
                    double mooe = (double)xitem.MOOE;
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, mooe.ToString("#,##0.00")));

                    //CO
                    double co = (double)xitem.CO;
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, co.ToString("#,##0.00")));

                    //Remarks
                    if (xitem.Remarks != null)
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, xitem.Remarks));
                    }
                    else
                    {
                        r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                    }  
                      
                }

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 9);
            }

        }
    }
}