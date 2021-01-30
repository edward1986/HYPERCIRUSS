using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblAOB_BidsMetaData
    {
        [Display(Name = "Item Bid")]
        public string ItemBid { get; set; }

        [Display(Name = "Unit Bid")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double UnitBid { get; set; }

        [Display(Name = "Total Bid")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double TotalBid { get; set; }

    }
    [MetadataType(typeof(tblAOB_BidsMetaData))]
    public partial class tblAOB_Bid
    {

        public tblItem Item
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblItems.Where(x => x.Id == ItemId).SingleOrDefault();
                }
            }
        }

        public tblSupplier Supplier
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblSuppliers.Where(x => x.Id == SupplierId).SingleOrDefault();
                }
            }
        }
    }
}