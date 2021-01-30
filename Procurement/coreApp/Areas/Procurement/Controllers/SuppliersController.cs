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
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class SuppliersController : ProcurementBaseController
    {
        public ActionResult Index()
        {            
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblSuppliers.ToList();
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
                tblSupplier item = context.tblSuppliers.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Supplier", item);
                }
            }
        }

        public ActionResult Create()
        {
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblSupplier model = new tblSupplier();
            return PartialView("_Supplier", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblSupplier model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using(procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        tblSupplier item = new tblSupplier
                        {
                            CompanyName = model.CompanyName,
                            Address = model.Address,
                            TIN = model.TIN,
                            VAT = model.VAT,
                            Email = model.Email,
                            TelephoneNo = model.TelephoneNo,
                            MobileNo = model.MobileNo,
                            ContactPerson = model.ContactPerson,
                            Gender = model.Gender,
                            Notes = model.Notes
                        };

                        context.tblSuppliers.InsertOnSubmit(item);
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
                tblSupplier item = context.tblSuppliers.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Supplier", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSupplier model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblSupplier item = context.tblSuppliers.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    
                    if (ModelState.IsValid)
                    {
                        item.CompanyName = model.CompanyName;
                        item.Address = model.Address;
                        item.TIN = model.TIN;
                        item.VAT = model.VAT;
                        item.Email = model.Email;
                        item.TelephoneNo = model.TelephoneNo;
                        item.MobileNo = model.MobileNo;
                        item.ContactPerson = model.ContactPerson;
                        item.Gender = model.Gender;
                        item.Notes = model.Notes;

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
                    tblSupplier item = context.tblSuppliers.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblSuppliers.DeleteOnSubmit(item);
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