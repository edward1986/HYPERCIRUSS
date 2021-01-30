using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using coreLib.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class PPMPModel
    {
        public tblPPMP ppmp { get; set; }
        public List<tblPPMPItem> Items { get; set; }

        public double TotalAmount(bool? inDBM = null)
        {
            return Items.Where(x => inDBM == null || x.ItemInDBM == inDBM.Value).Sum(x => x.Amount);
        }

        public PPMPModel()
        { }

        public PPMPModel(int ppmpId)
        {
            UpdateItems(ppmpId);
        }

        public int ImportItems(int[] ids, int[] category_ids, int[] period_ids)
        {
            int ret = 0;
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPPMP _ppmp = context.tblPPMPs.Where(x => x.Id == ppmp.Id).Single();

                _ppmp.PPMPIds = Common.AddIds(_ppmp.PPMPIds, ids);
                _ppmp.CategoryIds = Common.AddIds(_ppmp.CategoryIds, category_ids);
                _ppmp.Months = Common.AddIds(_ppmp.Months, period_ids);

                List<tblPPMPItem> _Items = context.tblPPMPItems.Where(x => x.APPId == null && x.PRId == null && x.CPRId == null && x.PPMPId == ppmp.Id).ToList();
                
                foreach (int id in ids)
                {
                    PPMPModel foreign = new PPMPModel(id);

                    foreach (tblPPMPItem item in foreign.Items)
                    {
                        if (!category_ids.Contains(item.ItemCategoryId)) continue;

                        if (!period_ids.Contains(1)) item.M1 = 0;
                        if (!period_ids.Contains(2)) item.M2 = 0;
                        if (!period_ids.Contains(3)) item.M3 = 0;
                        if (!period_ids.Contains(4)) item.M4 = 0;
                        if (!period_ids.Contains(5)) item.M5 = 0;
                        if (!period_ids.Contains(6)) item.M6 = 0;
                        if (!period_ids.Contains(7)) item.M7 = 0;
                        if (!period_ids.Contains(8)) item.M8 = 0;
                        if (!period_ids.Contains(9)) item.M9 = 0;
                        if (!period_ids.Contains(10)) item.M10 = 0;
                        if (!period_ids.Contains(11)) item.M11 = 0;
                        if (!period_ids.Contains(12)) item.M12 = 0;

                        if (!item.HasEntry()) continue;

                        tblPPMPItem ii = _Items.Where(x => x.ItemId == item.ItemId).SingleOrDefault();
                        if (ii == null)
                        {
                            ii = new tblPPMPItem
                            {
                                PPMPId = ppmp.Id,
                                ItemId = item.ItemId
                            };

                            ii.Merge(item);

                            _Items.Add(ii);
                            context.tblPPMPItems.InsertOnSubmit(ii);
                        }
                        else
                        {
                            ii.Merge(item);
                        }

                        ret += 1;
                    }
                }
                context.SubmitChanges();

                ppmp = context.tblPPMPs.Where(x => x.Id == ppmp.Id).Single();

                UpdateItems(ppmp.Id);
                UpdateAmount();
                
            }
            return ret;
        }

        private void UpdateItems(int ppmpId)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                ppmp = context.tblPPMPs.Where(x => x.Id == ppmpId).Single();

                Items = context.tblPPMPItems
                        .Where(x => x.APPId == null && x.PRId == null && x.CPRId == null && x.PPMPId == ppmpId)
                        .ToList()
                        .Where(x => x.Item != null)
                        .ToList();

                Items.ForEach(x => x.SetItemValues());
            }
        }

        private void UpdateAmount()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblPPMP _ppmp = context.tblPPMPs.Where(x => x.Id == ppmp.Id).Single();
                _ppmp._TotalAmount_InDBM = TotalAmount(true);
                _ppmp._TotalAmount_NotInDBM = TotalAmount(false);

                context.SubmitChanges();
            }
        }
    }
}