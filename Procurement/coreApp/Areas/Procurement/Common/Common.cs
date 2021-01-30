using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.Procurement
{   
    public static class Common
    {
        public static List<tblSetting> Settings(string effectivity)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                if (effectivity == Module.Core.ModuleConstants.DEFAULT_EFFECTIVITY)
                {
                    return context.tblSettings.ToList();
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(effectivity);

                    return Settings(dt);
                }
            }
        }

        public static List<tblSetting> Settings(DateTime dt)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblSetting> ret = context.tblSettings.ToList();

                List<DateTime> dates = context.tblSettings_Effectivities.Select(x => x.DateEffective).Distinct().OrderByDescending(x => x).Where(x => x <= dt).ToList();
                if (dates.Count == 0)
                {
                    return ret;
                }

                foreach (tblSetting setting in ret)
                {
                    foreach (DateTime _dt in dates)
                    {
                        tblSettings_Effectivity eff = context.tblSettings_Effectivities.Where(x => x.DateEffective == _dt && x.SettingId == setting.SettingId).SingleOrDefault();
                        if (eff != null)
                        {
                            setting.SettingValue = eff._Value;
                            setting.DateUpdated = _dt;
                            break;
                        }
                    }
                }

                return ret;
            }
        }

        public static string GetPONo(int poId, int supplierId)
        {
            return string.Format("{0}-{1}", poId.ToString("000"), supplierId.ToString("000"));
        }

        public static bool SupplierPOExists(string poNo, bool includeUnSubmitted = false)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(poNo))
            {
                if (IsValidPONoFormat(poNo))
                {
                    using (procurementDataContext context = new procurementDataContext())
                    {
                        int poId = int.Parse(poNo.Split('-')[0]);

                        tblPO po = context.tblPOs.Where(x => x.Id == poId).SingleOrDefault();
                        if (po != null)
                        {
                            if (includeUnSubmitted || po.HasBeenSubmitted)
                            {
                                ret = po.GetPONos().Any(x => x.Trim().ToLower() == poNo.Trim().ToLower());
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public static bool IsValidPONoFormat(string poNo)
        {
            bool ret = false;

            string[] tmp = poNo.Split('-');
            if (tmp.Length == 2)
            {
                int n;
                ret = int.TryParse(tmp[0], out n) && int.TryParse(tmp[1], out n);
            }

            return ret;
        }

        public static SupplierPO GetSupplierPO(string poNo, bool includeUnSubmitted = false)
        {
            SupplierPO ret = null;

            if (!string.IsNullOrEmpty(poNo))
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    int poId = -1;
                    int supplierId = -1;

                    if (poNo.Contains("-"))
                    {
                        string[] a = poNo.Split('-');
                        if (a.Length == 2)
                        {
                            int.TryParse(a[0], out poId);
                            int.TryParse(a[1], out supplierId);

                            tblPO po = context.tblPOs.Where(x => x.Id == poId).SingleOrDefault();
                            if (po != null)
                            {
                                if (includeUnSubmitted || po.HasBeenSubmitted)
                                {
                                    POModel pOModel = new POModel(po.Id);
                                    ret = Common.GetSupplierPOs(pOModel, supplierId).FirstOrDefault();
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public static List<SupplierPO> GetSupplierPOs(POModel poModel, int supplierId = -1)
        {
            List<tblAOB_Bid> wonBids;
            return GetSuppliers(poModel.po, out wonBids, supplierId)
                .Select(x => new SupplierPO(poModel, x, wonBids.Where(y => y.SupplierId == x.Id).ToList()))
                .ToList();
        }

        public static List<tblSupplier> GetSuppliers(tblPO po)
        {
            List<tblAOB_Bid> tmp;
            return GetSuppliers(po, out tmp);
        }

        public static List<tblSupplier> GetSuppliers(tblPO po, out List<tblAOB_Bid> wonBids, int supplierId = -1)
        {
            List<tblSupplier> ret = new List<tblSupplier>();

            wonBids = new List<tblAOB_Bid>();
            List<tblAOB> aobs = po.ConsolidatedAOBs();
            if (aobs.Any())
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    wonBids = context.tblAOB_Bids.Where(x => x.AOBId == aobs.First().Id && x.IsWinner).ToList();
                    int[] supplierIds = wonBids.Select(x => x.SupplierId).Distinct()
                        .Where(x => supplierId == -1 || x == supplierId)
                        .ToArray();

                    ret = context.tblSuppliers.Where(x => supplierIds.Contains(x.Id)).ToList();
                }
            }

            return ret;
        }

        public static string AddIds(string dest, int[] src)
        {
            string ret = dest;

            if (src != null)
            {
                if (string.IsNullOrEmpty(dest))
                {
                    dest = string.Join(",", src);
                }
                else
                {
                    List<int> tmp = dest.ToCleanString().Split(',').Select(x => int.Parse(x)).ToList();
                    foreach (int id in src)
                    {
                        if (!tmp.Contains(id))
                        {
                            tmp.Add(id);
                        }
                    }
                    dest = string.Join(",", tmp);
                }
            }

            return ret;
        }

        public static List<ConflictItem> _ItemsInConflict(string months, List<tblPPMPItem> items, List<tblPPMPItem> hostItems, string hostMonths)
        {
            List<ConflictItem> ret = new List<ConflictItem>();

            foreach (tblPPMPItem myItem in hostItems)
            {
                tblPPMPItem item = items.Where(x => x.ItemId == myItem.ItemId).SingleOrDefault();
                if (item != null)
                {
                    List<int> m = new List<int>();
                    string[] theirMonths = months.Split(',');

                    foreach (string myMonth in hostMonths.Split(','))
                    {
                        if (theirMonths.Contains(myMonth) && item.HasMonthEntries(int.Parse(myMonth)))
                        {
                            m.Add(int.Parse(myMonth));
                        }
                    }

                    if (m.Any())
                    {
                        ret.Add(new ConflictItem
                        {
                            Item = myItem,
                            Months = m
                        });
                    }
                }
            }

            return ret;
        }
    }

    public class SettingsModel
    {
        public double PriceMarkup { get; set; }

        public SettingsModel()
        { }

        public SettingsModel(List<tblSetting> settings)
        {
            PriceMarkup = Convert.ToDouble(settings.Where(x => x.Setting == "PriceMarkup").Single().SettingValue);
        }
    }

    public class SupplierPO
    {
        public tblPO PO { get; set; }
        public tblSupplier Supplier { get; set; }
        public List<SupplierPOItem> Items { get; set; }
        public List<tblAOB_Bid> Bids { get; set; }

        public SupplierPO(tblPO pO, tblSupplier supplier)
        {
            this.PO = pO;
            this.Supplier = supplier;
        }
        
        public SupplierPO(POModel pOModel, tblSupplier supplier, List<tblAOB_Bid> bids)
        {
            PO = pOModel.po;
            Supplier = supplier;
            Bids = bids;
            
            Items = bids.Join(pOModel.ConsolidatedItems, a => a.ItemId, b => b.ItemId, (a, b) => new SupplierPOItem { Bid = a, Item = b }).ToList();
        }      
        
        public string PONo
        {
            get
            {
                return Common.GetPONo(PO.Id, Supplier.Id);
            }
        }
    }

    public class SupplierPOItem
    {
        public tblAOB_Bid Bid { get; set; }
        public tblPPMPItem Item { get; set; }
    }

    public static class Constants
    {
        public static string DOCUMENT_HAS_BEEN_SUBMITTED = "Document has already been submitted";
        public static string DOCUMENT_HAS_NOT_BEEN_SUBMITTED = "Document has not been submitted";
        public static string DOCUMENT_HAS_BEEN_APPROVED = "Document has already been approved";
        public static string DOCUMENT_HAS_NOT_BEEN_APPROVED = "Document has not been approved";
        public static string ITEM_DESCRIPTION_EXISTS = "An item with the same description already exists";
    }
}
