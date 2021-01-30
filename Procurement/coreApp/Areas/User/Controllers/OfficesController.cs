using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using Module.DB;
using Module.Core;
using coreApp.Interfaces;
using coreApp.Filters;

namespace coreApp.Areas.User.Controllers
{
    [UserAccessAuthorize(allowedAccess: "hr_config")]
    [CampusInfoFilter]
    public class OfficesController : BaseAuthorizedController, ICampusController
    {
        public tblCampus campus { get; set; }

        public OfficesController()
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

            ViewBag.Campus = campus;

            var model = Cache.Get_Tables().Offices.Where(x => x.CampusID == campus.CampusID).OrderBy(x => x.Office).ToList();
            return View(model);


        }
        public ActionResult _Details(int? id)
        {
            ViewBag.Title = "Office";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;


            tblOffice office = Cache.Get_Tables().Offices.Where(x => x.OfficeId == id).SingleOrDefault();
            if (office == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Office", office);
            }
        }

        public ActionResult _Create()
        {
            ViewBag.Title = "Office";
            ViewBag.Subtitle = "New";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            return PartialView("_Office", new tblOffice { CampusID = campus.CampusID });
        }

        public void _CreatePost(FormCollection _model, ref queryResult res)
        {
            tblOffice model = (tblOffice)UpdateModel(new tblOffice().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {
                    tblOffice office = new tblOffice
                    {
                        Office = model.Office,
                        RCC = model.RCC,
                        TelephoneNo = model.TelephoneNo
                        
                    };

                    context.tblOffices.InsertOnSubmit(office);
                    context.SubmitChanges();

                    //OfficeSettings.setOfficeSettings(office.OfficeId);

                    res.Remarks = "Record was successfully created";

                    Cache.Update_Tables(_offices: true);
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
        }

        public ActionResult _Edit(int? id)
        {
            ViewBag.Title = "Office";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;


            tblOffice office = Cache.Get_Tables().Offices.Where(x => x.OfficeId == id).Single();
            if (office == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Office", office);
            }
        }

        public void _EditPost(FormCollection _model, ref queryResult res)
        {
            tblOffice model = (tblOffice)UpdateModel(new tblOffice().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {

                    tblOffice office = context.tblOffices.Where(x => x.OfficeId == model.OfficeId).SingleOrDefault();
                    if (office == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        office.Office = model.Office;
                        office.TelephoneNo = model.TelephoneNo;
                       office.RCC = model.RCC;
                        office.CampusID = model.CampusID;

                        context.SubmitChanges();

                        //OfficeSettings.setOfficeSettings(office.OfficeId);

                        res.Remarks = "Record was successfully updated";

                        Cache.Update_Tables(_offices: true);
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

                    tblOffice office = context.tblOffices.Where(x => x.OfficeId == id).SingleOrDefault();
                    if (office == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        context.tblOffices.DeleteOnSubmit(office);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";

                        Cache.Update_Tables(_offices: true);
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
