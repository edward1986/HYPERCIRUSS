using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Module.DB;
using Module.DB.Procs;

namespace coreApp
{
    public class UserAccess
    {
        public string userName { get; set; }
        public DateTime? asOfDate { get; set; }

        public AspNetUser user { get; set; }
        public tblEmployee employee { get; set; }
        public List<tblOffice> offices { get; set; } = new List<tblOffice>();
        public List<tblCampus> campuses { get; set; } = new List<tblCampus>();
        public string[] roles { get; set; } = new List<string>().ToArray();

        private tblEmployee_Access access { get; set; }
        private _HRPermission permission { get; set; }

        public bool HasPermission(string controller = "")
        {
            bool ret = false;

            if (IsAdmin || IsHRStaff)
            {
                ret = true;
            }
            else if (permission != null)
            {
                ret = coreProcs.hasPermission(controller, permission.Controllers);
            }

            return ret;
        }

        public bool HasAnyAccess
        {
            get
            {
                bool ret = false;

                if (access != null)
                {

                    PropertyInfo[] pis = access.GetType().GetProperties();

                    foreach (PropertyInfo pi in pis)
                    {
                        if ("AccessId,EmployeeId,campus_scope,office_scope,department_scope".Split(',').Contains(pi.Name)) continue;

                        if (Convert.ToBoolean(pi.GetValue(access)))
                        {
                            ret = true;
                            break;
                        }
                    }
                }

                return ret;
            }
        }

        public bool HasAnyPermission
        {
            get
            {
                bool ret = false;

                if (permission != null)
                {
                    ret = !string.IsNullOrEmpty(permission.Controllers);
                }

                return ret;
            }
        }

        public bool HasAccess(string accessType)
        {
            if (IsAdmin)
            {
                return true;
            }
            else
            {
                return checkAccess(accessType);
            }
        }

        public bool IsAdmin
        {
            get
            {
                return roles.Contains("admin") || checkAccess("system_admin");
            }
        }

        private bool checkAccess(string accessType)
        {
            if (access == null)
            {
                return false;
            }
            else
            {
                PropertyInfo pi = access.GetType().GetProperty(accessType);

                return Convert.ToBoolean(pi.GetValue(access));
            }
        }

        public bool IsEmployee
        {
            get
            {
                return roles.Contains("employee");
            }
        }

        public bool IsStaff
        {
            get
            {
                return IsHRStaff || IsFinanceStaff;
            }
        }

        public bool IsHRStaff
        {
            get
            {
                return HasAccess("hr_module");
            }
        }

        public bool IsFinanceStaff
        {
            get
            {
                return HasAccess("finance_module");
            }
        }

        public UserAccess(tblEmployee employee, DateTime? asOfDate = null)
        {
            user = Module.DB.Procs.Account.GetUser(id: employee.UserId);
            init(asOfDate);
        }

        public UserAccess(string _userName = "", DateTime? asOfDate = null)
        {
            user = Module.DB.Procs.Account.GetUser(userName: _userName == "" ? HttpContext.Current.User.Identity.Name : _userName);
            init(asOfDate);
        }

        private void init(DateTime? asOfDate)
        {
            this.asOfDate = asOfDate;

            if (user != null)
            {
                userName = user.UserName;

                using (coreDataContext context = new coreDataContext())
                {
                    roles = context.AspNetUserRoles.Where(x => x.UserId == user.Id).Join(context.AspNetRoles, a => a.RoleId, b => b.Id, (a, b) => b.Name).ToArray();
                }

                using (dalDataContext context = new dalDataContext())
                {
                    employee = context.tblEmployees.Where(x => x.UserId == user.Id).SingleOrDefault();
                    if (employee != null)
                    {
                        procs_tblEmployee pEmployee = new procs_tblEmployee(employee);


                        access = context.tblEmployee_Accesses.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                        if (access == null)
                        {
                            access = new tblEmployee_Access { EmployeeId = employee.EmployeeId };
                        }

                        permission = context._HRPermissions.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                    }
                }

                offices = OfficeScopes();
                campuses = CampusScopes();
            }
        }

        private List<tblOffice> OfficeScopes()
        {
            List<tblOffice> ret = new List<tblOffice>();

            if (IsAdmin)
            {
                ret = Cache.Get_Tables().Offices.ToList();
            }
            else
            {

                if (access != null)
                {
                    ret.AddRange(Cache.Get_Tables().Offices.ToList().Where(x => (access.office_scope ?? "").Split(',').Contains(x.OfficeId.ToString())).Select(x => x).ToList());
                }

            }

            return ret;
        }

        private List<tblCampus> CampusScopes()
        {
            List<tblCampus> ret = new List<tblCampus>();

            if (IsAdmin)
            {
                ret = Cache.Get_Tables().Campuses.ToList();
            }
            else
            {

                if (access != null)
                {
                    ret.AddRange(Cache.Get_Tables().Campuses.ToList().Where(x => (access.campus_scope ?? "").Split(',').Contains(x.CampusID.ToString())).Select(x => x).ToList());
                }

            }

            return ret;
        }

        public bool AllowedOffice(int officeId)
        {
            return IsAdmin || offices.Any(x => x.OfficeId == officeId);
        }

        public bool AllowedCampus(int campusId)
        {
            return IsAdmin || campuses.Any(x => x.CampusID == campusId);
        }

        public bool IsMe(int employeeId)
        {
            return employee.EmployeeId == employeeId;
        }

        public bool AllowedEmployee(int employeeId)
        {
            bool ret = false;

            if (IsAdmin || IsMe(employeeId))
            {
                ret = true;
            }
            else if (IsStaff)
            {
                using (dalDataContext context = new dalDataContext())
                {
                    tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
                    if (employee != null)
                    {
                        procs_tblEmployee pEmployee = new procs_tblEmployee(employee);

                        ret = offices.Any(x => pEmployee.IsInOffice(x.OfficeId, includeUnassigned: true));

                    }
                }
            }

            return ret;
        }
        public bool AllowedEmployeeCampus(int employeeId, tblCampus campus)
        {
            bool ret = false;

            if (IsAdmin || IsMe(employeeId))
            {
                ret = true;
            }
            else if (IsStaff)
            {
                using (dalDataContext context = new dalDataContext())
                {
                    tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
                    if (employee != null)
                    {
                        procs_tblEmployee pEmployee = new procs_tblEmployee(employee);

                        ret = campuses.Any(x => pEmployee.IsInCampus(x.CampusID, includeUnassigned: true));

                    }
                }
            }

            return ret;
        }

     
    }
}