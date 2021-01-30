using Module.DB;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblOfficeAllocationMetaData
    {
        [Range(1, 99999, ErrorMessage = "The Office field is required")]
        [Display(Name = "Office")]
        public int OfficeId { get; set; }

        [Range(1, 99999, ErrorMessage = "The Year field is required")]
        public int Year { get; set; }

        [Range(1, 99999, ErrorMessage = "The Fund field is required")]
        [Display(Name = "Fund")]
        public int FundId { get; set; }
        
        [Display(Name = "Amount")]
        [Range(0, 9999999999, ErrorMessage = "Invalid Amount value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Amount { get; set; }
    }

    [MetadataType(typeof(tblOfficeAllocationMetaData))]
    public partial class tblOfficeAllocation
    {
        public tblOffice Office
        {
            get
            {
                return Cache.Get_Tables().Offices.Where(x => x.OfficeId == OfficeId).SingleOrDefault();
            }
        }

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