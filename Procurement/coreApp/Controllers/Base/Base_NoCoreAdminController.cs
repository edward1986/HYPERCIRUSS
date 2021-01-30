using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("admin")]
    public class Base_NoCoreAdminController : Base_NoCoreAuthorizedController
    { }
}
