using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("admin")]
    public class BaseAdminController : BaseAuthorizedController
    { }
}
