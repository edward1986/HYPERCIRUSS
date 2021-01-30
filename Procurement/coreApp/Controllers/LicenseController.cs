using coreApp.Controllers;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Controllers
{
    public class LicenseController : Controller
    {
        public ActionResult Index()
        {
            machine.machine mac = new machine.machine();
            string sn = MvcApplication.ApplicationLicense.all_space_SN(mac.getMotherBoardSerial(), mac.getMachineUniqueID(false));
            ViewBag.SN = sn;

            return View();
        }

        [HttpPost]
        public ActionResult UploadLicenseFile(HttpPostedFileBase LicenseFile)
        {
            if (LicenseFile == null)
            {
                TempData["GlobalError"] = "No file to upload";
            }
            else
            {
                string path = Server.MapPath("~/application.lic");

                LicenseFile.SaveAs(path);
                MvcApplication.ApplicationLicense.Validate();
                

                TempData["GlobalMessage"] = "File was successfully uploaded";
            }            

            return RedirectToAction("Index");
        }
    }
}