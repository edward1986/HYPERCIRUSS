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

namespace coreApp.Areas.User.Controllers
{
    [UserAccessAuthorize(allowedAccess: "hr_config")]
    [OfficeInfoFilter]
    public class DepartmentsController : BaseAuthorizedController, IOfficeController
    {
        public tblOffice office { get; set; }

        public DepartmentsController()
        {
            IndexProc = new IndexDelegate(_Index);
            DetailsProc = new DetailsDelegate(_Details);
            CreateProc = new CreateDelegate(_Create);
            CreatePostProc = new CreatePostDelegate(_CreatePost);
            EditProc = new EditDelegate(_Edit);
            EditPostProc = new EditPostDelegate(_EditPost);
            DeletePostProc = new DeletePostDelegate(_Delete);
        }

        public ActionResult _Index()
        {

            using (dalDataContext context = new dalDataContext())
            {
                
                ViewBag.Office = context.tblOffices.Where(x => x.OfficeId == office.OfficeId).SingleOrDefault();

                ViewBag.Offices = context.tblOffices.Where(x => x.CampusID == office.CampusID).OrderBy(x => x.Office).ToList();
                               
            }
                      
            var model = Cache.Get_Tables().Departments.Where(x => x.OfficeId == office.OfficeId).OrderBy(x => x.Department).ToList();
            return View(model);
        }

        public ActionResult _Details(int? id)
        {
            ViewBag.Title = "Department";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            tblDepartment Department = Cache.Get_Tables().Departments.Where(x => x.DepartmentId == id).SingleOrDefault();
            if (Department == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Department", Department);
            }
        }

        public ActionResult _Create()
        {
            ViewBag.Title = "Department";
            ViewBag.Subtitle = "New";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            return PartialView("_Department", new tblDepartment { OfficeId = office.OfficeId });
        }

        public void _CreatePost(FormCollection _model, ref queryResult res)
        {
            tblDepartment model = (tblDepartment)UpdateModel(new tblDepartment().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {

                    tblDepartment Department = new tblDepartment
                    {
                        OfficeId = model.OfficeId,
                        Department = model.Department
                    };

                    context.tblDepartments.InsertOnSubmit(Department);
                    context.SubmitChanges();

                    res.Remarks = "Record was successfully created";

                    Cache.Update_Tables(_departments: true);

                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
        }

        public ActionResult _Edit(int? id)
        {
            ViewBag.Title = "Department";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblDepartment Department = Cache.Get_Tables().Departments.Where(x => x.DepartmentId == id).Single();
            if (Department == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Department", Department);
            }
        }


        public void _EditPost(FormCollection _model, ref queryResult res)
        {
            tblDepartment model = (tblDepartment)UpdateModel(new tblDepartment().GetType(), _model);

            model.OfficeId = office.OfficeId;

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {

                    tblDepartment Department = context.tblDepartments.Where(x => x.DepartmentId == model.DepartmentId).SingleOrDefault();
                    if (Department == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        Department.OfficeId = model.OfficeId;
                        Department.Department = model.Department;
                        
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully updated";

                        Cache.Update_Tables(_departments: true);
                    }

                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
        }


        public void _Delete(int id, ref queryResult res)
        {
            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {

                    tblDepartment Department = context.tblDepartments.Where(x => x.DepartmentId == id).SingleOrDefault();
                    if (Department == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        context.tblDepartments.DeleteOnSubmit(Department);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";

                        Cache.Update_Tables(_departments: true);
                    }

                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
        }
    }
}
