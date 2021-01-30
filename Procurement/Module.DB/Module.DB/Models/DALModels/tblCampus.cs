using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Module.DB
{

    public partial class tblCampusMetaData
    {
        [Display(Name = "Id")]
        public int CampusID { get; set; }

        [Required]
        public string Campus { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Country")]
        [Range(1, 999999, ErrorMessage = "The Country field is required")]
        public int CountryID { get; set; }

        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

       

    }

    [MetadataType(typeof(tblCampusMetaData))]
    public partial class tblCampus
    {
        [Display(Name = "Address")]
        public string CampusAddress
        {
            get
            {
                return Procs.Common.getAddress(Address, CountryID, PostalCode);
            }
        }

    }
}