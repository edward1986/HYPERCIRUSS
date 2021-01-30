using coreApp.Areas.Procurement.DAL;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblItemLifeMetaData
    {
        [Display(Name = "No. of Months")]
        public string NoOfMonths { get; set; }
    }

    [MetadataType(typeof(tblItemLifeMetaData))]
    public partial class tblItemLife
    {
        
    }
}