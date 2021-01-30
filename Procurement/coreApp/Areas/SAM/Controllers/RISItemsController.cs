using System.Linq;
using System.Web.Mvc;
using System;
using coreApp.Areas.SAM.Interfaces;
using coreLib.Objects;
using Module.Core;
using coreApp.Areas.SAM.Filters;
using System.Collections.Generic;

namespace coreApp.Areas.SAM.Controllers
{
    [RISInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_transactions")]
    public class RISItemsController : SAMBaseController, IRISController
    {
        public tblRI RIS { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                List<tblRISItem> model = context.tblRISItems.Where(x => x.RISId == RIS.Id).ToList();
                return View(model);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (samDataContext context = new samDataContext())
            {
                tblRISItem item = context.tblRISItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_RISItem", item);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblRISItem model = new tblRISItem();
            return PartialView("_RISItem", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblRISItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    if (model.POItemId == -1)
                    {
                        AddError("The Item field is required");
                    }
                    else
                    {
                        tblPOItem pOItem = context.tblPOItems.Where(x => x.Id == model.POItemId).Single();

                        StockModel stock = new StockModel(pOItem.Procurement_ItemId);
                        double balance = stock.Balance();

                        if (balance < model.Approved_Qty)
                        {
                            AddError("Approved Qty exceeds current balance (" + balance + ")");
                        }

                        if (context.tblRISItems.Any(x => x.RISId == RIS.Id && x.POItemId == model.POItemId))
                        {
                            AddError("Item already exists");
                        }
                    }

                    if (model.Approved_Qty > model.Requested_Qty)
                    {
                        AddError("Approved Qty cannot be greater than Requested Qty");
                    }
                    
                    if (ModelState.IsValid)
                    {

                        tblRISItem item = new tblRISItem
                        {
                            RISId = RIS.Id,
                            POItemId = model.POItemId,
                            Requested_Qty = model.Requested_Qty,
                            Approved_Qty = model.Approved_Qty,
                            Remarks = model.Remarks
                        };

                        item.UpdateFields();

                        context.tblRISItems.InsertOnSubmit(item);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully created";
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (samDataContext context = new samDataContext())
            {
                tblRISItem item = context.tblRISItems.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_RISItem", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblRISItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    tblRISItem item = context.tblRISItems.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (model.POItemId == -1)
                    {
                        AddError("The Item field is required");
                    }
                    else
                    {
                        tblPOItem pOItem = context.tblPOItems.Where(x => x.Id == model.POItemId).Single();

                        StockModel stock = new StockModel(pOItem.Procurement_ItemId);
                        double balance = stock.Balance() + item.Approved_Qty;

                        if (balance < model.Approved_Qty)
                        {
                            AddError("Approved Qty exceeds current balance (" + balance + ")");
                        }

                        if (context.tblRISItems.Any(x => x.RISId == RIS.Id && x.POItemId == model.POItemId && x.Id != item.Id))
                        {
                            AddError("Item already exists");
                        }
                    }

                    if (model.Approved_Qty > model.Requested_Qty)
                    {
                        AddError("Approved Qty cannot be greater than Requested Qty");
                    }

                    if (ModelState.IsValid)
                    {
                        item.POItemId = model.POItemId;
                        item.Requested_Qty = model.Requested_Qty;
                        item.Approved_Qty = model.Approved_Qty;
                        item.Remarks = model.Remarks;

                        item.UpdateFields();

                        context.SubmitChanges();

                        res.Remarks = "Record was successfully updated";
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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblRISItem item = context.tblRISItems.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblRISItems.DeleteOnSubmit(item);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";
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