using coreApp.Controllers;
using coreApp.DAL;
using coreApp.Filters;
using coreApp.Interfaces;
using Module.Core;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.User.Controllers
{
    [EmployeeInfoFilter]
    [EmployeeAuthorize]
    public class EmployeeAccessController : Base_NoCoreHRStaffController, IEmployeeController
    {
        public tblEmployee employee { get; set; }

        public ActionResult Edit()
        {

            ViewBag.Title = "Access List";
            ViewBag.Subtitle = "Edit";

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee_Access model = context.tblEmployee_Accesses.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                if (model == null)
                {
                    model = new tblEmployee_Access
                    {
                        EmployeeId = employee.EmployeeId
                    };
                }

                return PartialView("_Access", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string[] Access, tblEmployee_Access model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (Access == null)
            {
                Access = new string[] { };
            }

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    if (!Cache.Get().userAccess.HasAccess("hr_emp_access"))
                    {
                        AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                    }

                    if (Access.Contains("sam") && (employee.WarehouseId == null || employee.WarehouseId == -1))
                    {
                        AddError("Please specify Warehouse before adding SAMS access");
                    }

                    if (ModelState.IsValid)
                    {
                        tblEmployee_Access access = context.tblEmployee_Accesses.Where(x => x.AccessId == model.AccessId).SingleOrDefault();
                        if (access == null)
                        {
                            access = new tblEmployee_Access
                            {
                                EmployeeId = employee.EmployeeId,
                                office_scope = ""
                                //office_scope = "",
                                //department_scope =""
                            };
                            context.tblEmployee_Accesses.InsertOnSubmit(access);
                        }

                        if (Cache.Get().userAccess.IsAdmin)
                        {
                            access.system_admin = Access.Contains("system_admin");
                        }

                        access.hr_config = Access.Contains("hr_config");
                        access.hr_devices = Access.Contains("hr_devices");
                        access.hr_bulletinboard = Access.Contains("hr_bulletinboard");
                        access.hr_module = Access.Contains("hr_module");
                        access.hr_emp_login_account = Access.Contains("hr_emp_login_account");
                        access.hr_emp_access = Access.Contains("hr_emp_access");
                        access.hr_emp_confidential = Access.Contains("hr_emp_confidential");
                        access.hr_emp_timelogs_modify = Access.Contains("hr_emp_timelogs_modify");
                        access.hr_permissions = Access.Contains("hr_permissions");
                        access.hr_emp_leave = Access.Contains("hr_emp_leave");
                        access.hr_emp_leave_app = Access.Contains("hr_emp_leave_app");
                        access.hr_emp_travel = Access.Contains("hr_emp_travel");
                        access.hr_emp_travel_app = Access.Contains("hr_emp_travel_app");
                        access.hr_emp_ot = Access.Contains("hr_emp_ot");
                        access.hr_emp_ot_app = Access.Contains("hr_emp_ot_app");
                        access.hr_emp_career = Access.Contains("hr_emp_career");
                        access.hr_emp_info = Access.Contains("hr_emp_info");
                        access.hr_holidays = Access.Contains("hr_holidays");
                        access.hr_ps = Access.Contains("hr_ps");
                        access.finance_module = Access.Contains("finance_module");
                        access.leave_module = Access.Contains("leave_module");
                        access.leave_module_settings = Access.Contains("leave_module_settings");
                        access.finance_ps = Access.Contains("finance_ps");
                        access.finance_employee_loans = Access.Contains("finance_employee_loans");
                        access.finance_definitions = Access.Contains("finance_definitions");

                        access.procurement = Access.Contains("procurement");
                        access.procurement_settings = Access.Contains("procurement_settings");
                        access.procurement_item_settings = Access.Contains("procurement_item_settings");
                        access.procurement_allocate_funds = Access.Contains("procurement_allocate_funds");
                        access.procurement_access_department_ppmp = Access.Contains("procurement_access_department_ppmp");
                        access.procurement_access_office_ppmp = Access.Contains("procurement_access_office_ppmp");
                        access.procurement_access_app = Access.Contains("procurement_access_app");
                        access.procurement_ppmp_approver = Access.Contains("procurement_ppmp_approver");

                        access.sam = Access.Contains("sam");
                        access.sam_settings = Access.Contains("sam_settings");
                        access.sam_receiving = Access.Contains("sam_receiving");
                        access.sam_tagging = Access.Contains("sam_tagging");
                        access.sam_inspection = Access.Contains("sam_inspection");
                        access.sam_inspection_equip = Access.Contains("sam_inspection_equip");
                        access.sam_transactions = Access.Contains("sam_transactions");
                        access.sam_monitoring = Access.Contains("sam_monitoring");

                        context.SubmitChanges();

                        res.Remarks = "Access list was successfully updated";

                        Cache.Update();
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

        public ActionResult EditOffices()
        {
            ViewBag.Title = "Office Scope";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (dalDataContext context = new dalDataContext())
            {
                return PartialView("_OfficeScope", new UserAccess(employee).offices);
            }
        }

        [HttpPost]
        public ActionResult EditOffices(string[] CampusScopeSelection, string[] OfficeScopeSelection, FormCollection model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            if (CampusScopeSelection == null)
            {
                CampusScopeSelection = new string[] { };
            }
            
            if (OfficeScopeSelection == null)
            {
                OfficeScopeSelection = new string[] { };
            }

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    if (!Cache.Get().userAccess.HasAccess("hr_emp_access"))
                    {
                        AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                    }

                    if (ModelState.IsValid)
                    {
                        tblEmployee_Access access = context.tblEmployee_Accesses.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                        if (access == null)
                        {
                            access = new tblEmployee_Access
                            {
                                EmployeeId = employee.EmployeeId
                            };
                            context.tblEmployee_Accesses.InsertOnSubmit(access);
                        }

                        access.campus_scope = string.Join(",", CampusScopeSelection);
                        access.office_scope = string.Join(",", OfficeScopeSelection);
                        context.SubmitChanges();

                        //access.department_scope = string.Join(",", DepartmentScopeSelection);

                        res.Remarks = "Office scope was successfully updated";

                        Cache.Update();
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
