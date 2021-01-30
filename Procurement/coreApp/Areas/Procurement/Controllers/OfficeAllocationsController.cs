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
using coreLib.Extensions;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_allocate_funds")]
    public class OfficeAllocationsController : ProcurementBaseController, IYearController, IOfficeController
    {
        public int Year { get; set; }
        public tblOffice Office { get; set; }

        public ActionResult Index()
        {
            return View(new AnnualAllocationModel(Year));
        }

        [OfficeInfoFilter]
        public ActionResult OfficeIndex()
        {
            return View(new OfficeAllocationModel(Office.OfficeId, Year));
        }

        [OfficeInfoFilter]
        public ActionResult Edit(int? fundId)
        {
            if (fundId == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblOfficeAllocation fund = context.tblOfficeAllocations.Where(x => x.OfficeId == Office.OfficeId && x.Year == Year && x.FundId == fundId).SingleOrDefault();
                if (fund == null)
                {
                    fund = new tblOfficeAllocation
                    {
                        OfficeId = Office.OfficeId,
                        FundId = fundId.Value,
                        Year = Year
                    };
                }

                return PartialView("_OfficeFundAllocation", fund);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblOfficeAllocation model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (new FundAllocationModel(model.FundId).Balance(model.Year, model.OfficeId) - (model.Amount ?? 0) < 0)
                    {
                        AddError("Not enough funds");
                    }

                    if (ModelState.IsValid)
                    {
                        tblOfficeAllocation fundAlloc = context.tblOfficeAllocations.Where(x => x.OfficeId == model.OfficeId && x.Year == model.Year && x.FundId == model.FundId).SingleOrDefault();

                        OfficeAllocationModel officeModel = new OfficeAllocationModel(model.OfficeId, model.Year);
                        double usedAmount = 0;

                        if (fundAlloc != null)
                        {
                            usedAmount = officeModel.UsedAmount(fundAlloc.FundId);
                        }

                        if (model.Amount == null || model.Amount == 0)
                        {
                            if (fundAlloc != null)
                            {
                                if (usedAmount > 0)
                                {
                                    throw new Exception("Cannot delete record. Submitted Office PPMPs must first be returned");
                                }

                                context.tblOfficeAllocations.DeleteOnSubmit(fundAlloc);
                            }
                        }
                        else
                        {
                            if (usedAmount > model.Amount)
                            {
                                throw new Exception("Cannot update record. Submitted Office PPMPs already amounted more than the specified value (" + usedAmount.ToCurrencyString() + ").");
                            }

                            if (fundAlloc == null)
                            {
                                fundAlloc = new tblOfficeAllocation
                                {
                                    OfficeId = model.OfficeId,
                                    FundId = model.FundId,
                                    Year = model.Year,
                                    Amount = model.Amount
                                };

                                context.tblOfficeAllocations.InsertOnSubmit(fundAlloc);
                            }
                            else
                            {
                                fundAlloc.Amount = model.Amount;
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