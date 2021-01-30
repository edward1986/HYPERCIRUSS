using coreApp.Controllers;
using coreApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Controllers
{
    public class DefaultController : Base_NoCoreAuthorizedController
    {
        public ActionResult Index()
        {
            using (hr2017DataContext context = new hr2017DataContext())
            {
                var model = Cache.Get_Tables().Offices.OrderBy(x => x.Office).ToList();
                return View("~/Areas/AdminModule/Views/Offices/Index.cshtml", model);
            }
        }
    }
}