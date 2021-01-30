using System.Web.Mvc;

namespace coreApp.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
                "SetAllowancesDeductions",
                "User/SetAllowancesDeductions/{action}",
                new { controller = "SetAllowancesDeductions", action = "Index" }
            );

            context.MapRoute(
                "SalaryGrades",
                "User/SalaryGrades/{action}/{dt}",
                new { controller = "SalaryGrades", action = "Details", dt = UrlParameter.Optional }
            );

            context.MapRoute(
                "EmployeeAttendance",
                "User/Employee/Attendance/{employeeId}/{startDate}/{endDate}",
                new { controller = "EmployeeAttendance", action = "Index", employeeId = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
            );

            context.MapRoute(
                "PayrollSummaryEmployees",
                "User/PayrollSummaryEmployees/{action}/{psId}/{id}",
                new { controller = "PayrollSummary_Employees", action = "Index", psId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                "HRPermissions",
                "User/Employee/Permissions/{action}/{employeeId}",
                new { controller = "HRPermissions", action = "Index", employeeId = UrlParameter.Optional }
            );

            context.MapRoute(
               "EmployeeAccess",
               "User/Employee/Access/{action}/{employeeId}/{id}",
               new { controller = "EmployeeAccess", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                "EmployeeAccount",
                "User/Employee/Account/{action}/{employeeId}/{id}",
                new { controller = "EmployeeAccount", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "EmployeeAllowancesDeductions",
               "User/Employee/AllowancesDeductions/{action}/{employeeId}/{id}",
               new { controller = "EmployeeAllowancesDeductions", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                "EmployeeCivilServices",
                "User/Employee/CivilServices/{action}/{employeeId}/{id}",
                new { controller = "EmployeeCivilServices", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "EmployeeEducation",
               "User/Employee/Educ/{action}/{employeeId}/{id}",
               new { controller = "EmployeeEduc", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
               "EmployeeIssuances",
               "User/Employee/Issuances/{action}/{employeeId}/{id}",
               new { controller = "EmployeeIssuances", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                "EmployeeChildren",
                "User/Employee/Children/{action}/{employeeId}/{id}",
                new { controller = "EmployeeChildren", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                "EmployeeGroups",
                "User/Employee/Groups/{action}/{employeeId}/{id}",
                new { controller = "EmployeeGroups", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "EmployeeReferences",
               "User/Employee/References/{action}/{employeeId}/{id}",
               new { controller = "EmployeeReferences", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                "EmployeeFiles",
                "User/Employee/Files/{action}/{employeeId}/{id}",
                new { controller = "EmployeeFiles", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
              "EmployeeVoluntary",
              "User/Employee/Voluntary/{action}/{employeeId}/{id}",
              new { controller = "EmployeeVoluntary", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
          );

            context.MapRoute(
                "EmployeeWorkExps",
                "User/Employee/WorkExps/{action}/{employeeId}/{id}",
                new { controller = "EmployeeWorkExps", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                "EmployeeTrainings",
                "User/Employee/Trainings/{action}/{employeeId}/{id}",
                new { controller = "EmployeeTrainings", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "EmployeeTravels",
               "User/Employee/Travels/{action}/{employeeId}/{id}",
               new { controller = "EmployeeTravels", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
              "EmployeeOT",
              "User/Employee/OT/{action}/{employeeId}/{id}",
              new { controller = "EmployeeOT", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
          );

            context.MapRoute(
             "EmployeeRD",
             "User/Employee/RD/{action}/{employeeId}/{id}",
             new { controller = "EmployeeRD", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
         );

            context.MapRoute(
                "EmployeePhotos",
                "User/Employee/Photos/{action}/{employeeId}",
                new { controller = "EmployeePhotos", action = "Index", employeeId = UrlParameter.Optional }
            );

            context.MapRoute(
                "EmployeeInfo",
                "User/Employee/Info/{action}/{employeeId}/{id}",
                new { controller = "EmployeeInfo", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
               "EmployeeOtherInfo",
               "User/Employee/OtherInfo/{action}/{employeeId}/{id}",
               new { controller = "EmployeeOtherInfo", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                "EmployeeCareers",
                "User/Employee/Career/{action}/{employeeId}/{id}",
                new { controller = "EmployeeCareers", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
              "Rates",
              "User/Rates/{action}/{positionId}/{id}",
              new { controller = "Rates", action = "Index", positionId = UrlParameter.Optional, id = UrlParameter.Optional }
          );


            context.MapRoute(
               "Departments",
               "User/Departments/{action}/{officeId}/{id}",
               new { controller = "Departments", action = "Index", officeId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

             context.MapRoute(
               "Offices",
               "User/Offices/{action}/{campusID}/{id}",
               new { controller = "Offices", action = "Index", campusID = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
              "GroupMembers",
              "User/Groups/Members/{action}/{groupId}/{id}",
              new { controller = "GroupMembers", action = "Index", groupId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
              "EmployeeOverload",
              "User/Employee/Overload/{action}/{employeeId}/{id}",
              new { controller = "EmployeeOverload", action = "Index", employeeId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}