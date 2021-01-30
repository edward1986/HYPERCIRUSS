using System.ComponentModel.DataAnnotations;

namespace Module.DB
{
    public partial class _HRPermissionMetaData
    {
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Modules")]
        public string Controllers { get; set; }

    }

    [MetadataType(typeof(_HRPermissionMetaData))]
    public partial class _HRPermission
    {
        
    }

   
}