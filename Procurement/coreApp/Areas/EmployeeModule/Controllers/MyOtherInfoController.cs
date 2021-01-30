using System;
using System.Linq;
using System.Web.Mvc;
using coreApp.Filters;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp.Interfaces;
using Module.DB;
using coreLib.Objects;
using Module.Core;

namespace coreApp.Areas.EmployeeModule.Controllers
{
    [MyFilter]
    public class MyOtherInfoController : Base_NoCoreEmployeeController, IMyController
    {
        public tblEmployee employee { get; set; }
        
        public ActionResult Details(int? id)
        {
            using (hr2017DataContext context = new hr2017DataContext())
            {
                tblEmployee_OtherInfo Info = context.tblEmployee_OtherInfos.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                if (Info == null)
                {
                    Info = new tblEmployee_OtherInfo
                    {
                        EmployeeId = employee.EmployeeId
                    };

                    context.tblEmployee_OtherInfos.InsertOnSubmit(Info);
                    context.SubmitChanges();
                }

                return View(Info);
            }

        }

        public ActionResult Edit()
        {
            ViewBag.Title = "Other Information";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (hr2017DataContext context = new hr2017DataContext())
            {


                tblEmployee_OtherInfo Info = context.tblEmployee_OtherInfos.Where(x => x.EmployeeId == employee.EmployeeId).Single();
                if (Info == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("~/Areas/HRModule/Views/EmployeeOtherInfo/_OtherInfo.cshtml", Info);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblEmployee_OtherInfo model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    if (!Cache.Get().userAccess.HasPermission("EmployeeOtherInfo"))
                    {
                        AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                    }

                    if (ModelState.IsValid)
                    {
                        DBProcs.save_EmployeeOtherInfo(model);

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
