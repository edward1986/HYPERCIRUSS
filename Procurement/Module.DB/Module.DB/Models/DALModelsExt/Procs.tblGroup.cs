using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.DB.Procs
{
    public class procs_tblGroup
    {
        tblGroup group;

        public procs_tblGroup(tblGroup group)
        {
            this.group = group;
        }

        public List<tblEmployee> Members()
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblEmployee_Groups
                    .Where(x => x.GroupId == group.Id)
                    .Join(context.tblEmployees, a => a.EmployeeId, b => b.EmployeeId, (a, b) => b)
                    .ToList();
            }
        }
    }
}