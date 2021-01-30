using System.Linq;
using System.Web.Mvc;
using System;
using coreApp.Areas.SAM.Interfaces;
using coreLib.Objects;
using Module.Core;
using coreApp.Areas.SAM.Filters;
using System.Collections.Generic;
using coreApp.Areas.SAM.Enums;

namespace coreApp.Areas.SAM.Controllers
{
    [PTRInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_transactions")]
    public class PTRItemsController : SAMBaseController, IAREController
    {
        public tblARE ARE { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                List<tblAREItem> myItems = context.tblAREItems.Where(x => x.AREId == ARE.Id).ToList();                

                PTRModel model = new PTRModel
                {
                    From_Items = new List<tblAREItem>(),
                    Items = myItems
                };
                                
                if ((!FixedSettings.IncludeContractors || !ARE.FromContractor))
                {
                    model.From_Items = context.tblAREs.Where(x => x.To_Id == ARE.From_Id && x.To_Type == ARE.From_Type)
                             .Join(context.tblAREItems, a => a.Id, b => b.AREId, (a, b) => b)
                             .Join(context.tblInventoryItems, a => a._PropertyNo, b => b.PropertyNo, (a, b) => new { a = a, b = new InventoryItemModel(b) })
                             .ToList()
                             .Where(x => x.b.LatestLog._AREType != (int)AREType.Return && !x.b.LatestLog.ToContractor && x.b.LatestLog.To_Id == ARE.From_Id && !myItems.Any(y => y._PropertyNo == x.a._PropertyNo))
                             .Select(x => x.a)
                             .Union(myItems)
                             .OrderBy(x => x._ItemName)
                             .ToList();
                }
                else
                {
                    model.From_Items = context.tblAREs.Where(x => x.To_Id == ARE.From_Id && x.To_Type == ARE.From_Type)
                             .Join(context.tblAREItems, a => a.Id, b => b.AREId, (a, b) => b)
                             .Join(context.tblInventoryItems, a => a._PropertyNo, b => b.PropertyNo, (a, b) => new { a = a, b = new InventoryItemModel(b) })
                             .ToList()
                             .Where(x => x.b.LatestLog._AREType != (int)AREType.Return && x.b.LatestLog.To_Id == ARE.From_Id && !myItems.Any(y => y._PropertyNo == x.a._PropertyNo))
                             .Select(x => x.a)
                             .Union(myItems)
                             .OrderBy(x => x._ItemName)
                             .ToList();
                }

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UpdateItems(int[] data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (data == null)
            {
                data = new int[] { };
            }

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    
                    if (ModelState.IsValid)
                    {
                        List<tblAREItem> removeItems = context.tblAREItems.Where(x => x.AREId == ARE.Id).ToList();
                        
                        List<tblAREItem> insertItems = new List<tblAREItem>();

                        foreach (tblAREItem item in context.tblAREItems.Where(x => data.Contains(x.Id)))
                        {
                            tblAREItem newItem = new tblAREItem
                            {
                                AREId = ARE.Id,
                                InventoryItemId = item.InventoryItemId
                            };

                            newItem.UpdateFields();

                            insertItems.Add(newItem);
                        }

                        context.tblAREItems.DeleteAllOnSubmit(removeItems);
                        context.tblAREItems.InsertAllOnSubmit(insertItems);

                        context.SubmitChanges();

                        res.Remarks = "Items were successfully updated";
                    }
                    else
                    {
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }
    }
}