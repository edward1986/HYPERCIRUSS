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
using Module.Core;

namespace coreApp.Areas.Procurement.Controllers
{
    [APPInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class CompanyAPPItemsController : ProcurementBaseController, IAPPController
    {
        public tblAPP APP { get; set; }

        public ActionResult Index(bool? fromAPR)
        {
            Session["FromAPR"] = null;
            if (fromAPR != null)
            {
                Session["FromAPR"] = fromAPR.Value;
            }

            if (!APP.HasBeenSubmitted)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    ViewBag.Categories = context.tblCategories
                        .OrderBy(x => x.Category)
                        .Select(x => new SelectListItem { Text = x.Category, Value = x.Id.ToString(), Selected = APP.ContainsCategory(x.Id) })
                        .ToList();

                    List<SelectListItem> m = SelectItems.getMonths(showBlankItem: false);
                    m.ForEach(x =>
                    {
                        x.Selected = APP.ContainsMonth(int.Parse(x.Value));
                    });

                    ViewBag.Months = m;

                    ViewBag.PPMPs = context.tblPPMPs
                        .Where(x => x.Year == APP.Year)
                        .ToList()
                        .Where(x => x.IsApproved && !x.IsDepartmentPPMP)
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = APP.ContainsPPMP(x.Id), Data = new System.Collections.Hashtable { { "ppmp", x } } })
                        .ToList();

                    ViewBag.InDBM = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Yes", Value = "1", Selected = APP.ContainsInDBM(1) },
                        new SelectListItem { Text = "No", Value = "0", Selected = APP.ContainsInDBM(0) }
                    };
                }
            }

            return View(APP);
        }

        public ActionResult MainList_Company()
        {
            return PartialView(APP);
        }

        public ActionResult ItemList(int year)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                var model = context.tblItems.Where(x => x.Year == year).OrderBy(x => x.Name).ToList();
                return PartialView(model);
            }
        }

        [HttpPost]
        public ActionResult ImportItems(int[] ids, int[] indbm_ids, int[] category_ids, int[] period_ids)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (APP.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ids == null)
                    {
                        AddError("No item selected");
                    }

                    if (indbm_ids == null)
                    {
                        AddError("No In-DBM flag selected");
                    }

                    if (category_ids == null)
                    {
                        AddError("No category selected");
                    }

                    if (period_ids == null)
                    {
                        AddError("No month selected");
                    }

                    if (ModelState.IsValid)
                    {
                        APPModel model = new APPModel(APP.Id);
                        model.ImportItems(ids, indbm_ids, category_ids, period_ids);

                        res.Remarks = "Selected PPMPs were successfully consolidated";
                        TempData["GlobalMessage"] = res.Remarks;
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
                tblAppItem item = context.tblAppItems.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_AppItem", item);
                }
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
                tblAppItem item = context.tblAppItems.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_AppItem", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAppItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblAppItem item = context.tblAppItems.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (ModelState.IsValid)
                    {
                        item.MOP = model.MOP;
                        item.Advertisement = model.Advertisement;
                        item.Submission = model.Submission;
                        item.NoticeOfAward = model.NoticeOfAward;
                        item.ContractSigning = model.ContractSigning;
                        item.Remarks = model.Remarks;

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
        public ActionResult UpdateItems(List<APPItemModel> data)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {

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