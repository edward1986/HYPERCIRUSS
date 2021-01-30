using System.Linq;
using System.Web.Mvc;
using System;
using coreApp.Areas.SAM.Interfaces;
using coreLib.Objects;
using Module.Core;
using coreApp.Areas.SAM.Filters;
using System.Collections.Generic;
using coreApp.Areas.Procurement.DAL;

namespace coreApp.Areas.SAM.Controllers
{
    [ReceiptInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_receiving")]
    public class ReceivingItemsController : SAMBaseController, IReceiptController
    {
        public tblReceipt Receipt { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                POModel model = Common.GetPOModel(Receipt._PONo, warehouse.Id, Receipt.Id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UpdateItems(bool isClear, List<UpdateDataItem> data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    List<tblReceiptItem> receiptItems = context.tblReceiptItems.Where(x => x.ReceiptId == Receipt.Id).ToList();
                    List<tblReceiptItem> newItems = new List<tblReceiptItem>();
                    List<tblReceiptItem> removeItems = new List<tblReceiptItem>();

                    removeItems = receiptItems.Where(x => !data.Any(y => y.itemId == x.POItemId)).ToList();

                    foreach (UpdateDataItem item in data)
                    {
                        tblReceiptItem receiptItem = receiptItems.Where(x => x.POItemId == item.itemId).SingleOrDefault();
                        if (receiptItem == null)
                        {
                            newItems.Add(new tblReceiptItem
                            {
                                ReceiptId = Receipt.Id,
                                POItemId = item.itemId,
                                Qty = item.qty
                            });
                        }
                        else
                        {
                            int tagged = context.tblInventoryItems.Where(x => x.ReceiptItemId == receiptItem.Id).Count();
                            if (item.qty < tagged)
                            {
                                AddError(string.Format("{0} item(s) have already been tagged for \"{1}\"", tagged, item.itemName));
                            }

                            receiptItem.Qty = item.qty;
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        tblReceipt receipt = context.tblReceipts.Where(x => x.Id == Receipt.Id).Single();
                        receipt.ItemsLastUpdated = DateTime.Now;

                        context.tblReceiptItems.DeleteAllOnSubmit(removeItems);
                        context.tblReceiptItems.InsertAllOnSubmit(newItems);

                        context.SubmitChanges();

                        if (isClear)
                        {
                            res.Remarks = "Item entries were successfully cleared";
                        }
                        else
                        {
                            res.Remarks = "Items were successfully updated";
                        }
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