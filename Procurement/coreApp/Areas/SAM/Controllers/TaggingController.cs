using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;

namespace coreApp.Areas.SAM.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_tagging")]
    public class TaggingController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index(bool showCompleted = false)
        {
            ViewBag.ShowCompleted = showCompleted;
            List<TagItemModel> model = Common.GetTaggingModels(Year).Where(x => x.Receipt.WarehouseId == warehouse.Id && x.ReceiptItem.Qty > 0)
                .Where(x => (showCompleted && x.IsComplete) || (!showCompleted && !x.IsComplete))
                .ToList();

            return View(model);
        }
    }
}