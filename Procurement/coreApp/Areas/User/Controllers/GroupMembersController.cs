using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp.Filters;
using coreApp;
using coreApp.Interfaces;
using coreLib.Objects;
using Module.DB;
using Module.Core;
using System.Collections.Generic;

namespace coreApp.Areas.User.Controllers
{
    [UserAccessAuthorize(allowedAccess: "hr_config")]
    [GroupInfoFilter]
    public class GroupMembersController : Base_NoCoreAuthorizedController, IGroupController
    {
        public tblGroup group { get; set; }
        
        public ActionResult Index()
        {

            ViewBag.Group = group;

            using(dalDataContext context = new dalDataContext())
            {
                var model = context.tblEmployee_Groups.Where(x => x.GroupId == group.Id)
                    .Join(context.tblEmployees, a => a.EmployeeId, b => b.EmployeeId, (a, b) => b)
                    .ToList()
                    .OrderBy(x => x.FullName)
                    .ToList();

                return View(model);
            }
        }


        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Member";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == id).SingleOrDefault();
                if (employee == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_Member", employee);
                }
            }
        }
        
        public ActionResult Add()
        {
            ViewBag.Title = "Members";
            ViewBag.Subtitle = "Add";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;
            
            return PartialView("_Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult Add(string employeeIds)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (dalDataContext context = new dalDataContext())
                {
                    
                    if (string.IsNullOrEmpty(employeeIds))
                    {
                        AddError("No item selected");
                    }

                    if (ModelState.IsValid)
                    {
                        List<tblEmployee_Group> currentlist = context.tblEmployee_Groups.Where(x => x.GroupId == group.Id).ToList();

                        int[] ids = employeeIds.Split(',').Select(x => int.Parse(x)).ToArray();

                        List<tblEmployee_Group> newlist = new List<tblEmployee_Group>();

                        foreach (int id in ids)
                        {
                            if (!currentlist.Any(x => x.EmployeeId == id))
                            {
                                newlist.Add(new tblEmployee_Group
                                {
                                    GroupId = group.Id,
                                    EmployeeId = id
                                });
                            }
                        }

                        context.tblEmployee_Groups.InsertAllOnSubmit(newlist);
                        context.SubmitChanges();


                        res.Remarks = "Items were successfully added";
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
                using (dalDataContext context = new dalDataContext())
                {
                    if (ModelState.IsValid)
                    {

                        tblEmployee_Group member = context.tblEmployee_Groups.Where(x => x.GroupId == group.Id && x.EmployeeId == id).SingleOrDefault();
                        if (member == null)
                        {
                            throw new Exception(ModuleConstants.ID_NOT_FOUND);
                        }
                        else
                        {
                            context.tblEmployee_Groups.DeleteOnSubmit(member);
                            context.SubmitChanges();

                            res.Remarks = "Record was successfully deleted";
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
