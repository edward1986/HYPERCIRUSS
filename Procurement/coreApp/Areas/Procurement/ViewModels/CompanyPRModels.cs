using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class CompanyPRModels
    {
        public int Year { get; set; }

        public List<tblCompanyPR> PRs { get; set; }
        public List<tblAPR> APRs { get; set; }
        
        public double TotalAmountOfAllAPRs()
        {
            return APRs.Sum(x => x._TotalAmount ?? 0);
        }

        public double TotalAmountOfSubmittedPRs(int? excludePRId = null)
        {
            return PRs
                .Where(x => excludePRId == -1 || x.Id != excludePRId)
                .ToList()
                .Where(x => x.HasBeenSubmitted)
                .Sum(x => x._TotalAmount ?? 0);
        }

        public double Balance(int excludePRId = -1)
        {
            return TotalAmountOfAllAPRs() - TotalAmountOfSubmittedPRs(excludePRId);
        }

        public CompanyPRModels(int year)
        {
            this.Year = year;
            
            using (procurementDataContext context = new procurementDataContext())
            {
                PRs = context.tblCompanyPRs.Where(x => x.Year == Year).ToList();
                APRs = context.tblAPRs
                    .Where(x => x.Year == Year)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .ToList();
            }
        }
        
    }
}