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
    [InventoryInspectionInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_inspection_equip")]
    public class InventoryInspectionItemsController : SAMBaseController, IInventoryInspectionController
    {
        public tblInventoryInspection InventoryInspection { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                List<tblInventoryItemStatus> model = context.tblInventoryItemStatus.Where(x => x.InspectionId == InventoryInspection.Id).ToList();

                ViewBag.InventoryInspections = context.tblInventoryInspections.Where(x => x.ReportDate.Year == InventoryInspection.ReportDate.Year).ToList();

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
                tblInventoryItemStatus item = context.tblInventoryItemStatus.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Item", item);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblInventoryItemStatus model = new tblInventoryItemStatus
            {
                Status = (int)InventoryItemStatus.Usable
            };
            return PartialView("_Item", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblInventoryItemStatus model, string PropertyNo)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    using (coreApp.Areas.Procurement.DAL.procurementDataContext _context = new Procurement.DAL.procurementDataContext())
                    {
                        tblInventoryItem inventoryItem = context.tblInventoryItems.Where(x => x.PropertyNo.Trim().ToLower() == PropertyNo.Trim().ToLower()).SingleOrDefault();

                        if (string.IsNullOrEmpty(PropertyNo))
                        {
                            AddError("The Property No. field is required");
                        }
                        else if (inventoryItem == null)
                        {
                            AddError("Property No. not found");
                        }
                        
                        if (ModelState.IsValid)
                        {
                            tblInventoryItemStatus item = new tblInventoryItemStatus
                            {
                                InspectionId = InventoryInspection.Id,
                                InventoryItemId = inventoryItem.Id,
                                Remarks = model.Remarks,
                                Status = model.Status
                            };

                            context.tblInventoryItemStatus.InsertOnSubmit(item);
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
                tblInventoryItemStatus item = context.tblInventoryItemStatus.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Item", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblInventoryItemStatus model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    tblInventoryItemStatus item = context.tblInventoryItemStatus.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        item.Remarks = model.Remarks;
                        item.Status = model.Status;

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
                    tblInventoryItemStatus item = context.tblInventoryItemStatus.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblInventoryItemStatus.DeleteOnSubmit(item);
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