using coreApp.Areas.SAM.Enums;
using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class POModel
    {
        public tblPO PO { get; set; }
        public List<POItemModel> Items { get; set; }
        public bool IsComplete { get; set; }
        public double DeliveryPercentage { get; set; }
        public POStatus Status { get; set; }
        
        public double Amount { get; set; }
        
        public List<tblReceiptItem> ReceiptItems;
        
        public POModel(tblPO PO, int exclude_ReceiptId = -1)
        {
            this.PO = PO;

            using (samDataContext context = new samDataContext())
            {
                List<tblPOItem> poItems = context.tblPOItems.Where(x => x.POId == PO.Id).ToList();

                ReceiptItems = context.tblReceiptItems.ToList()
                    .Join(poItems, a => a.POItemId, b => b.Id, (a, b) => a)
                    .ToList();

                Items = poItems.Select(x => new POItemModel(x, ReceiptItems, exclude_ReceiptId)).ToList();

                DeliveryPercentage = Items.Sum(x => x.DeliveryEquiv) / Items.Sum(x => x.Item.Amount);
                IsComplete = DeliveryPercentage >= 1;

                Status = IsComplete ? POStatus.Complete :
                    Items.Any(x => x.Delivered > 0) ? POStatus.Partial :
                    POStatus.Undelivered;


                Amount = Items.Sum(x => x.Item.Amount);
            }
        }

        public string Status_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(POStatus), Status);
            }
        }
    }
}