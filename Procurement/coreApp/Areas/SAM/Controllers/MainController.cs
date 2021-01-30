using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;

namespace coreApp.Areas.SAM.Controllers
{
    public class MainController : SAMBaseController
    {
        public ActionResult Index()
        {
            Session["area"] = "SAM";

            return View();
        }
    }
}