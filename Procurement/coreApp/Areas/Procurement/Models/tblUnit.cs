using System.ComponentModel.DataAnnotations;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblUnitMetaData
    {
        
        [Required]
        public string Unit { get; set; }
        
    }

    [MetadataType(typeof(tblUnitMetaData))]
    public partial class tblUnit
    { }
}