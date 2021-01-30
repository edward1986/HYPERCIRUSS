using Module.DB;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblDepartmentAllocationMetaData
    {
        [Range(1, 99999, ErrorMessage = "The Department field is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Range(1, 99999, ErrorMessage = "The Year field is required")]
        public int Year { get; set; }
                
        [Display(Name = "Amount")]
        [Range(0, 9999999999, ErrorMessage = "Invalid Amount value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Amount { get; set; }
    }

    [MetadataType(typeof(tblDepartmentAllocationMetaData))]
    public partial class tblDepartmentAllocation
    {
        public tblDepartment  Department
        {
            get
            {
                return Cache.Get_Tables().Departments.Where(x => x.DepartmentId == DepartmentId).SingleOrDefault();
            }
        }
    }
}