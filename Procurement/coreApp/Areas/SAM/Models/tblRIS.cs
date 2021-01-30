using coreApp.Areas.Procurement.DAL;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblRISMetaData
    {
        [Required]
        [Display(Name = "Date")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime RequisitionDate { get; set; }

        [Display(Name = "Employee")]
        [Range(1, 99999, ErrorMessage = "The Employee field is required")]
        public int EmployeeId { get; set; }

        [Display(Name = "Position")]
        public string _Position { get; set; }

        [Display(Name = "Office")]
        public string _Office { get; set; }

        [Display(Name = "Department")]
        public string _Department { get; set; }

        [Display(Name = "RIS No.")]
        public string _RISNo { get; set; }
    }

    [MetadataType(typeof(tblRISMetaData))]
    public partial class tblRI
    {
        public void UpdateRISNo()
        {
            using (samDataContext context = new samDataContext())
            {
                tblRI item = context.tblRIs.Where(x => x.Id == Id).Single();
                item._RISNo = string.Format("{0}-{1}", RequisitionDate.Year.ToString(), Id.ToString("0000"));

                context.SubmitChanges();
            }

        }

        public void UpdateFields()
        {
            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == EmployeeId).Single();
                
                _EmployeeName = employee.FullName_FN;
                _Office = employee.Office.Office;
                _OfficeId = employee.Office.OfficeId;
                _Department = employee.Department;
                _RCC = employee.Office.RCC;
            }
        }
        
    }
}