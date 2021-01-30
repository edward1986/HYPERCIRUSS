using System.Web.Mvc;

namespace coreApp.Areas.EmployeeModule
{
    public class EmployeeModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EmployeeModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              "MyHolidays",
              "EmployeeModule/Holidays/{action}/{employeeId}/{id}/{startDate}/{endDate}",
              new { controller = "Holidays", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
           );

            context.MapRoute(
               "MyPayslips",
               "EmployeeModule/MyPayslips/{action}/{employeeId}/{id}",
               new { controller = "MyPayslips", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "MyTimeLogs",
               "EmployeeModule/MyTimeLogs/{action}/{employeeId}/{id}/{startDate}/{endDate}",
               new { controller = "MyTimeLogs", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
            );

            context.MapRoute(
               "MyLeaveApplications",
               "EmployeeModule/MyLeaveApplications/{action}/{employeeId}/{id}",
               new { controller = "MyLeaveApplications", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "MyTravelApplications",
               "EmployeeModule/MyTravelApplications/{action}/{employeeId}/{id}",
               new { controller = "MyTravelApplications", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
              "MyOTApplications",
              "EmployeeModule/MyOTApplications/{action}/{employeeId}/{id}",
              new { controller = "MyOTApplications", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
               "MyLeaveCredits",
               "EmployeeModule/MyLeaveCredits/{action}/{employeeId}/{leaveTypeId}",
               new { controller = "MyLeaveCredits", action = "Index", employeeId = UrlParameter.Optional, leaveTypeId = UrlParameter.Optional }
           );

            context.MapRoute(
                "MyAttendance",
                "EmployeeModule/MyAttendance/{employeeId}/{startDate}/{endDate}",
                new { controller = "MyAttendance", action = "Index", employeeId = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
            );

            context.MapRoute(
                "MySchedules",
                "EmployeeModule/MySchedules/{employeeId}/{startDate}/{endDate}",
                new { controller = "MySchedules", action = "Index", employeeId = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
            );

            context.MapRoute(
               "MyReferences",
               "EmployeeModule/MyReferences/{employeeId}/{startDate}/{endDate}",
               new { controller = "MyReferences", action = "Index", employeeId = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
           );

            context.MapRoute(
                "EmployeeModule_default",
                "EmployeeModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}