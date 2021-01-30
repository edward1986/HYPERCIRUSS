using System.Web.Mvc;

namespace coreApp.Areas.Procurement
{
    public class ProcurementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Procurement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                  "ProcurementSettings",
                  "Procurement/Settings/{action}/{dt}/{id}",
                  new { action = "Index", controller = "ProcurementSettings", dt = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                 "Items",
                 "Procurement/Items/{action}/{year}/{id}",
                 new { action = "Index", controller = "Items", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                  "ManagePR",
                  "Procurement/ManagePR/{action}/{year}/{id}",
                  new { action = "Index", controller = "ManagePR", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                 "ManagePPMP",
                 "Procurement/ManagePPMP/{action}/{year}/{id}",
                 new { action = "Index", controller = "ManagePPMP", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "NOAItems",
                 "Procurement/NOA/Items/{action}/{noaId}/{id}",
                 new { action = "Index", controller = "NOAItems", noaId = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "NOA",
                 "Procurement/NOA/{action}/{year}/{id}",
                 new { action = "Index", controller = "NOA", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "POItems",
                 "Procurement/PO/Items/{action}/{poId}/{id}",
                 new { action = "Index", controller = "POItems", poId = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                  "PO",
                  "Procurement/PO/{action}/{year}/{id}",
                  new { action = "Index", controller = "PO", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                "AOBItems",
                "Procurement/AOB/Items/{action}/{aobId}/{id}",
                new { action = "Index", controller = "AOBItems", aobId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                  "AOB",
                  "Procurement/AOB/{action}/{year}/{id}",
                  new { action = "Index", controller = "AOB", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                 "RFQItems",
                 "Procurement/RFQ/Items/{action}/{rfqId}/{id}",
                 new { action = "Index", controller = "RFQItems", rfqId = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                  "RFQ",
                  "Procurement/RFQ/{action}/{year}/{id}",
                  new { action = "Index", controller = "RFQ", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                  "ConsolidatedPRItems",
                  "Procurement/ConsolidatedPR/Items/{action}/{cprId}/{id}",
                  new { action = "Index", controller = "ConsolidatedPRItems", cprId = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                  "ConsolidatedPR",
                  "Procurement/ConsolidatedPR/{action}/{year}/{id}",
                  new { action = "Index", controller = "ConsolidatedPR", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                  "CompanyAPRItems",
                  "Procurement/CompanyAPR/Items/{action}/{aprId}/{id}",
                  new { action = "Index", controller = "CompanyAPRItems", aprId = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                  "CompanyAPR",
                  "Procurement/CompanyAPR/{action}/{year}/{id}",
                  new { action = "Index", controller = "CompanyAPR", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                 "CompanyAPPItems",
                 "Procurement/CompanyAPP/Items/{action}/{appId}/{id}",
                 new { action = "Index", controller = "CompanyAPPItems", appId = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                "CompanyAPP",
                "Procurement/CompanyAPP/{action}/{year}/{id}",
                new { action = "Index", controller = "CompanyAPP", year = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                 "DeleteOfficePRItems",
                 "Procurement/OfficePR/DeleteItems/{prId}",
                 new { action = "DeleteItems", controller = "OfficePRItems", prId = UrlParameter.Optional }
             );

            context.MapRoute(
                 "ApplyMQ",
                 "Procurement/OfficePR/ApplyMQ/{prId}",
                 new { action = "ApplyMQ", controller = "OfficePRItems", prId = UrlParameter.Optional }
             );

            context.MapRoute(
                 "OfficePRItems",
                 "Procurement/OfficePR/Items/{action}/{prId}/{id}",
                 new { action = "Index", controller = "OfficePRItems", prId = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                  "CompanyPRItems",
                  "Procurement/CompanyPR/Items/{action}/{prId}/{id}",
                  new { action = "Index", controller = "CompanyPRItems", prId = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                "CompanyPR",
                "Procurement/CompanyPR/{action}/{year}/{id}",
                new { action = "Index", controller = "CompanyPR", year = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                "OfficePR",
                "Procurement/OfficePR/{action}/{year}/{id}",
                new { action = "Index", controller = "OfficePR", year = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                  "OfficePPMPItems",
                  "Procurement/OfficePPMP/Items/{action}/{ppmpId}/{id}",
                  new { action = "Index", controller = "OfficePPMPItems", ppmpId = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                  "OfficePPMP",
                  "Procurement/OfficePPMP/{action}/{year}/{id}",
                  new { action = "Index", controller = "OfficePPMP", year = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                  "DepartmentPPMPItems",
                  "Procurement/DepartmentPPMP/Items/{action}/{ppmpId}/{id}",
                  new { action = "Index", controller = "DepartmentPPMPItems", ppmpId = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            context.MapRoute(
                   "DepartmentPPMP",
                   "Procurement/DepartmentPPMP/{action}/{year}/{id}",
                   new { action = "Index", controller = "DepartmentPPMP", year = UrlParameter.Optional, id = UrlParameter.Optional }
               );

            context.MapRoute(
                   "OfficeAllocations",
                   "Procurement/OfficeAllocations/{action}/{year}/{officeId}/{fundId}",
                   new { action = "Index", controller = "OfficeAllocations", year = UrlParameter.Optional, officeId = UrlParameter.Optional, fundId = UrlParameter.Optional }
               );

            context.MapRoute(
                  "DepartmentAllocations",
                  "Procurement/DepartmentAllocations/{action}/{year}/{deptId}",
                  new { action = "Index", controller = "DepartmentAllocations", deptId = UrlParameter.Optional, year = UrlParameter.Optional }
              );

            context.MapRoute(
                   "SOF",
                   "Procurement/SOF/{action}/{year}/{fundId}",
                   new { action = "Index", controller = "SOF", year = UrlParameter.Optional, fundId = UrlParameter.Optional }
               );

            context.MapRoute(
             "ProcurementOffices",
             "Procurement/Offices/{action}/{campusID}/{id}",
             new { controller = "Offices", action = "Index", campusID = UrlParameter.Optional, id = UrlParameter.Optional }
         );

            context.MapRoute(
              "ProcurementDepartments",
              "Procurement/Departments/{action}/{officeId}/{id}",
              new { controller = "Departments", action = "Index", officeId = UrlParameter.Optional, id = UrlParameter.Optional }
          );


            context.MapRoute(
                "Procurement_default",
                "Procurement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}