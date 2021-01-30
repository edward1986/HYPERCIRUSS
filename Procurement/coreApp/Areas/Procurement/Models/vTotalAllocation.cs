using Module.DB;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{
    public partial class vTotalAllocationMetaData
    {
        public string Fund { get; set; }

        public double Allocated { get; set; }
         public double YearAllocated { get; set; }
         public double YearReceived { get; set; }
         public double Received { get; set; }

    }

    [MetadataType(typeof(vTotalAllocationMetaData))]
    public partial class vTotalAllocation
    {
        public string Percentage
        {
            get
            {
                double xPercent = (double)(Allocated / Received * 100);

                return xPercent.ToString() + "%";
            }
        }
    }

}