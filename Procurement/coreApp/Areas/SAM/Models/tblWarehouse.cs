using coreApp.Areas.Procurement.DAL;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblWarehouseMetaData
    {
        [Required]
        [Display(Name = "Warehouse Name")]
        public string WarehouseName { get; set; }

        public string Address { get; set; }
    }

    [MetadataType(typeof(tblWarehouseMetaData))]
    public partial class tblWarehouse
    {
        public List<tblEmployee> Employees()
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblEmployees.Where(x => x.WarehouseId == Id).ToList();
            }
        }

        public string Description
        {
            get
            {
                return string.Format("{0}{1}", WarehouseName, string.IsNullOrEmpty(Address) ? "" : " (" + Address + ")");
            }
        }
    }
}