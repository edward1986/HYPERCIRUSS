using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.Models;
using coreApp.Controllers;
using coreApp.DAL;
using Module.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Controllers
{
    public class HomeController : Base_NoCoreAuthorizedController
    {

        public ActionResult Index()
        {

            UserAccess access = Cache.Get().userAccess;


            if (access.IsAdmin || (access.HasAccess("procurement") && access.HasAccess("sam")))
            {
                return View();
            }
            else if (access.HasAccess("procurement")) {


                return RedirectToAction("Index", "Main", new { area = "Procurement" });

            }
            else if (access.HasAccess("sam"))
            {
                return RedirectToAction("Index", "Main", new { area = "SAM" });
            }

            return View("Error/index");
        }

        public ActionResult GetEmployeePhoto(string type, int employeeId)
        {
            return GetPhoto(type, employeeId);
        }

    }
}