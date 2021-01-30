using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Module.DB
{
    public partial class tblDepartmentMetaData
    {
        [Display(Name = "Id")]
        public int DepartmentId { get; set; }

        [Required]
        [Range(1, 99999, ErrorMessage = "The Office field is required")]
        [Display(Name = "Office")]
        public int OfficeId { get; set; }

        [Required]
        public string Department { get; set; }

    }

    [MetadataType(typeof(tblDepartmentMetaData))]
    public partial class tblDepartment
    {
        public tblOffice Office
        {
            get
            {
                return Procs.Common.Global_CachedTables.Offices.Where(x => x.OfficeId == OfficeId).SingleOrDefault();
            }
        }
    }
}