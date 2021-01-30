using coreApp.Areas.Procurement.DAL;
using Module.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class AnnualDepartmentAllocationModel
    {
        public int Year { get; set; }
        public int OfficeId { get; set; }
        public List<DepartmentAllocationModel> Departments { get; set; }

        public double TotalAmountAllocatedToDepartments(int excludeDepartmentId = -1)
        {
            return Departments
                .Where(x => excludeDepartmentId == -1 || x.Department.DepartmentId != excludeDepartmentId)
                .Sum(x => x.Amount ?? 0);
        }

        public List<tblOfficeAllocation> Allocations()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblOfficeAllocations.Where(x => x.OfficeId == OfficeId && x.Year == Year).ToList();
            }
        }

        public double TotalAmountReceived()
        {
            double ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                var t = context.tblOfficeAllocations.Where(x => x.OfficeId == OfficeId && x.Year == Year);
                if (t.Any())
                {
                    ret = t.Sum(x => x.Amount ?? 0);
                }
            }
            return ret;
        }
        public double Balance(int excludeDepartmentId = -1)
        {
            return TotalAmountReceived() - TotalAmountAllocatedToDepartments(excludeDepartmentId);
        }

        public AnnualDepartmentAllocationModel(int year, int officeId)
        {
            this.Year = year;
            this.OfficeId = officeId;

            Departments = new List<DepartmentAllocationModel>();

            foreach (var dept in Cache.Get_Tables().Departments.Where(x => x.OfficeId == officeId))
            {
                DepartmentAllocationModel dm = new DepartmentAllocationModel(dept.DepartmentId, year);
                Departments.Add(dm);
            }
        }
        
    }

    //public class OfficeAllocationModel
    //{
    //    public Module.DB.tblOffice Office { get; set; }
    //    public List<FundAllocationModel> Funds { get; set; }
    //    public string Funds_Desc
    //    {
    //        get
    //        {
    //            return string.Join(", ", Funds.Where(x => x.Amount > 0).Select(x => x.Fund.Fund).ToArray());
    //        }
    //    }
    //    public double TotalAmountAllocatedToOffice
    //    {
    //        get
    //        {
    //            return Funds.Sum(x => x.Amount ?? 0);
    //        }
    //    }

    //    public OfficeAllocationModel(int officeId, int year)
    //    {
    //        using (coreApp.DAL.hr2017DataContext hrcontext = new coreApp.DAL.hr2017DataContext())
    //        {
    //            using (procurementDataContext context = new procurementDataContext())
    //            {
    //                Office = hrCache.Get_Tables().Offices.Where(x => x.OfficeId == officeId).SingleOrDefault();
    //                Funds = new List<FundAllocationModel>();

    //                foreach(tblFund fund in context.tblFunds)
    //                {
    //                    FundAllocationModel fm = new FundAllocationModel
    //                    {
    //                        Fund = fund,
    //                        Amount = 0
    //                    };

    //                    tblOfficeAllocation o = context.tblOfficeAllocations.Where(x => x.OfficeId == officeId && x.Year == year && x.FundId == fund.Id).SingleOrDefault();
    //                    if (o != null)
    //                    {
    //                        fm.Amount = o.Amount;
    //                    }

    //                    Funds.Add(fm);
    //                }
    //            }
    //        }
    //    }
    //}

    public class DepartmentAllocationModel
    {
        public tblDepartment  Department { get; set; }
        public int Year { get; set; }
        public double? Amount { get; set; }
                
        public double UsedAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblPPMPs
                    .Where(x => x.Year == Year && x.DepartmentId == Department.DepartmentId)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .Sum(x => new PPMPModel(x.Id).TotalAmount());
            }
        }

        public DepartmentAllocationModel()
        { }

        public DepartmentAllocationModel(int deptId, int year)
        {
            this.Year = year;

            using (procurementDataContext context = new procurementDataContext())
            {
                Department = Cache.Get_Tables().Departments.Where(x => x.DepartmentId == deptId).SingleOrDefault();

                var t = context.tblDepartmentAllocations.Where(x => x.DepartmentId == deptId && x.Year == year).SingleOrDefault();
                if (t != null)
                {
                    Amount = t.Amount;
                }
            }
        }
    }
    
}