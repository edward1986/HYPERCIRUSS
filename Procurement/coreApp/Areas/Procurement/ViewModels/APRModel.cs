using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using coreLib.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class APRModel
    {
        public bool useOrigPrice { get; set; } = true;

        public tblAPR apr { get; set; }
        private List<tblPPMPItem> Items { get; set; }
        public List<tblPPMPItem> ConsolidatedItems { get; set; }
        public List<tblAPR_OO> OOS = new List<tblAPR_OO>();

        public double TotalAmount()
        {
            return apr._TotalAmount ?? ConsolidatedItems.Sum(x => x.Amount);
        }

        public double TotalOOSAmount()
        {
            return ConsolidatedItems.Where(x => x.IsOOS).Sum(x => x.OOSAmount);
        }

        public APRModel()
        { }

        public APRModel(int aprId)
        {
            UpdateItems(aprId);
        }

        public APRModel(int aprId, bool useOrigPrice)
        {
            this.useOrigPrice = useOrigPrice;
            UpdateItems(aprId);
        }

        public void ImportItems(int[] ids, int[] category_ids, int[] period_ids, int[] fund_ids)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPR _apr = context.tblAPRs.Where(x => x.Id == apr.Id).Single();

                _apr.APPIds = string.Join(",", ids);
                _apr.CategoryIds = string.Join(",", category_ids);
                _apr.Months = string.Join(",", period_ids);
                _apr.FundIds = string.Join(",", fund_ids);

                context.SubmitChanges();

                apr = context.tblAPRs.Where(x => x.Id == apr.Id).Single();

                UpdateAmount();
            }
        }

        public List<tblPPMPItem> ConsolidateItems()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblPPMPItem> ret = new List<tblPPMPItem>();
                int[] fundIds = new int[] { };

                if (apr.FundIds != null)
                {
                    fundIds = apr.FundIds.Split(',').Select(x => int.Parse(x)).ToArray();
                }

                foreach (tblPPMPItem item in Items.Where(x => fundIds.Contains(x.ItemFundId)))
                {
                    if (!apr.ContainsCategory(item.ItemCategoryId) || !item.ItemInDBM) continue;

                    if (!apr.ContainsMonth(1)) item.M1 = 0;
                    if (!apr.ContainsMonth(2)) item.M2 = 0;
                    if (!apr.ContainsMonth(3)) item.M3 = 0;
                    if (!apr.ContainsMonth(4)) item.M4 = 0;
                    if (!apr.ContainsMonth(5)) item.M5 = 0;
                    if (!apr.ContainsMonth(6)) item.M6 = 0;
                    if (!apr.ContainsMonth(7)) item.M7 = 0;
                    if (!apr.ContainsMonth(8)) item.M8 = 0;
                    if (!apr.ContainsMonth(9)) item.M9 = 0;
                    if (!apr.ContainsMonth(10)) item.M10 = 0;
                    if (!apr.ContainsMonth(11)) item.M11 = 0;
                    if (!apr.ContainsMonth(12)) item.M12 = 0;

                    if (!item.HasEntry()) continue;

                    var ii = ret.Where(x => x.ItemId == item.ItemId).SingleOrDefault();

                    if (ii == null)
                    {
                        ii = new tblPPMPItem
                        {
                            useOrigPrice = useOrigPrice,
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

                    ii.OOS = null;

                    tblAPR_OO _oos = OOS.Where(x => x.ItemId == ii.ItemId).SingleOrDefault();
                    if (_oos != null)
                    {
                        ii.OOS = _oos.Qty;
                        ii.DBMPrice = _oos.NewPrice;
                    }

                    ii.SetItemValues();
                }

                return ret;
            }
        }

        private void UpdateItems(int aprId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                apr = context.tblAPRs.Where(x => x.Id == aprId).Single();
                OOS = context.tblAPR_OOs.Where(x => x.APRId == aprId).ToList();

                Items = context.tblPPMPItems
                        .Where(x => (apr.APPIds ?? "").Split(',').Contains(x.APPId.ToString()))
                        .ToList();

                Items.ForEach(x =>
                {
                    x.useOrigPrice = useOrigPrice;
                    x.SetItemValues();
                });

                ConsolidatedItems = ConsolidateItems();

            }
        }

        public void UpdateAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblAPR _apr = context.tblAPRs.Where(x => x.Id == apr.Id).Single();
                _apr._TotalAmount = ConsolidatedItems.Sum(x => x.Amount);

                context.SubmitChanges();
            }
        }
    }
}