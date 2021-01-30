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
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_ppmp_approver")]
    public class ManagePRController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {            
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblPRs
                    .Where(x => x.Year == Year).ToList()
                    .Where(x => x.HasBeenSubmitted).ToList();

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

            using (procurementDataContext context = new procurementDataContext())
            {
                tblPR pr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                if (pr == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("~/Areas/Procurement/Views/OfficePR/_OfficePR.cshtml", pr);
                }
            }
        }

        [HttpPost]
        public ActionResult Approve(int id)
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

                    if (!pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    if (pr.IsApproved)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_APPROVED);
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = coreApp.Cache.Get().userAccess;

                        pr.ApprovedBy_UserId = access.user.Id;
                        pr.ApproveDate = DateTime.Now;

                        context.SubmitChanges();

                        res.Remarks = "Document was successfully approved";
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
                    tblPR pr = context.tblPRs.Where(x => x.Id == id).SingleOrDefault();
                    if (pr == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    
                    if (!pr.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_NOT_BEEN_SUBMITTED);
                    }

                    if (pr.IsApproved)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_APPROVED);
                    }

                    if (ModelState.IsValid)
                    {
                        UserAccess access = coreApp.Cache.Get().userAccess;
                        
                        pr.ReturnedBy_UserId = access.user.Id;
                        pr.ReturnDate = DateTime.Now;
                        pr.ReturnMessage = returnMessage;

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
    }
}