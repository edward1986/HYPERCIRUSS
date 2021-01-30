using System.Web.Mvc;
using System;
using coreLib.Objects;
using Module.Core;

namespace coreApp.Controllers
{

    public class CoreController : Base
    {
        protected delegate ActionResult IndexDelegate();
        protected delegate ActionResult DetailsDelegate(int? id);
        protected delegate ActionResult CreateDelegate();
        protected delegate void CreatePostDelegate(FormCollection model, ref queryResult res);
        protected delegate ActionResult EditDelegate(int? id);
        protected delegate ActionResult ViewDelegate(int? id);
        protected delegate void EditPostDelegate(FormCollection model, ref queryResult res);
        protected delegate void DeletePostDelegate(int id, ref queryResult res);

        protected IndexDelegate IndexProc;
        protected DetailsDelegate DetailsProc;
        protected CreateDelegate CreateProc;
        protected CreatePostDelegate CreatePostProc;
        protected EditDelegate EditProc;
        protected ViewDelegate ViewProc;
        protected EditPostDelegate EditPostProc;
        protected DeletePostDelegate DeletePostProc;
        

        public ActionResult Index()
        {
            return IndexProc();
        }
        

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            return DetailsProc(id);
        }

         public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            return ViewProc(id);
        }

        public ActionResult Create()
        {
            return CreateProc();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };
           
            try
            {
                CreatePostProc(model, ref res);
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            return EditProc(id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                EditPostProc(model, ref res);
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
                DeletePostProc(id, ref res);
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }

        protected object UpdateModel(Type objType, FormCollection form)
        {
            var obj = Activator.CreateInstance(objType);

            var binder = Binders.GetBinder(objType);

            var bindingContext = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => obj, objType),
                ModelState = ModelState,
                ValueProvider = form
            };

            binder.BindModel(ControllerContext, bindingContext);

            return obj;
        }
    }
}
