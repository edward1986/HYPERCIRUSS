using System.Web.Mvc;

namespace coreApp.Controllers
{
    [UserAccessAuthorize("employee")]
    public class Base_NoCoreEmployeeController : Base_NoCoreAuthorizedController
    { }
}
