using System.ComponentModel.DataAnnotations;

namespace Module.DB
{
    public partial class tblPositionsMetaData
    {
        [Display(Name = "Id")]
        public int PositionId { get; set; }

        [Required]
        public string Position { get; set; }

        [Display(Name = "Faculty")]
        public bool IsFaculty { get; set; }
    }

    [MetadataType(typeof(tblPositionsMetaData))]
    public partial class tblPosition
    { }
}