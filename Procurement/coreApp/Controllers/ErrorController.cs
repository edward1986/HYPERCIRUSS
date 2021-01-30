using coreApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Controllers
{
    public class ErrorController : Base
    {        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountError()
        {
            return View();
        }
    }
}