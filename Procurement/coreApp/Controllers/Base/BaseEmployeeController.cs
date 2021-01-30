using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("employee")]
    public class BaseEmployeeController : BaseAuthorizedController
    { }
}
