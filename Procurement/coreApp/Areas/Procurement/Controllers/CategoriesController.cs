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
    public class CategoriesController : ProcurementBaseController
    {
        public ActionResult Index()
        {            
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblCategories.ToList();
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
                tblCategory cat = context.tblCategories.Where(x => x.Id == id).SingleOrDefault();
                if (cat == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Cat", cat);
                }
            }
        }

        public ActionResult Create()
        {
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblCategory model = new tblCategory();
            return PartialView("_Cat", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblCategory model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using(procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        tblCategory cat = new tblCategory
                        {
                            Code = model.Code,
                            Category = model.Category,
                            Description = model.Description
                        };

                        context.tblCategories.InsertOnSubmit(cat);
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
                tblCategory cat = context.tblCategories.Where(x => x.Id == id).Single();
                if (cat == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Cat", cat);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCategory model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblCategory cat = context.tblCategories.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (cat == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (context.tblCategories.Any(x => x.Id != model.Id && x.Category.Trim().ToLower() == model.Category.Trim().ToLower()))
                    {
                        AddError("Category already exists");
                    }

                    {
                        AddError("Cannot delete this category. Items with this category has already been added.");
                    }

                    if (ModelState.IsValid)
                    {
                        cat.Code = model.Code;
                        cat.Category = model.Category;
                        cat.Description = model.Description;

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
                    tblCategory cat = context.tblCategories.Where(x => x.Id == id).SingleOrDefault();
                    if (cat == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (context.tblItems.Any(x => x.CategoryId == id))
                    {
                        AddError("Cannot delete this category. Items with this category has already been added.");
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblCategories.DeleteOnSubmit(cat);
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