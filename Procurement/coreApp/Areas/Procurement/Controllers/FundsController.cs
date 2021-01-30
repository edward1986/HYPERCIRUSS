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

namespace coreApp.Areas.Procurement.Controllers
{
    [UserAccessAuthorize(allowedAccess: "procurement_settings")]
    public class FundsController : ProcurementBaseController
    {
        public ActionResult Index()
        {            
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblFunds.ToList();
                return View(model);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.Title = "Fund";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblFund fund = context.tblFunds.Where(x => x.Id == id).SingleOrDefault();
                if (fund == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Fund", fund);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.Title = "Fund";
            ViewBag.Subtitle = "Create";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblFund model = new tblFund();
            return PartialView("_Fund", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblFund model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using(procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        tblFund fund = new tblFund
                        {
                            Fund = model.Fund,
                            Description = model.Description,
                            Fund_Cluster = model.Fund_Cluster
                        };

                        context.tblFunds.InsertOnSubmit(fund);
                        context.SubmitChanges();

                        res.Remarks = "Fund was successfully created";
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
                tblFund fund = context.tblFunds.Where(x => x.Id == id).Single();
                if (fund == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Fund", fund);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblFund model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblFund fund = context.tblFunds.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (fund == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    
                    if (ModelState.IsValid)
                    {
                        fund.Fund = model.Fund;
                        fund.Description = model.Description;
                        fund.Fund_Cluster = model.Fund_Cluster;

                        context.SubmitChanges();

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
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblFund fund = context.tblFunds.Where(x => x.Id == id).SingleOrDefault();
                    if (fund == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblFunds.DeleteOnSubmit(fund);
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
    }
}