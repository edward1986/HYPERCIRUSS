using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("admin,finance-staff")]
    public class BaseFinanceStaffController : BaseAuthorizedController
    { }
}
