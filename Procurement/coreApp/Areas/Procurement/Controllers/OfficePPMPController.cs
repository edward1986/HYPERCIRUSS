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
using coreLib.Extensions;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_office_ppmp")]
    public class OfficePPMPController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            tblEmployee employee = Cache.Get().userAccess.employee;

            var model = new OfficePPMPModel(Year, employee);
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
                    return PartialView("_OfficePPMP", ppmp);
                }
            }
        }

        public ActionResult Create()
        {
            tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblPPMP model = new tblPPMP
            {
                Year = Year
            };
            return PartialView("_OfficePPMP", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblPPMP model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                UserAccess access = Cache.Get().userAccess;
                tblEmployee employee = access.employee;

                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        if (model.Fund == null)
                        {
                            throw new Exception("The \"Fund\" field is required");
                        }

                        if (context.tblPPMPs.Any(x => x.Year == Year && x.OfficeId == employee.Office.OfficeId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        tblPPMP ppmp = new tblPPMP
                        {
                            PPMPNumber = getPPMPNumber(model.Year, employee.Office.OfficeId),
                            Description = model.Description,
                            OfficeId = employee.Office.OfficeId,
                            DepartmentId = null,
                            Year = Year,
                            CreatedBy_UserId = access.user.Id,
                            CreateDate = DateTime.Now,
                            FundId = model.FundId
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

        private string getPPMPNumber(int year, int officeId)
        {
            string ret;

            using (procurementDataContext context = new procurementDataContext())
            {
                int ppmp = context.tblPPMPs.Count(x => x.Year == year && x.OfficeId == officeId);

                ret = year + "-" + officeId + "-" + (ppmp + 1).ToString("D3");

            }

            return ret;
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
                    return PartialView("_OfficePPMP", ppmp);
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

                        if (model.Fund == null)
                        {
                            throw new Exception("The \"Fund\" field is required");
                        }

                        if (context.tblPPMPs.Any(x => x.Id != model.Id && x.Year == Year && x.OfficeId == employee.Office.OfficeId && x.Description.ToLower().Trim() == model.Description.ToLower().Trim()))
                        {
                            throw new Exception(Constants.ITEM_DESCRIPTION_EXISTS);
                        }

                        ppmp.Description = model.Description;
                        ppmp.FundId = model.FundId;

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

                    var model = new OfficePPMPModel(Year, employee);
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

                    if (!ppmp.IsDepartmentPPMP)
                    {
                        AddError("Document is not a Department PPMP");
                    }

                    if (!ppmp.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    if (ppmp.IsApproved)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_APPROVED);
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = coreApp.Cache.Get().userAccess;

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
            DepartmentPPMPController dept = new DepartmentPPMPController();

            using (procurementDataContext context = new procurementDataContext())
            {
                tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == id).SingleOrDefault();
                if (ppmp == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string unit = ppmp.Office.Office;
                string fn = string.Format("ppmp-{0}-{1}", unit.ToLower(), ppmp.Year.ToString());

                return new asposeWordsTemplateReport(dept.CustomizeDoc_Aspose, new reports.Aspose.ReportHeaderParams(Server.MapPath("~/")) { ReportLogo_Width = 40, ReportLogo_Height = 40 }).Get("procurement-ppmp", fn, ppmp, dlWord);
            }
        }
    }
}