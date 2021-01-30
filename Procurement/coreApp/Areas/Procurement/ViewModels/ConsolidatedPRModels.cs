using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class ConsolidatedPRModel_Utility
    {
        public int Year { get; set; }
        public List<tblConsolidatedPR> CPRs { get; set; }
        public List<tblCompanyPR> CompanyPRs { get; set; }
        public List<tblPR> OfficePRs { get; set; }

        public double TotalAmountOfAllPRs()
        {
            return OfficePRs.Sum(x => x._TotalAmount ?? 0) + CompanyPRs.Sum(x => x._TotalAmount ?? 0);
        }

        public double TotalAmount(int cprId)
        {
            double ret = 0;

            tblConsolidatedPR cpr = CPRs.Where(x => x.Id == cprId).SingleOrDefault();
            if (cpr != null)
            {
                ret = cpr._TotalAmount ?? 0;
            }

            return ret;
        }

        public double TotalAmountOfSubmittedCPRs(int excludeCPRId = -1)
        {
            return CPRs
                .Where(x => excludeCPRId == -1 || x.Id != excludeCPRId)
                .ToList()
                .Where(x => x.HasBeenSubmitted)
                .Sum(x => x._TotalAmount ?? 0);
        }

        public ConsolidatedPRModel_Utility(int year, List<tblConsolidatedPR> cprs = null)
        {
            Year = year;

            if (cprs == null)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    CPRs = context.tblConsolidatedPRs
                        .Where(x => x.Year == year)
                        .ToList();
                }
            }
            else
            {
                CPRs = cprs;
            }
        }
    }

    public class ConsolidatedPRModel
    {
        public int Year { get; set; }
        public List<tblConsolidatedPR> CPRs { get; set; }
        public List<tblCompanyPR> CompanyPRs { get; set; }
        public List<tblPR> OfficePRs { get; set; }

        public ConsolidatedPRModel_Utility utility { get; set; }

        public ConsolidatedPRModel(int year)
        {
            this.Year = year;

            using (procurementDataContext context = new procurementDataContext())
            {
                CPRs = context.tblConsolidatedPRs.Where(x => x.Year == Year).ToList();
                OfficePRs = context.tblPRs.Where(x => x.Year == Year).ToList().Where(x => x.IsApproved).ToList();
                CompanyPRs = context.tblCompanyPRs.Where(x => x.Year == Year).ToList().Where(x => x.HasBeenSubmitted).ToList();
            }

            utility = new ConsolidatedPRModel_Utility(year, CPRs)
            {
                CompanyPRs = CompanyPRs,
                OfficePRs = OfficePRs
            };

        }
    }
}