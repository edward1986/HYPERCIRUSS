using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class RFQModel_Utility
    {
        public int Year { get; set; }
        public List<tblRFQ> RFQs { get; set; }
        public List<tblConsolidatedPR> CPR { get; set; }

        public double TotalAmountOfAllRFQs()
        {
            return CPR.Sum(x => x._TotalAmount ?? 0);
        }

        public double TotalAmount(int rfqId)
        {
            double ret = 0;

            tblRFQ rfq = RFQs.Where(x => x.Id == rfqId).SingleOrDefault();
            if (rfq != null)
            {
                ret = rfq._TotalAmount ?? 0;
            }

            return ret;
        }

        public double TotalAmountOfSubmittedRFQs(int excludeRFQId = -1)
        {
            return RFQs
                .Where(x => excludeRFQId == -1 || x.Id != excludeRFQId)
                .ToList()
                .Where(x => x.HasBeenSubmitted)
                .Sum(x => x._TotalAmount ?? 0);
        }

        public RFQModel_Utility(int year, List<tblRFQ> rfqs = null)
        {
            Year = year;

            if (rfqs == null)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    RFQs = context.tblRFQs
                        .Where(x => x.Year == year)
                        .ToList();
                }
            }
            else
            {
                RFQs = rfqs;
            }
        }
    }

    public class RFQModels
    {
        public int Year { get; set; }
        public List<tblRFQ> RFQs { get; set; }
        public List<tblConsolidatedPR> ConsolidatedPRs { get; set; }

        public RFQModel_Utility utility { get; set; }

        public RFQModels(int year)
        {
            this.Year = year;

            using (procurementDataContext context = new procurementDataContext())
            {
                RFQs = context.tblRFQs.Where(x => x.Year == Year).ToList();
                ConsolidatedPRs = context.tblConsolidatedPRs
                    .Where(x => x.Year == Year)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .ToList();
            }

            utility = new RFQModel_Utility(year, RFQs)
            {
                CPR = ConsolidatedPRs
            };

        }
    }
}