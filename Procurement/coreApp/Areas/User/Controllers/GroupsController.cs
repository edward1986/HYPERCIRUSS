using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using Module.DB;
using Module.Core;

namespace coreApp.Areas.User.Controllers
{
    [UserAccessAuthorize(allowedAccess: "hr_config")]
    public class GroupsController : BaseAuthorizedController
    {
        public GroupsController()
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
            var model = Cache.Get_Tables().Groups.OrderBy(x => x.GroupName).ToList();
            return View(model);
        }

        public ActionResult _Details(int? id)
        {
            ViewBag.Title = "Group";
            ViewBag.Subtitle = "Details";

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;


            tblGroup group = Cache.Get_Tables().Groups.Where(x => x.Id == id).SingleOrDefault();
            if (group == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Group", group);
            }
        }

        public ActionResult _Create()
        {
            ViewBag.Title = "Group";
            ViewBag.Subtitle = "New";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            return PartialView("_Group", new tblGroup());
        }

        public void _CreatePost(FormCollection _model, ref queryResult res)
        {
            tblGroup model = (tblGroup)UpdateModel(new tblGroup().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {
                    tblGroup group = new tblGroup
                    {
                        GroupName = model.GroupName
                    };

                    context.tblGroups.InsertOnSubmit(group);
                    context.SubmitChanges();

                    //GroupSettings.setGroupSettings(group.Id);

                    res.Remarks = "Record was successfully created";

                    Cache.Update_Tables(_groups: true);
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
        }

        public ActionResult _Edit(int? id)
        {
            ViewBag.Title = "Group";
            ViewBag.Subtitle = "Edit";

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;


            tblGroup group = Cache.Get_Tables().Groups.Where(x => x.Id == id).Single();
            if (group == null)
            {
                return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
            }
            else
            {
                return PartialView("_Group", group);
            }
        }


        public void _EditPost(FormCollection _model, ref queryResult res)
        {
            tblGroup model = (tblGroup)UpdateModel(new tblGroup().GetType(), _model);

            using (dalDataContext context = new dalDataContext())
            {
                if (ModelState.IsValid)
                {

                    tblGroup group = context.tblGroups.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (group == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        group.GroupName = model.GroupName;

                        context.SubmitChanges();

                        //GroupSettings.setGroupSettings(group.Id);

                        res.Remarks = "Record was successfully updated";
                        
                        Cache.Update_Tables(_groups: true);

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

                    tblGroup group = context.tblGroups.Where(x => x.Id == id).SingleOrDefault();
                    if (group == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        context.tblGroups.DeleteOnSubmit(group);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";

                        Cache.Update_Tables(_groups: true);

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
