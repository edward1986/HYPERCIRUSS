using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.Areas.SAM.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblRISItemMetaData
    {
        [Display(Name = "Item")]
        public int POItemId { get; set; }

        [Display(Name = "Requested Qty")]
        [Range(1, 99999, ErrorMessage = "Invalid Requested Qty field value")]
        public int Requested_Qty { get; set; }

        [Display(Name = "Approved Qty")]
        [Range(0, 99999, ErrorMessage = "Invalid Approved Qty field value")]
        public int Approved_Qty { get; set; }

        [Display(Name = "Item Name")]
        public string _ItemName { get; set; }
        
        [Display(Name = "Unit")]
        public string _Unit { get; set; }

        [Display(Name = "Unit Cost")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double _UnitCost { get; set; }
    }

    [MetadataType(typeof(tblRISItemMetaData))]
    public partial class tblRISItem
    {
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Amount
        {
            get
            {
                return (_UnitCost ?? 0) * Approved_Qty;
            }
        }

        public void UpdateFields()
        {
            using (samDataContext context = new samDataContext())
            {
                tblPOItem pOItem = context.tblPOItems.Where(x => x.Id == POItemId).Single();

                _ItemId = pOItem.Procurement_ItemId;
                _PONo = pOItem._PONo;
                _UnitCost = pOItem.UnitCost;
            }

            using (procurementDataContext context = new procurementDataContext())
            {
                tblItem item = context.tblItems.Where(x => x.Id == _ItemId).Single();

                _ItemName = item.Name;
                _Unit = item.Unit.Unit;
                _StockNo = item.StockNo;
            }            
        }
    }
}