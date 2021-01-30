using coreApp.Areas.Procurement;
using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using Module.DB;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public partial class tblInventoryInspectionMetaData
    {        
        [Required]
        [Display(Name = "Report Date")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime ReportDate { get; set; }

        [Range(1, 99999, ErrorMessage = "The Inspected By field is required")]
        [Display(Name = "Inspected By")]
        public int EmployeeId { get; set; }
    }

    [MetadataType(typeof(tblInventoryInspectionMetaData))]
    public partial class tblInventoryInspection
    {
        [Display(Name = "Inspected By")]
        public string InspectedBy
        {
            get
            {
                string ret = "";
                tblEmployee employee = GetEmployee();
                if (employee != null)
                {
                    ret = employee.FullName;
                }
                return ret;
            }
        }

        public tblEmployee GetEmployee()
        {
            using(dalDataContext context = new dalDataContext())
            {
                return context.tblEmployees.Where(x => x.EmployeeId == EmployeeId).SingleOrDefault();
            }
        }
    }
}