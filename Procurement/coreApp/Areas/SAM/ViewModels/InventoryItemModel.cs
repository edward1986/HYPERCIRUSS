using coreApp.Areas.SAM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class InventoryItemModel
    {
        public tblInventoryItem Item { get; set; }
        public TagItemModel Tag { get; set; }
        public List<tblARE> Logs { get; set; }
        public tblARE LatestLog { get; set; }
        public InventoryItemStatusModel LatestInspection { get; set; }
        
        public InventoryItemModel(int inventoryItemId)
        {
            using (samDataContext context = new samDataContext())
            {
                tblInventoryItem item = context.tblInventoryItems.Where(x => x.Id == inventoryItemId).Single();
                this.Item = item;

                init();
            }
        }

        public InventoryItemModel(tblInventoryItem Item)
        {
            this.Item = Item;

            init();
        }

        private void init()
        {
            Tag = Common.GetTaggingModels(ReceiptItemId: Item.ReceiptItemId).FirstOrDefault();

            using (samDataContext context = new samDataContext())
            {
                Logs = context.tblAREItems.Where(x => x.InventoryItemId == Item.Id)
                        .Join(context.tblAREs, a => a.AREId, b => b.Id, (a, b) => b)
                        .OrderBy(x => x.AREDate)
                        .ToList();

                LatestLog = Logs
                    .OrderByDescending(x => x.AREDate)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefault();


                LatestInspection = new InventoryItemStatusModel(Item, Tag);
            }
        }
    }

    public class InventoryItemStatusModel
    {
        public DateTime ReportDate { get; set; }
        public string InspectedBy { get; set; }
        public InventoryItemStatus Status { get; set; }
                
        public InventoryItemStatusModel(tblInventoryItem item, TagItemModel tag)
        {
            using (samDataContext context = new samDataContext())
            {
                tblInventoryInspection latestInspection = context.tblInventoryInspections.OrderByDescending(x => x.ReportDate).FirstOrDefault();

                var tmp = context.tblInventoryItemStatus.Where(x => x.InventoryItemId == item.Id)
                    .Join(context.tblInventoryInspections, a => a.InspectionId, b => b.Id, (a, b) => new { a = a, b = b })
                    .OrderByDescending(x => x.b.ReportDate)
                    .FirstOrDefault();

                if (tmp == null)
                {
                    if (latestInspection == null)
                    {
                        Status = InventoryItemStatus.Usable;
                        ReportDate = tag.Receipt.ReceivedDate;
                        InspectedBy = tag.Receipt.ReceivedBy;
                    }
                    else
                    {
                        Status = InventoryItemStatus.Unchecked;
                        ReportDate = latestInspection.ReportDate;
                        InspectedBy = latestInspection.InspectedBy;
                    }
                }
                else
                {
                    Status = (InventoryItemStatus)tmp.a.Status;
                    ReportDate = tmp.b.ReportDate;
                    InspectedBy = tmp.b.InspectedBy;
                }
            }
        }

        public string Status_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(InventoryItemStatus), Status);
            }
        }
    }
}