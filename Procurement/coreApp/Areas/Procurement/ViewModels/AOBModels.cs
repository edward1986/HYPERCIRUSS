using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class AOBModels
    {
        public int Year { get; set; }

        public List<tblAOB> AOBs { get; set; }
        public List<tblRFQ> RFQs { get; set; }
        
        public AOBModels(int year)
        {
            this.Year = year;

            AOBs = new List<tblAOB>();
            RFQs = new List<tblRFQ>();

            using (procurementDataContext context = new procurementDataContext())
            {
                AOBs = context.tblAOBs.Where(x => x.Year == Year).ToList();

                RFQs = context.tblRFQs
                   .Where(x => x.Year == Year)
                   .ToList()
                   .Where(x => x.HasBeenSubmitted)
                   .ToList();
            }
        }
        
    }
}