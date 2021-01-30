using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class POItemModel
    {
        public tblPOItem Item { get; set; }
        public int Delivered { get; set; }
        public int UnDelivered { get; set; }
        public double DeliveryEquiv { get; set; }
        public bool IsComplete { get; set; }
        public int ReceiptQty { get; set; }

        public POItemModel(tblPOItem Item)
        {
            this.Item = Item;

            using(samDataContext context = new samDataContext())
            {
                List<tblReceiptItem> receiptItems = context.tblReceiptItems.Where(x => x.POItemId == Item.Id).ToList();
                init(receiptItems);
            }
        }

        public POItemModel(tblPOItem Item, List<tblReceiptItem> receiptItems, int exclude_ReceiptId = -1)
        {
            this.Item = Item;
            init(receiptItems, exclude_ReceiptId);
        }

        void init(List<tblReceiptItem> receiptItems, int exclude_ReceiptId = -1)
        {
            var tmp = receiptItems.Where(x => x.POItemId == Item.Id && (exclude_ReceiptId == -1 || x.ReceiptId != exclude_ReceiptId));
            if (tmp.Any())
            {
                Delivered = tmp.Sum(x => x.Qty);
            }
            else
            {
                Delivered = 0;
            }

            UnDelivered = Item.Qty - Delivered;
            
            double _unitCost = Item.Amount / Item.Qty;
            DeliveryEquiv = Delivered * _unitCost;
            
            IsComplete = Delivered >= Item.Qty;

            if (exclude_ReceiptId == -1)
            {
                ReceiptQty = -1;
            }
            else
            {
                var tmp2 = receiptItems.Where(x => x.ReceiptId == exclude_ReceiptId && x.POItemId == Item.Id).SingleOrDefault();
                if (tmp2 == null)
                {
                    ReceiptQty = 0;
                }
                else
                {
                    ReceiptQty = tmp2.Qty;
                }
            }
        }
    }
}