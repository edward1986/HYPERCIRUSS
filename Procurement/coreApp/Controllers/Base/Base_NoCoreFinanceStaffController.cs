using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("admin,finance-staff")]
    public class Base_NoCoreFinanceStaffController : Base_NoCoreAuthorizedController
    { }
}
