using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using coreLib.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class StockModel
    {
        public PeriodModel periodModel { get; set; }
        public tblItem Item { get; set; }

        public List<StockEntryInfo> Entries { get; set; }

        public string StockNo
        {
            get
            {
                return Item.StockNo;
            }
        }
        
        public StockModel(int itemId, PeriodModel periodModel = null)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                Item = context.tblItems.Where(x => x.Id == itemId).First();

                this.periodModel = new PeriodModel(coreLib.Enums.PeriodModelInitType.ThisYear);

                if (periodModel != null)
                {
                    this.periodModel = periodModel;
                }
            }
            
            init();
        }

        void init()
        {
            using (samDataContext context = new samDataContext())
            {
                Entries = context.tblReceiptItems
                    .Join(context.tblReceipts, a => a.ReceiptId, b => b.Id, (a, b) => new { receiptItem = a, receipt = b })
                    .Join(context.tblPOItems, a => a.receiptItem.POItemId, b => b.Id, (a, b) => new { a = a, poItem = b })
                    .Where(x => x.poItem.Procurement_ItemId == Item.Id)
                    .Select(x => new StockEntryInfo
                    {
                        IsReceipt = true,
                        Date = x.a.receipt.ReceivedDate,
                        Qty = x.a.receiptItem.Qty,
                        Receipt = x.a.receipt
                    })
                    .ToList();

                Entries.AddRange(
                    context.tblRISItems.Where(x => x._ItemId == Item.Id)
                    .Join(context.tblRIs, a => a.RISId, b => b.Id, (a, b) => new StockEntryInfo
                    {
                        IsReceipt = false,
                        Date = b.RequisitionDate,
                        Qty = a.Approved_Qty,
                        Issuance = b
                    })
                    .ToList()
                    );
            }
        }

        public List<StockEntryInfo> Current_Entries()
        {
            return Entries.Where(x => periodModel.Within(x.Date)).ToList();
        }

        public double Previous_DR()
        {
            return Entries.Where(x => x.IsReceipt && x.Date < periodModel.StartDate).Sum(x => x.Qty);
        }

        public double Current_DR()
        {
            return Entries.Where(x => x.IsReceipt && periodModel.Within(x.Date)).Sum(x => x.Qty);
        }

        public double Next_DR()
        {
            return Entries.Where(x => x.IsReceipt && x.Date > periodModel.EndDate).Sum(x => x.Qty);
        }

        public double DR()
        {
            return Entries.Where(x => x.IsReceipt).Sum(x => x.Qty);
        }

        public double Previous_CR()
        {
            return Entries.Where(x => !x.IsReceipt && x.Date < periodModel.StartDate).Sum(x => x.Qty);
        }

        public double Current_CR()
        {
            return Entries.Where(x => !x.IsReceipt && periodModel.Within(x.Date)).Sum(x => x.Qty);
        }

        public double Next_CR()
        {
            return Entries.Where(x => !x.IsReceipt && x.Date > periodModel.EndDate).Sum(x => x.Qty);
        }

        public double CR()
        {
            return Entries.Where(x => !x.IsReceipt).Sum(x => x.Qty);
        }

        public double Previous_Balance()
        {
            return Previous_DR() - Previous_CR();
        }

        public double Current_Balance()
        {
            return Current_DR() - Current_CR();
        }

        public double Next_Balance()
        {
            return Next_DR() - Next_CR();
        }
        
        public double Balance()
        {
            return DR() - CR();
        }
    }

    public class StockEntryInfo
    {
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        public bool IsReceipt { get; set; }        
        public int Qty { get; set; }

        public tblReceipt Receipt { get; set; }
        public tblRI Issuance { get; set; }

        public tblItem _Item { get; set; }
        public int? _Life { get; set; }
        
    }

}