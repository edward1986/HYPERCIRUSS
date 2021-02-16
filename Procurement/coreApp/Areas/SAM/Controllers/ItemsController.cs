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
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using coreLib.Extensions;
using Module.Core;
using Newtonsoft.Json;
using System.Data.Linq;

namespace coreApp.Areas.SAM.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_settings")]
    public class ItemsController : SAMBaseController, IYearController
    {
        

        public int Year { get; set; }

        public ActionResult Index()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblItems.Where(x => x.Year == Year)
                    .OrderBy(x => x.Name)
                    .ToList();

                return View(model);
            }
        }
        public JsonResult procurementItems(DataTablesParam param)
        {

           


            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblItems;
                var List = new object();

                int pageNo = 1;

                if (param.iDisplayStart >= param.iDisplayLength)
                {

                    pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

                }
                
                int totalCount = 0;
                if (param.sSearch != null)
                {
                    totalCount = model.Where(x => x.Name.ToLower().Contains(param.sSearch.ToLower()) ).Count();
                    List = model.Where(x => x.Name.ToLower().Contains(param.sSearch.ToLower()) )
                        .Skip((pageNo - 1) * param.iDisplayLength)
                    .Take(param.iDisplayLength)
                    .Select(item => new { Value = item.Id, Name = item.Name, Unit = item.Unit.Unit, Category = item.Category.Category })
                    .ToList();
                }
                else{
                    totalCount = model.Count();
                    List = model.Skip((pageNo - 1) * param.iDisplayLength)
                    .Take(param.iDisplayLength).OrderBy(x => x.Name)
                    .Select(item => new { Value = item.Id, Name = item.Name, Unit = item.Unit.Unit, Category = item.Category.Category })
                    .ToList();
                }
                
                return Json(new { Success = true,
                    sEcho = param.sEcho,
                    iTotalDisplayRecords = totalCount,
                    iTotalRecords = totalCount, aaData = List
                }, JsonRequestBehavior.AllowGet);
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
                tblItem item = context.tblItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }

                return PartialView("_Item", item);
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblItem model = new tblItem
            {
                Year = Year
            };

            return PartialView("_Item", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true,  Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        tblItem item = new tblItem
                        {
                            Name = model.Name,
                            Description = model.Description,
                            CategoryId = model.CategoryId,
                            UnitId = model.UnitId,
                            Year = model.Year,
                            InDBM = model.InDBM,
                            OrigPrice = model.OrigPrice
                        };

                        context.tblItems.InsertOnSubmit(item);
                        context.SubmitChanges();
                        res.Data = item.Id;
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
                tblItem item = context.tblItems.Where(x => x.Id == id).Single();
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
        public ActionResult Edit(tblItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblItem item = context.tblItems.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    bool priceChange = item.OrigPrice != model.OrigPrice;

                    if (item.HasSubmittedPPMPs() && priceChange)
                    {
                        AddError("Cannot update item. Submitted PPMPs using this item must first be returned.");
                    }

                    if (ModelState.IsValid)
                    {
                        item.Name = model.Name;
                        item.Description = model.Description;
                        item.CategoryId = model.CategoryId;
                        item.UnitId = model.UnitId;
                        item.InDBM = model.InDBM;
                        item.OrigPrice = model.OrigPrice;

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
                    tblItem item = context.tblItems.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (item.HasSubmittedPPMPs())
                    {
                        AddError("Cannot delete item. Submitted PPMPs using this item must first be returned.");
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblItems.DeleteOnSubmit(item);
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

        [HttpPost]
        public ActionResult DeleteAllItems(int year)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    bool proceed = true;

                    var items = context.tblItems.Where(x => x.Year == year);
                    foreach (tblItem item in items)
                    {
                        if (item.HasSubmittedPPMPs())
                        {
                            proceed = false;
                            break;
                        }
                    }

                    if (!proceed)
                    {
                        throw new Exception("Cannot delete items. Submitted PPMPs using these items must first be returned.");
                    }


                    context.tblItems.DeleteAllOnSubmit(items);
                    context.SubmitChanges();

                    res.Remarks = "Items were successfully deleted";
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
        public ActionResult CopyItemSet(int currentYear, int newYear, bool overwrite = true)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                if (currentYear >= newYear)
                {
                    throw new Exception("Invalid year specified");
                }

                using (procurementDataContext context = new procurementDataContext())
                {
                    var items = context.tblItems.Where(x => x.Year == newYear).ToList();

                    foreach (tblItem item in context.tblItems.Where(x => x.Year == currentYear))
                    {
                        tblItem newItem = new tblItem
                        {
                            Name = item.Name,
                            Description = item.Description,
                            CategoryId = item.CategoryId,
                            UnitId = item.UnitId,
                            Year = newYear,
                            InDBM = item.InDBM,
                            OrigPrice = item.OrigPrice
                        };

                        var i = items.Where(x => x.Name.ToCleanString() == item.Name.ToCleanString());
                        if (i.Any())
                        {
                            if (overwrite)
                            {
                                context.tblItems.DeleteAllOnSubmit(i);
                                context.tblItems.InsertOnSubmit(newItem);
                            }
                        }
                        else
                        {
                            context.tblItems.InsertOnSubmit(newItem);
                        }
                    }

                    context.SubmitChanges();

                    res.Remarks = "Items were successfully copied";
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