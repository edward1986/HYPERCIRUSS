using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.SAM.Enums;
using Module.DB;
using Module.DB.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace coreApp.Areas.SAM
{
    public static class SelectItems
    {
        public static List<SelectListItem> getWarehouses()
        {
            using (samDataContext context = new samDataContext())
            {
                List<SelectListItem> items = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };
                items.AddRange(context.tblWarehouses.OrderBy(x => x.WarehouseName).Select(x => new SelectListItem() { Text = x.WarehouseName, Value = x.Id.ToString() }));

                return items;
            }
        }

        public static List<SelectListItem> getCategoryTypes(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Item = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(CategoryType)))
            {
                _Item.Add(new SelectListItem() { Text = s, Value = i.ToString(), Selected = defaultValue == i });
                i++;
            }

            return _Item;
        }

        public static List<SelectListItem> getInventoryItems(int? warehouseId, AREType aREType, string defaultText = "", int defaultValue = -1)
        {
            using (samDataContext context = new samDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };

                if (aREType == AREType.PAR)
                {
                    _Item.AddRange(
                       context.tblReceipts.Where(x => x.WarehouseId == warehouseId)
                            .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                            .Join(context.tblInventoryItems, a => a.Id, b => b.ReceiptItemId, (a, b) => b)
                           .Select(x => new InventoryItemModel(x))
                           .ToList()
                           .Where(x => !x.Logs.Any())
                           .Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item.PropertyNo, x.Item._ItemName), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                       );
                }
                else if (aREType == AREType.PTR)
                {
                    _Item.AddRange(
                        context.tblReceipts.Where(x => x.WarehouseId == warehouseId)
                            .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                            .Join(context.tblInventoryItems, a => a.Id, b => b.ReceiptItemId, (a, b) => b)
                           .Select(x => new InventoryItemModel(x))
                           .ToList()
                           .Where(x => x.LatestLog != null)
                           .Where(x => x.LatestLog._AREType != (int)AREType.Return)
                           .Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item.PropertyNo, x.Item._ItemName), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                       );
                }
                else if (aREType == AREType.Return)
                {
                    _Item.AddRange(
                        context.tblReceipts.Where(x => x.WarehouseId == warehouseId)
                            .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                            .Join(context.tblInventoryItems, a => a.Id, b => b.ReceiptItemId, (a, b) => b)
                           .Select(x => new InventoryItemModel(x))
                           .ToList()
                           .Where(x => x.LatestLog != null)
                           .Where(x => x.LatestLog._AREType == (int)AREType.Return)
                           .Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item.PropertyNo, x.Item._ItemName), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                       );
                }


                return _Item;
            }
        }

        public static List<SelectListItem> getAllInventoryItems(int employeeId, string defaultText = "", int defaultValue = -1)
        {
            using (samDataContext context = new samDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };


                IssuanceModel model = new IssuanceModel(employeeId);
                _Item.AddRange(
                    model.Equipment.Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item._ItemName, x.Item.PropertyNo), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                    );

                _Item.AddRange(
                    model.SemiExpendables.Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item._ItemName, x.Item.PropertyNo), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                    );

                return _Item.OrderBy(x => x.Text).ToList();
            }
        }

        public static List<SelectListItem> getAllInventoryItems_Cont(int contractorId, string defaultText = "", int defaultValue = -1)
        {
            using (samDataContext context = new samDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };


                IssuanceModel_Cont model = new IssuanceModel_Cont(contractorId);
                _Item.AddRange(
                    model.Equipment.Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item._ItemName, x.Item.PropertyNo), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                    );

                _Item.AddRange(
                    model.SemiExpendables.Select(x => new SelectListItem() { Text = string.Format("{0} | {1}", x.Item._ItemName, x.Item.PropertyNo), Value = x.Item.Id.ToString(), Selected = x.Item.Id == defaultValue })
                    );

                return _Item.OrderBy(x => x.Text).ToList();
            }
        }

        public static List<SelectListItem> getInventoryStatus(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Item = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(InventoryItemStatus)))
            {
                _Item.Add(new SelectListItem() { Text = s, Value = i.ToString(), Selected = defaultValue == i });
                i++;
            }

            return _Item;
        }

        public static List<SelectListItem> getSupplyItems(int? warehouseId, string defaultText = "", int defaultValue = -1)
        {
            using (samDataContext context = new samDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };
                _Item.AddRange(
                    context.tblReceipts.Where(x => x.WarehouseId == warehouseId)
                    .Join(context.tblReceiptItems, a => a.Id, b => b.ReceiptId, (a, b) => b)
                    .Join(context.tblPOItems, a => a.POItemId, b => b.Id, (a, b) => b)
                    .Where(x => x._CategoryType == (int)CategoryType.Supply)     
                    .Distinct()
                    .Select(x => new SelectListItem() { Text = string.Format("{0} (PO #{1})", x.ItemName, x._PONo), Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                    );

                return _Item;
            }
        }

        public static List<SelectListItem> getContractors(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            using (samDataContext context = new samDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblContractors
                      .OrderBy(x => x.CompanyName).ToList()
                      .Select(x => new SelectListItem() { Text = x.CompanyName, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        /*public static List<SelectListItem> getUsers(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true, int employmentType = -1, string userId = "")
        {
            using (dalDataContext context = new dalDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "", Selected = defaultValue == -1 });
                }
                if (employmentType == -1)
                {
                    tblEmployee user = context.tblEmployees.Where(x => x.UserId == userId).SingleOrDefault();
                    if(user != null) {
                        defaultValue = user.EmployeeId;
                    }
                }


                var existingUsers = from employees in context.tblEmployees
                                           join users in context.AspNetUsers
                                           on employees.UserId equals users.Id
                                           select employees;


                _Item.AddRange(
                 existingUsers
                    .ToList()
                    .Select(x => new SelectListItem() { Text = x.FullName, Value = x.EmployeeId.ToString(), Selected = x.EmployeeId == defaultValue })
                    .OrderBy(x => x.Text)
                    .ToList()
                  );
                return _Item;
            }
        }*/
    }
}