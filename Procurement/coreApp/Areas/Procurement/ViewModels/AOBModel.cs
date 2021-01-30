using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class AOBModel
    {
        public tblAOB aob { get; set; }
        public RFQModel rfqModel { get; set; }
        public List<tblPPMPItem> ConsolidatedItems { get; set; }
        public List<tblAOB_Bid> Bids { get; set; }

        public AOBModel(int aobId)
        {
            UpdateItems(aobId);
        }

        public void ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAOB _aob = context.tblAOBs.Where(x => x.Id == aob.Id).Single();

                _aob.RFQIds = string.Join(",", ids);
                _aob.CategoryIds = string.Join(",", category_ids);
                _aob.Months = string.Join(",", period_ids);

                context.SubmitChanges();

                UpdateItems(aob.Id);
            }
        }

        public List<tblPPMPItem> ConsolidateItems()
        {
            List<tblPPMPItem> ret = new List<tblPPMPItem>();

            if (rfqModel != null)
            {
                foreach (tblPPMPItem _item in rfqModel.ConsolidatedItems)
                {
                    if (!aob.ContainsCategory(_item.ItemCategoryId)) continue;

                    if (!aob.ContainsMonth(1)) _item.M1 = 0;
                    if (!aob.ContainsMonth(2)) _item.M2 = 0;
                    if (!aob.ContainsMonth(3)) _item.M3 = 0;
                    if (!aob.ContainsMonth(4)) _item.M4 = 0;
                    if (!aob.ContainsMonth(5)) _item.M5 = 0;
                    if (!aob.ContainsMonth(6)) _item.M6 = 0;
                    if (!aob.ContainsMonth(7)) _item.M7 = 0;
                    if (!aob.ContainsMonth(8)) _item.M8 = 0;
                    if (!aob.ContainsMonth(9)) _item.M9 = 0;
                    if (!aob.ContainsMonth(10)) _item.M10 = 0;
                    if (!aob.ContainsMonth(11)) _item.M11 = 0;
                    if (!aob.ContainsMonth(12)) _item.M12 = 0;

                    if (!_item.HasEntry()) continue;

                    var ii = ret.Where(x => x.ItemId == _item.ItemId).SingleOrDefault();

                    if (ii == null)
                    {
                        ii = new tblPPMPItem
                        {
                            PPMPId = _item.PPMPId,
                            ItemId = _item.ItemId
                        };

                        ii.Merge(_item);

                        ret.Add(ii);
                    }
                    else
                    {
                        ii.Merge(_item);
                    }

                    ii.SetItemValues();
                }
            }

            ConsolidatedItems = ret;

            return ret;
        }

        private List<ConflictItem> ItemsInConflict(string months, List<tblPPMPItem> _items)
        {
            return Common._ItemsInConflict(months, _items, ConsolidatedItems, aob.Months);
        }
        
        public List<GroupedConflictItems> ItemsInConflictWithOtherAOBs()
        {
            List<GroupedConflictItems> ret = new List<GroupedConflictItems>();

            using (procurementDataContext context = new procurementDataContext())
            {
                var aobs = context.tblAOBs
                    .Where(x => x.Id != aob.Id && x.Year == aob.Year)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted && int.Parse(x.RFQIds) == int.Parse(aob.RFQIds ?? "-1"))
                    .ToList();
                
                foreach (tblAOB _aob in aobs)
                {
                    List<ConflictItem> conflicts = new AOBModel(_aob.Id).ItemsInConflict(aob.Months, ConsolidatedItems);

                    if (conflicts.Any())
                    {
                        ret.Add(
                        new GroupedConflictItems
                        {
                            active = "aob",
                            AOB = _aob,
                            Items = conflicts
                        });
                    }
                    
                }
            }

            return ret;
        }

        private void UpdateItems(int aobId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                aob = context.tblAOBs.Where(x => x.Id == aobId).Single();

                tblRFQ tmp = context.tblRFQs
                    .Where(x => (aob.RFQIds ?? "").Split(',').Contains(x.Id.ToString()))
                    .FirstOrDefault();

                if (tmp != null)
                {
                    rfqModel = new RFQModel(tmp.Id);
                }

                Bids = context.tblAOB_Bids.Where(x => x.AOBId == aobId).ToList();

                ConsolidateItems();
            }
        }

        public tblAOB_Bid getBid(int supplierId, int itemId)
        {
            return Bids.Where(x => x.SupplierId == supplierId && x.ItemId == itemId).SingleOrDefault();
        }
    }
}