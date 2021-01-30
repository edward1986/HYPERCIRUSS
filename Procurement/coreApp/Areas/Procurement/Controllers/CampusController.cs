using coreApp.Controllers;
using coreLib.Objects;
using Module.Core;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.Controllers
{
    [UserAccessAuthorize(allowedAccess: "procurement")]
    public class CampusController : BaseAuthorizedController
    {
        public CampusController()
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
            var model = Cache.Get_Tables().Campuses.OrderBy(x => x.Campus).ToList();
            return View(model);
        }

        public ActionResult _Details(int? id)
        {
            ViewBag.Title = "Campus";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;


            tblCampus campus = Cache.Get_Tables().Campuses.Where(x => x.CampusID == id).SingleOrDefault();

            if (campus == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Campus", campus);
            }
        }

        public ActionResult _Create()
        {
            ViewBag.Title = "Campus";
            ViewBag.Subtitle = "New";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            return PartialView("_Campus", new tblCampus());
        }

        public void _CreatePost(FormCollection _model, ref queryResult res)
        {
            tblCampus model = (tblCampus)UpdateModel(new tblCampus().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {
                    tblCampus campus = new tblCampus
                    {
                        Campus = model.Campus,
                        Address = model.Address,
                        CountryID = model.CountryID,
                        PostalCode = model.PostalCode,
                        ContactNumber = model.ContactNumber
                    };

                    context.tblCampus.InsertOnSubmit(campus);
                    context.SubmitChanges();

                    //OfficeSettings.setOfficeSettings(office.OfficeId);

                    res.Remarks = "Campus was successfully created";

                    Cache.Update_Tables(_Campuses: true);
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
        }

        public ActionResult _Edit(int? id)
        {
            ViewBag.Title = "Campus";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;


            tblCampus campus = Cache.Get_Tables().Campuses.Where(x => x.CampusID == id).Single();
            if (campus == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Campus", campus);
            }
        }


        public void _EditPost(FormCollection _model, ref queryResult res)
        {
            tblCampus model = (tblCampus)UpdateModel(new tblCampus().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {

                    tblCampus campus = context.tblCampus.Where(x => x.CampusID == model.CampusID).SingleOrDefault();
                    if (campus == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        campus.Campus = model.Campus;
                        campus.Address = model.Address;
                        campus.CountryID = model.CountryID;
                        campus.PostalCode = model.PostalCode;
                        campus.ContactNumber = model.ContactNumber;

                        context.SubmitChanges();

                        res.Remarks = "Campus was successfully updated";

                        Cache.Update_Tables(_Campuses: true);
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

                    tblCampus campus = context.tblCampus.Where(x => x.CampusID == id).SingleOrDefault();
                    if (campus == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        context.tblCampus.DeleteOnSubmit(campus);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";

                        Cache.Update_Tables(_Campuses: true);
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