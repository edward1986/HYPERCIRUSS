using coreApp.DAL;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblFundAllocationMetaData
    {
        [Range(0, 99999, ErrorMessage = "The Fund field is required")]
        [Display(Name = "Fund")]
        public int FundId { get; set; }

        [Range(0, 99999, ErrorMessage = "The Year field is required")]
        public int Year { get; set; }
        
        [Display(Name = "Amount")]
        [Range(0, 9999999999, ErrorMessage = "Invalid Amount value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Amount { get; set; }
    }

    [MetadataType(typeof(tblFundAllocationMetaData))]
    public partial class tblFundAllocation
    {

        public tblFund Fund
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblFunds.Where(x => x.Id == FundId).SingleOrDefault();
                }
            }
        }

    }
}