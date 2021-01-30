using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class CPRModel
    {
        public tblConsolidatedPR cpr { get; set; }
        public List<tblPPMPItem> Items { get; set; }
        public List<tblMOP> Categories { get; set; }
        
        public double TotalAmount()
        {
            return cpr._TotalAmount ?? Items.Sum(x => x.Amount);
        }

        public CPRModel(int cprId)
        {
            UpdateItems(cprId);
        }

        public void ImportItems(int[] ids, int[] cids, int[] category_ids, int[] period_ids, int[] fund_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblConsolidatedPR _cpr = context.tblConsolidatedPRs.Where(x => x.Id == cpr.Id).Single();

                _cpr.PRIds = "";
                _cpr.CompanyPRIds = "";

                if (ids != null)
                {
                    _cpr.PRIds = string.Join(",", ids);
                }

                if (cids != null)
                {
                    _cpr.CompanyPRIds = string.Join(",", cids);
                }
                
                _cpr.CategoryIds = string.Join(",", category_ids);
                _cpr.Months = string.Join(",", period_ids);
                _cpr.FundIds = string.Join(",", fund_ids);

                context.SubmitChanges();

                cpr = context.tblConsolidatedPRs.Where(x => x.Id == cpr.Id).Single();

                SaveItems();
                UpdateItems(cpr.Id);
                UpdateAmount();
            }
        }

        public List<tblPPMPItem> ConsolidatedItems()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                Items = new List<tblPPMPItem>();

                if (!string.IsNullOrEmpty(cpr.PRIds))
                {
                    foreach (int prId in cpr.PRIds.Split(',').Select(x => int.Parse(x)))
                    {
                        PRModel prModel = new PRModel(prId);
                        Items.AddRange(prModel.Items);
                    }
                }

                if (!string.IsNullOrEmpty(cpr.CompanyPRIds))
                {
                    foreach (int prId in cpr.CompanyPRIds.Split(',').Select(x => int.Parse(x)))
                    {
                        CompanyPRModel prModel = new CompanyPRModel(prId);
                        Items.AddRange(prModel.Items);
                    }
                }

                List<tblPPMPItem> ret = new List<tblPPMPItem>();
                int[] fundIds = new int[] { };

                if (cpr.FundIds != null)
                {
                    fundIds = cpr.FundIds.Split(',').Select(x => int.Parse(x)).ToArray();
                }

                foreach (tblPPMPItem item in Items.Where(x => fundIds.Contains(x.ItemFundId)))
                {
                    if (!cpr.ContainsCategory(item.ItemCategoryId)) continue;

                    if (!cpr.ContainsMonth(1)) item.M1 = 0;
                    if (!cpr.ContainsMonth(2)) item.M2 = 0;
                    if (!cpr.ContainsMonth(3)) item.M3 = 0;
                    if (!cpr.ContainsMonth(4)) item.M4 = 0;
                    if (!cpr.ContainsMonth(5)) item.M5 = 0;
                    if (!cpr.ContainsMonth(6)) item.M6 = 0;
                    if (!cpr.ContainsMonth(7)) item.M7 = 0;
                    if (!cpr.ContainsMonth(8)) item.M8 = 0;
                    if (!cpr.ContainsMonth(9)) item.M9 = 0;
                    if (!cpr.ContainsMonth(10)) item.M10 = 0;
                    if (!cpr.ContainsMonth(11)) item.M11 = 0;
                    if (!cpr.ContainsMonth(12)) item.M12 = 0;

                    if (!item.HasEntry()) continue;

                    var ii = ret.Where(x => x.ItemId == item.ItemId).SingleOrDefault();

                    if (ii == null)
                    {
                        ii = new tblPPMPItem
                        {
                            PPMPId = item.PPMPId,
                            ItemId = item.ItemId,
                            OOS = item.OOS,
                            DBMPrice = item.DBMPrice,
                            CPR_OOS = item.OOS,
                            CPR_NewPrice = item.DBMPrice
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

                return ret;
            }
        }

        private List<ConflictItem> ItemsInConflict(string months, List<tblPPMPItem> items)
        {
            return Common._ItemsInConflict(months, items, Items, cpr.Months);
        }

        public List<GroupedConflictItems> ItemsInConflictWithOtherCPRs()
        {
            List<GroupedConflictItems> ret = new List<GroupedConflictItems>();
            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> prIds = new List<int>();
                List<int> companyPRIds = new List<int>();

                if (!string.IsNullOrEmpty(cpr.PRIds))
                {
                    prIds = cpr.PRIds.Split(',').Select(x => int.Parse(x)).ToList();
                }

                if (!string.IsNullOrEmpty(cpr.CompanyPRIds))
                {
                    companyPRIds = cpr.CompanyPRIds.Split(',').Select(x => int.Parse(x)).ToList();
                }

                if (prIds.Any())
                {
                    var cprs = context.tblConsolidatedPRs
                        .Where(x => x.Id != cpr.Id && x.Year == cpr.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted && (x.ContainedPRIds(prIds).Any() || x.ContainedCompanyPRIds(companyPRIds).Any()))
                        .ToList();

                    List<tblPPMPItem> consolidatedItems = Items;

                    foreach (tblConsolidatedPR _cpr in cprs)
                    {
                        List<ConflictItem> conflicts = new CPRModel(_cpr.Id).ItemsInConflict(cpr.Months, consolidatedItems);

                        if (conflicts.Any())
                        {
                            ret.Add(
                            new GroupedConflictItems
                            {
                                active = "cpr",
                                CPR = _cpr,
                                Items = conflicts
                            });
                        }

                    }

                }

                //if (companyPRIds.Any())
                //{
                //    var cprs = context.tblConsolidatedPRs
                //        .Where(x => x.Id != cpr.Id && x.Year == cpr.Year)
                //        .ToList()
                //        .Where(x => x.HasBeenSubmitted && x.ContainedCompanyPRIds(companyPRIds).Any())
                //        .ToList();

                //    List<tblPPMPItem> consolidatedItems = Items;

                //    foreach (tblConsolidatedPR _cpr in cprs)
                //    {
                //        List<ConflictItem> conflicts = new CPRModel(_cpr.Id).ItemsInConflict(cpr.Months, consolidatedItems);

                //        if (conflicts.Any())
                //        {
                //            ret.Add(
                //            new GroupedConflictItems
                //            {
                //                CPR = _cpr,
                //                Items = conflicts
                //            });
                //        }

                //    }

                //}

                return ret;
            }
        }

        public void SaveItems()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                var existing = context.tblPPMPItems
                    .Where(x => x.CPRId == cpr.Id)
                    .ToList();

                var consolidated = ConsolidatedItems();
                consolidated.ForEach(x =>
                {
                    x.CPRId = cpr.Id;                    
                });

                context.tblPPMPItems.DeleteAllOnSubmit(existing);
                context.tblPPMPItems.InsertAllOnSubmit(consolidated);

                context.SubmitChanges();
            }
        }
        
        private void UpdateItems(int cprId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                cpr = context.tblConsolidatedPRs.Where(x => x.Id == cprId).Single();
                
                Items = context.tblPPMPItems
                    .Where(x => x.CPRId == cprId)
                    .ToList();

                Items.ForEach(x => x.SetItemValues());

                Categories = Items
                    .Select(x => x.ItemCategoryId)
                    .Distinct()
                    .Select(y => new tblMOP
                    {
                        PRId = cprId,
                        CategoryId = y,
                        ModeOfProcurement = -1,
                        _Amount = Items.Where(z => z.ItemCategoryId == y).Sum(z => z.Amount)
                    })
                    .ToList();

                Categories.ForEach(z =>
                {
                    tblMOP i = context.tblMOPs.Where(x => x.PRId == cprId && x.Type == (int)MOPType.ConsolidatedPR && x.CategoryId == z.CategoryId).SingleOrDefault();
                    if (i != null)
                    {
                        z.ModeOfProcurement = i.ModeOfProcurement;
                        z.AllowPartial = i.AllowPartial;
                    }
                });
            }
        }

        private void UpdateAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblConsolidatedPR _cpr = context.tblConsolidatedPRs.Where(x => x.Id == cpr.Id).Single();
                _cpr._TotalAmount = Items.Sum(x => x.Amount);

                context.SubmitChanges();
            }
        }
    }
}