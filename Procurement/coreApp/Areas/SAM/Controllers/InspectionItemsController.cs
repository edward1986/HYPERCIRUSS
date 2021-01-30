using System.Linq;
using System.Web.Mvc;
using System;
using coreApp.Areas.SAM.Interfaces;
using coreLib.Objects;
using Module.Core;
using coreApp.Areas.SAM.Filters;
using System.Collections.Generic;
using coreApp.Areas.Procurement.DAL;

namespace coreApp.Areas.SAM.Controllers
{
    [ReceiptInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_inspection")]
    public class InspectionItemsController : SAMBaseController, IReceiptController
    {
        public tblReceipt Receipt { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                List<tblReceiptItem> model = context.tblReceiptItems.Where(x => x.ReceiptId == Receipt.Id).ToList();

                ViewBag.Inspections = context.tblReceipts.Where(x => x.ReceivedDate.Year == Receipt.ReceivedDate.Year && x.WarehouseId == warehouse.Id)
                                            .ToList()
                                            .Where(x => x.HasBeenSubmitted)
                                            .ToList();

                return View(model);
            }
        }
    }
}