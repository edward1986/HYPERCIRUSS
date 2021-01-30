using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Data;
using Module.DB.Procs;

namespace Module.DB
{
    public partial class tblEmployeesMetaData
    {
        [Display(Name = "Id")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee No.")]
        public int IdNo { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Name Ext.")]
        public string NameExt { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public tblDepartment Department { get; set; }

    }

    [MetadataType(typeof(tblEmployeesMetaData))]
    public partial class tblEmployee
    {
        //bool? _IsActive = null;

        public bool IsActive(DateTime? asOf = null)
        {            
            return true;
        }
                    
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return coreLib.Procs.getFullName(LastName, FirstName, MiddleName, NameExt).ToUpper();
            }
        }

        [Display(Name = "Full Name")]
        public string FullName_FN
        {
            get
            {
                return coreLib.Procs.getFullName_FN(LastName, FirstName, MiddleName, NameExt).ToUpper();
            }
        }

        public tblCampus Campus
        {
            get
            {
                tblCampus ret = null;

                tblDepartment department = Procs.Common.Global_CachedTables.Departments.Where(x => x.DepartmentId == DepartmentId).SingleOrDefault();
                if (department != null)
                {
                    tblOffice office = Procs.Common.Global_CachedTables.Offices.Where(x => x.OfficeId == department.OfficeId).SingleOrDefault();

                    ret = Procs.Common.Global_CachedTables.Campuses.Where(x => x.CampusID == office.CampusID).SingleOrDefault();

                }

                return ret;
            }
        }

        public tblOffice Office
        {
            get
            {
                tblOffice ret = null;

                tblDepartment department = Procs.Common.Global_CachedTables.Departments.Where(x => x.DepartmentId == DepartmentId).SingleOrDefault();
                if (department != null)
                {
                    ret = Procs.Common.Global_CachedTables.Offices.Where(x => x.OfficeId == department.OfficeId).SingleOrDefault();
                }

                return ret;
            }
        }

        public string Department
        {
            get
            {
                string ret = "";

                tblDepartment department = Procs.Common.Global_CachedTables.Departments.Where(x => x.DepartmentId == DepartmentId).SingleOrDefault();
                if (department != null)
                {
                    ret = department.Department;
                }

                return ret;
            }
        }

        public static implicit operator tblEmployee(procs_tblEmployee v)
        {
            throw new NotImplementedException();
        }
    }
}