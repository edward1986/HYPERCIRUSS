using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class POModels
    {
        public int Year { get; set; }

        public List<tblPO> POs { get; set; }
        public List<tblAOB> AOBs { get; set; }
        
        public POModels(int year)
        {
            this.Year = year;

            POs = new List<tblPO>();
            AOBs = new List<tblAOB>();

            using (procurementDataContext context = new procurementDataContext())
            {
                POs = context.tblPOs.Where(x => x.Year == Year).ToList();

                AOBs = context.tblAOBs
                   .Where(x => x.Year == Year)
                   .ToList()
                   .Where(x => x.HasBeenSubmitted)
                   .ToList();
            }
        }
        
    }
}