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
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.SAM.Enums;

namespace coreApp.Areas.SAM.Controllers
{
    [UserAccessAuthorize(allowedAccess: "sam_settings")]
    [YearInfoFilter]
    public class ItemLifeController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblItem> model = context.tblItems.Where(x => x.Year == Year)
                    .Join(context.tblCategories.Where(x => x.Type == (int)CategoryType.Equipment), a => a.CategoryId, b => b.Id, (a, b) => a)
                    .ToList();

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
                tblItem item = context.tblItems.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }

                return PartialView("_Item", item);
            }
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

                return PartialView("_Item", item);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, string NoOfMonths)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    if (!string.IsNullOrEmpty(NoOfMonths))
                    {
                        int n;
                        if (!int.TryParse(NoOfMonths, out n))
                        {
                            AddError("Invalid No. of Months value");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        tblItemLife life = context.tblItemLifes.Where(x => x.ItemId == Id).SingleOrDefault();

                        if (string.IsNullOrEmpty(NoOfMonths))
                        {
                            if (life != null)
                            {
                                context.tblItemLifes.DeleteOnSubmit(life);
                                context.SubmitChanges();
                            }
                        }
                        else if (life == null)
                        {
                            life = new tblItemLife { ItemId = Id, NoOfMonths = int.Parse(NoOfMonths) };

                            context.tblItemLifes.InsertOnSubmit(life);
                            context.SubmitChanges();
                        }
                        else
                        {
                            life.NoOfMonths = int.Parse(NoOfMonths);
                            context.SubmitChanges();
                        }

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
    }
}