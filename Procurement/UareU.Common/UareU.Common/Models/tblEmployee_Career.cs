using System;
using System.Linq;

namespace UareU.Common
{
    
    public partial class tblEmployee_Career
    {
      

        public tblOffice Office
        {
            get
            {
                tblOffice ret = null;

                using (dalDataContext context = new dalDataContext())
                {
                    tblDepartment department = context.tblDepartments.Where(x => x.DepartmentId == this.DepartmentId).SingleOrDefault();
                    if (department != null)
                    {
                        ret = context.tblOffices.Where(x => x.OfficeId == department.OfficeId).SingleOrDefault();
                    }
                }

                return ret;
            }
        }

        public string Department
        {
            get
            {
                string ret = "";

                using (dalDataContext context = new dalDataContext())
                {
                    tblDepartment department = context.tblDepartments.Where(x => x.DepartmentId == this.DepartmentId).SingleOrDefault();
                    if (department != null)
                    {
                        ret = department.Department;
                    }
                }

                return ret;
            }
        }

        public tblPosition Position
        {
            get
            {
                using (dalDataContext context = new dalDataContext())
                {
                    return context.tblPositions.Where(x => x.PositionId == this.PositionId).SingleOrDefault();
                }
            }
        }
        
    }
}