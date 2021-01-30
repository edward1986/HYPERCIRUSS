using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Models;
using Module.DB;

namespace coreApp.Areas.Procurement.Controllers
{
    [UserAccessAuthorize(allowedAccess: "procurement")]
    public class MainController : ProcurementBaseController
    {
        // GET: Procurement/Procurement
        public ActionResult Index()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                ViewBag.allocations = context.vTotalAllocations.Where(x => x.YearReceived == DateTime.Now.Year).OrderByDescending(x => x.Allocated).Take(5).ToList();

                ViewBag.Year = DateTime.Now.Year;
            }

            tblEmployee employee = Cache.Get().userAccess.employee;

            ViewBag.OfficeModel = new OfficePPMPModel(DateTime.Now.Year, employee);

            Session["area"] = "Procurement";

            return View();
        }
    }
}