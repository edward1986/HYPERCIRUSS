using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblReceiptItemMetaData
    {
       
    }

    [MetadataType(typeof(tblReceiptItemMetaData))]
    public partial class tblReceiptItem
    {

        public string GeneratePropertyNo()
        {
            using (samDataContext context = new samDataContext())
            {
                tblReceipt receipt = context.tblReceipts.Where(x => x.Id == ReceiptId).SingleOrDefault();
                List<tblInventoryItem> existing = context.tblInventoryItems.Where(x => x.ReceiptItemId == Id).ToList();

                int itemNo = 1;
                while (existing.Any(x => x.ItemNo == itemNo))
                {
                    itemNo++;
                }

                return string.Format("{0}-{1}-{2}-{3}", receipt._PONo, receipt.ReceivedDate.ToString("yyyyMMdd"), Id, itemNo);
            }            
        }

        public tblPOItem GetPOItem()
        {
            using(samDataContext context = new samDataContext())
            {
                return context.tblPOItems.Where(x => x.Id == POItemId).SingleOrDefault();
            }
        }
    }
}