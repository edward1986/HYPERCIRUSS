using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class CompanyPRModel
    {
        public tblCompanyPR pr { get; set; }
        public List<tblPPMPItem> Items { get; set; }
        public APRModel aprModel { get; set; }
        public List<tblMOP> Categories { get; set; }


        public double TotalOOSAmount()
        {
            return pr._TotalAmount ?? Items.Sum(x => x.OOSAmount);
        }

        public CompanyPRModel(int prId)
        {
            UpdateItems(prId);
        }

        public void ImportItems(int id)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblCompanyPR _pr = context.tblCompanyPRs.Where(x => x.Id == pr.Id).Single();

                _pr.APRId = id;
                
                context.SubmitChanges();

                pr = context.tblCompanyPRs.Where(x => x.Id == pr.Id).Single();

                UpdateItems(pr.Id);
                UpdateAmount();
            }
        }
        

        private void UpdateItems(int prId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                pr = context.tblCompanyPRs.Where(x => x.Id == prId).SingleOrDefault();

                Items = new List<tblPPMPItem>();

                if (pr.APRId != null)
                {
                    aprModel = new APRModel(pr.APRId.Value, false);
                    Items = aprModel.ConsolidatedItems.Where(x => x.IsOOS).ToList();
                }

                Categories = Items
                    .Select(x => x.ItemCategoryId)
                    .Distinct()
                    .Select(y => new tblMOP
                    {
                        PRId = prId,
                        CategoryId = y,
                        ModeOfProcurement = -1
                    })
                    .ToList();

                Categories.ForEach(z =>
                {
                    tblMOP i = context.tblMOPs.Where(x => x.PRId == prId && x.Type == (int)MOPType.AgencyPR && x.CategoryId == z.CategoryId).SingleOrDefault();
                    if (i != null)
                    {
                        z.ModeOfProcurement = i.ModeOfProcurement;
                    }
                });
            }
        }

        private void UpdateAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblCompanyPR _pr = context.tblCompanyPRs.Where(x => x.Id == pr.Id).Single();

                _pr._TotalAmount = Items.Sum(x => x.OOSAmount);

                context.SubmitChanges();
            }
        }
    }

    
}