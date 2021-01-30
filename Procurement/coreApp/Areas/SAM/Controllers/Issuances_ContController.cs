using coreLib.Objects;
using Module.Core;
using Module.DB;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Controllers
{
    [UserAccessAuthorize(allowedAccess: "sam_monitoring")]
    public class Issuances_ContController : SAMBaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            using (samDataContext context = new samDataContext())
            {
                tblContractor contractor = context.tblContractors.Where(x => x.Id == id).SingleOrDefault();
                if (contractor == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    ViewBag.Contractor = contractor;

                    IssuanceModel_Cont model = new IssuanceModel_Cont(contractor.Id);



                    return View(model);
                }
            }
        }
    }
}