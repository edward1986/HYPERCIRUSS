using System;
using System.Linq;

namespace Module.DB.Procs
{
    public class procs_tblEmployee
    {
        tblEmployee employee;

        public procs_tblEmployee(tblEmployee employee)
        {
            this.employee = employee;
        }


        public virtual tblEmployee_Info Info()
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblEmployee_Infos.Where(x => x.EmployeeId == employee.EmployeeId).OrderBy(x => x.InfoId).FirstOrDefault();
            }
        }

        public bool IsInOffice(int officeId, DateTime? asOfDate = null, bool includeUnassigned = false)
        {
            bool ret = false;


            tblOffice office = employee.Office;

            if (office == null)
            {
                ret = includeUnassigned;
            }
            else
            {
                ret = office.OfficeId == officeId;
            }


            return ret;
        }
        public bool IsInCampus(int campusId, DateTime? asOfDate = null, bool includeUnassigned = false)
        {
            bool ret = false;

            tblCampus campus = employee.Campus;

            if (campus == null)
            {
                ret = includeUnassigned;
            }
            else
            {
                ret = campus.CampusID == campusId;
            }


            return ret;
        }

        
        public bool IsInDepartment(int departmentId, DateTime? asOfDate = null, bool includeUnassigned = false)
        {
            bool ret = false;

           
                tblDepartment department = Common.Global_CachedTables.Departments.Where(x => x.DepartmentId == employee.DepartmentId).SingleOrDefault();

                if (department == null)
                {
                    ret = includeUnassigned;
                }
                else
                {
                    ret = employee.DepartmentId == departmentId;
                }
            

            return ret;
        }
              
        public bool IsInGroup(int groupId)
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblEmployee_Groups.Any(x => x.GroupId == groupId && x.EmployeeId == employee.EmployeeId);
            }
        }

        public virtual string BIRStatus()
        {
            string ret = "S";

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee_Info info = context.tblEmployee_Infos.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault();
                if (info != null)
                {
                    ret = info.BIRStatus;
                }
            }

            return ret;
        }

        public virtual tblEmployee_Access Access()
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblEmployee_Accesses.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault() ?? new tblEmployee_Access { EmployeeId = employee.EmployeeId };
            }
        }

        public virtual _HRPermission Permissions()
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context._HRPermissions.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault() ?? new _HRPermission { EmployeeId = employee.EmployeeId };
            }
        }
    }
}
