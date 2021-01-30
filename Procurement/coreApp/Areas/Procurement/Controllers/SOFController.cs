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
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_allocate_funds")]
    public class SOFController : ProcurementBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return View(new SOFModel(Year));
            }
        }
        
        public ActionResult Edit(int? fundId)
        {
            if (fundId == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.Title = "Fund Allocation";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblFundAllocation fund = context.tblFundAllocations.Where(x => x.Year == Year && x.FundId == fundId).SingleOrDefault();
                if (fund == null)
                {
                    fund = new tblFundAllocation
                    {
                        FundId = fundId.Value,
                        Year = Year
                    };
                }

                return PartialView("_SOF", fund);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblFundAllocation model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        tblFundAllocation fund = context.tblFundAllocations.Where(x => x.Year == model.Year && x.FundId == model.FundId).SingleOrDefault();
                        if (model.Amount == null || model.Amount == 0)
                        {
                            if (fund != null)
                            {
                                context.tblFundAllocations.DeleteOnSubmit(fund);
                            }
                        }
                        else
                        {
                            if (fund == null)
                            {
                                fund = new tblFundAllocation
                                {
                                    FundId = model.FundId,
                                    Year = model.Year,
                                    Amount = model.Amount
                                };

                                context.tblFundAllocations.InsertOnSubmit(fund);
                            }
                            else
                            {
                                fund.Amount = model.Amount;
                            }
                        }

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
        
    }
}