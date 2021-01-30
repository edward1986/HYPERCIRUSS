using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.Areas.Procurement.DAL;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using System.Collections.Generic;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [UserAccessAuthorize(allowedAccess: "procurement_settings")]
    public class UnitsController : ProcurementBaseController
    {
        public ActionResult Index()
        {            
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblUnits.ToList();
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

            using (procurementDataContext context = new procurementDataContext())
            {
                tblUnit unit = context.tblUnits.Where(x => x.Id == id).SingleOrDefault();
                if (unit == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Unit", unit);
                }
            }
        }

        public ActionResult Create()
        {
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblUnit model = new tblUnit();
            return PartialView("_Unit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblUnit model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using(procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        tblUnit unit = new tblUnit
                        {
                            Unit = model.Unit
                        };

                        context.tblUnits.InsertOnSubmit(unit);
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

            using (procurementDataContext context = new procurementDataContext())
            {
                tblUnit unit = context.tblUnits.Where(x => x.Id == id).Single();
                if (unit == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Unit", unit);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblUnit model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblUnit unit = context.tblUnits.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (unit == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (context.tblUnits.Any(x => x.Id != model.Id && x.Unit.Trim().ToLower() == model.Unit.Trim().ToLower()))
                    {
                        AddError("Unit already exists");
                    }

                    if (ModelState.IsValid)
                    {
                        unit.Unit = model.Unit;

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
                using (procurementDataContext context = new procurementDataContext())
                {
                    tblUnit unit = context.tblUnits.Where(x => x.Id == id).SingleOrDefault();
                    if (unit == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (context.tblItems.Any(x => x.UnitId == id))
                    {
                        AddError("Cannot delete this unit. Items with this unit has already been added.");
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblUnits.DeleteOnSubmit(unit);
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