using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using coreApp.Controllers;
using coreApp.DAL;
using Module.DB;
using Module.DB.Procs;

namespace coreApp.Areas.User.Controllers
{
    public class EmployeeSearchController : Base_NoCoreAuthorizedController
    {
        public ActionResult GetList(string lastName = "", string firstName = "", int mfoId = -1, int campusId = -1, int officeId = -1, int departmentId = -1, int positionId = -1, int groupId = -1, int employmentType = -1, bool excludeNoCareer = false, bool excludeNoOffice = false, string altSource = "", string active = "")
        {
            List<tblEmployee> model = GetEmployees(lastName, firstName, mfoId, campusId, officeId, departmentId, positionId, groupId, employmentType, excludeNoCareer, excludeNoOffice, altSource, active);

            ViewBag.AllOffices = officeId == -1;

            return PartialView("List", model);
        }

        [ChildActionOnly]
        public ActionResult Search(coreApp.Models.EmployeeSearchModel model)
        {

            ViewBag.Offices = Cache.Get_Tables().Offices.OrderBy(x => x.Office)
                  .ToList()
                  .Where(x => Cache.Get().userAccess.AllowedCampus(x.Campus.CampusID))
                  .ToList();

            //ViewBag.Departments = Cache.Get_Tables().Departments.OrderBy(x => x.Department)
            //        .ToList()
            //        .Where(x => Cache.Get().userAccess.AllowedOffice(x.Office.OfficeId))
            //        .ToList();

            return PartialView(model);
        }

        public List<tblEmployee> GetEmployees(string lastName = "", string firstName = "", int mfoId = -1, int campusId = -1, int officeId = -1, int departmentId = -1, int positionId = -1, int groupId = -1, int employmentType = -1, bool excludeNoCareer = false, bool excludeNoOffice = false, string altSource = "", string active = "")
        {
            List<tblEmployee> model = new List<tblEmployee>();

            using (dalDataContext context = new dalDataContext())
            {
                List<tblEmployee> filter1 = new List<tblEmployee>();

                if (string.IsNullOrEmpty(altSource))
                {
                    filter1 = context.tblEmployees.ToList();
                }
                else
                {
                    filter1 = context.tblEmployees.Where(x => altSource.Split(',').Contains(x.EmployeeId.ToString())).ToList();
                }

                filter1 = filter1
                    .Where(x => 
                    (string.IsNullOrEmpty(lastName) || x.LastName.ToLower().Contains(lastName.ToLower())) && 
                    (string.IsNullOrEmpty(firstName) || x.FirstName.ToLower().Contains(firstName.ToLower()))
                    )
                    .ToList();

               

                if (campusId != -1)
                {
                    filter1 = filter1.Where(x => new procs_tblEmployee(x).IsInCampus(campusId)).ToList();
                }

                if (officeId != -1)
                {
                    filter1 = filter1.Where(x => new procs_tblEmployee(x).IsInOffice(officeId)).ToList();
                }

                if (departmentId != -1)
                {
                    filter1 = filter1.Where(x => new procs_tblEmployee(x).IsInDepartment(departmentId)).ToList();
                }

               
                if (groupId != -1)
                {
                    filter1 = filter1.Where(x => new procs_tblEmployee(x).IsInGroup(groupId)).ToList();
                }
                               

                UserAccess access = Cache.Get().userAccess;

                filter1 = filter1.Where(x => access.AllowedEmployee(x.EmployeeId)).ToList();
                             

                if (active == "")
                {
                    model = filter1
                    .OrderBy(x => x.FullName)
                    .ToList();
                }
                else
                {
                    model = filter1
                    .Where(x => x.IsActive() == (active == "active"))
                    .OrderBy(x => x.FullName)
                    .ToList();
                }                

                return model;
            }

        }
    }
}
