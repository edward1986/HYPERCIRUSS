using System.Web.Mvc;

namespace coreApp.Controllers
{
    [Authorize]
    [AccountAuthorize]
    public class BaseAuthorizedController : CoreController
    { }
}
