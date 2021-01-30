using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class CompanyAPRModel
    {
        public int Year { get; set; }
        public List<tblAPR> APRs { get; set; }
        public List<tblAPP> CompanyAPPs { get; set; }

      
        public CompanyAPRModel(int year)
        {
            this.Year = year;

            APRs = new List<tblAPR>();
            CompanyAPPs = new List<tblAPP>();

            using (procurementDataContext context = new procurementDataContext())
            {
                APRs = context.tblAPRs.Where(x => x.Year == Year).ToList();
                CompanyAPPs = context.tblAPPs.Where(x => x.Year == Year).ToList().Where(x => x.HasBeenSubmitted).ToList();
            }
        }
        
    }

    public class OOSModel
    {
        public int ItemId { get; set; }
        public int? Qty { get; set; }
        public double? NewPrice { get; set; }
    }

}