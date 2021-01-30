using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace UareU.Common
{
    public partial class tblEmployee
    {
        public string FullName
        {
            get
            {
                string ret = "";

                if (!string.IsNullOrEmpty(LastName) || !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(MiddleName) || !string.IsNullOrEmpty(NameExt))
                {
                    string mn = "";
                    if (!string.IsNullOrEmpty(MiddleName))
                    {
                        List<string> _mn = MiddleName.Split(' ').ToList();
                        mn = " " + _mn.Last().ToString()[0].ToString() + ".";
                    }

                    ret = string.Format("{0}{1}, {2}{3}",
                        LastName,
                        string.IsNullOrEmpty(NameExt) ? "" : (" " + NameExt),
                        FirstName,
                        mn
                        );
                }
                return ret;
            }
        }

        public tblEmployee_Career LatestCareer
        {
            get
            {
                tblEmployee_Career ret = null;

                using (dalDataContext context = new dalDataContext())
                {
                    ret = context.tblEmployee_Careers.Where(x => x.EmployeeId == EmployeeId).OrderByDescending(x => x.DateEffective).ThenByDescending(x => x.CareerId).FirstOrDefault();
                }

                return ret;
            }
        }
    }
}