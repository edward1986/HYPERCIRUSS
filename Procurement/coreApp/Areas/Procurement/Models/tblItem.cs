using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblItemMetaData
    {
        
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 99999, ErrorMessage = "The Category field is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Range(0, 99999, ErrorMessage = "The Unit field is required")]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }

        [Display(Name = "In DBM")]
        public bool InDBM { get; set; }

        [Required]
        [Range(0.000001, 9999999999, ErrorMessage = "Invalid Price value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double OrigPrice { get; set; }

        [Range(0, 99999)]
         public int MFO { get; set; }
    }

    [MetadataType(typeof(tblItemMetaData))]
    public partial class tblItem
    {
        public double _priceMarkup { get; set; } = 0;

        public double Price
        {
            get
            {
                return OrigPrice + (OrigPrice * (_priceMarkup / 100));
            }
        }

        public tblUnit Unit
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblUnits.Where(x => x.Id == UnitId).SingleOrDefault();
                }
            }
        }

        public tblCategory Category
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblCategories.Where(x => x.Id == CategoryId).SingleOrDefault();
                }
            }
        }

        public bool HasSubmittedPPMPs()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblPPMPs
                    .Where(x => x.Year == Year)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .Select(x => x.Id)
                    .Join(context.tblPPMPItems, a => a, b => b.PPMPId, (a, b) => b)
                    .Any(x => x.ItemId == Id);
            }
        }

        public string StockNo
        {
            get
            {
                return string.IsNullOrEmpty(_StockNo) ? Id.ToString("00000") : _StockNo;
            }
        }

        public int? GetItemLife()
        {
            int? ret = FixedSettings.EquipmentLife;

            using (SAM.samDataContext _context = new SAM.samDataContext())
            {
                SAM.tblItemLife life = _context.tblItemLifes.Where(x => x.ItemId == Id).SingleOrDefault();
                if (life != null)
                {
                    ret = life.NoOfMonths;
                }
            }

            return ret;
        }

        public string MFO_desc
        {
            get
            {
                return Enum.GetName(typeof(MFO), MFO);
            }
        }

    }
}