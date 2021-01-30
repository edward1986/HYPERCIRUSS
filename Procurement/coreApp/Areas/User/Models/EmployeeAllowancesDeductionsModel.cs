using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace coreApp.Models
{

    public class EmployeeAllowancesDeductionsModel
    {
        public List<tblEmployee_Allowance> Allowances { get; set; }
        public List<tblEmployee_Deduction> Deductions { get; set; }
    }
}