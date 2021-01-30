using System;
using System.Linq;
using System.Web.Mvc;
using coreApp.Filters;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp.Interfaces;
using Module.DB;
using coreApp.Areas.SAM;
using coreLib.Objects;
using Module.Core;
using System.Collections.Generic;

namespace coreApp.Areas.EmployeeModule.Controllers
{
    [MyFilter]
    public class MyIssuancesController : Base_NoCoreEmployeeController, IMyController
    {
        public tblEmployee employee { get; set; }
        
        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                IssuanceModel model = new IssuanceModel(employee.EmployeeId);

                return View(model);
            }
        }

    }
}
