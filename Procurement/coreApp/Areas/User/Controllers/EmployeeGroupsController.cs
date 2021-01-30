using System;
using System.Linq;
using System.Web.Mvc;
using coreApp.Filters;
using coreApp.Controllers;
using coreApp.DAL;
using coreApp.Interfaces;
using coreLib.Objects;
using Module.DB;
using Module.Core;
using System.Collections.Generic;

namespace coreApp.Areas.User.Controllers
{
    [EmployeeInfoFilter]
    [EmployeeAuthorize]
    public class EmployeeGroupsController : Base_NoCoreHRStaffController, IEmployeeController
    {
        public tblEmployee employee { get; set; }

        public ActionResult Index()
        {
            using (dalDataContext context = new dalDataContext())
            {

                var model = context.tblEmployee_Groups.Where(x => x.EmployeeId == employee.EmployeeId)
                    .Join(context.tblGroups, a => a.GroupId, b => b.Id, (a, b) => b)
                    .OrderBy(x => x.GroupName)
                    .ToList();

                return View(model);
            }
        }

        public ActionResult Edit()
        {
            using (dalDataContext context = new dalDataContext())
            {
                var model = context.tblEmployee_Groups.Where(x => x.EmployeeId == employee.EmployeeId)
                    .Join(context.tblGroups, a => a.GroupId, b => b.Id, (a, b) => b)
                    .OrderBy(x => x.GroupName)
                    .ToList();

                return PartialView(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string[] Groups)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (Groups == null)
            {
                Groups = new string[] { };
            }

            try
            {
                using (dalDataContext context = new dalDataContext())
                {

                    if (ModelState.IsValid)
                    {
                        List<tblEmployee_Group> existing = context.tblEmployee_Groups.Where(x => x.EmployeeId == employee.EmployeeId).ToList();

                        if (existing.Any())
                        {
                            context.tblEmployee_Groups.DeleteAllOnSubmit(existing);
                        }

                        List<tblEmployee_Group> current = Groups.Select(x => new tblEmployee_Group
                        {
                            EmployeeId = employee.EmployeeId,
                            GroupId = int.Parse(x)
                        })
                        .ToList();

                        context.tblEmployee_Groups.InsertAllOnSubmit(current);
                        context.SubmitChanges();

                        res.Remarks = "Group list was successfully updated";
                        
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
