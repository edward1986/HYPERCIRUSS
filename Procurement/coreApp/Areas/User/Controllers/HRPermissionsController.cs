using System.Linq;
using System.Web.Mvc;
using System;
using coreApp.Controllers;
using coreApp.DAL;
using coreApp.Interfaces;
using coreApp.Filters;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.User.Controllers
{
    [EmployeeInfoFilter]
    public class HRPermissionsController : Base_NoCoreHRStaffController, IEmployeeController
    {
        public tblEmployee employee { get; set; }

        public ActionResult Edit()
        {
            
            ViewBag.Title = "Permissions";
            ViewBag.Subtitle = "Edit";

            using (dalDataContext context = new dalDataContext())
            {
                _HRPermission model = context._HRPermissions.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                if (model == null)
                {
                    model = new _HRPermission
                    {
                        EmployeeId = employee.EmployeeId
                    };
                }

                return PartialView("_Permission", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string[] Permissions, _HRPermission model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    if (!Cache.Get().userAccess.HasAccess("hr_permissions"))
                    {
                        AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                    }

                    if (ModelState.IsValid)
                    {
                        _HRPermission p = context._HRPermissions.Where(x => x.Id == model.Id).SingleOrDefault();

                        if (Permissions == null)
                        {
                            if (p != null)
                            {
                                context._HRPermissions.DeleteOnSubmit(p);
                            }
                        }
                        else
                        {                           
                            if (p == null)
                            {
                                p = new _HRPermission
                                {
                                    EmployeeId = employee.EmployeeId
                                };
                                context._HRPermissions.InsertOnSubmit(p);
                            }

                            p.Controllers = string.Join(",", Permissions);
                        }
                        
                        context.SubmitChanges();

                        res.Remarks = "Permissions were successfully updated";

                        Cache.Update();
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