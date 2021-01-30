using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using Module.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class DepartmentPPMPModel_Utility
    {
        public int Year { get; set; }
        public List<tblPPMP> PPMPs { get; set; }

        public int DepartmentId { get; set; }
        public tblOffice Office { get; set; }


        public bool NegativeBalance(int ppmpId)
        {
            double totalFunds = TotalAmountReceived();
            double totalSubmitted = TotalAmountOfSubmittedPPMPs();
            double balance = totalFunds - totalSubmitted;

            return balance < 0;
        }

        public double TotalAmountOfSubmittedPPMPs(int excludePPMPId = -1)
        {
            return PPMPs
                .Where(x => excludePPMPId == -1 || x.Id != excludePPMPId)
                .Where(x => x.HasBeenSubmitted)
                .Sum(x => x._TotalAmount);
        }

        public double TotalAmountReceived()
        {
            double ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                var t = context.tblDepartmentAllocations.Where(x => x.DepartmentId == DepartmentId && x.Year == Year).SingleOrDefault();
                if (t != null)
                {
                    ret = t.Amount ?? 0;
                }
            }
            return ret;
        }

        public double Balance(int excludePPMPId = -1)
        {
            return TotalAmountReceived() - TotalAmountOfSubmittedPPMPs(excludePPMPId);
        }

        public DepartmentPPMPModel_Utility(int year, tblOffice office, int departmentId, List<tblPPMP> ppmps = null)
        {
            Year = year;
            DepartmentId = departmentId;

            using (procurementDataContext context = new procurementDataContext())
            {
                if (ppmps == null)
                {
                    PPMPs = context.tblPPMPs
                        .Where(x => x.Year == year && x.OfficeId == office.OfficeId)
                        .ToList();

                }
                else
                {
                    PPMPs = ppmps;
                }
            }
        }
    }

    public class DepartmentPPMPModel
    {
        public int Year { get; set; }
        public tblOffice Office { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public List<tblPPMP> PPMPs { get; set; }

        public DepartmentPPMPModel_Utility utility { get; set; }

        public DepartmentPPMPModel(int year, tblEmployee employee)
        {
            init(year, employee.Office, employee.Department, employee.DepartmentId);
        }

        public DepartmentPPMPModel(int year, tblOffice office, string department, int departmentId)
        {
            init(year, office, department, departmentId);
        }

        void init(int year, tblOffice office, string department, int departmentId)
        {
            this.Year = year;
            this.Office = office;
            this.DepartmentId = departmentId;
            this.Department = department;
            
            using (procurementDataContext context = new procurementDataContext())
            {
                PPMPs = context.tblPPMPs
                    .Where(x => x.Year == year && x.OfficeId == office.OfficeId && x.DepartmentId == departmentId)
                    .ToList();
            }

            utility = new DepartmentPPMPModel_Utility(year, office, departmentId, PPMPs);
        }

    }
    
    
    
}