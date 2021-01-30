using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.Controllers
{
    public class ContractController : Controller
    {
        // GET: Procurement/Contract
        public ActionResult Index()
        {
            return View();
        }
    }
}