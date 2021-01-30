using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using coreLib.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class RFQModel
    {
        public tblRFQ rfq { get; set; }
        private List<tblPPMPItem> Items { get; set; }
        public List<tblPPMPItem> ConsolidatedItems { get; set; }

        public double TotalAmount()
        {
            return rfq._TotalAmount ?? ConsolidatedItems.Sum(x => x.Amount);
        }

        public double TotalOOSAmount()
        {
            return ConsolidatedItems.Where(x => x.IsOOS).Sum(x => x.OOSAmount);
        }

        public RFQModel()
        { }

        public RFQModel(int rfqId)
        {
            UpdateItems(rfqId);
        }

        public void ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblRFQ _rfq = context.tblRFQs.Where(x => x.Id == rfq.Id).Single();

                _rfq.CPRIds = string.Join(",", ids);
                _rfq.CategoryIds = string.Join(",", category_ids);
                _rfq.Months = string.Join(",", period_ids);

                context.SubmitChanges();

                rfq = context.tblRFQs.Where(x => x.Id == rfq.Id).Single();

                ConsolidateItems();
            }
        }

        public List<tblPPMPItem> ConsolidateItems()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblPPMPItem> ret = new List<tblPPMPItem>();
                

                foreach (tblPPMPItem item in Items)
                {
                    if (!rfq.ContainsCategory(item.ItemCategoryId) || 
                        item.CategoryMOP() == (int)Enums.ModeOfProcurement.PublicBidding) continue;

                    if (!rfq.ContainsMonth(1)) item.M1 = 0;
                    if (!rfq.ContainsMonth(2)) item.M2 = 0;
                    if (!rfq.ContainsMonth(3)) item.M3 = 0;
                    if (!rfq.ContainsMonth(4)) item.M4 = 0;
                    if (!rfq.ContainsMonth(5)) item.M5 = 0;
                    if (!rfq.ContainsMonth(6)) item.M6 = 0;
                    if (!rfq.ContainsMonth(7)) item.M7 = 0;
                    if (!rfq.ContainsMonth(8)) item.M8 = 0;
                    if (!rfq.ContainsMonth(9)) item.M9 = 0;
                    if (!rfq.ContainsMonth(10)) item.M10 = 0;
                    if (!rfq.ContainsMonth(11)) item.M11 = 0;
                    if (!rfq.ContainsMonth(12)) item.M12 = 0;

                    if (!item.HasEntry()) continue;

                    var ii = ret.Where(x => x.ItemId == item.ItemId).SingleOrDefault();

                    if (ii == null)
                    {
                        ii = new tblPPMPItem
                        {
                            PPMPId = item.PPMPId,
                            ItemId = item.ItemId,
                            OOS = item.OOS,
                            DBMPrice = item.DBMPrice
                        };

                        ii.Merge(item);

                        ret.Add(ii);
                    }
                    else
                    {
                        ii.Merge(item);
                    }
                    
                    ii.SetItemValues();
                }

                ConsolidatedItems = ret;
                UpdateAmount();

                return ret;
            }
        }

        private List<ConflictItem> ItemsInConflict(string months, List<tblPPMPItem> items)
        {
            return Common._ItemsInConflict(months, items, ConsolidatedItems, rfq.Months);
        }

        public List<GroupedConflictItems> ItemsInConflictWithOtherRFQs()
        {
            List<GroupedConflictItems> ret = new List<GroupedConflictItems>();

            List<int> cprIds = new List<int>();
            if (!string.IsNullOrEmpty(rfq.CPRIds))
            {
                cprIds = rfq.CPRIds.Split(',').Select(x => int.Parse(x)).ToList();
            }

            if (cprIds.Any())
            {

                using (procurementDataContext context = new procurementDataContext())
                {
                    var rfqs = context.tblRFQs
                        .Where(x => x.Id != rfq.Id && x.Year == rfq.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted && x.ContainedCPRIds(cprIds).Any())
                        .ToList();
                    
                    foreach (tblRFQ _rfq in rfqs)
                    {
                        List<ConflictItem> conflicts = new RFQModel(_rfq.Id).ItemsInConflict(rfq.Months, ConsolidatedItems);

                        if (conflicts.Any())
                        {
                            ret.Add(
                            new GroupedConflictItems
                            {
                                active = "rfq",
                                RFQ = _rfq,
                                Items = conflicts
                            });
                        }

                    }
                }
            }

            return ret;
        }

        private void UpdateItems(int rfqId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                rfq = context.tblRFQs.Where(x => x.Id == rfqId).Single();

                Items = context.tblPPMPItems
                        .Where(x => (rfq.CPRIds ?? "").Split(',').Contains(x.CPRId.ToString()))
                        .ToList();

                Items.ForEach(x => x.SetItemValues());

                ConsolidateItems();

            }
        }

        private void UpdateAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblRFQ _rfq = context.tblRFQs.Where(x => x.Id == rfq.Id).Single();
                _rfq._TotalAmount = ConsolidatedItems.Sum(x => x.Amount);

                context.SubmitChanges();
            }
        }
    }
}