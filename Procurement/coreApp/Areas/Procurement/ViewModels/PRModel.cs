using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class PRModel
    {
        public tblPR pr { get; set; }
        public PPMPModel ppmpModel { get; set; }
        public List<tblPPMPItem> Items { get; set; }
        public List<tblMOP> Categories { get; set; }


        public double TotalAmount()
        {
            return pr._TotalAmount ?? Items.Sum(x => x.Amount);
        }

        public PRModel(int prId)
        {
            UpdateItems(prId);
        }

        public void ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPR _pr = context.tblPRs.Where(x => x.Id == pr.Id).Single();

                _pr.PPMPIds = string.Join(",", ids);
                _pr.CategoryIds = string.Join(",", category_ids);
                _pr.Months = string.Join(",", period_ids);

                context.SubmitChanges();

                pr = context.tblPRs.Where(x => x.Id == pr.Id).Single();

                ppmpModel = context.tblPPMPs
                   .Where(x => (pr.PPMPIds ?? "").Split(',').Contains(x.Id.ToString()))
                   .Select(x => new PPMPModel(x.Id))
                   .FirstOrDefault();

                SaveItems();
                UpdateItems(pr.Id);
                UpdateAmount();
            }
        }

        public List<tblPPMPItem> PPMPItems()
        {
            List<tblPPMPItem> ret = new List<tblPPMPItem>();

            if (ppmpModel != null)
            {
                foreach (tblPPMPItem _item in ppmpModel.Items)
                {
                    if (_item.ItemInDBM) continue;
                    if (!pr.ContainsCategory(_item.ItemCategoryId)) continue;

                    if (!pr.ContainsMonth(1)) _item.M1 = 0;
                    if (!pr.ContainsMonth(2)) _item.M2 = 0;
                    if (!pr.ContainsMonth(3)) _item.M3 = 0;
                    if (!pr.ContainsMonth(4)) _item.M4 = 0;
                    if (!pr.ContainsMonth(5)) _item.M5 = 0;
                    if (!pr.ContainsMonth(6)) _item.M6 = 0;
                    if (!pr.ContainsMonth(7)) _item.M7 = 0;
                    if (!pr.ContainsMonth(8)) _item.M8 = 0;
                    if (!pr.ContainsMonth(9)) _item.M9 = 0;
                    if (!pr.ContainsMonth(10)) _item.M10 = 0;
                    if (!pr.ContainsMonth(11)) _item.M11 = 0;
                    if (!pr.ContainsMonth(12)) _item.M12 = 0;

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

            return ret;
        }

        private List<ConflictItem> ItemsInConflict(string months, List<tblPPMPItem> _items)
        {
            return Common._ItemsInConflict(months, _items, Items, pr.Months);
        }
        
        public List<GroupedConflictItems> ItemsInConflictWithOtherPRs()
        {
            List<GroupedConflictItems> ret = new List<GroupedConflictItems>();

            using (procurementDataContext context = new procurementDataContext())
            {
                var prs = context.tblPRs
                    .Where(x => x.Id != pr.Id && x.Year == pr.Year && x.OfficeId == pr.OfficeId)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted && int.Parse(x.PPMPIds) == int.Parse(pr.PPMPIds ?? "-1"))
                    .ToList();
                
                foreach (tblPR _pr in prs)
                {
                    List<ConflictItem> conflicts = new PRModel(_pr.Id).ItemsInConflict(pr.Months, Items);

                    if (conflicts.Any())
                    {
                        ret.Add(
                        new GroupedConflictItems
                        {
                            active = "pr",
                            PR = _pr,
                            Items = conflicts
                        });
                    }
                    
                }
            }

            return ret;
        }

        public void SaveItems()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                var existing = context.tblPPMPItems
                    .Where(x => x.PRId == pr.Id)
                    .ToList();

                var consolidated = PPMPItems();
                consolidated.ForEach(x =>
                {
                    x.PRId = pr.Id;
                });

                context.tblPPMPItems.DeleteAllOnSubmit(existing);
                context.tblPPMPItems.InsertAllOnSubmit(consolidated);

                context.SubmitChanges();
            }
        }

        private void UpdateItems(int prId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                pr = context.tblPRs.Where(x => x.Id == prId).Single();

                tblPPMP tmp = context.tblPPMPs
                    .Where(x => (pr.PPMPIds ?? "").Split(',').Contains(x.Id.ToString()))
                    .FirstOrDefault();

                if (tmp != null)
                {
                    ppmpModel = new PPMPModel(tmp.Id);
                }
                
                Items = context.tblPPMPItems
                    .Where(x => x.PRId == prId)
                    .ToList();

                Items.ForEach(x => x.SetItemValues());

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
                    tblMOP i = context.tblMOPs.Where(x => x.PRId == prId && x.Type == (int)MOPType.OfficePR && x.CategoryId == z.CategoryId).SingleOrDefault();
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
                tblPR _pr = context.tblPRs.Where(x => x.Id == pr.Id).Single();
                _pr._TotalAmount = Items.Sum(x => x.Amount);

                context.SubmitChanges();
            }
        }
    }
}