using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("admin,hr-staff")]
    public class Base_NoCoreHRStaffController : Base_NoCoreAuthorizedController
    { }
}
