using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace coreApp.Areas.SAM
{

    public partial class tblInventoryItemMetaData
    {
        [Required]
        [Display(Name = "Property No.")]
        public string PropertyNo { get; set; }

        [Display(Name = "Serial No.")]
        public string SerialNo { get; set; }

        [Display(Name = "Model")]
        public string ModelNo { get; set; }
        
    }

    [MetadataType(typeof(tblInventoryItemMetaData))]
    public partial class tblInventoryItem
    {
        public void UpdateFields()
        {
            using (samDataContext context = new samDataContext())
            {
                tblPOItem item = context.tblReceiptItems.Where(x => x.Id == ReceiptItemId)
                    .Join(context.tblPOItems, a => a.POItemId, b => b.Id, (a, b) => b)
                    .Single();

                _ItemName = item.ItemName;
                _IsSemiExpendable = item.IsSemiExpendable;
            }

        }

        public int ItemNo
        {
            get
            {
                int ret = 0;

                if (!string.IsNullOrEmpty(PropertyNo))
                {
                    string[] tmp = PropertyNo.Split('-');
                    ret = int.Parse(tmp[tmp.Length - 1]);
                }

                return ret;
            }
        }

        public string LastIssuedTo
        {
            get
            {
                string ret = "";

                InventoryItemModel iim = new InventoryItemModel(this);

                if (iim.LatestLog != null)
                {
                    if (iim.LatestLog._AREType == (int)AREType.Return)
                    {
                        ret = "RETURNED TO CUSTODIAN";
                    }
                    else
                    {
                        ret = iim.LatestLog._To_Name + (iim.LatestLog.ToContractor ? " (Contractor)" : "");
                    }
                }
                
                return ret;
            }
        }
    }
}