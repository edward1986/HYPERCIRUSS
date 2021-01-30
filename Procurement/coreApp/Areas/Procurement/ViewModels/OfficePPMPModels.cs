using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using Module.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class OfficePPMPModel_Utility
    {
        public int Year { get; set; }
        public List<tblOfficeAllocation> Allocations { get; set; }
        public List<tblPPMP> PPMPs { get; set; }

        public tblOffice Office { get; set; }

        public bool NegativeBalance(int ppmpId)
        {
            double totalFunds = TotalAmountReceived();
            double totalSubmitted = TotalAmountOfSubmittedPPMPs();
            double balance = totalFunds - totalSubmitted;

            return balance < 0;
        }

        public double TotalAmountOfSubmittedPPMPs(int excludePPMPId = -1, int? fundId = null)
        {
            return PPMPs
                .Where(x => excludePPMPId == -1 || x.Id != excludePPMPId)
                .Where(x => fundId == null || x.FundId == fundId)
                .Where(x => x.HasBeenSubmitted)
                .Sum(x => x._TotalAmount);
        }
        public double TotalAmountReceived(int? fundId = null)
        {
            double ret = 0;
            if (Allocations.Any())
            {
                ret = Allocations.Where(x => fundId == null || x.FundId == fundId).Sum(x => x.Amount ?? 0);
            }
            return ret;
        }
        public double Balance(int excludePPMPId = -1, int? fundId = null)
        {
            return TotalAmountReceived(fundId) - TotalAmountOfSubmittedPPMPs(excludePPMPId, fundId);
        }

        public OfficePPMPModel_Utility(int year, tblOffice office, List<tblPPMP> ppmps = null)
        {
            Year = year;

            using (procurementDataContext context = new procurementDataContext())
            {
                Allocations = context.tblOfficeAllocations.Where(x => x.OfficeId == office.OfficeId && x.Year == year).ToList();

                if (ppmps == null)
                {
                    PPMPs = context.tblPPMPs
                        .Where(x => x.Year == year && x.OfficeId == Office.OfficeId)
                        .ToList();

                }
                else
                {
                    PPMPs = ppmps;
                }
            }
        }
    }

    public class OfficePPMPModel
    {
        public int Year { get; set; }
        public tblOffice Office { get; set; }
        public List<tblPPMP> PPMPs { get; set; }
        public List<tblPPMP> DepartmentPPMPs { get; set; }

        public OfficePPMPModel_Utility utility { get; set; }

        public OfficePPMPModel(int year, tblEmployee employee)
        {
            init(year, employee.Office);
        }

        public OfficePPMPModel(int year, tblOffice office)
        {
            init(year, office);
        }

        void init(int year, tblOffice office)
        {
            this.Year = year;
            this.Office = office;

            using (procurementDataContext context = new procurementDataContext())
            {
                PPMPs = context.tblPPMPs
                    .Where(x => x.Year == year && x.OfficeId == Office.OfficeId && x.DepartmentId == null)
                    .ToList();

                DepartmentPPMPs = context.tblPPMPs
                    .Where(x => x.Year == year && x.OfficeId == Office.OfficeId && x.DepartmentId != null)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .ToList();
            }

            utility = new OfficePPMPModel_Utility(year, office, PPMPs);
        }
    }
    
}