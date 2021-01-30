using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.Areas.SAM.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblPOItemMetaData
    {
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Unit")]
        public int UnitId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Range(0, 99999, ErrorMessage = "Invalid Qty field value")]
        public int Qty { get; set; }

        [Required]
        [Display(Name = "Unit Cost")]
        [Range(0.000001, 9999999999, ErrorMessage = "Invalid Unit Cost value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double UnitCost { get; set; }
                
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Amount { get; set; }

        [Range(1, 99999, ErrorMessage = "The Item Name field is required")]
        public int Procurement_ItemId { get; set; }
    }

    [MetadataType(typeof(tblPOItemMetaData))]
    public partial class tblPOItem
    {
        public bool FromProcurement { get; set; } = false;      
        

        public void UpdateFields()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblUnit unit = context.tblUnits.Where(x => x.Id == UnitId).Single();
                tblCategory category = context.tblCategories.Where(x => x.Id == CategoryId).Single();

                _Unit = unit.Unit;
                _Category = category.Category;

                if (category.Type == (int)CategoryType.Supply)
                {
                    _CategoryType = (int)CategoryType.Supply;
                }
                else
                {
                    _CategoryType = (int)CategoryType.Equipment;
                }
                
            }

            using (samDataContext context = new samDataContext())
            {
                tblPO po = context.tblPOs.Where(x => x.Id == POId).Single();

                _PONo = po.PONo;
            }
        }

        public bool IsSemiExpendable
        {
            get
            {
                return UnitCost < Common.EquipmentMinAmount;
            }
        }

        public string CategoryType_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(CategoryType), _CategoryType);
            }
        }

        public string StockNo
        {
            get
            {
                string ret = "";

                using (procurementDataContext context = new procurementDataContext())
                {
                    tblItem item = context.tblItems.Where(x => x.Id == Procurement_ItemId).SingleOrDefault();
                    if (item != null)
                    {
                        ret = item.StockNo;
                    }

                }

                return ret;
            }
        }
    }
}