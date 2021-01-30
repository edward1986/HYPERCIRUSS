using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblPPMPItemMetaData
    {
        [Required]
        public int PPMPId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Display(Name = "Jan")]
        public int M1 { get; set; }

        [Display(Name = "Feb")]
        public int M2 { get; set; }

        [Display(Name = "Mar")]
        public int M3 { get; set; }

        [Display(Name = "Apr")]
        public int M4 { get; set; }

        [Display(Name = "May")]
        public int M5 { get; set; }

        [Display(Name = "Jun")]
        public int M6 { get; set; }

        [Display(Name = "Jul")]
        public int M7 { get; set; }

        [Display(Name = "Aug")]
        public int M8 { get; set; }

        [Display(Name = "Sep")]
        public int M9 { get; set; }

        [Display(Name = "Oct")]
        public int M10 { get; set; }

        [Display(Name = "Nov")]
        public int M11 { get; set; }

        [Display(Name = "Dec")]
        public int M12 { get; set; }
    }

    [MetadataType(typeof(tblPPMPItemMetaData))]
    public partial class tblPPMPItem
    {
        public bool useOrigPrice { get; set; } = false;

        public int? SAM_QtyDelivered { get; set; }
        public double? DBMPrice { get; set; }
        public int? OOS { get; set; }
        
        public bool IsOOS
        {
            get
            {
                return OOS != null || CPR_OOS != null;
            }
        }

        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double OOSAmount
        {
            get
            {
                double itemPrice = 0;
                if (DBMPrice != null)
                {
                    itemPrice = DBMPrice.Value;
                }
                else if (CPR_NewPrice != null)
                {
                    itemPrice = CPR_NewPrice.Value;
                }
                else if (Item != null)
                {
                    itemPrice = useOrigPrice ? Item.OrigPrice : Item.Price;
                }

                int _oos = Qty;

                if (OOS != null)
                {
                    _oos = OOS.Value;
                }
                else if (CPR_OOS != null)
                {
                    _oos = CPR_OOS.Value;
                }

                return _oos * itemPrice;
            }
        }

        public tblItem Item
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    DateTime dt = new DateTime(PPMP.CreateDate.Year, 12, 31);
                    SettingsModel settings = new SettingsModel(Common.Settings(dt));

                    tblItem item = context.tblItems.Where(x => x.Id == ItemId).SingleOrDefault();
                    if (item != null)
                    {
                        item._priceMarkup = settings.PriceMarkup;
                    }

                    return item;
                }
            }
        }            

        public void SetItemValues()
        {
            tblItem i = Item;
            ItemName = i.Name;
            ItemDescription = i.Description;
            ItemCategoryId = i.CategoryId;
            ItemCategory = i.Category.Category;
            ItemUnitId = i.UnitId;
            ItemUnit = i.Unit.Unit;
            ItemInDBM = i.InDBM;
            useOrigPrice = !i.InDBM;
            ItemPrice = useOrigPrice ? i.OrigPrice : i.Price;

            ItemFundId = -1;

            tblPPMP ppmp = PPMP;

            if (ppmp != null)
            {
                ItemFundId = ppmp.FundId ?? -1;
            }
            
        }

        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCategory { get; set; }
        public int ItemUnitId { get; set; }
        public string ItemUnit { get; set; }
        public bool ItemInDBM { get; set; }

        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double ItemPrice { get; set; }

        public int ItemFundId { get; set; }

        public tblPPMP PPMP
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblPPMPs.Where(x => x.Id == PPMPId).SingleOrDefault();
                }
            }
        }

        public tblAPP APP()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                if (APPId == null)
                {
                    return null;
                }
                else
                {
                    return context.tblAPPs.Where(x => x.Id == APPId.Value).SingleOrDefault();
                }
            }
        }

        public tblPR PR()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                if (PRId == null)
                {
                    return null;
                }
                else
                {
                    return context.tblPRs.Where(x => x.Id == PRId.Value).SingleOrDefault();
                }
            }
        }

        public int Q1
        {
            get
            {
                return (M1 ?? 0) + (M2 ?? 0) + (M3 ?? 0);
            }
        }

        public int Q2
        {
            get
            {
                return (M4 ?? 0) + (M5 ?? 0) + (M6 ?? 0);
            }
        }

        public int Q3
        {
            get
            {
                return (M7 ?? 0) + (M8 ?? 0) + (M9 ?? 0);
            }
        }

        public int Q4
        {
            get
            {
                return (M10 ?? 0) + (M11 ?? 0) + (M12 ?? 0);
            }
        }

        public int Qty
        {
            get
            {
                return Q1 + Q2 + Q3 + Q4;
            }
        }

        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Amount
        {
            get
            {
                double itemPrice = 0;
                if (Item != null)
                {
                    if (DBMPrice != null)
                    {
                        itemPrice = DBMPrice.Value;
                    }
                    else if (CPR_NewPrice != null)
                    {
                        itemPrice = CPR_NewPrice.Value;
                    }
                    else
                    {
                        useOrigPrice = !Item.InDBM;
                        
                        itemPrice = useOrigPrice ? Item.OrigPrice : Item.Price;
                    }
                }

                return Qty * itemPrice;
            }
        }

        public string Stringify()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Merge(tblPPMPItem item)
        {
            if ((item.M1 ?? 0) != 0) M1 = (M1 ?? 0) + item.M1;
            if ((item.M2 ?? 0) != 0) M2 = (M2 ?? 0) + item.M2;
            if ((item.M3 ?? 0) != 0) M3 = (M3 ?? 0) + item.M3;
            if ((item.M4 ?? 0) != 0) M4 = (M4 ?? 0) + item.M4;
            if ((item.M5 ?? 0) != 0) M5 = (M5 ?? 0) + item.M5;
            if ((item.M6 ?? 0) != 0) M6 = (M6 ?? 0) + item.M6;
            if ((item.M7 ?? 0) != 0) M7 = (M7 ?? 0) + item.M7;
            if ((item.M8 ?? 0) != 0) M8 = (M8 ?? 0) + item.M8;
            if ((item.M9 ?? 0) != 0) M9 = (M9 ?? 0) + item.M9;
            if ((item.M10 ?? 0) != 0) M10 = (M10 ?? 0) + item.M10;
            if ((item.M11 ?? 0) != 0) M11 = (M11 ?? 0) + item.M11;
            if ((item.M12 ?? 0) != 0) M12 = (M12 ?? 0) + item.M12;
        }

        public bool HasEntry()
        {
            return
                (M1 ?? 0) > 0 ||
                (M2 ?? 0) > 0 ||
                (M3 ?? 0) > 0 ||
                (M4 ?? 0) > 0 ||
                (M5 ?? 0) > 0 ||
                (M6 ?? 0) > 0 ||
                (M7 ?? 0) > 0 ||
                (M8 ?? 0) > 0 ||
                (M9 ?? 0) > 0 ||
                (M10 ?? 0) > 0 ||
                (M11 ?? 0) > 0 ||
                (M12 ?? 0) > 0;
        }

        public bool HasMonthEntries(int m)
        {
            return
                (M1 ?? 0) > 0 && m == 1 ||
                (M2 ?? 0) > 0 && m == 2 ||
                (M3 ?? 0) > 0 && m == 3 ||
                (M4 ?? 0) > 0 && m == 4 ||
                (M5 ?? 0) > 0 && m == 5 ||
                (M6 ?? 0) > 0 && m == 6 ||
                (M7 ?? 0) > 0 && m == 7 ||
                (M8 ?? 0) > 0 && m == 8 ||
                (M9 ?? 0) > 0 && m == 9 ||
                (M10 ?? 0) > 0 && m == 10 ||
                (M11 ?? 0) > 0 && m == 11 ||
                (M12 ?? 0) > 0 && m == 12;
        }

        public int CategoryMOP()
        {
            int ret = -1;

            using (procurementDataContext context = new procurementDataContext())
            {
                tblMOP mop = context.tblMOPs.Where(x => x.PRId == CPRId && x.CategoryId == ItemCategoryId && x.Type == (int)MOPType.ConsolidatedPR).SingleOrDefault();
                if (mop != null)
                {
                    ret = mop.ModeOfProcurement;
                }
            }

            return ret;
        }
    }
}