using System.Collections;

namespace Module.Core
{
    public static class ModuleConstants
    {
        public const int PHILIPPINES_COUNTRY_ID = 446;
        public const string BAD_REQUEST = "Bad request";
        public const string ID_NOT_FOUND = "Id not found";
        public const string EMPLOYEE_ID_NOT_FOUND = "Employee Id not found";
        public const string OFFICE_ID_NOT_FOUND = "Office Id not found";
        public const string DEPARTMENT_ID_NOT_FOUND = "Department Id not found";
        public const string EMPLOYEE_HAS_NO_CAREER_DEFINED = "Employee has no career defined";
        public const string DEFAULT_EFFECTIVITY = "DEFAULT";
        public const string PRINT_OPTIONS = "Select document format";

        public static Hashtable HRPermissionModules = new Hashtable
        {
            { "EmployeeInfo", "Personal Information" },
            { "EmployeeEduc", "Education" },
            { "EmployeeTrainings", "Trainings" },
            { "EmployeeWorkExps", "Work Experiences" },
            { "EmployeeChildren", "Children" },
            { "EmployeeCivilServices", "Civil Services" },
            { "EmployeePhotos_PDS", "Photos" },
            { "EmployeeReferences", "References" },
            { "EmployeeVoluntary", "Voluntary Works" },
            { "EmployeeOtherInfo", "Other Information" },
        };
    }

    public static class ModuleConstants_Authorization
    {
        public const string MODULEACCESS_MODULE_IS_NOT_ENABLED = "Module Access: This module is not enabled";

        public const string USERACCESS_UNAUTHORIZED_ACCESS_TO_RESOURCE = "User Access: You are not authorized to access the resource";
        public const string USERACCESS_UNAUTHORIZED_ACTION = "User Access: You are not authorized to perform this action";

        public const string EMPLOYEEACCESS_UNAUTHORIZED_ACCESS_TO_RESOURCE = "Employee Access: You are not authorized to access the employee record";
    }
}
