using coreLib.Objects;
using Module.Core;
using Module.DB;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Controllers
{
    [UserAccessAuthorize(allowedAccess: "sam_monitoring")]
    public class IssuancesController : SAMBaseController
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

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == id).SingleOrDefault();
                if (employee == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    ViewBag.Employee = employee;

                    IssuanceModel model = new IssuanceModel(employee.EmployeeId);



                    return View(model);
                }
            }
        }
    }
}