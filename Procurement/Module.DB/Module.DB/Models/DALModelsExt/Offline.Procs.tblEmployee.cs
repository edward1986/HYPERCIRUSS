using System;
using System.Linq;

namespace Module.DB.Procs
{
    public class offline_procs_tblEmployee : procs_tblEmployee
    {
        OfflineDataSource offlineDS;
        bool useOffline
        {
            get
            {
                return offlineDS != null;
            }
        }

        tblEmployee employee;

        public offline_procs_tblEmployee(tblEmployee employee, ref OfflineDataSource offlineDS) : base(employee)
        {
            this.offlineDS = offlineDS;
            this.employee = employee;
        }
              

        public override string BIRStatus()
        {
            string ret = "S";

            vwEmployee_Info_NoPhoto info = offlineDS.EmployeeInfos.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
            if (info != null)
            {
                ret = info.BIRStatus;
            }

            return ret;
        }

        public override tblEmployee_Access Access()
        {
            return offlineDS.Accesses.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault() ?? new tblEmployee_Access { EmployeeId = employee.EmployeeId };
        }

        public override _HRPermission Permissions()
        {
            return offlineDS.Permissions.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault() ?? new _HRPermission { EmployeeId = employee.EmployeeId };
        }
    }
}
