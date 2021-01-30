using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Module.DB
{

    public partial class tblOfficesMetaData
    {
        [Display(Name = "Id")]
        public int OfficeId { get; set; }

        [Required]
        public string Office { get; set; }
               
        [Display(Name = "Contact Number")]
        public string TelephoneNo { get; set; }

        [Display(Name = "Responsibility Center Code")]
        public string RCC { get; set; }
              

        [Required]
        [Range(1, 99999, ErrorMessage = "The Campus field is required")]
        [Display(Name = "Campus ID")]
        public int CampusID { get; set; }

    }

    [MetadataType(typeof(tblOfficesMetaData))]
    public partial class tblOffice
    {
       
        public tblCampus Campus
        {
            get
            {
                return Procs.Common.Global_CachedTables.Campuses.Where(x => x.CampusID == CampusID).SingleOrDefault();
            }
        }

        public string OfficeAddress
        {
            get
            {
                return Procs.Common.Global_CachedTables.Campuses.Where(x => x.CampusID == CampusID).Select(x => x.Campus).Single() + ", " + Procs.Common.Global_CachedTables.Campuses.Where(x => x.CampusID == CampusID).Select(x => x.Address).Single();
            }
        }
    }
}