using coreApp.Areas.Procurement;
using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using Module.DB;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public partial class tblInventoryItemStatusMetaData
    {        
    }

    [MetadataType(typeof(tblInventoryItemStatusMetaData))]
    public partial class tblInventoryItemStatus
    {
        [Display(Name = "Search Property No.")]
        public string PropertyNo_Search { get; set; }

        public string Status_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(InventoryItemStatus), Status);
            }
        }
    }
}