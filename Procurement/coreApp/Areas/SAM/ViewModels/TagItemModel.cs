using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class TagItemModel
    {
        public tblReceipt Receipt { get; set; }
        public tblReceiptItem ReceiptItem { get; set; }
        public tblPOItem POItem { get; set; }

        public int Tagged
        {
            get
            {
                using(samDataContext context = new samDataContext())
                {
                    return context.tblInventoryItems.Where(x => x.ReceiptItemId == ReceiptItem.Id).Count();
                }
            }
        }

        public bool IsComplete
        {
            get
            {
                return Tagged >= ReceiptItem.Qty;
            }
        }
    }
}