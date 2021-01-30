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
    public class CompanyAPRController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            var model = new CompanyAPRModel(Year);
            return View(model);
        }

        public ActionResult Details(int? id, bool isapp = false)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;
            ViewBag.IsAPP = isapp;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (isapp)
                {
                    tblAPP app = context.tblAPPs.Where(x => x.Id == id).SingleOrDefault();
                    if (app == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("~/Areas/Procurement/Views/CompanyAPP/_CompanyAPP.cshtml", app);
                    }
                }
                else
                {
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (apr == null)
                    {
                        return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        return PartialView("_CompanyAPR", apr);
                    }
                }

            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblAPR model = new tblAPR
            {
                Year = Year
            };
            return PartialView("_CompanyAPR", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblAPR model, string[] ModeOfDelivery, string[] InsufficientFunds)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (ModeOfDelivery != null)
            {
                if (ModeOfDelivery.Count() > 0)
                {
                    model.Form_IssueCommonUse_Pickup_FastLane = ModeOfDelivery[0] == "Form_IssueCommonUse_Pickup_FastLane";
                    model.Form_IssueCommonUse_Pickup_Schedule = ModeOfDelivery[0] == "Form_IssueCommonUse_Pickup_Schedule";
                    model.Form_IssueCommonUse_Pickup_Delivery = ModeOfDelivery[0] == "Form_IssueCommonUse_Pickup_Delivery";
                }
            }

            if (InsufficientFunds != null)
            {
                if (InsufficientFunds.Count() > 0)
                {
                    model.Form_InsufficientFunds_ReduceQty = InsufficientFunds[0] == "Form_InsufficientFunds_ReduceQty";
                    model.Form_InsufficientFunds_Bill = InsufficientFunds[0] == "Form_InsufficientFunds_Bill";
                    model.Form_InsufficientFunds_Charge = InsufficientFunds[0] == "Form_InsufficientFunds_Charge";
                }
            }

            try
            {
                
                using (procurementDataContext context = new procurementDataContext())
                {
                    
                    if (ModelState.IsValid)
                    {
                        if (context.tblAPRs.Any(x => x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }
                        
                        tblAPR apr = new tblAPR
                        {
                            Description = model.Description,
                            Year = Year,
                            Form_AgencyControlNo = model.Form_AgencyControlNo,
                            Form_PSAPRNo = model.Form_PSAPRNo,
                            Form_IssueCommonUse = model.Form_IssueCommonUse,
                            Form_IssueCommonUse_PriceListNo = model.Form_IssueCommonUse_PriceListNo,
                            Form_IssueCommonUse_PriceListNo_Date = model.Form_IssueCommonUse_PriceListNo_Date,
                            Form_IssueCommonUse_Pickup_FastLane = model.Form_IssueCommonUse_Pickup_FastLane,
                            Form_IssueCommonUse_Pickup_Schedule = model.Form_IssueCommonUse_Pickup_Schedule,
                            Form_IssueCommonUse_Pickup_Delivery = model.Form_IssueCommonUse_Pickup_Delivery,
                            Form_InsufficientFunds_ReduceQty = model.Form_InsufficientFunds_ReduceQty,
                            Form_InsufficientFunds_Bill = model.Form_InsufficientFunds_Bill,
                            Form_InsufficientFunds_Charge = model.Form_InsufficientFunds_Charge,
                            Form_InsufficientFunds_Charge_APRNo = model.Form_InsufficientFunds_Charge_APRNo,
                            Form_InsufficientFunds_Charge_Date = model.Form_InsufficientFunds_Charge_Date,
                            Form_PurchaseNonCommon = model.Form_PurchaseNonCommon,
                            Form_PurchaseNonCommon_CompleteSpecs = model.Form_PurchaseNonCommon_CompleteSpecs,
                            Form_PurchaseNonCommon_ObR = model.Form_PurchaseNonCommon_ObR,
                            Form_PurchaseNonCommon_CBA = model.Form_PurchaseNonCommon_CBA,
                            Form_PurchaseNonCommon_Payment = model.Form_PurchaseNonCommon_Payment,
                            Form_PurchaseNonCommon_Others = model.Form_PurchaseNonCommon_Others,
                            Form_PurchaseNonCommon_OthersText = model.Form_PurchaseNonCommon_OthersText,
                            Form_FundDeposit_CheckNo = model.Form_FundDeposit_CheckNo,
                            Form_FundDeposit_Amount = model.Form_FundDeposit_Amount,
                            Form_FundDeposit_AmountInWords = model.Form_FundDeposit_AmountInWords
                        };

                        context.tblAPRs.InsertOnSubmit(apr);
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
                tblAPR apr = context.tblAPRs.Where(x => x.Id == id).Single();
                if (apr == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_CompanyAPR", apr);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAPR model, string[] ModeOfDelivery, string[] InsufficientFunds)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (ModeOfDelivery != null)
            {
                if (ModeOfDelivery.Count() > 0)
                {
                    model.Form_IssueCommonUse_Pickup_FastLane = ModeOfDelivery[0] == "Form_IssueCommonUse_Pickup_FastLane";
                    model.Form_IssueCommonUse_Pickup_Schedule = ModeOfDelivery[0] == "Form_IssueCommonUse_Pickup_Schedule";
                    model.Form_IssueCommonUse_Pickup_Delivery = ModeOfDelivery[0] == "Form_IssueCommonUse_Pickup_Delivery";
                }
            }

            if (InsufficientFunds != null)
            {
                if (InsufficientFunds.Count() > 0)
                {
                    model.Form_InsufficientFunds_ReduceQty = InsufficientFunds[0] == "Form_InsufficientFunds_ReduceQty";
                    model.Form_InsufficientFunds_Bill = InsufficientFunds[0] == "Form_InsufficientFunds_Bill";
                    model.Form_InsufficientFunds_Charge = InsufficientFunds[0] == "Form_InsufficientFunds_Charge";
                }                
            }

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (apr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (apr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        if (context.tblAPRs.Any(x => x.Id != model.Id && x.Year == Year && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        apr.Description = model.Description;

                        apr.Form_AgencyControlNo = model.Form_AgencyControlNo;
                        apr.Form_PSAPRNo = model.Form_PSAPRNo;
                        apr.Form_IssueCommonUse = model.Form_IssueCommonUse;
                        apr.Form_IssueCommonUse_PriceListNo = model.Form_IssueCommonUse_PriceListNo;
                        apr.Form_IssueCommonUse_PriceListNo_Date = model.Form_IssueCommonUse_PriceListNo_Date;
                        apr.Form_IssueCommonUse_Pickup_FastLane = model.Form_IssueCommonUse_Pickup_FastLane;
                        apr.Form_IssueCommonUse_Pickup_Schedule = model.Form_IssueCommonUse_Pickup_Schedule;
                        apr.Form_IssueCommonUse_Pickup_Delivery = model.Form_IssueCommonUse_Pickup_Delivery;
                        apr.Form_InsufficientFunds_ReduceQty = model.Form_InsufficientFunds_ReduceQty;
                        apr.Form_InsufficientFunds_Bill = model.Form_InsufficientFunds_Bill;
                        apr.Form_InsufficientFunds_Charge = model.Form_InsufficientFunds_Charge;
                        apr.Form_InsufficientFunds_Charge_APRNo = model.Form_InsufficientFunds_Charge_APRNo;
                        apr.Form_InsufficientFunds_Charge_Date = model.Form_InsufficientFunds_Charge_Date;
                        apr.Form_PurchaseNonCommon = model.Form_PurchaseNonCommon;
                        apr.Form_PurchaseNonCommon_CompleteSpecs = model.Form_PurchaseNonCommon_CompleteSpecs;
                        apr.Form_PurchaseNonCommon_ObR = model.Form_PurchaseNonCommon_ObR;
                        apr.Form_PurchaseNonCommon_CBA = model.Form_PurchaseNonCommon_CBA;
                        apr.Form_PurchaseNonCommon_Payment = model.Form_PurchaseNonCommon_Payment;
                        apr.Form_PurchaseNonCommon_Others = model.Form_PurchaseNonCommon_Others;
                        apr.Form_PurchaseNonCommon_OthersText = model.Form_PurchaseNonCommon_OthersText;
                        apr.Form_FundDeposit_CheckNo = model.Form_FundDeposit_CheckNo;
                        apr.Form_FundDeposit_Amount = model.Form_FundDeposit_Amount;
                        apr.Form_FundDeposit_AmountInWords = model.Form_FundDeposit_AmountInWords;

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
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (apr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (apr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblAPRs.DeleteOnSubmit(apr);
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
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (apr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (apr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (string.IsNullOrEmpty(apr.APPIds))
                    {
                        AddError("Document has no item");
                    }

                    if (apr.ContainsAPPsInSubmittedAPRs())
                    {
                        AddError("This APR contains APP that has already been included in other submitted APR. Please click on \"Add/Remove Agency APPs (For Consolidation)\" button (in items view) and re-apply your selection");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = coreApp.Cache.Get().userAccess;

                        apr.SubmittedBy_UserId = access.user.Id;
                        apr.SubmitDate = DateTime.Now;

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
                    tblAPP app = context.tblAPPs.Where(x => x.Id == id).SingleOrDefault();
                    if (app == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (!app.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    if (app.ReferringAPRs().Any(x => x.HasBeenSubmitted))
                    {
                        AddError("Document has already been included in a submitted APR");
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = coreApp.Cache.Get().userAccess;

                        //remove from apps
                        foreach (tblAPR apr in app.ReferringAPRs())
                        {
                            apr.RemoveAPPId(app.Id);
                        }

                        app.ReturnedBy_UserId = access.user.Id;
                        app.ReturnDate = DateTime.Now;
                        app.ReturnMessage = returnMessage;

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

        public ActionResult OutOfStock(int id)
        {
            APRModel model = new APRModel(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult OutOfStock_Update(int id, bool isClear, List<OOSModel> data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblAPR apr = context.tblAPRs.Where(x => x.Id == id).Single();
                    if (apr.HasBeenIncludedInSubmittedAgencyPR())
                    {
                        AddError("Document has already been included in a submitted Agency PR");
                    }

                    if (ModelState.IsValid)
                    {
                        var tmp = context.tblAPR_OOs.Where(x => x.APRId == id);
                        context.tblAPR_OOs.DeleteAllOnSubmit(tmp);

                        context.SubmitChanges();

                        if (!isClear && data != null)
                        {
                            var tmp2 = data.Select(x => new tblAPR_OO
                            {
                                APRId = id,
                                ItemId = x.ItemId,
                                Qty = x.Qty,
                                NewPrice = x.NewPrice
                            });

                            context.tblAPR_OOs.InsertAllOnSubmit(tmp2);
                            context.SubmitChanges();

                            APRModel aPRModel = new APRModel(id);
                            aPRModel.UpdateAmount();
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
                tblAPR apr = context.tblAPRs.Where(x => x.Id == id).SingleOrDefault();
                if (apr == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("apr-{0}", apr.Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("procurement-apr", fn, apr, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPR apr = (tblAPR)data;
                APRModel model = new APRModel(apr.Id);

                List<tblPPMPItem> items = model.ConsolidatedItems.OrderBy(x => x.ItemName).ToList();


                string[] fields = new string[] {
                    "agency-name", "agency-address", "agency-acct-code", "agency-control-no", "ps-apr-no",
                    "icu-pricelistno", "icu-date", "if-charge-aprno", "if-charge-date", "pnc-otherstext",
                    "fd-checkno", "fd-amountinwords", "fd-amount"
                };

                string[] fieldValues = new string[] {
                    FixedSettings.AgencyName,
                    FixedSettings.AgencyAddress,
                    FixedSettings.AgencyCode,
                    apr.Form_AgencyControlNo,
                    apr.Form_PSAPRNo,
                    apr.Form_IssueCommonUse_PriceListNo,
                    apr.Form_IssueCommonUse_PriceListNo_Date == null ? "" : apr.Form_IssueCommonUse_PriceListNo_Date.Value.ToShortDateString(),
                    apr.Form_InsufficientFunds_Charge_APRNo,
                    apr.Form_InsufficientFunds_Charge_Date == null ? "" : apr.Form_InsufficientFunds_Charge_Date.Value.ToShortDateString(),
                    apr.Form_PurchaseNonCommon_OthersText,
                    apr.Form_FundDeposit_CheckNo,
                    apr.Form_FundDeposit_AmountInWords,
                    apr.Form_FundDeposit_Amount == null ? "" : apr.Form_FundDeposit_Amount.Value.ToCurrencyString()
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                DocumentBuilder builder = new DocumentBuilder(wordDoc);

                //BOOKMARKS

                Procs.setTick(apr.Form_IssueCommonUse == true, "ICU", wordDoc);
                Procs.setTick(apr.Form_IssueCommonUse_Pickup_FastLane == true, "MOD_PickupFL", wordDoc);
                Procs.setTick(apr.Form_IssueCommonUse_Pickup_Schedule == true, "MOD_PickupS", wordDoc);
                Procs.setTick(apr.Form_IssueCommonUse_Pickup_Delivery == true, "MOD_PickupD", wordDoc);
                Procs.setTick(apr.Form_InsufficientFunds_ReduceQty == true, "IF_Reduce", wordDoc);
                Procs.setTick(apr.Form_InsufficientFunds_Bill == true, "IF_Bill", wordDoc);
                Procs.setTick(apr.Form_InsufficientFunds_Charge == true, "IF_Charge", wordDoc);
                Procs.setTick(apr.Form_PurchaseNonCommon == true, "PNC", wordDoc);
                Procs.setTick(apr.Form_PurchaseNonCommon_CompleteSpecs == true, "PNC_Complete", wordDoc);
                Procs.setTick(apr.Form_PurchaseNonCommon_ObR == true, "PNC_ObR", wordDoc);
                Procs.setTick(apr.Form_PurchaseNonCommon_CBA == true, "PNC_CBA", wordDoc);
                Procs.setTick(apr.Form_PurchaseNonCommon_Payment == true, "PNC_Payment", wordDoc);
                Procs.setTick(apr.Form_PurchaseNonCommon_Others == true, "PNC_Others", wordDoc);

                Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);
                
                row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int i = 1;
                foreach (tblPPMPItem item in items)
                {
                    item.useOrigPrice = true;

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];
                    int cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemName));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.Qty.ToString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemUnit));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, item.ItemPrice.ToCurrencyString()));
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, (item.ItemPrice * item.Qty).ToCurrencyString()));

                    i++;
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
                r.Cells[5].FirstParagraph.AppendChild(new Run(wordDoc, items.Sum(x => x.ItemPrice * x.Qty).ToCurrencyString()));
                

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);

               

            }
        }
    }
}