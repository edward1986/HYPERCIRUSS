using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using Module.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class OfficePRModel
    {
        public int Year { get; set; }
        public tblOffice Office { get; set; }

        public List<tblPR> PRs { get; set; }
        public List<tblPPMP> OfficePPMPs { get; set; }
        
        public double TotalAmountOfAllPPMPs()
        {
            return OfficePPMPs.Sum(x => x._TotalAmount);
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
            return TotalAmountOfAllPPMPs() - TotalAmountOfSubmittedPRs(excludePRId);
        }

        public OfficePRModel(int year, tblEmployee employee)
        {
            this.Year = year;
            this.Office = employee.Office;

            PRs = new List<tblPR>();
            OfficePPMPs = new List<tblPPMP>();

            using (procurementDataContext context = new procurementDataContext())
            {
                PRs = context.tblPRs.Where(x => x.Year == Year && x.OfficeId == Office.OfficeId).ToList();

                List<int> ids = new List<int>();

                context.tblAPPs
                   .Where(x => x.Year == Year)
                   .ToList()
                   .Where(x => x.HasBeenSubmitted)
                   .ToList()
                   .ForEach(x =>
                   {
                       ids.AddRange(x.PPMPIds.Split(',').Select(y => int.Parse(y)));
                   });

                OfficePPMPs = context.tblPPMPs.Where(x => x.Year == Year && x.OfficeId == Office.OfficeId && ids.Contains(x.Id)).ToList();

               
                                
                //foreach (var app in applst)
                //{   
                //    APPModel m = new APPModel(app.Id);
                //    m.Items.ForEach(x =>
                //    {
                //        if (!OfficePPMPs.Any(y => y.ppmp.Id == x.PPMPId) && x.PPMP.OfficeId == Office.OfficeId)
                //        {
                //            OfficePPMPs.Add(new PPMPModel(x.PPMPId));
                //        }
                //    });
                //}
            }
        }
        
    }

    public class ReqQtyModel
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}