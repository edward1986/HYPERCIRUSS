using System.Web.Mvc;
using coreApp.DAL;
using coreApp.Filters;
using coreApp.Controllers;
using coreApp.Interfaces;
using Module.DB;

namespace coreApp.Areas.EmployeeModule.Controllers
{
    [MyFilter]
    public class MyProfileController : Base_NoCoreEmployeeController, IMyController
    {
        public tblEmployee employee { get; set; }

        public ActionResult Details()
        {

            return View(employee);
        }
        
    }
}
