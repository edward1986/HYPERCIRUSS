using coreApp.Areas.Procurement.DAL;
using Module.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class AnnualAllocationModel
    {
        public int Year { get; set; }
        public List<OfficeAllocationModel> Offices { get; set; }
        public double TotalAmountAllocatedToOffices(int excludeOfficeId = -1)
        {
            return Offices
                .Where(x => excludeOfficeId == -1 || x.Office.OfficeId != excludeOfficeId)
                .Sum(x => x.TotalAmountAllocatedToOffice());
        }
        public double TotalAmountReceived()
        {
            double ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                var t = context.tblFundAllocations.Where(x => x.Year == Year);
                if (t.Any())
                {
                    ret = t.Sum(x => x.Amount ?? 0);
                }
            }
            return ret;
        }
        public double Balance(int excludeOfficeId = -1)
        {
            return TotalAmountReceived() - TotalAmountAllocatedToOffices(excludeOfficeId);
        }

        public AnnualAllocationModel(int year)
        {
            this.Year = year;

            Offices = new List<OfficeAllocationModel>();

            using (DAL.procurementDataContext context = new DAL.procurementDataContext())
            {
                foreach (var office in Cache.Get_Tables().Offices)
                {
                    OfficeAllocationModel om = new OfficeAllocationModel(office.OfficeId, year);
                    Offices.Add(om);
                }

            }
        }

    }

    public class OfficeAllocationModel
    {
        public int OfficeId { get; set; }
        public int Year { get; set; }
        public tblOffice Office { get; set; }
        public List<FundAllocationModel> Funds { get; set; }
        public string Funds_Desc
        {
            get
            {
                return string.Join(", ", Funds.Where(x => x.Amount > 0).Select(x => x.Fund.Fund).ToArray());
            }
        }
        public double TotalAmountAllocatedToOffice()
        {
            return Funds.Sum(x => x.Amount ?? 0);
        }

        public double UsedAmount(int fundId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblPPMPs
                    .Where(x => x.Year == Year && x.OfficeId == OfficeId && x.FundId == fundId)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted && !x.IsDepartmentPPMP)
                    .Sum(x => new PPMPModel(x.Id).TotalAmount());
            }
        }

        public OfficeAllocationModel(int officeId, int year)
        {
            OfficeId = officeId;
            Year = year;

            using (procurementDataContext context = new procurementDataContext())
            {
                Office = Cache.Get_Tables().Offices.Where(x => x.OfficeId == officeId).SingleOrDefault();
                Funds = new List<FundAllocationModel>();

                foreach (tblFund fund in context.tblFunds)
                {
                    FundAllocationModel fm = new FundAllocationModel
                    {
                        Fund = fund,
                        Amount = 0
                    };

                    tblOfficeAllocation o = context.tblOfficeAllocations.Where(x => x.OfficeId == officeId && x.Year == year && x.FundId == fund.Id).SingleOrDefault();
                    if (o != null)
                    {
                        fm.Amount = o.Amount;
                    }

                    Funds.Add(fm);
                }
            }
        }
    }

    public class FundAllocationModel
    {
        public tblFund Fund { get; set; }
        public double? Amount { get; set; }
        public double TotalAmountReceived(int year)
        {
            double ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                var t = context.tblFundAllocations.Where(x => x.FundId == Fund.Id && x.Year == year).SingleOrDefault();
                if (t != null)
                {
                    ret = t.Amount ?? 0;
                }
            }
            return ret;
        }

        public double TotalAmountAllocatedToOffices(int year, int excludeOfficeId = -1)
        {
            double ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                var tmp = context.tblOfficeAllocations.Where(x => x.FundId == Fund.Id && x.Year == year).ToList();

                var t = tmp
                    .Join(Cache.Get_Tables().Offices, a => a.OfficeId, b => b.OfficeId, (a, b) => a)
                    .ToList();
                if (t.Any())
                {
                    ret = t
                        .Where(x => excludeOfficeId == -1 || x.Office.OfficeId != excludeOfficeId)
                        .Sum(x => x.Amount ?? 0);
                }
            }
            return ret;
        }
        public double Balance(int year, int excludeOfficeId = -1)
        {
            return TotalAmountReceived(year) - TotalAmountAllocatedToOffices(year, excludeOfficeId);
        }

        public FundAllocationModel()
        { }

        public FundAllocationModel(int fundId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                Fund = context.tblFunds.Where(x => x.Id == fundId).SingleOrDefault();
            }
        }
    }

    public class SOFModel
    {
        public List<FundAllocationModel> Funds { get; set; }
        public int Year { get; set; }
        public double Total
        {
            get
            {
                return Funds.Sum(x => x.Amount ?? 0);
            }
        }

        public SOFModel(int year)
        {
            this.Year = year;

            Funds = new List<FundAllocationModel>();
            
            using(procurementDataContext context = new procurementDataContext())
            {
                foreach (tblFund fund in context.tblFunds)
                {
                    FundAllocationModel fm = new FundAllocationModel
                    {
                        Fund = fund
                    };

                    tblFundAllocation o = context.tblFundAllocations.Where(x => x.Year == year && x.FundId == fund.Id).SingleOrDefault();
                    if (o != null)
                    {
                        fm.Amount = o.Amount;
                    }

                    Funds.Add(fm);
                }
            }
        }
    }
}