using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    
    public class ConflictItem
    {
        public tblPPMPItem Item { get; set; }
        public List<int> Months { get; set; }

        public List<string> strMonths
        {
            get
            {
                return Months.Select(x => new DateTime(2000, x, 1).ToString("MMM")).ToList();
            }
        }
    }

    public class GroupedConflictItems
    {
        public string active { get; set; }
        
        public tblConsolidatedPR CPR { get; set; }
        public tblPR PR { get; set; }
        public tblCompanyPR CompanyPR { get; set; }
        public tblAPP APP { get; set; }
        public tblRFQ RFQ { get; set; }
        public tblAOB AOB { get; set; }
        public tblPO PO { get; set; }

        public List<ConflictItem> Items { get; set; }

        public bool ItemIsInConflict(int itemId, int m)
        {
            return Items.Any(x => x.Item.ItemId == itemId && x.Months.Contains(m));
        }
    }
    
}