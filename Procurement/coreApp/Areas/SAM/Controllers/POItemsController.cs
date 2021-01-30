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
    [POInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_receiving")]
    public class POItemsController : SAMBaseController, IPOController
    {
        public tblPO PO { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                POModel poModel = new POModel(PO);
                return View(poModel.Items);
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
                tblPOItem item = context.tblPOItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_POItem", item);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblPOItem model = new tblPOItem
            {
                Qty = 1,
                Amount = 0,
                _CategoryType = -1
            };
            return PartialView("_POItem", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblPOItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    using (coreApp.Areas.Procurement.DAL.procurementDataContext _context = new Procurement.DAL.procurementDataContext())
                    {

                        if (PO.Locked)
                        {
                            AddError("P.O. Items cannot be modified");
                        }

                        if (ModelState.IsValid)
                        {
                            coreApp.Areas.Procurement.DAL.tblItem procurementItem = _context.tblItems.Where(x => x.Id == model.Procurement_ItemId).Single();

                            tblPOItem item = new tblPOItem
                            {
                                POId = PO.Id,
                                UnitId = model.UnitId == -1 ? procurementItem.UnitId : model.UnitId,
                                CategoryId = model.CategoryId == -1 ? procurementItem.CategoryId : model.CategoryId,
                                ItemName = procurementItem.Name,
                                Qty = model.Qty,
                                UnitCost = model.UnitCost,
                                Amount = model.Qty * model.UnitCost,
                                Procurement_ItemId = model.Procurement_ItemId
                            };

                            item.UpdateFields();

                            if (model._CategoryType != -1)
                            {
                                item._CategoryType = model._CategoryType;
                            }

                            context.tblPOItems.InsertOnSubmit(item);
                            context.SubmitChanges();

                            res.Remarks = "Record was successfully created";
                        }
                        else
                        {
                            throw new Exception(coreProcs.ShowErrors(ModelState));
                        }
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
                tblPOItem item = context.tblPOItems.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_POItem", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPOItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    using (coreApp.Areas.Procurement.DAL.procurementDataContext _context = new Procurement.DAL.procurementDataContext())
                    {

                        tblPOItem item = context.tblPOItems.Where(x => x.Id == model.Id).SingleOrDefault();
                        if (item == null)
                        {
                            throw new Exception(ModuleConstants.ID_NOT_FOUND);
                        }

                        POModel pOModel = new POModel(PO);

                        if (PO.Locked)
                        {
                            AddError("P.O. Items cannot be modified");
                        }

                        if (pOModel.Items.Any(x => x.Item.Id == model.Id && x.Delivered > model.Qty))
                        {
                            AddError("Receipts greater than this quantity have already been made for this item");
                        }

                        if (ModelState.IsValid)
                        {
                            coreApp.Areas.Procurement.DAL.tblItem procurementItem = _context.tblItems.Where(x => x.Id == model.Procurement_ItemId).Single();

                            item.UnitId = model.UnitId == -1 ? procurementItem.UnitId : model.UnitId;
                            item.CategoryId = model.CategoryId == -1 ? procurementItem.CategoryId : model.CategoryId;
                            item.Qty = model.Qty;
                            item.UnitCost = model.UnitCost;
                            item.Amount = model.Qty * model.UnitCost;

                            item.UpdateFields();

                            if (model._CategoryType != -1)
                            {
                                item._CategoryType = model._CategoryType;
                            }

                            context.SubmitChanges();


                            //update files                            
                            List<tblRISItem> RIS = context.tblRISItems.Where(x => x.POItemId == item.Id).ToList();
                            RIS.ForEach(x => x.UpdateFields());

                            context.SubmitChanges();

                            res.Remarks = "Record was successfully updated";
                        }
                        else
                        {
                            throw new Exception(coreProcs.ShowErrors(ModelState));
                        }
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
                    tblPOItem item = context.tblPOItems.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    POModel pOModel = new POModel(PO);

                    if (PO.Locked)
                    {
                        AddError("P.O. Items cannot be modified");
                    }

                    if (pOModel.Items.Any(x => x.Item.Id == id && x.Delivered > 0))
                    {
                        AddError("Receipts have already been made for this item");
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblPOItems.DeleteOnSubmit(item);
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