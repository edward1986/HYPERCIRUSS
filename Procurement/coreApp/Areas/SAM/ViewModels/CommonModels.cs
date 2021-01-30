using coreApp.Areas.SAM.Enums;
using System.Collections.Generic;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class PTRModel
    {
        public List<tblAREItem> From_Items { get; set; }
        public List<tblAREItem> Items { get; set; }

        public bool ItemContains(string propertyNo)
        {
            return Items.Any(x => x._PropertyNo == propertyNo);
        }
    }

    public class UpdateDataItem
    {
        public int itemId { get; set; }
        public string itemName { get; set; }
        public int qty { get; set; }
    }

    public class IssuanceModel
    {
        public List<InventoryItemModel> Equipment { get; set; }
        public List<InventoryItemModel> SemiExpendables { get; set; }
        public List<RISItemModel> Supplies { get; set; }

        public IssuanceModel(int employeeId)
        {
            using (samDataContext context = new samDataContext())
            {
                List<AREItemModel> areItemModels = context.tblAREItems.Where(x => x._IsSemiExpendable != true)
                    .Join(context.tblAREs.Where(x => x.To_Id == employeeId), a => a.AREId, b => b.Id, (a, b) => new AREItemModel(a, b))
                    .ToList();

                int[] areInventoryIds = areItemModels.Select(x => x.Item.InventoryItemId).Distinct().ToArray();

                List<AREItemModel> icsItemModels = context.tblAREItems.Where(x => x._IsSemiExpendable == true)
                   .Join(context.tblAREs.Where(x => x.To_Id == employeeId), a => a.AREId, b => b.Id, (a, b) => new AREItemModel(a, b))
                   .ToList();

                int[] icsInventoryIds = icsItemModels.Select(x => x.Item.InventoryItemId).Distinct().ToArray();

                Equipment = context.tblInventoryItems
                    .Where(x => areInventoryIds.Contains(x.Id))
                    .Select(x => new InventoryItemModel(x))
                    .ToList()
                    .Where(x => x.LatestLog.To_Id == employeeId && x.LatestLog._AREType != (int)AREType.Return && !x.LatestLog.ToContractor)
                    .OrderByDescending(x => x.LatestLog.AREDate)
                    .ToList();

                SemiExpendables = context.tblInventoryItems
                    .Where(x => icsInventoryIds.Contains(x.Id))
                    .Select(x => new InventoryItemModel(x))
                    .ToList()
                    .Where(x => x.LatestLog.To_Id == employeeId && x.LatestLog._AREType != (int)AREType.Return && !x.LatestLog.ToContractor)
                    .OrderByDescending(x => x.LatestLog.AREDate)
                    .ToList();

                Supplies = context.tblRISItems
                    .Join(context.tblRIs.Where(x => x.EmployeeId == employeeId), a => a.RISId, b => b.Id, (a, b) => new RISItemModel
                    {
                        RIS = b,
                        Item = a
                    })
                    .OrderByDescending(x => x.RIS.RequisitionDate)
                    .ToList();
            }
        }
    }

    public class IssuanceModel_Cont
    {
        public int ContractorId { get; set; }
        public List<InventoryItemModel> Equipment { get; set; }
        public List<InventoryItemModel> SemiExpendables { get; set; }

        public IssuanceModel_Cont(int contractorId)
        {
            this.ContractorId = contractorId;

            using (samDataContext context = new samDataContext())
            {
                List<AREItemModel> areItemModels = context.tblAREItems.Where(x => x._IsSemiExpendable != true)
                     .Join(context.tblAREs.Where(x => x.To_Id == contractorId), a => a.AREId, b => b.Id, (a, b) => new AREItemModel(a, b))
                     .ToList();

                int[] areInventoryIds = areItemModels.Select(x => x.Item.InventoryItemId).Distinct().ToArray();

                List<AREItemModel> icsItemModels = context.tblAREItems.Where(x => x._IsSemiExpendable == true)
                   .Join(context.tblAREs.Where(x => x.To_Id == contractorId), a => a.AREId, b => b.Id, (a, b) => new AREItemModel(a, b))
                   .ToList();

                int[] icsInventoryIds = icsItemModels.Select(x => x.Item.InventoryItemId).Distinct().ToArray();

                Equipment = context.tblInventoryItems
                   .Where(x => areInventoryIds.Contains(x.Id))
                   .Select(x => new InventoryItemModel(x))
                   .ToList()
                   .Where(x => x.LatestLog.To_Id == contractorId && x.LatestLog._AREType != (int)AREType.Return && x.LatestLog.ToContractor)
                   .OrderByDescending(x => x.LatestLog.AREDate)
                   .ToList();

                SemiExpendables = context.tblInventoryItems
                    .Where(x => icsInventoryIds.Contains(x.Id))
                    .Select(x => new InventoryItemModel(x))
                    .ToList()
                    .Where(x => x.LatestLog.To_Id == contractorId && x.LatestLog._AREType != (int)AREType.Return && x.LatestLog.ToContractor)
                    .OrderByDescending(x => x.LatestLog.AREDate)
                    .ToList();
            }
        }
    }

}