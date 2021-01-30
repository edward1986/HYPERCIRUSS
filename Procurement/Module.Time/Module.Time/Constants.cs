using System;

namespace Module.Time
{
    public static class Constants
    {
        public const string SETTING_ID_APPLY_GSIS = "ApplyGSIS";
        public const string SETTING_ID_APPLY_SSS = "ApplySSS";
        public const string SETTING_ID_APPLY_PH = "ApplyPH";
        public const string SETTING_ID_APPLY_PAGIBIG = "ApplyPAGIBIG";
        public const string SETTING_ID_APPLY_WTAX = "ApplyWTAX";
        public const string SETTING_ID_WORKHOURSPERDAY = "WorkHoursPerDay";
        public const string SETTING_ID_WORKINGDAYSPERYEAR = "WorkingDaysPerYear";
        public const string SETTING_ID_WORKINGDAYSPERMONTH = "WorkingDaysPerMonth";
        public const string SETTING_ID_USEWORKINGDAYSPERMONTH = "UseWorkingDaysPerMonth";
        public const string SETTING_ID_FIXEDPHAMOUNTFORJO = "FixedPHAmountForJO";
        public const string SETTING_ID_USEFIXEDPHAMOUNTFORJO = "UseFixedPHAmountForJO";
        public const string SETTING_ID_LATEUTROUNDOFF = "LateUTRoundOff";
        public const string SETTING_ID_NIGHTPREMIUM = "NightPremium";
        public const string SETTING_ID_NIGHTPREMIUM_START = "NightPremium_Start";
        public const string SETTING_ID_NIGHTPREMIUM_DURATION = "NightPremium_Duration";
        public const string SETTING_ID_RESTDAY = "RestDay";
        public const string SETTING_ID_SPECIALHOLIDAY = "SpecialHoliday";
        public const string SETTING_ID_SPECIALHOLIDAY_RESTDAY = "SpecialHoliday_RestDay";
        public const string SETTING_ID_REGULARHOLIDAY= "RegularHoliday";
        public const string SETTING_ID_REGULARHOLIDAY_RESTDAY = "RegularHoliday_RestDay";
        public const string SETTING_ID_SUNDAY = "Sunday";
        public const string SETTING_ID_SUNDAY_7D = "SundayRequires7DaysWork";
        public const string SETTING_ID_FLEXITIME= "FlexiTime";
        public const string SETTING_ID_LATETOLERANCE = "LateTolerance";
        public const string SETTING_ID_AUTOCHARGE_ABSLATEUT_TO_VL = "AutoChargeAbsLateUTToVL";
        public const string SETTING_ID_MINNETNOTIFICATION = "MinNetNotification";
        public const string SETTING_ID_ACAPERA = "ACAPERA";
        public const string SETTING_ID_AGENCYSHARE_WTAX_RATE = "AgencyShareWTaxRate";
        public const string SETTING_ID_FACULTY40_MINHOURS_PER_DAY = "Faculty40_MinHoursPerDay";
        public const string SETTING_ID_FACULTY_MAX_HOURS_PER_DAY = "FacultyMaxHoursPerDay";
        public const string SETTING_ID_ontractual_MAX_HOURS_PER_WEEK = "ContractualMaxHoursPerWeek";
        public const string SETTING_ID_FACULTY40_ENABLE = "Faculty40_Enable";
        public const string SETTING_ID_OT = "OT";
        public const string SETTING_ID_FIXEDLATEPENALTY = "FixedLatePenalty";
        public const string SETTING_ID_USEFIXEDLATEPENALTY = "UseFixedLatePenalty";
        public const string SETTING_ID_COMMISSION_IS_TAXABLE = "CommissionIsTaxable";
        public const string SETTING_ID_USE_PAGIBIG_PERCENTAGE = "UsePAGIBIG_Percentage";
        public const string SETTING_ID_NOWORKSHPERM = "NoWorkRequiredOnSpecialHolidayPerm";
        public const string SETTING_ID_USE_NIGHT_PREMIUM = "UseNightPremium";
        public const string SETTING_ID_USE_REGULAR_HOLIDAY_RATE = "UseRegularHolidayRate";
        public const string SETTING_ID_USE_SPECIAL_HOLIDAY_RATE = "UseSpecialHolidayRate";
        public const string SETTING_ID_USE_REST_DAY_RATE = "UserRestDayRate";
        public const string SETTING_ID_USE_SUNDAY_RATE = "UseSundayRate";
        
        public const int COUNTRY_PHILIPPINES = 446;

        public static DateTime VALID_MIN_DATETIME = new DateTime(1900, 1, 1);
        public static DateTime VALID_MAX_DATETIME = new DateTime(2999, 12, 31);
    }
}
