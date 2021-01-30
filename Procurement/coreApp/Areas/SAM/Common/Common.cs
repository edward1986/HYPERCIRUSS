using coreApp.Areas.SAM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class Common
    {
        public static double EquipmentMinAmount = 15000;

        public static List<TagItemModel> GetTaggingModels(int Year = -1, int ReceiptItemId = -1)
        {
            using (samDataContext context = new samDataContext())
            {
                return context.tblReceipts.Where(x => Year == -1 || x.ReceivedDate.Year == Year)
                    .Join(context.tblReceiptItems.Where(x => ReceiptItemId == -1 || x.Id == ReceiptItemId), a => a.Id, b => b.ReceiptId, (a, b) => new { a = a, b = b })
                    .Join(context.tblPOItems, a => a.b.POItemId, b => b.Id, (a, b) => new TagItemModel { Receipt = a.a, ReceiptItem = a.b, POItem = b })
                    .ToList()
                    .Where(x => (x.POItem._CategoryType == (int)CategoryType.Equipment))
                    .OrderBy(x => x.POItem.ItemName)
                    .ThenBy(x => x.Receipt.ReceivedDate)
                    .ToList();
            }
        }

        public static bool POExists(string poNo)
        {
            bool ret = false;

            using (samDataContext context = new samDataContext())
            {
                ret = context.tblPOs.Any(x => x.PONo.Trim().ToLower() == poNo.Trim().ToLower());
            }

            return ret;
        }

        public static POModel GetPOModel(string poNo, int warehouseId, int exclude_ReceiptId = -1, bool importProcurementPO = false)
        {
            POModel ret = null;

            using (samDataContext context = new samDataContext())
            {
                tblPO po = context.tblPOs.Where(x => x.PONo.Trim().ToLower() == poNo.Trim().ToLower()).SingleOrDefault();
                if (po != null)
                {
                    ret = new POModel(po, exclude_ReceiptId);
                }
                else if (importProcurementPO)
                {
                    Procurement.SupplierPO supplierPO = Procurement.Common.GetSupplierPO(poNo);
                    if (supplierPO != null)
                    {
                        tblPO newPO = new tblPO
                        {
                            PONo = supplierPO.PONo,
                            PODate = supplierPO.PO.SubmitDate.Value,
                            SupplierId = supplierPO.Supplier.Id,                            
                            Procurement_POId = supplierPO.PO.Id,
                            CreatedBy_UserId = Cache.Get().userAccess.user.Id,
                            Form_PlaceOfDelivery = supplierPO.PO.Form_PlaceOfDelivery,
                            Form_DateOfDelivery = supplierPO.PO.Form_DateOfDelivery,
                            Form_DeliveryTerm = supplierPO.PO.Form_DeliveryTerm,
                            Form_PaymentTerm = supplierPO.PO.Form_PaymentTerm,
                            WarehouseId = warehouseId,
                            OfficeId = supplierPO.PO.CreatorCareer().Office.OfficeId
                        };

                        newPO.UpdateFields();

                        context.tblPOs.InsertOnSubmit(newPO);
                        context.SubmitChanges();

                        List<tblPOItem> Items = new List<tblPOItem>();
                        foreach (Procurement.SupplierPOItem sItem in supplierPO.Items)
                        {
                            Procurement.DAL.tblPPMPItem item = sItem.Item;

                            double amount = sItem.Bid.TotalBid == null ? 0 : sItem.Bid.TotalBid.Value;
                            double unitcost = sItem.Bid.UnitBid == null ? (amount / item.Qty) : sItem.Bid.UnitBid.Value;

                            tblPOItem newItem = new tblPOItem
                            {
                                FromProcurement = true,
                                POId = newPO.Id,
                                UnitId = item.ItemUnitId,                                
                                CategoryId = item.ItemCategoryId, 
                                ItemName = item.ItemName,
                                Qty = item.Qty,
                                UnitCost = unitcost,
                                Amount = amount,
                                Procurement_ItemId = item.ItemId
                            };

                            newItem.UpdateFields();

                            Items.Add(newItem);
                        }

                        context.tblPOItems.InsertAllOnSubmit(Items);
                        context.SubmitChanges();

                        ret = new POModel(newPO, exclude_ReceiptId);
                    }
                }
            }

            return ret;
        }

        public static int GetDuration(DateTime AcquisitionDate, DateTime? asOf = null)
        {
            DateTime dt = DateTime.Today;
            if (asOf != null)
            {
                dt = asOf.Value;
            }
            
            double itemAge_days = dt.Subtract(AcquisitionDate).TotalDays;
            double itemAge_months = (itemAge_days / 365.25) * 12;

            return (int)itemAge_months;
        }

        public static int GetTotalItemDuration(int ItemId, DateTime? asOf = null)
        {
            DateTime dt = DateTime.Today;
            if (asOf != null)
            {
                dt = asOf.Value;
            }

            using (samDataContext context = new samDataContext())
            {
                var tmp = context.tblAREItems
                    .Where(x => x.InventoryItemId == ItemId)                    
                    .Join(context.tblAREs, a => a.AREId, b => b.Id, (a, b) => new tmp1 { tblAREItem = a, aREDate = b.AREDate, duration = 0 })
                    .Where(x => x.aREDate <= dt)
                    .OrderByDescending(x => x.aREDate)
                    .ToList();

                DateTime tmpDate = dt;
                foreach (var t in tmp)
                {
                    t.duration = GetDuration(t.aREDate, tmpDate);
                    tmpDate = t.aREDate;
                }

                return tmp.Any() ? tmp.Sum(x => x.duration) : 0;
            }
        }

        public static int GetItemRemainingLife(int ItemId, int? _Life, DateTime? asOf = null)
        {
            int duration = GetTotalItemDuration(ItemId, asOf);
            int life = _Life ?? 60;

            int remaining = life - duration;

            return remaining < 0 ? 0 : remaining;
        }
    }
    
    public class tmp1
    {
        public tblAREItem tblAREItem;
        public DateTime aREDate;
        public int duration;
    }
}