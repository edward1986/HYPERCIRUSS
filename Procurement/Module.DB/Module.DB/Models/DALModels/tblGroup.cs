using System.ComponentModel.DataAnnotations;

namespace Module.DB
{

    public partial class tblGroupsMetaData
    {
        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

    }

    [MetadataType(typeof(tblGroupsMetaData))]
    public partial class tblGroup
    {
        
    }
}