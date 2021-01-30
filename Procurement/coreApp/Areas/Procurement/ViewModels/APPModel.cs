using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using coreLib.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class APPModel
    {
        public tblAPP app { get; set; }
        public List<tblPPMPItem> Items { get; set; }


        public double TotalAmount()
        {
            return app._TotalAmount ?? Items.Sum(x => x.Amount);
        }

        public APPModel()
        { }

        public APPModel(int appId)
        {
            UpdateItems(appId);
        }

        public void ImportItems(int[] ids, int[] indbm_ids, int[] category_ids, int[] period_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPP _app = context.tblAPPs.Where(x => x.Id == app.Id).Single();

                _app.PPMPIds = string.Join(",", ids);
                _app.InDBMs = string.Join(",", indbm_ids);
                _app.CategoryIds = string.Join(",", category_ids);
                _app.Months = string.Join(",", period_ids);

                var existingItems = context.tblAppItems
                   .Where(x => x.AppId == app.Id).ToList();

                context.tblAppItems.DeleteAllOnSubmit(existingItems);

                foreach (int ppmpId in ids)
                {
                    tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == ppmpId).SingleOrDefault();
                    tblAppItem appItem = new tblAppItem
                    {
                        AppId = app.Id,
                        PPMPId = ppmpId,
                        PPMPNumber = ppmp.PPMPNumber

                    };

                    context.tblAppItems.InsertOnSubmit(appItem);
                }

                context.SubmitChanges();

                app = context.tblAPPs.Where(x => x.Id == app.Id).Single();

                SaveItems();
                UpdateItems(app.Id);
                UpdateAmount();
            }
        }

        public List<tblPPMPItem> ConsolidatedItems(int[] fundIds = null)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblPPMPItem> ret = new List<tblPPMPItem>();

                List<PPMPModel> ppmpModels = context.tblPPMPs
                       .Where(x => (app.PPMPIds ?? "").Split(',').Contains(x.Id.ToString()))
                       .Select(x => new PPMPModel(x.Id))
                       .ToList();

                foreach (PPMPModel ppmp in ppmpModels)
                {

                    if (fundIds != null)
                    {
                        if (!fundIds.Any(x => x == ppmp.ppmp.FundId)) continue;
                    }

                    foreach (tblPPMPItem item in ppmp.Items)
                    {
                        bool inDBM = item.ItemInDBM;

                        if ((inDBM && !app.ContainsInDBM(1)) || (!inDBM && !app.ContainsInDBM(0))) continue;
                        if (!app.ContainsCategory(item.ItemCategoryId)) continue;

                        if (!app.ContainsMonth(1)) item.M1 = 0;
                        if (!app.ContainsMonth(2)) item.M2 = 0;
                        if (!app.ContainsMonth(3)) item.M3 = 0;
                        if (!app.ContainsMonth(4)) item.M4 = 0;
                        if (!app.ContainsMonth(5)) item.M5 = 0;
                        if (!app.ContainsMonth(6)) item.M6 = 0;
                        if (!app.ContainsMonth(7)) item.M7 = 0;
                        if (!app.ContainsMonth(8)) item.M8 = 0;
                        if (!app.ContainsMonth(9)) item.M9 = 0;
                        if (!app.ContainsMonth(10)) item.M10 = 0;
                        if (!app.ContainsMonth(11)) item.M11 = 0;
                        if (!app.ContainsMonth(12)) item.M12 = 0;

                        if (!item.HasEntry()) continue;

                        var ii = ret.Where(x => x.ItemId == item.ItemId).SingleOrDefault();

                        if (ii == null)
                        {
                            ii = new tblPPMPItem
                            {
                                PPMPId = item.PPMPId,
                                ItemId = item.ItemId
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
                }

                return ret;
            }
        }

        private List<ConflictItem> ItemsInConflict(string months, List<tblPPMPItem> items)
        {
            return Common._ItemsInConflict(months, items, Items, app.Months);
        }

        public List<GroupedConflictItems> ItemsInConflictWithOtherAPPs()
        {
            List<GroupedConflictItems> ret = new List<GroupedConflictItems>();

            List<int> ppmpIds = new List<int>();
            if (!string.IsNullOrEmpty(app.PPMPIds))
            {
                ppmpIds = app.PPMPIds.Split(',').Select(x => int.Parse(x)).ToList();
            }

            if (ppmpIds.Any())
            {

                using (procurementDataContext context = new procurementDataContext())
                {
                    var apps = context.tblAPPs
                        .Where(x => x.Id != app.Id && x.Year == app.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted && x.ContainedPPMPIds(ppmpIds).Any())
                        .ToList();

                    List<tblPPMPItem> consolidatedItems = Items;

                    foreach (tblAPP _app in apps)
                    {
                        List<ConflictItem> conflicts = new APPModel(_app.Id).ItemsInConflict(app.Months, consolidatedItems);

                        if (conflicts.Any())
                        {
                            ret.Add(
                            new GroupedConflictItems
                            {
                                active = "app",
                                APP = _app,
                                Items = conflicts
                            });
                        }

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
                    .Where(x => x.APPId == app.Id && (app.PPMPIds ?? "").Split(',').Contains(x.PPMPId.ToString()))
                    .ToList();

                var consolidated = ConsolidatedItems();
                consolidated.ForEach(x =>
                {
                    x.APPId = app.Id;
                });


                context.tblPPMPItems.DeleteAllOnSubmit(existing);
                context.tblPPMPItems.InsertAllOnSubmit(consolidated);


                context.SubmitChanges();
            }
        }

        private void UpdateItems(int appId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                app = context.tblAPPs.Where(x => x.Id == appId).Single();

                Items = context.tblPPMPItems
                    .Where(x => x.APPId == appId && (app.PPMPIds ?? "").Split(',').Contains(x.PPMPId.ToString()))
                    .ToList();

                Items.ForEach(x => x.SetItemValues());
            }
        }

        private void UpdateAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPP _app = context.tblAPPs.Where(x => x.Id == app.Id).Single();
                List<tblAppItem> _appItem = context.tblAppItems.Where(x => x.AppId == app.Id).ToList();

                _app._TotalAmount = Items.Sum(x => x.Amount);

                foreach (tblAppItem item in _appItem)
                {
                    List<tblPPMPItem> ppmpItems = context.tblPPMPItems.Where(x => x.PPMPId == item.PPMPId && x.APPId == null).ToList();                   
                    //List<tblPPMPItem> itemCategory = ppmpItems.Where(x => _app.CategoryIds.Contains(x.ItemCategoryId.ToString())).ToList();

                    //item.Total = Items.Where(x => x.PPMPId == item.PPMPId).Sum(x => x.Amount);
                    //item.MOOE = Items.Where(x => x.PPMPId == item.PPMPId && x.Item.MFO == (int)MFO.MOOE).Sum(x => x.Amount);
                    //item.CO = Items.Where(x => x.PPMPId == item.PPMPId && x.Item.MFO == (int)MFO.CO).Sum(x => x.Amount);
                                        
                    item.Total = ppmpItems.Sum(x => x.Amount);
                    item.MOOE = ppmpItems.Where(x => x.Item.MFO == (int)MFO.MOOE).Sum(x => x.Amount);
                    item.CO = ppmpItems.Where(x => x.Item.MFO == (int)MFO.CO).Sum(x => x.Amount);

                }

                context.SubmitChanges();
            }
        }
    }
}