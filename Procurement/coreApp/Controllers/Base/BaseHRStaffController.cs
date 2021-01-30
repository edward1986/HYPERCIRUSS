using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("admin,hr-staff")]
    public class BaseHRStaffController : BaseAuthorizedController
    { }
}
