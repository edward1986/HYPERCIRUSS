using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class CompanyAPPModel_Utility
    {
        public int Year { get; set; }
        public List<tblAPP> Apps { get; set; }

        public bool NegativeBalance(int appId)
        {
            double totalFunds = TotalAmountReceived();
            double totalSubmitted = TotalAmountOfSubmittedAPPs(appId);
            double totalAmount = TotalAmount(appId);
            double balance = totalFunds - totalSubmitted - totalAmount;

            return balance < 0;
        }

        public double TotalAmount(int appId)
        {
            double ret = 0;

            tblAPP app = Apps.Where(x => x.Id == appId).SingleOrDefault();
            if (app != null)
            {
                ret = app._TotalAmount ?? 0;
            }

            return ret;
        }

        public double TotalAmountOfSubmittedAPPs(int excludeAPPId = -1)
        {
            return Apps
                .Where(x => excludeAPPId == -1 || x.Id != excludeAPPId)
                .ToList()
                .Where(x => x.HasBeenSubmitted)
                .Sum(x => x._TotalAmount ?? 0);
        }

        public double TotalAmountReceived()
        {
            double ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                var t = context.tblOfficeAllocations.Where(x => x.Year == Year);
                if (t.Any())
                {
                    ret = t.Sum(x => x.Amount ?? 0);
                }
            }
            return ret;
        }

        public double Balance(int excludeAPPId = -1)
        {
            return TotalAmountReceived() - TotalAmountOfSubmittedAPPs(excludeAPPId);
        }

        public CompanyAPPModel_Utility(int year, List<tblAPP> apps = null)
        {
            Year = year;

            if (apps == null)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    Apps = context.tblAPPs
                        .Where(x => x.Year == year)
                        .ToList();
                }
            }
            else
            {
                Apps = apps;
            }
        }
    }

    public class CompanyAPPModel
    {
        public int Year { get; set; }
        public List<tblAPP> APPs { get; set; }
        public List<tblPPMP> OfficePPMPs { get; set; }

        public CompanyAPPModel_Utility utility { get; set; }

        public CompanyAPPModel(int year)
        {
            this.Year = year;
            
            using (procurementDataContext context = new procurementDataContext())
            {
                APPs = context.tblAPPs.Where(x => x.Year == Year).ToList();
                OfficePPMPs = context.tblPPMPs
                    .Where(x => x.DepartmentId == null && x.Year == Year)
                    .ToList()
                    .Where(x => x.IsApproved)
                    .ToList();


                utility = new CompanyAPPModel_Utility(year, APPs);
            }
        }
    }
}