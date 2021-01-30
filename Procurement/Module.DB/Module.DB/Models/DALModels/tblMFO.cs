using System.ComponentModel.DataAnnotations;

namespace Module.DB
{

    public partial class tblMFOsMetaData
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
    }

    [MetadataType(typeof(tblMFOsMetaData))]
    public partial class tblMFO
    { }
}