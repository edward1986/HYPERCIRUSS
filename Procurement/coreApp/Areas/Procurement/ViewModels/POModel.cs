using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class POModel
    {
        public tblPO po { get; set; }
        public AOBModel aobModel { get; set; }
        public List<tblPPMPItem> ConsolidatedItems { get; set; }

        public POModel(int poId)
        {
            UpdateItems(poId);
        }

        public void ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPO _po = context.tblPOs.Where(x => x.Id == po.Id).Single();

                _po.AOBIds = string.Join(",", ids);
                _po.CategoryIds = string.Join(",", category_ids);
                _po.Months = string.Join(",", period_ids);

                context.SubmitChanges();

                UpdateItems(po.Id);
            }
        }

        public List<tblPPMPItem> ConsolidateItems()
        {
            List<tblPPMPItem> ret = new List<tblPPMPItem>();

            if (aobModel != null)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    List<tblAOB_Bid> wonBids = context.tblAOB_Bids.Where(x => x.AOBId == aobModel.aob.Id && x.IsWinner).ToList();
                    int[] itemIds = wonBids.Select(x => x.ItemId).ToArray();

                    foreach (tblPPMPItem _item in aobModel.ConsolidatedItems.Where(x => itemIds.Contains(x.ItemId)))
                    {
                        if (!po.ContainsCategory(_item.ItemCategoryId)) continue;

                        if (!po.ContainsMonth(1)) _item.M1 = 0;
                        if (!po.ContainsMonth(2)) _item.M2 = 0;
                        if (!po.ContainsMonth(3)) _item.M3 = 0;
                        if (!po.ContainsMonth(4)) _item.M4 = 0;
                        if (!po.ContainsMonth(5)) _item.M5 = 0;
                        if (!po.ContainsMonth(6)) _item.M6 = 0;
                        if (!po.ContainsMonth(7)) _item.M7 = 0;
                        if (!po.ContainsMonth(8)) _item.M8 = 0;
                        if (!po.ContainsMonth(9)) _item.M9 = 0;
                        if (!po.ContainsMonth(10)) _item.M10 = 0;
                        if (!po.ContainsMonth(11)) _item.M11 = 0;
                        if (!po.ContainsMonth(12)) _item.M12 = 0;

                        if (!_item.HasEntry()) continue;

                        var ii = ret.Where(x => x.ItemId == _item.ItemId).SingleOrDefault();

                        if (ii == null)
                        {
                            ii = new tblPPMPItem
                            {
                                PPMPId = _item.PPMPId,
                                ItemId = _item.ItemId,
                                ItemName = wonBids.Where(x => x.ItemId == _item.ItemId).Select(x => x.ItemBid).Single(),
                                ItemPrice = (double)wonBids.Where(x => x.ItemId == _item.ItemId).Select(x => x.TotalBid).Single(),
                                CPR_NewPrice = (double)wonBids.Where(x => x.ItemId == _item.ItemId).Select(x => x.TotalBid).Single(),
                                ItemUnit = _item.ItemUnit

                            };

                            ii.Merge(_item);

                            ret.Add(ii);
                        }
                        else
                        {
                            ii.Merge(_item);
                        }

                        //ii.SetItemValues();

                        //ii.ItemName = wonBids.Where(x => x.ItemId == _item.ItemId).Select(x => x.ItemBid).Single();
                                               
                       
                    }
                }
            }

            ConsolidatedItems = ret;

            return ret;
        }

        private List<ConflictItem> ItemsInConflict(string months, List<tblPPMPItem> _items)
        {
            return Common._ItemsInConflict(months, _items, ConsolidatedItems, po.Months);
        }
        
        public List<GroupedConflictItems> ItemsInConflictWithOtherPOs()
        {
            List<GroupedConflictItems> ret = new List<GroupedConflictItems>();

            using (procurementDataContext context = new procurementDataContext())
            {
                var pos = context.tblPOs
                    .Where(x => x.Id != po.Id && x.Year == po.Year)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted && int.Parse(x.AOBIds) == int.Parse(po.AOBIds ?? "-1"))
                    .ToList();
                
                foreach (tblPO _po in pos)
                {
                    List<ConflictItem> conflicts = new POModel(_po.Id).ItemsInConflict(po.Months, ConsolidatedItems);

                    if (conflicts.Any())
                    {
                        ret.Add(
                        new GroupedConflictItems
                        {
                            active = "po",
                            PO = _po,
                            Items = conflicts
                        });
                    }
                    
                }
            }

            return ret;
        }

        private void UpdateItems(int poId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                po = context.tblPOs.Where(x => x.Id == poId).Single();

                tblAOB tmp = context.tblAOBs
                    .Where(x => (po.AOBIds ?? "").Split(',').Contains(x.Id.ToString()))
                    .FirstOrDefault();

                if (tmp != null)
                {
                    aobModel = new AOBModel(tmp.Id);
                }
                
                ConsolidateItems();
            }
        }
    }
}