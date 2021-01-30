using System.ComponentModel.DataAnnotations;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblFundMetaData
    {
        
        [Required]
        [Display(Name = "Fund Name")]
        public string Fund { get; set; }

        [Display(Name = "Fund Culster")]
        public string Fund_Cluster { get; set; }
        
    }

    [MetadataType(typeof(tblFundMetaData))]
    public partial class tblFund
    { }
}