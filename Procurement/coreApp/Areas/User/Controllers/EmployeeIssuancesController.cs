using System;
using System.Linq;
using System.Web.Mvc;
using coreApp.Filters;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp.Interfaces;
using coreLib.Objects;
using Module.DB;
using Module.Core;
using System.Collections.Generic;
using coreApp.Areas.SAM;

namespace coreApp.Areas.User.Controllers
{
    [EmployeeInfoFilter]
    [EmployeeAuthorize]
    public class EmployeeIssuancesController : BaseAuthorizedController, IEmployeeController
    {
        public tblEmployee employee { get; set; }

        public EmployeeIssuancesController()
        {
            IndexProc = new IndexDelegate(_Index);
        }

        public ActionResult _Index()
        {
            IssuanceModel model = new IssuanceModel(employee.EmployeeId);

            return View(model);
        }
    }
}