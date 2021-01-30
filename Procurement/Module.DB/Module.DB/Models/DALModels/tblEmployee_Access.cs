using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Module.DB
{

    public partial class tblEmployee_AccessMetaData
    {
        [Display(Name = "Id")]
        public int AccessId { get; set; }

        [Display(Name = "System Administrator")]
        public bool system_admin { get; set; }

        [Display(Name = "HR Module")]
        public bool hr_module { get; set; }

        [Display(Name = "Set configurations")]
        public bool hr_config { get; set; }

        [Display(Name = "Access to devices")]
        public bool hr_devices { get; set; }

        [Display(Name = "Define holidays")]
        public bool hr_holidays { get; set; }

        [Display(Name = "Access confidential employee records")]
        public bool hr_emp_confidential { get; set; }

        [Display(Name = "Access attendance monitoring")]
        public bool hr_att { get; set; }

        [Display(Name = "Access bulletin board")]
        public bool hr_bulletinboard { get; set; }

        [Display(Name = "Access employee records")]
        public bool hr_emp { get; set; }

        [Display(Name = "Define employee access")]
        public bool hr_emp_access { get; set; }

        [Display(Name = "Access employee personal information")]
        public bool hr_emp_info { get; set; }

        [Display(Name = "Access employee career")]
        public bool hr_emp_career { get; set; }

        [Display(Name = "Access employee schedules")]
        public bool hr_emp_sched { get; set; }

        [Display(Name = "Access employee time logs (Can modify Mode)")]
        public bool hr_emp_timelogs { get; set; }

        [Display(Name = "Modify employee time logs")]
        public bool hr_emp_timelogs_modify { get; set; }

        [Display(Name = "Access employee leave credits")]
        public bool hr_emp_leave { get; set; }

        [Display(Name = "Access employee leave applications")]
        public bool hr_emp_leave_app { get; set; }

        [Display(Name = "Access employee travel records")]
        public bool hr_emp_travel { get; set; }

        [Display(Name = "Access employee travel applications")]
        public bool hr_emp_travel_app { get; set; }

        [Display(Name = "Access employee overtime records")]
        public bool hr_emp_ot { get; set; }

        [Display(Name = "Access employee Overtime Application")]
        public bool hr_emp_ot_app { get; set; }

        [Display(Name = "Access payroll summaries")]
        public bool hr_ps { get; set; }

        [Display(Name = "Set HR permissions")]
        public bool hr_permissions { get; set; }

        [Display(Name = "Access employee login account")]
        public bool hr_emp_login_account { get; set; }

        [Display(Name = "Campus scope")]
        public string campus_scope { get; set; }

        [Display(Name = "Office scope")]
        public string office_scope { get; set; }

       [Display(Name = "Leave Module Settings")]
        public bool leave_module_settings { get; set; }

        [Display(Name = "Leave Module")]
        public bool leave_module { get; set; }

        [Display(Name = "Finance Module")]
        public bool finance_module { get; set; }

        [Display(Name = "Access payroll")]
        public bool finance_ps { get; set; }

        [Display(Name = "Define allowances/deductions")]
        public bool finance_definitions { get; set; }

        [Display(Name = "Define/Access employee loans")]
        public bool finance_employee_loans { get; set; }

        [Display(Name = "Unpost Payroll Summaries")]
        public bool finance_module_unpost_ps { get; set; }

        [Display(Name = "Procurement System")]
        public bool procurement { get; set; }

        [Display(Name = "Modify Settings")]
        public bool procurement_settings { get; set; }

        [Display(Name = "Allocate Funds")]
        public bool procurement_allocate_funds { get; set; }

        [Display(Name = "Access Department PPMP")]
        public bool procurement_access_department_ppmp { get; set; }

        [Display(Name = "Access Office PPMP")]
        public bool procurement_access_office_ppmp { get; set; }

        [Display(Name = "Access Agency APP")]
        public bool procurement_access_app { get; set; }

        [Display(Name = "Manage Items")]
        public bool procurement_item_settings { get; set; }

        [Display(Name = "Approve Submitted PPMPs")]
        public bool procurement_ppmp_approver { get; set; }

        [Display(Name = "Supply/Asset Management System")]
        public bool sam { get; set; }

        [Display(Name = "Modify Settings")]
        public bool sam_settings { get; set; }

        [Display(Name = "Receiving")]
        public bool sam_receiving { get; set; }

        [Display(Name = "Tagging")]
        public bool sam_tagging { get; set; }

        [Display(Name = "Inspection")]
        public bool sam_inspection { get; set; }

        [Display(Name = "Transactions")]
        public bool sam_transactions { get; set; }

        [Display(Name = "Monitoring")]
        public bool sam_monitoring { get; set; }

        [Display(Name = "Equipment Inspection")]
        public bool sam_inspection_equip { get; set; }

    }

    [MetadataType(typeof(tblEmployee_AccessMetaData))]
    public partial class tblEmployee_Access
    {
        public List<tblCampus> getCampuses()
        {
            return Procs.Common.Global_CachedTables.Campuses.Where(x => (campus_scope ?? "").Split(',').Contains(x.CampusID.ToString())).OrderBy(x => x.Campus).ToList();
        }

        public List<tblOffice> getOffices()
        {
            return Procs.Common.Global_CachedTables.Offices.Where(x => (office_scope ?? "").Split(',').Contains(x.OfficeId.ToString())).OrderBy(x => x.Office).ToList();
        }

        public void Remove_SAMS_Access()
        {
            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee_Access access = context.tblEmployee_Accesses.Where(x => x.AccessId == AccessId).SingleOrDefault();
                if (access != null)
                {
                    access.sam = false;
                    access.sam_settings = false;
                    access.sam_receiving = false;
                    access.sam_tagging = false;
                    access.sam_inspection = false;
                    access.sam_inspection_equip = false;
                    access.sam_transactions = false;
                    access.sam_monitoring = false;

                    context.SubmitChanges();
                }
            }
        }
    }


}