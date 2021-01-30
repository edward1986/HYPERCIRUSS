using coreApp.Areas.SAM.Filters;
using coreApp.Areas.SAM.Interfaces;
using coreApp.Controllers;
using System.Web.Mvc;

namespace coreApp.Areas.SAM.Controllers
{
    //[LicenseAuthorize("SAM")]
    [UserAccessAuthorize(allowedAccess: "sam")]
    [WarehouseFilter]
    public class SAMBaseController : Base_NoCoreAuthorizedController, ISAMController
    {
        public tblWarehouse warehouse { get; set; }
    }
}