using System.Linq;
using System.Web.Mvc;
using System;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp.Enums;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.User.Controllers
{
    [UserAccessAuthorize(allowedAccess: "hr_permissions")]
    public class SetHRPermissionsController : Base_NoCoreHRStaffController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectedEmployees(string selection)
        {
            List<tblEmployee> model = new List<tblEmployee>();

            if (!string.IsNullOrEmpty(selection))
            {
                using (dalDataContext context = new dalDataContext())
                {
                    model = context.tblEmployees.Where(x => selection.Split(',').Contains(x.EmployeeId.ToString())).ToList();
                    
                }
            }

            return PartialView(model);
        }

        public ActionResult SelectedPermissions(string selection)
        {
            List<string> model = new List<string>();

            if (!string.IsNullOrEmpty(selection))
            {
                foreach (string s in selection.Split(','))
                {
                    model.Add(ModuleConstants.HRPermissionModules[s].ToString());
                }
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSelection(string employees, string permissions)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                if (string.IsNullOrEmpty(employees))
                {
                    throw new Exception("No employee selected");
                }

                using (dalDataContext context = new dalDataContext())
                {
                    foreach(string s in employees.Split(','))
                    {
                        int id = int.Parse(s);
                        
                        _HRPermission p = context._HRPermissions.Where(x => x.EmployeeId == id).SingleOrDefault();
                        if (p == null)
                        {
                            p = new _HRPermission { EmployeeId = id };
                            context._HRPermissions.InsertOnSubmit(p);
                        }

                        p.Controllers = string.IsNullOrEmpty(permissions) ? "" : permissions;
                                                                        
                        context.SubmitChanges();

                        res.Remarks = "Permissions were successfully set";
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