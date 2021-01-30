using coreApp.Controllers;
using coreApp.DAL;
using coreApp.Filters;
using coreApp.Interfaces;
using coreApp.Models;
using Microsoft.AspNet.Identity;
using Module.Core;
using Module.DB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.User.Controllers
{
    [EmployeeInfoFilter]
    [EmployeeAuthorize]
    public class EmployeeAccountController : Base_NoCoreHRStaffController, IEmployeeController
    {
        public tblEmployee employee { get; set; }

        public ActionResult CreateAccount()
        {
            ViewBag.Title = "Employee Account";
            ViewBag.Subtitle = "Create";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            return PartialView("_Account", new EmployeeAccountModel
            {
                EmployeeId = employee.EmployeeId,
                Email = employee.Email
            });
        }

        [HttpPost]
        public ActionResult CreateAccount(EmployeeAccountModel model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            try
            {
                using (dalDataContext context = new dalDataContext())
                {

                    ApplicationUser u = UserManager.FindByEmail(employee.Email);
                    if (u != null)
                    {
                        AddError("Employee login account already exists");
                    }

                    if (ModelState.IsValid)
                    {
                        var result = Account.CreateUser(UserManager, new NewUserModel
                        {
                            UserName = model.Username,
                            Email = employee.Email,
                            Password = FixedSettings.DefaultPassword,
                            Roles = new string[] { "employee" }
                        });

                        if (result.Succeeded)
                        {
                            tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == model.EmployeeId).Single();
                            employee.UserId = UserManager.FindByEmail(model.Email).Id;

                            context.SubmitChanges();

                            res.Remarks = "Employee login account was successfully created";
                        }
                        else
                        {
                            AddErrors(result);
                            throw new Exception(coreProcs.ShowErrors(ModelState));
                        }
                        
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
        public ActionResult EnableAccount(string userId)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            try
            {
                if (!Cache.Get().userAccess.HasAccess("hr_emp_login_account"))
                {
                    AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                }

                ApplicationUser u = UserManager.FindById(userId);
                if (u == null)
                {
                    AddError("Employee login account does not exists");
                }
                else if (!Module.DB.Procs.Account.IsAccountDisabled(u.Id))
                {
                    AddError("Employee login account is already enabled");
                }

                if (ModelState.IsValid)
                {

                    Module.DB.Procs.Account.DisableAccount(userId, false);

                    res.Remarks = "Employee login account was successfully enabled";
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }

            }
            catch (Exception e)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(e);
            }

            return Json(res);
            
        }

        [HttpPost]
        public ActionResult DisableAccount(string userId)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            try
            {
                if (!Cache.Get().userAccess.HasAccess("hr_emp_login_account"))
                {
                    AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                }

                ApplicationUser u = UserManager.FindById(userId);
                if (u == null)
                {
                    AddError("Employee login account does not exists");
                }
                else if (Module.DB.Procs.Account.IsAccountDisabled(u.Id))
                {
                    AddError("Employee login account is already disabled");
                }

                if (ModelState.IsValid)
                {

                    Module.DB.Procs.Account.DisableAccount(userId, true);

                    res.Remarks = "Employee login account was successfully disabled";
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }

            }
            catch (Exception e)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(e);
            }

            return Json(res);

        }

        [HttpPost]
        public ActionResult ResetPassword(string userId)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            try
            {
                if (!Cache.Get().userAccess.HasAccess("hr_emp_login_account"))
                {
                    AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                }

                ApplicationUser u = UserManager.FindById(userId);
                if (u == null)
                {
                    AddError("Employee login account does not exists");
                }
                else if (Module.DB.Procs.Account.IsAccountDisabled(u.Id))
                {
                    AddError("Employee login account is disabled");
                }

                if (ModelState.IsValid)
                {
                    var code = UserManager.GeneratePasswordResetToken(userId);
                    var result = UserManager.ResetPassword(userId, code, FixedSettings.DefaultPassword);

                    if (result.Succeeded)
                    {
                        res.Remarks = "Employee login password was successfully reset";
                    }
                    else
                    {
                        AddErrors(result);
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }                    
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }

            }
            catch (Exception e)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(e);
            }

            return Json(res);

        }

        [HttpPost]
        public ActionResult DeleteAccount(string userId)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Remarks = "", Err = "" };

            try
            {
                if (!Cache.Get().userAccess.HasAccess("hr_emp_login_account"))
                {
                    AddError(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
                }

                ApplicationUser u = UserManager.FindById(userId);
                if (u == null)
                {
                    AddError("Employee login account does not exists");
                }

                if (ModelState.IsValid)
                {
                    var result = UserManager.Delete(u);

                    if (result.Succeeded)
                    {
                        res.Remarks = "Employee login account was successfully deleted";
                    }
                    else
                    {
                        AddErrors(result);
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }

            }
            catch (Exception e)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(e);
            }

            return Json(res);

        }
    }
}
