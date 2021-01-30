using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.Controllers
{
    [NOAInfoFilter]
    [UserAccessAuthorize(allowedAccess: "procurement_access_app")]
    public class NOAItemsController : ProcurementBaseController, INOAController
    {
        public tblNOA NOA { get; set; }

        // GET: Procurement/NOAItems
        public ActionResult Index()
        {

            using (procurementDataContext context = new procurementDataContext())
            {

                List<tblNOAItem> item = context.tblNOAItems.Where(x => x.NOAId == NOA.Id).ToList();

                ViewBag.Year = NOA.Year;

                return View(item);
            }


        }
    }
}