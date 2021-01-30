using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblAREItemMetaData
    {        
        [Display(Name = "Item")]
        [Range(1, 99999, ErrorMessage = "The Item field is required")]
        public int InventoryItemId { get; set; }

        [Display(Name = "Property No.")]
        public string _PropertyNo { get; set; }

        [Display(Name = "Item Name")]
        public string _ItemName { get; set; }

        [Display(Name = "Acquisition Date")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime _AcquisitionDate { get; set; }
        
        [Display(Name = "Unit Cost")]
        [Range(0.000001, 9999999999, ErrorMessage = "Invalid Unit Cost value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double _UnitCost { get; set; }

        [Display(Name = "Estimated Useful Life")]
        public int? _Life { get; set; }
    }

    [MetadataType(typeof(tblAREItemMetaData))]
    public partial class tblAREItem
    {
        public void UpdateFields()
        {
            int itemId;

            using (samDataContext context = new samDataContext())
            {
                tblInventoryItem inventoryItem = context.tblInventoryItems.Where(x => x.Id == InventoryItemId).Single();
                var item = context.tblReceiptItems.Where(x => x.Id == inventoryItem.ReceiptItemId)
                    .Join(context.tblReceipts, a => a.ReceiptId, b => b.Id, (a, b) => new { a = a, b = b })
                    .Join(context.tblPOItems, a => a.a.POItemId, b => b.Id, (a, b) => new { a = a.b, b = b })
                    .First();

                _ItemName = item.b.ItemName;
                _AcquisitionDate = item.a.ReceivedDate;
                _Unit = item.b._Unit;
                _UnitCost = item.b.UnitCost;
                _PropertyNo = inventoryItem.PropertyNo;

                itemId = item.b.Procurement_ItemId;
                _IsSemiExpendable = item.b.IsSemiExpendable;
            }

            using (samDataContext _context = new samDataContext())
            {
                _Life = _IsSemiExpendable == true ? FixedSettings.SemiExpendableLife : FixedSettings.EquipmentLife;

                tblItemLife life = _context.tblItemLifes.Where(x => x.ItemId == itemId).SingleOrDefault();
                if (life != null)
                {
                    _Life = life.NoOfMonths;
                }
            }
        }

        public int RemainingLife(DateTime? asOf = null)
        {
            return Common.GetItemRemainingLife(InventoryItemId, _Life, asOf);
        }

        public tblARE GetARE()
        {
            using (samDataContext context = new samDataContext())
            {
                return context.tblAREs.Where(x => x.Id == AREId).Single();
            }
        }
    }
}