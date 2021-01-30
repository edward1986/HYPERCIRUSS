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
using coreApp.Filters;
using coreApp.Interfaces;
using coreLib.Extensions;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [MyFilter]
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_office_ppmp")]
    public class DepartmentAllocationsController : ProcurementBaseController, IYearController, IMyController, IDepartmentController
    {
        public int Year { get; set; }
        public tblDepartment  Department { get; set; }
        public tblEmployee employee { get; set; }

        public ActionResult Index()
        {
            tblEmployee employee = coreApp.Cache.Get().userAccess.employee;

            ViewBag.Office = employee.Office;

            return View(new AnnualDepartmentAllocationModel(Year, employee.Office.OfficeId));
        }

        [DepartmentInfoFilter]
        public ActionResult DepartmentIndex()
        {
            return View(new DepartmentAllocationModel(Department.DepartmentId, Year));
        }

        [DepartmentInfoFilter]
        public ActionResult Edit(int? deptId)
        {
            if (deptId == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblDepartmentAllocation fund = context.tblDepartmentAllocations.Where(x => x.DepartmentId == deptId && x.Year == Year).SingleOrDefault();
                if (fund == null)
                {
                    fund = new tblDepartmentAllocation
                    {
                        DepartmentId = Department.DepartmentId,                        
                        Year = Year
                    };
                }

                return PartialView("_DepartmentFundAllocation", fund);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblDepartmentAllocation model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                if (model.Amount == null) model.Amount = 0;

                using (procurementDataContext context = new procurementDataContext())
                {
                    tblEmployee employee = coreApp.Cache.Get().userAccess.employee;
                    if (new AnnualDepartmentAllocationModel(model.Year, employee.Office.OfficeId).Balance(model.DepartmentId) - (model.Amount ?? 0) < 0)
                    {
                        AddError("No enough funds");
                    }

                    tblDepartmentAllocation fundAlloc = context.tblDepartmentAllocations.Where(x => x.DepartmentId == model.DepartmentId && x.Year == model.Year).SingleOrDefault();
                    bool isDelete = model.Amount == 0;

                    DepartmentAllocationModel deptModel = new DepartmentAllocationModel(model.DepartmentId, model.Year);
                    double usedAmount = deptModel.UsedAmount();

                    if (ModelState.IsValid)
                    {
                        if (isDelete)
                        {
                            if (fundAlloc != null)
                            {
                                if (usedAmount > 0)
                                {
                                    throw new Exception("Cannot delete record. Submitted Department PPMPs must first be returned");
                                }

                                context.tblDepartmentAllocations.DeleteOnSubmit(fundAlloc);
                            }
                        }
                        else
                        {
                            if (usedAmount > model.Amount)
                            {
                                throw new Exception("Cannot update record. Submitted Department PPMPs already amounted more than the specified value (" + usedAmount.ToCurrencyString() + ").");
                            }

                            if (fundAlloc == null)
                            {
                                fundAlloc = new tblDepartmentAllocation
                                {
                                    DepartmentId = model.DepartmentId,
                                    Year = model.Year,
                                    Amount = model.Amount
                                };

                                context.tblDepartmentAllocations.InsertOnSubmit(fundAlloc);
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