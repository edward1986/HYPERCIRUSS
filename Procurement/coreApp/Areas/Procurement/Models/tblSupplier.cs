using coreApp.Areas.Procurement.Enums;
using System.ComponentModel.DataAnnotations;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblSupplierMetaData
    {
        
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Telephone No.")]
        public string TelephoneNo { get; set; }

        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

    }

    [MetadataType(typeof(tblSupplierMetaData))]
    public partial class tblSupplier
    {
        public bool HasNotes
        {
            get
            {
                return !string.IsNullOrEmpty(Notes);
            }
        }

        public string VAT_Desc
        {
            get
            {
                string ret = "";

                if (VAT != null)
                {
                    ret = System.Enum.GetName(typeof(VAT), VAT);
                }

                return ret;
            }
        }
    }
}