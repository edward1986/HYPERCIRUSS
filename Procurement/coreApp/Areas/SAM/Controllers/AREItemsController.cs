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
    [AREInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_transactions")]
    public class AREItemsController : SAMBaseController, IAREController
    {
        public tblARE ARE { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                List<tblAREItem> model = context.tblAREItems.Where(x => x.AREId == ARE.Id).ToList();
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
                tblAREItem item = context.tblAREItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_AREItem", item);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblAREItem model = new tblAREItem();
            return PartialView("_AREItem", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblAREItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    if (context.tblAREItems.Any(x => x.AREId == model.AREId && x.InventoryItemId == model.InventoryItemId))
                    {
                        AddError("Item already exists");
                    }

                    if (ModelState.IsValid)
                    {

                        tblAREItem item = new tblAREItem
                        {
                            AREId = ARE.Id,
                            InventoryItemId = model.InventoryItemId
                        };

                        item.UpdateFields();

                        context.tblAREItems.InsertOnSubmit(item);
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
                tblAREItem item = context.tblAREItems.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_AREItem", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAREItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {

                    tblAREItem item = context.tblAREItems.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (context.tblAREItems.Any(x => x.Id != model.Id && x.AREId == model.AREId && x.InventoryItemId == model.InventoryItemId))
                    {
                        AddError("Item already exists");
                    }

                    if (ModelState.IsValid)
                    {
                        item.InventoryItemId = model.InventoryItemId;

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
                    tblAREItem item = context.tblAREItems.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblAREItems.DeleteOnSubmit(item);
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