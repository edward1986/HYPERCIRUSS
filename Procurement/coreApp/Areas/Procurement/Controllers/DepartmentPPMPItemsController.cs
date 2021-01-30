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

namespace coreApp.Areas.Procurement.Controllers
{
    [PPMPInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_department_ppmp")]
    public class DepartmentPPMPItemsController : ProcurementBaseController, IPPMPController
    {
        public tblPPMP PPMP { get; set; }

        public virtual ActionResult Index(bool? fromAPP, bool? fromPR)
        {
            Session["FromAPP"] = null;
            if (fromAPP != null)
            {
                Session["FromAPP"] = fromAPP.Value;
            }

            Session["FromPR"] = null;
            if (fromPR != null)
            {
                Session["FromPR"] = fromPR.Value;
            }

            return View(PPMP);
        }

        public ActionResult MainList()
        {
            return PartialView(PPMP);
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
        public ActionResult SaveItem(tblPPMPItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (model.Qty <= 0)
                    {
                        AddError("No quantity specified");
                    }

                    if (PPMP.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        tblPPMPItem item = context.tblPPMPItems.Where(x => x.APPId == null && x.PRId == null && x.CPRId == null && x.PPMPId == model.PPMPId && x.ItemId == model.ItemId).SingleOrDefault();
                        if (item == null)
                        {
                            context.tblPPMPItems.InsertOnSubmit(model);
                        }
                        else
                        {
                            item.M1 = model.M1 == 0 ? null : model.M1;
                            item.M2 = model.M2 == 0 ? null : model.M2;
                            item.M3 = model.M3 == 0 ? null : model.M3;
                            item.M4 = model.M4 == 0 ? null : model.M4;
                            item.M5 = model.M5 == 0 ? null : model.M5;
                            item.M6 = model.M6 == 0 ? null : model.M6;
                            item.M7 = model.M7 == 0 ? null : model.M7;
                            item.M8 = model.M8 == 0 ? null : model.M8;
                            item.M9 = model.M9 == 0 ? null : model.M9;
                            item.M10 = model.M10 == 0 ? null : model.M10;
                            item.M11 = model.M11 == 0 ? null : model.M11;
                            item.M12 = model.M12 == 0 ? null : model.M12;
                        }

                        context.SubmitChanges();

                        tblPPMP ppmp = context.tblPPMPs.Where(x => x.Id == PPMP.Id).Single();
                        PPMPModel ppmpModel = new PPMPModel(PPMP.Id);

                        ppmp._TotalAmount_InDBM = ppmpModel.TotalAmount(true);
                        ppmp._TotalAmount_NotInDBM = ppmpModel.TotalAmount(false);

                        context.SubmitChanges();

                        res.Remarks = "Record was successfully saved";
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
        public ActionResult DeleteItem(tblPPMPItem model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (PPMP.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (ModelState.IsValid)
                    {
                        tblPPMPItem item = context.tblPPMPItems.Where(x => x.APPId == null && x.PRId == null && x.CPRId == null && x.PPMPId == model.PPMPId && x.ItemId == model.ItemId).SingleOrDefault();
                        if (item == null)
                        {
                            throw new Exception("Item does not exist");
                        }

                        context.tblPPMPItems.DeleteOnSubmit(item);
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
        public ActionResult ClearItems()
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (ModelState.IsValid)
                    {
                        var items = context.tblPPMPItems.Where(x => x.APPId == null && x.PRId == null & x.CPRId == null && x.PPMPId == PPMP.Id);

                        context.tblPPMPItems.DeleteAllOnSubmit(items);
                        context.SubmitChanges();

                        res.Remarks = "Records were successfully cleared";
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