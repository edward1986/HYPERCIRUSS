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
    [CompanyPRInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class CompanyPRItemsController : ProcurementBaseController, ICompPRController
    {
        public tblCompanyPR PR { get; set; }

        public ActionResult Index(bool? fromCPR)
        {
            Session["FromCPR"] = null;
            if (fromCPR != null)
            {
                Session["FromCPR"] = fromCPR.Value;
            }

            if (!PR.HasBeenSubmitted)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    ViewBag.APRs = context.tblAPRs
                        .Where(x => x.Year == PR.Year)
                        .ToList()
                        .Where(x => x.HasBeenSubmitted && !x.InSubmittedCompanyPR())
                        .Select(x => new SelectListItemExt { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == PR.APRId, Data = new System.Collections.Hashtable { { "apr", x } } })
                        .ToList();
                    
                }
            }

            return View(PR);
        }

        public ActionResult MainList_CompanyPR()
        {
            return PartialView("MainList_Company_PR", PR);
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
        public ActionResult ImportItems(int? id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (PR.HasBeenSubmitted)
                    {
                        AddError(Constants.DOCUMENT_HAS_BEEN_SUBMITTED);
                    }

                    if (id == null)
                    {
                        AddError("No item selected");
                    }

                    if (ModelState.IsValid)
                    {
                        CompanyPRModel model = new CompanyPRModel(PR.Id);
                        model.ImportItems(id.Value);

                        res.Remarks = "APR was successfully selected";
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
        
    }
}