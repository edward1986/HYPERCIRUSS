using coreApp.Areas.Procurement.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblContractorMetaData
    {
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        [Display(Name = "Telephone No.")]
        public string TelephoneNo { get; set; }

        [Required]
        public string Representative { get; set; }

        [Display(Name = "Mobile No.")]
        public string Representative_MobileNo { get; set; }

    }

    [MetadataType(typeof(tblContractorMetaData))]
    public partial class tblContractor
    {
    }
}