using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using coreApp.Controllers;
using coreApp.DAL;
using Microsoft.AspNet.Identity;
using coreLib.Objects;
using coreLib.Extensions;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.User.Controllers
{
    public class EmployeesController : Base_NoCoreHRStaffController
    {


        public ActionResult Index()
        {

            return View();

        }

        public ActionResult ViewList(int? id)
        {
            ViewBag.Campus = Cache.Get().userAccess.campuses;

            using (dalDataContext context = new dalDataContext())
            {
                tblOffice office;
                tblDepartment department = null;

                office = context.tblOffices.SingleOrDefault(x => x.CampusID == id);

                if (office != null)
                {
                    department = context.tblDepartments.SingleOrDefault(x => x.OfficeId == office.OfficeId);

                }

                if (department != null)
                {
                    List<tblEmployee> employee = context.tblEmployees.Where(x => x.DepartmentId == department.DepartmentId).ToList();

                    return View("_List", employee);
                }

                return View("_List");
            }

        }

        public ActionResult Create()
        {
            ViewBag.Title = "Employee";
            ViewBag.Subtitle = "New";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            return PartialView("_Employee", new tblEmployee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection _model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            
            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    if (_model.Get("selectedDepartment") == "-1")
                    {

                        ModelState.AddModelError("Department", "Department field is required");

                    }
                    else
                    {
                        _model.Set("DepartmentId", _model.Get("selectedDepartment").Split(',')[1]);
                        
                    }

                    var model = new tblEmployee();

                    model.LastName = _model["LastName"];
                    model.FirstName = _model["FirstName"];
                    model.MiddleName = _model["MiddleName"];
                    model.NameExt = _model["NameExt"];
                    model.Email = _model["Email"];
                    model.IdNo = _model["IdNo"];
                    model.DepartmentId = Convert.ToInt32(_model["DepartmentId"]);
                    model.WarehouseId = Convert.ToInt32(_model["WarehouseId"]);

                    if (ModelState.IsValid)
                    {
                        if (context.AspNetUsers.Where(x => x.Email == model.Email.ToLower().Trim()).Any() ||
                        context.tblEmployees.Where(x => x.Email == model.Email.ToLower().Trim()).Any())
                        {
                            ModelState.AddModelError("EmailExists", "Email already exists");
                        }
                    }

                    if (ModelState.IsValid)
                    {

                        tblEmployee Employee = new tblEmployee
                        {
                            LastName = model.LastName,
                            FirstName = model.FirstName,
                            MiddleName = model.MiddleName,
                            NameExt = model.NameExt,
                            Email = model.Email.ToLower().Trim(),
                            IdNo = model.IdNo,
                            DepartmentId = model.DepartmentId,
                            WarehouseId = model.WarehouseId
                        };

                        context.tblEmployees.InsertOnSubmit(Employee);
                        context.SubmitChanges();

                        res.Data = Employee.EmployeeId;
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

        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Employee's";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee employee = context.tblEmployees.SingleOrDefault(x => x.EmployeeId == id);

                if (employee == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    ViewBag.Employee = employee;
                    return PartialView("Details", employee);
                }
            }

        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Payroll Summary";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == id).Single();
                if (employee == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {

                    return PartialView("_Employee", employee);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection _model, bool? GeoAuth_DeviceRef_Reset)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            var xmodel = new tblEmployee();

            if (_model.Get("selectedDepartment") == "-1")
            {

                ModelState.AddModelError("Department", "Department field is required");

            }
            else
            {
                _model.Set("DepartmentId", _model.Get("selectedDepartment").Split(',')[1]);

            }

            xmodel.EmployeeId = Convert.ToInt32(_model["EmployeeId"]);
            xmodel.LastName = _model["LastName"];
            xmodel.FirstName = _model["FirstName"];
            xmodel.MiddleName = _model["MiddleName"];
            xmodel.NameExt = _model["NameExt"];
            xmodel.Email = _model["Email"];
            xmodel.IdNo = _model["IdNo"];
            xmodel.DepartmentId = Convert.ToInt32(_model["DepartmentId"]);
            xmodel.WarehouseId = Convert.ToInt32(_model["WarehouseId"]);

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == xmodel.EmployeeId).SingleOrDefault();

                    if (context.AspNetUsers.ToList().Where(x => (string.IsNullOrEmpty(employee.UserId) || x.Id != employee.UserId) && x.Email.ToCleanString() == xmodel.Email.ToCleanString()).Any() ||
                        context.tblEmployees.ToList().Where(x => x.EmployeeId != employee.EmployeeId && x.Email.ToCleanString() == xmodel.Email.ToCleanString()).Any())
                    {
                        ModelState.AddModelError("EmailExists", "Email already exists");
                    }
                                       
                   
                    if (ModelState.IsValid)
                    {

                        if (employee == null)
                        {
                            throw new Exception(ModuleConstants.ID_NOT_FOUND);
                        }
                        else
                        {
                            employee.LastName = xmodel.LastName;
                            employee.FirstName = xmodel.FirstName;
                            employee.MiddleName = xmodel.MiddleName;
                            employee.NameExt = xmodel.NameExt;
                            employee.Email = xmodel.Email.ToLower().Trim();
                            employee.IdNo = xmodel.IdNo;
                            employee.DepartmentId = xmodel.DepartmentId;
                            employee.WarehouseId = xmodel.WarehouseId;

                            context.SubmitChanges();

                            if (xmodel.WarehouseId == null || xmodel.WarehouseId == -1)
                            {
                                tblEmployee_Access access = context.tblEmployee_Accesses.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                                if (access != null)
                                {
                                    access.Remove_SAMS_Access();
                                }
                            }

                            res.Remarks = "Record was successfully updated";

                            Cache.Update();
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
        [EmployeeAuthorize("id")]
        public ActionResult Delete(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    if (ModelState.IsValid)
                    {

                        tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == id).SingleOrDefault();
                        if (employee == null)
                        {
                            throw new Exception(ModuleConstants.ID_NOT_FOUND);
                        }
                        else
                        {

                            bool proceed = true;
                            IdentityResult result = null;

                            if (employee.UserId != null)
                            {
                                result = Account.DeleteUser(UserManager, employee.UserId);
                                proceed = result.Succeeded;
                            }

                            if (proceed)
                            {

                                context.tblEmployees.DeleteOnSubmit(employee);
                                context.SubmitChanges();

                                res.Remarks = "Record was successfully deleted";

                            }
                            else
                            {
                                AddErrors(result);
                            }
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

    }
}
