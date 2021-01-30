using System;
using System.Collections.Generic;
using System.Linq;
using Module.DB;
using Module.DB.Procs;
using Module.DB.Enums;
using Module.Core;
using coreLib.Objects;
using Module.Leave;

namespace Module.Time
{

    public class employeeLeaves
    {
        public List<LeaveChargesModel> Charges { get; set; }

        public employeeLeaves(int employeeId, DateTime startDate, DateTime endDate, etPeriod period = null)
        {
            Charges = new List<LeaveChargesModel>();

            if (period == null)
            {
                period = new etPeriod(employeeId, startDate, endDate);
            }

            foreach (tblLeaveType leaveType in Common.Global_CachedTables.LeaveTypes)
            {
                employeeLeave empLeave = new employeeLeave(employeeId, leaveType.Id, endDate);
                if (!empLeave.hasApplicableCareer) continue;

                double c = period.Days.Where(x => !x.Leave_WithoutPay).Sum(x => x.getLeaveEq(leaveType.Id));

                if (c != 0)
                {
                    Charges.Add(new LeaveChargesModel
                    {
                        leaveType = leaveType,
                        charge = c
                    });
                }

            }
        }

    }

    public class AttendanceModel
    {
        public bool showHeader { get; set; }
        public bool showList { get; set; }
        public bool showTotal { get; set; }
        public etPeriod period { get; set; }
        public PeriodModel periodModel { get; set; }

        public AttendanceModel()
        {
            showHeader = true;
            showList = true;
            showTotal = true;
        }
    }

    public class TimeSettingsModel
    {
        public bool ApplyGSIS { get; set; }
        public bool ApplySSS { get; set; }
        public bool ApplyPH { get; set; }
        public bool ApplyPAGIBIG { get; set; }
        public bool ApplyWTAX { get; set; }
        public double WorkHoursPerDay { get; set; }
        public double WorkingDaysPerYear { get; set; }
        public double WorkingDaysPerMonth { get; set; }
        public bool UseWorkingDaysPerMonth { get; set; }
        public double FixedPHAmountForJO { get; set; }
        public bool UseFixedPHAmountForJO { get; set; }
        public int LateUTRoundOff { get; set; }
        public double NightPremium { get; set; }
        public DateTime NightPremium_Start { get; set; }
        public double NightPremium_Duration { get; set; }
        public double RestDay { get; set; }
        public double SpecialHoliday { get; set; }
        public double SpecialHoliday_RestDay { get; set; }
        public double RegularHoliday { get; set; }
        public double RegularHoliday_RestDay { get; set; }
        public double Sunday { get; set; }
        public bool SundayRequires7DaysWork { get; set; }
        public double FlexiTime { get; set; }
        public double LateTolerance { get; set; }
        public bool AutoChargeAbsLateUTToVL { get; set; }
        public double MinNetNotification { get; set; }
        public double ACAPERA { get; set; }
        public double AgencyShareWTaxRate { get; set; }
        public bool Faculty40_Enable { get; set; }
        public double Faculty40_MinHoursPerDay { get; set; }
        public double FacultyMaxHoursPerDay { get; set; }
        public double ContractualMaxHoursPerWeek { get; set; }
        public double OT { get; set; }
        public double FixedLatePenalty { get; set; }
        public bool UseFixedLatePenalty { get; set; }
        public bool CommissionIsTaxable { get; set; }
        public bool UsePAGIBIG_Percentage { get; set; }
        public bool NoWorkRequiredOnSpecialHolidayPerm { get; set; }

        public bool UseNightPremium { get; set; }
        public bool UseRegularHolidayRate { get; set; }
        public bool UseSpecialHolidayRate { get; set; }
        public bool UserRestDayRate { get; set; }
        public bool UseSundayRate { get; set; }

        public TimeSettingsModel()
        { }

        public TimeSettingsModel(List<tblTMSetting> settings)
        {
            ApplyGSIS = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_APPLY_GSIS).Single().SettingValue);
            ApplySSS = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_APPLY_SSS).Single().SettingValue);
            ApplyPH = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_APPLY_PH).Single().SettingValue);
            ApplyPAGIBIG = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_APPLY_PAGIBIG).Single().SettingValue);
            ApplyWTAX = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_APPLY_WTAX).Single().SettingValue);
            WorkHoursPerDay = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_WORKHOURSPERDAY).Single().SettingValue);
            WorkingDaysPerYear = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_WORKINGDAYSPERYEAR).Single().SettingValue);
            WorkingDaysPerMonth = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_WORKINGDAYSPERMONTH).Single().SettingValue);
            UseWorkingDaysPerMonth = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USEWORKINGDAYSPERMONTH).Single().SettingValue);
            FixedPHAmountForJO = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_FIXEDPHAMOUNTFORJO).Single().SettingValue);
            UseFixedPHAmountForJO = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USEFIXEDPHAMOUNTFORJO).Single().SettingValue);
            LateUTRoundOff = Convert.ToInt16(settings.Where(x => x.Setting == Constants.SETTING_ID_LATEUTROUNDOFF).Single().SettingValue);
            NightPremium = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_NIGHTPREMIUM).Single().SettingValue);
            NightPremium_Start = Convert.ToDateTime(settings.Where(x => x.Setting == Constants.SETTING_ID_NIGHTPREMIUM_START).Single().SettingValue);
            NightPremium_Duration = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_NIGHTPREMIUM_DURATION).Single().SettingValue);
            RestDay = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_RESTDAY).Single().SettingValue);
            SpecialHoliday = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_SPECIALHOLIDAY).Single().SettingValue);
            SpecialHoliday_RestDay = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_SPECIALHOLIDAY_RESTDAY).Single().SettingValue);
            RegularHoliday = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_REGULARHOLIDAY).Single().SettingValue);
            RegularHoliday_RestDay = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_REGULARHOLIDAY_RESTDAY).Single().SettingValue);
            Sunday = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_SUNDAY).Single().SettingValue);
            SundayRequires7DaysWork = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_SUNDAY_7D).Single().SettingValue);
            FlexiTime = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_FLEXITIME).Single().SettingValue);
            LateTolerance = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_LATETOLERANCE).Single().SettingValue);
            AutoChargeAbsLateUTToVL = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_AUTOCHARGE_ABSLATEUT_TO_VL).Single().SettingValue);
            MinNetNotification = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_MINNETNOTIFICATION).Single().SettingValue);
            ACAPERA = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_ACAPERA).Single().SettingValue);
            AgencyShareWTaxRate = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_AGENCYSHARE_WTAX_RATE).Single().SettingValue);

            Faculty40_Enable = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_FACULTY40_ENABLE).Single().SettingValue);
            Faculty40_MinHoursPerDay = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_FACULTY40_MINHOURS_PER_DAY).Single().SettingValue);
            FacultyMaxHoursPerDay = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_FACULTY_MAX_HOURS_PER_DAY).Single().SettingValue);
            ContractualMaxHoursPerWeek = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_ontractual_MAX_HOURS_PER_WEEK).Single().SettingValue);

            OT = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_OT).Single().SettingValue);

            FixedLatePenalty = Convert.ToDouble(settings.Where(x => x.Setting == Constants.SETTING_ID_FIXEDLATEPENALTY).Single().SettingValue);
            UseFixedLatePenalty = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USEFIXEDLATEPENALTY).Single().SettingValue);

            CommissionIsTaxable = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_COMMISSION_IS_TAXABLE).Single().SettingValue);
            UsePAGIBIG_Percentage = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USE_PAGIBIG_PERCENTAGE).Single().SettingValue);

            NoWorkRequiredOnSpecialHolidayPerm = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_NOWORKSHPERM).Single().SettingValue);

            UseNightPremium = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USE_NIGHT_PREMIUM).Single().SettingValue);
            UseRegularHolidayRate = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USE_REGULAR_HOLIDAY_RATE).Single().SettingValue);
            UseSpecialHolidayRate = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USE_SPECIAL_HOLIDAY_RATE).Single().SettingValue);
            UserRestDayRate = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USE_REST_DAY_RATE).Single().SettingValue);
            UseSundayRate = Convert.ToBoolean(settings.Where(x => x.Setting == Constants.SETTING_ID_USE_SUNDAY_RATE).Single().SettingValue);
        }
    }

    public class etData
    {
        OfflineDataSource offlineDS;
        public bool IsOnline
        {
            get
            {
                return offlineDS == null;
            }
        }

        public List<tblGlobalSchedule_Segment> tblGlobalSchedule_Segments = new List<tblGlobalSchedule_Segment>();
        public List<tblEmployee_Schedule> tblEmployee_Schedules;
        public List<tblEmployee_Schedule_Segment> tblEmployee_Schedule_Segments = new List<tblEmployee_Schedule_Segment>();
        public List<tblEmployee_ScheduleOverride> tblEmployee_ScheduleOverrides;
        public List<tblEmployee_ScheduleOverride_Segment> tblEmployee_ScheduleOverride_Segments = new List<tblEmployee_ScheduleOverride_Segment>();

        public List<tblEmployee_TimeLog> tblEmployee_TimeLogs;
        public List<tblEmployee_Travel> tblEmployee_Travels;
        public List<tblEmployee_OT> tblEmployee_OTs;
        public List<tblEmployee_RD> tblEmployee_RDs;
        public List<tblHoliday> tblHolidays, tblHolidays_Raw;
        public List<employeeLeave> leaves = new List<employeeLeave>();

        public List<tblEmployeeOverload> overloads = new List<tblEmployeeOverload>();

        public TimeSettingsModel settings;

        public int employeeId = -1;
        public DateTime startDate, endDate;
        public int officeId = -1;
        public double WorkHoursPerDay;

        public tblEmployee Employee;
        public tblEmployee_Schedule Schedule;

        public tblEmployee_Career Career;

        //public etData(TimeSettingsModel settings)
        //{
        //    this.settings = settings;
        //}

        public bool IsRegularFaculty
        {
            get
            {
                return Career.IsFaculty && Career.EmploymentType == (int)EmploymentType.Permanent;
            }
        }

        public bool IsContractualFaculty
        {
            get
            {
                return Career.IsFaculty && Career.EmploymentType == (int)EmploymentType.Contractual;
            }
        }
        //public bool WithOverload
        //{
        //    get
        //    {
        //        return Career.IsFaculty && Career.WithOverload;
        //    }
        //}

        //public double DoctoralHours
        //{
        //    get
        //    {
        //        return Career.DoctoralHours;
        //    }
        //}
        //public double MasteralHours
        //{
        //    get
        //    {
        //        return Career.MasteralHours;
        //    }
        //}
        //public double UndergradHours
        //{
        //    get
        //    {
        //        return Career.UndergradHours;
        //    }
        //}

        //public double TotalOverloadHoursPerWeek
        //{
        //    get
        //    {
        //        return Career.DoctoralHours + Career.MasteralHours + Career.UndergradHours;
        //    }
        //}

        public bool IsTemporaryFaculty
        {
            get
            {
                return Career.IsFaculty && Career.EmploymentType == (int)EmploymentType.Temporary;
            }
        }

        public etData(int employeeId, DateTime startDate, DateTime endDate, ref OfflineDataSource offlineDS, TimeSettingsModel settings = null)
        {
            this.offlineDS = offlineDS;
            this.settings = settings;

            this.employeeId = employeeId;
            this.startDate = startDate;
            this.endDate = endDate;


            Employee = offlineDS.Employees.Where(x => x.EmployeeId == employeeId).Single();

            Career = new offline_procs_tblEmployee(Employee, ref offlineDS).LatestCareer();

            if (Career == null)
            {
                throw new Exception(ModuleConstants.EMPLOYEE_HAS_NO_CAREER_DEFINED);
            }

            if (Career.Office != null)
            {
                officeId = Career.Office.OfficeId;
            }

            tblGlobalSchedule_Segments = offlineDS.GlobalScheduleSegments.Where(x => x.AllowEmployee(Career)).ToList();

            tblEmployee_Schedules = offlineDS.vwEmployeeSchedules.Where(x => x.EmployeeId == employeeId).ToList().Select(x => new tblEmployee_Schedule(x)).ToList();


            foreach (tblEmployee_Schedule sched in tblEmployee_Schedules)
            {
                tblEmployee_Schedule_Segments.AddRange(new EmployeeScheduleSegments(sched.ScheduleId, tblGlobalSchedule_Segments, ref offlineDS).tblEmployee_Schedule_Segments);
            }

            tblEmployee_ScheduleOverrides = offlineDS.EmployeeScheduleOverrides.ToList().Where(x => x.EmployeeId == employeeId).ToList();

            foreach (tblEmployee_ScheduleOverride sched in tblEmployee_ScheduleOverrides)
            {
                tblEmployee_ScheduleOverride_Segments.AddRange(tblGlobalSchedule_Segments.Select(x => new tblEmployee_ScheduleOverride_Segment(x) { ScheduleId = sched.ScheduleId }).ToList());

                tblEmployee_ScheduleOverride_Segments.AddRange(
                    offlineDS.EmployeeScheduleOverride_Segments.Where(x => x.ScheduleId == sched.ScheduleId)
                    );
            }

            tblEmployee_TimeLogs = offlineDS.EmployeeTimeLogs.Where(x => x.EmployeeId == employeeId && x.TimeLog >= startDate.AddDays(-1) && x.TimeLog <= endDate.AddDays(2)).ToList();
            tblEmployee_Travels = offlineDS.EmployeeTravels.Where(x => x.EmployeeId == employeeId).ToList();
            tblEmployee_OTs = offlineDS.EmployeeOTs.Where(x => x.EmployeeId == employeeId).ToList();
            tblEmployee_RDs = offlineDS.EmployeeRDs.Where(x => x.EmployeeId == employeeId).ToList();

            tblHolidays_Raw = offlineDS.Holidays.Where(x => x.MatchOffice(officeId)).ToList();

            init(startDate, endDate);
        }

        public etData(int employeeId, DateTime startDate, DateTime endDate, TimeSettingsModel settings = null)
        {
            this.settings = settings;
            this.employeeId = employeeId;
            this.startDate = startDate;
            this.endDate = endDate;

            using (dalDataContext context = new dalDataContext())
            {

                Employee = context.tblEmployees.Where(x => x.EmployeeId == employeeId).Single();

                Career = new procs_tblEmployee(Employee).LatestCareer();

                if (Career == null)
                {
                    throw new Exception(ModuleConstants.EMPLOYEE_HAS_NO_CAREER_DEFINED);
                }

                if (Career.Office != null)
                {
                    officeId = Career.Office.OfficeId;
                }

                tblGlobalSchedule_Segments = Common.Global_CachedTables.GlobalScheduleSegments.Where(x => x.AllowEmployee(Career)).ToList();

                tblEmployee_Schedules = context.vwEmployee_Schedules.Where(x => x.EmployeeId == employeeId).ToList().Select(x => new tblEmployee_Schedule(x)).ToList();

                foreach (tblEmployee_Schedule sched in tblEmployee_Schedules)
                {
                    tblEmployee_Schedule_Segments.AddRange(new EmployeeScheduleSegments(sched.ScheduleId, tblGlobalSchedule_Segments).tblEmployee_Schedule_Segments);
                }

                tblEmployee_ScheduleOverrides = context.tblEmployee_ScheduleOverrides.ToList().Where(x => x.EmployeeId == employeeId).ToList();

                foreach (tblEmployee_ScheduleOverride sched in tblEmployee_ScheduleOverrides)
                {
                    tblEmployee_ScheduleOverride_Segments.AddRange(tblGlobalSchedule_Segments.Select(x => new tblEmployee_ScheduleOverride_Segment(x) { ScheduleId = sched.ScheduleId }).ToList());

                    tblEmployee_ScheduleOverride_Segments.AddRange(
                        context.tblEmployee_ScheduleOverride_Segments.Where(x => x.ScheduleId == sched.ScheduleId).ToList()
                        );
                }

                tblEmployee_TimeLogs = context.tblEmployee_TimeLogs.Where(x => x.EmployeeId == employeeId && x.TimeLog >= startDate.AddDays(-1) && x.TimeLog <= endDate.AddDays(2)).ToList();
                tblEmployee_Travels = context.tblEmployee_Travels.Where(x => x.EmployeeId == employeeId).ToList();
                tblEmployee_OTs = context.tblEmployee_OTs.Where(x => x.EmployeeId == employeeId).ToList();
                tblEmployee_RDs = context.tblEmployee_RDs.Where(x => x.EmployeeId == employeeId).ToList();
                overloads = context.tblEmployeeOverloads.Where(x => x.EmployeeID == employeeId && x.StartDate <= startDate && x.EndDate >= endDate).ToList();

                tblHolidays_Raw = Common.Global_CachedTables.Holidays.Where(x => x.MatchOffice(officeId)).ToList();

                init(startDate, endDate);
            }
        }

        public void init(DateTime startDate, DateTime endDate)
        {
            Holidays h = new Holidays(startDate, endDate, tblHolidays_Raw);
            tblHolidays = h.tblHolidays.OrderBy(x => x.DateValue).ToList();

            if (settings == null)
            {
                settings = new TimeSettingsModel(Procs.TMSettings(startDate, officeId));
            }

            WorkHoursPerDay = settings.WorkHoursPerDay;

            if (IsOnline)
            {
                leaves = Common.Global_CachedTables.LeaveTypes.Select(x => new employeeLeave(employeeId, x.Id, endDate)).ToList();
            }
            else
            {
                leaves = offlineDS.LeaveTypes.Select(x => new employeeLeave(employeeId, x.Id, ref offlineDS, endDate)).ToList();
            }
        }
    }

    public class etPeriod
    {
        public DateTime startDate, endDate;
        public etData data;

        public DateTime lastLog;

        public List<etDay> Days = new List<etDay>();

        public double TotalWorkHours { get; set; }
        public double TotalDaysHoliday { get; set; }
        public double TotalWorkDays { get; set; }
        public double TotalWorkDaysCount { get; set; }
        public double TotalHolidaysReg { get; set; }
        public double TotalHolidaysSpl { get; set; }
        public double TotalHoursWorked { get; set; }
        public double TotalHoursWorked_DayEq { get; set; }
        public double TotalHoursLate { get; set; }
        public double TotalHoursUndertime { get; set; }
        public double TotalDaysAbsent { get; set; }
        public double NoOfFullDaysAbsent { get; set; }

        public double TotalOT { get; set; }
        public double TotalOT_Eq { get; set; }
        public double TotalNP { get; set; }
        public double TotalNP_Eq { get; set; }
        public double TotalRH_Eq { get; set; }
        public double TotalSH_Eq { get; set; }
        public double TotalRD_Eq { get; set; }
        public double TotalRDRH_Eq { get; set; }
        public double TotalRDSH_Eq { get; set; }
        public double TotalSun_Eq { get; set; }

        public double TotalHoursFlexiTime { get; set; }
        public double TotalHoursLateTolerance { get; set; }

        public double TotalLeaveEq { get; set; }

        public int? segmentType { get; set; }
        public int DaysLate { get; set; }

        public bool excludeOT;

        public bool WithOverload
        {
            get
            {
                return Faculty40 ? data.overloads.Any() : false;
            }
        }

        public double WeeklyOverload
        {
            get
            {
                return data.overloads.Sum(x => x.HoursPerWeek);
            }
        }

        public double TotalOverloadHours { get; set; }

        public double TotalOverloadHoursToDistribute { get; set; }

        public double TotalDoctoralHours { get; set; }
        public double TotalMasteralHours { get; set; }
        public double TotalUndergradHours { get; set; }
        public double TotalSpecialClassHours { get; set; }
        public double TotalPetitionClassHours { get; set; }

        public double DoctoralRate { get; set; }
        public double MasteralRate { get; set; }
        public double UndergradRate { get; set; }
        public double SpecialClassRate { get; set; }
        public double PetitionClassRate { get; set; }

        public double DoctoralHours { get; set; }
        public double MasteralHours { get; set; }
        public double UndergradHours { get; set; }
        public double SpecialClassHours { get; set; }
        public double PetitionClassHours { get; set; }

        public bool Faculty40
        {
            get
            {
                return data.settings.Faculty40_Enable && ((data.IsRegularFaculty || data.IsTemporaryFaculty));
            }
        }

        public etPeriod(int employeeId, DateTime startDate, DateTime endDate, int? segmentType = (int)SegmentType.Regular, bool excludeOT = false)
        {
            etData data = new etData(employeeId, startDate, endDate);
            init(startDate, endDate, data, segmentType, excludeOT: excludeOT);
        }

        public etPeriod(DateTime startDate, DateTime endDate, etData data, int? segmentType = (int)SegmentType.Regular, bool excludeOT = false)
        {
            init(startDate, endDate, data, segmentType, excludeOT: excludeOT);
        }

        private void init(DateTime startDate, DateTime endDate, etData data, int? segmentType, int periodLimitInMonths = -1, bool excludeOT = false)
        {
            this.excludeOT = excludeOT;

            double noOfDays = (endDate - startDate).TotalDays;
            double noOfMonths = (noOfDays / 365) * 12;

            if (periodLimitInMonths != -1 && noOfMonths > periodLimitInMonths)
            {
                throw new Exception("Cannot calculate beyond 6 months");
            }

            this.segmentType = segmentType;
            this.startDate = startDate;
            this.endDate = endDate;
            this.data = data;

            getDays();

            var holidays = Days.Where(x => x.IsHoliday && !x.NoSchedule);

            TotalHolidaysReg = holidays.Where(x => x.IsRegularHoliday).Sum(x => x.TotalWorkHours) / data.WorkHoursPerDay;
            TotalHolidaysSpl = holidays.Where(x => x.IsSpecialHoliday).Sum(x => x.TotalWorkHours) / data.WorkHoursPerDay;

            TotalWorkHours = Days.Sum(x => x.TotalWorkHours);
            TotalWorkDays = TotalWorkHours / data.WorkHoursPerDay;

            TotalWorkDaysCount = Days.Sum(x => x.WorkDayCount);

            if (Faculty40)
            {
                faculty40 f40 = new faculty40(this);

                TotalWorkHours = f40.TotalWorkHours;
                TotalWorkDays = TotalWorkHours / data.WorkHoursPerDay;

                TotalWorkDaysCount = f40.TotalWorkDaysCount;

                TotalHoursWorked = f40.TotalHoursWorked;
                TotalHoursWorked_DayEq = f40.TotalHoursWorked_DayEq;
                TotalHoursLate = 0;
                TotalHoursUndertime = 0;

                NoOfFullDaysAbsent = 0;
                TotalDaysAbsent = f40.TotalDaysAbsent;

                TotalOverloadHours = f40.TotalHoursOverload;

                TotalOverloadHoursToDistribute = TotalOverloadHours;

                if (WithOverload)
                {
                    int counter = 1;

                    DoctoralHours = data.overloads.Where(x => x.OverloadType == 0).Sum(x => x.HoursPerWeek);
                    MasteralHours = data.overloads.Where(x => x.OverloadType == 1).Sum(x => x.HoursPerWeek);
                    UndergradHours = data.overloads.Where(x => x.OverloadType == 2).Sum(x => x.HoursPerWeek);
                    SpecialClassHours = data.overloads.Where(x => x.OverloadType == 3).Sum(x => x.HoursPerWeek);
                    PetitionClassHours = data.overloads.Where(x => x.OverloadType == 4).Sum(x => x.HoursPerWeek);

                    DoctoralRate = data.overloads.Where(x => x.OverloadType == 0).Sum(x => x.HourlyRate);
                    MasteralRate = data.overloads.Where(x => x.OverloadType == 1).Sum(x => x.HourlyRate);
                    UndergradRate = data.overloads.Where(x => x.OverloadType == 2).Sum(x => x.HourlyRate);
                    SpecialClassRate = data.overloads.Where(x => x.OverloadType == 3).Sum(x => x.HourlyRate);
                    PetitionClassRate = data.overloads.Where(x => x.OverloadType == 4).Sum(x => x.HourlyRate);

                    while (TotalOverloadHoursToDistribute != 0)
                    {
                        if (counter == 1)
                        {
                            if (DoctoralHours > 0)
                            {
                                if (TotalOverloadHoursToDistribute >= DoctoralHours)
                                {
                                    TotalDoctoralHours += DoctoralHours;
                                    TotalOverloadHoursToDistribute -= DoctoralHours;
                                }
                                else
                                {
                                    TotalDoctoralHours += TotalOverloadHoursToDistribute;
                                    TotalOverloadHoursToDistribute = 0;
                                }
                            }

                            counter = 2;
                        }
                        else if (counter == 2)
                        {
                            if (MasteralHours > 0)
                            {
                                if (TotalOverloadHoursToDistribute >= MasteralHours)
                                {
                                    TotalMasteralHours += MasteralHours;
                                    TotalOverloadHoursToDistribute -= MasteralHours;
                                }
                                else
                                {
                                    TotalMasteralHours += TotalOverloadHoursToDistribute;
                                    TotalOverloadHoursToDistribute = 0;
                                }
                            }

                            counter = 3;
                        }
                        else if (counter == 3)
                        {
                            if (UndergradHours > 0)
                            {
                                if (TotalOverloadHoursToDistribute >= UndergradHours)
                                {
                                    TotalUndergradHours += UndergradHours;
                                    TotalOverloadHoursToDistribute -= UndergradHours;
                                }
                                else
                                {
                                    TotalUndergradHours += TotalOverloadHoursToDistribute;
                                    TotalOverloadHoursToDistribute = 0;
                                }
                            }

                            counter = 4;
                        }
                        else if (counter == 4)
                        {
                            if (SpecialClassHours > 0)
                            {
                                if (TotalOverloadHoursToDistribute >= SpecialClassHours)
                                {
                                    TotalSpecialClassHours += SpecialClassHours;
                                    TotalOverloadHoursToDistribute -= SpecialClassHours;
                                }
                                else
                                {
                                    TotalSpecialClassHours += TotalOverloadHoursToDistribute;
                                    TotalOverloadHoursToDistribute = 0;
                                }
                            }

                            counter = 5;
                        }
                        else if (counter == 5)
                        {
                            if (PetitionClassHours > 0)
                            {
                                if (TotalOverloadHoursToDistribute >= PetitionClassHours)
                                {
                                    TotalPetitionClassHours += PetitionClassHours;
                                    TotalOverloadHoursToDistribute -= PetitionClassHours;
                                }
                                else
                                {
                                    TotalPetitionClassHours += TotalOverloadHoursToDistribute;
                                    TotalOverloadHoursToDistribute = 0;
                                }
                            }

                            counter = 1;
                        }

                    }
                }
            }
            else
            {

                TotalWorkHours = Days.Sum(x => x.TotalWorkHours);
                TotalWorkDays = TotalWorkHours / (data.Career.Position.Position == "Security Guard" ? 6 : data.WorkHoursPerDay);

                TotalWorkDaysCount = Days.Sum(x => x.WorkDayCount);

                if (data.IsContractualFaculty && data.settings.ContractualMaxHoursPerWeek > 0)
                {
                    double NumberOfWeeks = (endDate - startDate).TotalDays / 7;

                    TotalHoursWorked = Days.Sum(x => x.TotalHoursWorked);

                    if (TotalHoursWorked > (NumberOfWeeks * data.settings.ContractualMaxHoursPerWeek))
                    {
                        TotalHoursWorked = NumberOfWeeks * data.settings.ContractualMaxHoursPerWeek;
                    }
                  
                }
                else
                {
                    TotalHoursWorked = Days.Sum(x => x.TotalHoursWorked);
                }
               
                TotalHoursWorked_DayEq = Days.Sum(x => x.TotalHoursWorked_DayEq);
                TotalHoursLate = Days.Sum(x => x.TotalMinsLate) / 60;
                TotalHoursUndertime = Days.Sum(x => x.TotalMinsUndertime) / 60;

                NoOfFullDaysAbsent = Days.Where(a => a.IsWorkDay && a.IsAbsent && !a.IsOnTravel && !a.IsOnLeave && !a.IsOnHalfDayLeave).Sum(x => x.Absent_DayEq) +
                    Days.Where(x => x.IsWorkDay && x.IsAbsent && x.IsOnLeave).Sum(x => x.Absent_DayEq);
                TotalDaysAbsent = NoOfFullDaysAbsent + Days.Where(x => x.IsWorkDay && !x.IsAbsent && !x.IsOnTravel).Sum(x => x.Absent_DayEq);

                TotalDaysAbsent += Days.Where(x => x.IsWorkDay && !x.IsAbsent && !x.IsOnTravel && x.Leave_WithoutPay).Sum(x => x.Times.Where(y => y.IsAbsent).Sum(z => z.segment.WorkDayEq));

                TotalDaysAbsent += Days.Where(x => x.IsWorkDay && !x.IsAbsent && !x.IsOnTravel && !x.IsOnLeave).Sum(x => x.Times.Where(y => y.IsAbsent && !y.OTSegment).Sum(z => z.segment.WorkDayEq));

            }

            TotalLeaveEq = Days.Sum(x => x.LeaveEq);

            TotalOT = Days.Sum(x => x.OT);
            TotalOT_Eq = Days.Sum(x => x.OT_Eq);
            TotalNP = Days.Sum(x => x.NP);
            TotalNP_Eq = Days.Sum(x => x.NP_Eq);
            TotalRH_Eq = Days.Sum(x => x.RH_Eq);
            TotalSH_Eq = Days.Sum(x => x.SH_Eq);
            TotalRD_Eq = Days.Sum(x => x.RD_Eq);
            TotalRDRH_Eq = Days.Sum(x => x.RDRH_Eq);
            TotalRDSH_Eq = Days.Sum(x => x.RDSH_Eq);

            TotalSun_Eq = data.settings.SundayRequires7DaysWork ?
                Days.Where(x => x.Sun_Eq > 0).Where(x => sunHas7Dprior(x)).Sum(x => x.Sun_Eq) :
                Days.Sum(x => x.Sun_Eq);

            TotalHoursFlexiTime = Days.Sum(x => x.minsFlexiTime) / 60;
            TotalHoursLateTolerance = Days.Sum(x => x.minsLateTolerance) / 60;

            DaysLate = Days.Count(x => x.IsReallyLate);
        }

        private bool sunHas7Dprior(etDay day)
        {
            DateTime startDate = day.d.AddDays(-6);

            List<etDay> periodDays = Days.Where(x => x.d.Date >= startDate && x.d.Date <= day.d).OrderBy(x => x.d).ToList();
            if (periodDays.Count < 7)
            {
                etPeriod subPeriod = new etPeriod(data.employeeId, startDate, periodDays.First().d.AddDays(-1));
                periodDays.AddRange(subPeriod.Days);
            }

            return !periodDays.Any(x => x.IsAbsent);
        }

        private void getDays()
        {
            //get valid schedules
            var tmp = data.tblEmployee_Schedules.Where(x => x.Enabled && !(x.DateTo < startDate) && !(x.DateFrom > endDate))
                                                        .OrderBy(x => x.DateFrom).ThenBy(x => x.ScheduleId);

            if (tmp.Any())
            {
                //get sched period and widest period
                DateTime sDt = tmp.Min(x => x.DateFrom), eDt = tmp.Max(x => x.DateTo), wsDt, weDt;
                if (startDate < sDt) { wsDt = startDate; } else { wsDt = sDt; }
                if (endDate > eDt) { weDt = endDate; } else { weDt = eDt; }

                //create schedule map
                List<etDay> days = new List<etDay>();
                bool firstPass = true;
                bool passedSched = true;

                DateTime d = wsDt;

                while (d <= weDt)
                {
                    if (!passedSched) { days.Add(new etDay(d, segmentType, excludeOT)); }

                    if (days.Count > 0) { d = days.Max(x => x.d).AddDays(1); }

                    passedSched = false;
                    foreach (tblEmployee_Schedule sched in tmp)
                    {
                        if (sched.DateFrom <= d && sched.DateTo >= d)
                        {
                            DateTime dd = sched.datePlusDuration(d, firstPass);
                            if (dd > sched.DateTo)
                            {
                                dd = sched.DateTo;
                            }

                            days.AddRange(createDays(d, dd, sched, ref data));

                            d = days.Max(x => x.d).AddDays(1);

                            firstPass = false;
                            passedSched = true;
                        }
                    }
                }

                //fill empty days
                d = days.Min(x => x.d);
                int diff = (int)(d - wsDt).TotalDays;

                if (diff >= 1)
                {
                    days.InsertRange(0, createDays(wsDt, d.AddDays(-1), null, ref data));
                }

                d = days.Max(x => x.d);
                diff = (int)(weDt - d).TotalDays;

                if (diff >= 1)
                {
                    days.AddRange(createDays(d.AddDays(1), weDt, null, ref data));
                }

                //filter days map by period
                Days = days.Where(x => x.d >= startDate && x.d <= endDate).ToList();


            }
            else
            {
                Days = createDays(startDate, endDate, null, ref data);
            }

            foreach (etDay _day in Days)
            {
                _day.init(lastLog, segmentType);
                lastLog = _day.lastLog;
            }

        }

        private List<etDay> createDays(DateTime startDate, DateTime endDate, tblEmployee_Schedule schedule, ref etData data)
        {
            List<etDay> days = new List<etDay>();

            DateTime d = startDate;
            while (d <= endDate)
            {
                tblEmployee_ScheduleOverride scheduleOverride = data.tblEmployee_ScheduleOverrides.Where(x => x.passedValidity(d) && x.passedException(d))
                    .OrderByDescending(x => x.ScheduleId)
                    .FirstOrDefault();

                if (scheduleOverride == null)
                {
                    days.Add(new etDay(d, schedule, data, segmentType, excludeOT));
                }
                else
                {
                    if (data.tblEmployee_ScheduleOverride_Segments.Where(x => x.ScheduleId == scheduleOverride.ScheduleId).Any())
                    {
                        days.Add(new etDay(d, new tblEmployee_Schedule(scheduleOverride) { ScheduleId = scheduleOverride.ScheduleId }, data, segmentType, excludeOT));
                    }
                    else
                    {
                        days.Add(new etDay(d, null, data, segmentType, excludeOT));
                    }
                }

                d = d.AddDays(1);
            }

            return days;
        }

    }

    public class etDay
    {
        public tblEmployee_Schedule schedule;
        public DateTime d;
        public List<etTime> _Times = new List<etTime>();
        public List<etTime> Times = new List<etTime>();
        public List<etBreak> Breaks = new List<etBreak>();

        public etData data;

        public DateTime lastLog;

        public int WorkDayCount { get; set; }
        public bool IsOnTravel { get; set; } = false;
        public bool IsAbsent { get; set; } = false;
        public bool IsOnLeave { get; set; } = false;
        public string LeaveRemarks { get; set; } = "";

        public bool IsOnHalfDayLeave { get; set; } = false;
        public bool Leave_WithoutPay { get; set; } = false;
        public double LeaveEq { get; set; } = 0;
        public double Absent_DayEq { get; set; } = 0;
        public bool NoSchedule { get; set; } = true;
        public tblHoliday Holiday { get; set; } = null;
        public bool IsHoliday { get; set; } = false;
        public bool IsSunday { get; set; } = false;
        public bool IsSaturday { get; set; } = false;

        public double TotalWorkHours { get; set; } = 0;
        public double TotalHoursWorked { get; set; } = 0;
        public double TotalHoursWorked_DayEq { get; set; } = 0;
        public double TotalMinsLate { get; set; } = 0;
        public double TotalMinsUndertime { get; set; } = 0;
        public double WorkDayEq { get; set; } = 0;
        public double OT { get; set; } = 0;
        public double OT_Eq { get; set; } = 0;
        public double NP { get; set; } = 0;
        public double NP_Eq { get; set; } = 0;
        public double RH_Eq { get; set; } = 0;
        public double SH_Eq { get; set; } = 0;
        public double RD_Eq { get; set; } = 0;
        public double RDRH_Eq { get; set; } = 0;
        public double RDSH_Eq { get; set; } = 0;
        public double Sun_Eq { get; set; } = 0;

        public double minsFlexiTime { get; set; } = 0;
        public double minsLateTolerance { get; set; } = 0;

        public bool IsWorkDay { get; set; } = false;
        public bool IsRestDay { get; set; } = false;
        public bool IsRegularHoliday { get; set; } = false;
        public bool IsSpecialHoliday { get; set; } = false;

        public int? segmentType { get; set; } = null;

        int LateUTRoundOff;

        public bool IsReallyLate { get; set; }
        public bool excludeOT;

        public bool withOT;
        public bool withException;

        private double tmpOT, tmpOT_NotOTRateOnly;
        private List<etTime> otTimes;

        public bool Faculty40
        {
            get
            {
                return data.settings.Faculty40_Enable && (data.IsRegularFaculty || data.IsTemporaryFaculty);

            }
        }

        public double WorkHoursPerDay
        {
            get
            {
                return data.settings.WorkHoursPerDay;
            }
        }

        public bool Faculty40_Saturday_Absence
        {
            get
            {
                return Faculty40 && IsAbsent && d.DayOfWeek == DayOfWeek.Saturday;
            }
        }
        public bool Faculty40_Sunday_Absence
        {
            get
            {
                return Faculty40 && IsAbsent && d.DayOfWeek == DayOfWeek.Sunday;
            }
        }


        public etDay(DateTime d, int? segmentType, bool excludeOT = false)
        {
            this.excludeOT = excludeOT;
            this.d = d;
        }

        public etDay(DateTime d, tblEmployee_Schedule schedule, etData data, int? segmentType, bool excludeOT = false)
        {
            this.excludeOT = excludeOT;
            this.d = d;
            this.schedule = schedule;
            this.data = data;
        }

        public double getLeaveEq(int leaveTypeId = -1)
        {
            double ret = 0;

            if (IsWorkDay)
            {
                employeeLeave el = data.leaves.Where(x => leaveTypeId == -1 || x.leaveTypeId == leaveTypeId).OrderByDescending(x => x.leaveEq(d)).FirstOrDefault();

                if (el != null)
                {

                    if (el.LeaveName() == "Work From Home")
                    {
                        LeaveRemarks = "Work From Home";
                    }
                    else if (el.LeaveName() == "Home Quarantine")
                    {
                        LeaveRemarks = "Home Quarantine";
                    }
                    else
                    {
                        LeaveRemarks = "On Leave";
                    }

                    LeaveEq = el.leaveEq(d);
                    if (LeaveEq > 0)
                    {
                        IsOnLeave = LeaveEq > .5;
                        IsOnHalfDayLeave = LeaveEq <= .5;

                    }

                    ret = LeaveEq;
                }
            }

            return ret;
        }

        public bool getLeaveWoPay(int leaveTypeId = -1)
        {
            bool ret = false;

            if (IsWorkDay)
            {
                employeeLeave el = data.leaves.Where(x => leaveTypeId == -1 || x.leaveTypeId == leaveTypeId).OrderByDescending(x => x.leaveEq(d)).FirstOrDefault();

                if (el != null)
                {
                    tblEmployee_LeaveCredit lc = el.table.Where(x => x.IsDrEntry && x.Match(d)).FirstOrDefault();

                    if (lc != null)
                    {
                        ret = el.LeaveWithoutPay;
                    }
                }
            }

            return ret;
        }

        private etDay PreviousDay(DateTime d, int noOfdaysBack)
        {
            DateTime dt = d.AddDays(-1 * noOfdaysBack);
            DateTime endDate = dt.Date.AddDays(1).AddSeconds(-1);

            etPeriod etPeriod = new etPeriod(data.employeeId, dt, endDate);
            return etPeriod.Days.First();
        }

        public etDay ImmediateWorkDay(DateTime d, DateTime limit)
        {
            etDay tmp = PreviousDay(d, 1);
            if (tmp.IsWorkDay)
            {
                return tmp;
            }
            else if (d <= limit)
            {
                return null;
            }
            else
            {
                return ImmediateWorkDay(tmp.d, limit);
            }
        }

        public void init(DateTime lastLog, int? segmentType)
        {
            this.segmentType = segmentType;
            this.lastLog = lastLog;
            IsHoliday = false;
            IsSunday = false;

            double _flexiTime = 0;
            double _lateTolerance = 0;

            IsReallyLate = false;

            minsFlexiTime = 0;
            minsLateTolerance = 0;
            OT = 0;
            OT_Eq = 0;
            NP = 0;
            NP_Eq = 0;
            RH_Eq = 0;
            SH_Eq = 0;
            RD_Eq = 0;
            RDRH_Eq = 0;
            RDSH_Eq = 0;
            Sun_Eq = 0;

            tmpOT = 0;
            tmpOT_NotOTRateOnly = 0;

            if (data == null)
            {
                return;
            }

            LateUTRoundOff = data.settings.LateUTRoundOff;

            _flexiTime = data.settings.FlexiTime * 60;
            _lateTolerance = data.settings.LateTolerance;

            Holiday = data.tblHolidays.Where(x => x.Match(d)).FirstOrDefault();
            IsHoliday = Holiday != null;

            if (IsHoliday)
            {
                IsRegularHoliday = Holiday.Type == (int)HolidayType.Regular;
                IsSpecialHoliday = Holiday.Type == (int)HolidayType.Special;
            }

            IsSunday = d.DayOfWeek == DayOfWeek.Sunday;
            IsSaturday = d.DayOfWeek == DayOfWeek.Saturday;

            IsRestDay = data.tblEmployee_RDs.Any(x => d >= x.StartDate && d <= x.EndDate && x.inDays(d.DayOfWeek));

            withOT = data.tblEmployee_OTs.Any(x => d >= x.StartDate && d <= x.EndDate);
            withException = schedule is null ? false : !schedule.passedException(d);

            if (schedule != null)
            {

                if (schedule.passedValidity(d) && schedule.passedException(d) || withOT)
                {
                    getTimes();
                    getBreaks();
                }
            }

            if (segmentType == null)
            {
                Times = _Times.ToList();
            }
            else
            {
                if (segmentType == (int)SegmentType.Regular)
                {
                    Times = _Times.Where(x => x.segment.SegmentType == segmentType || x.segment.SegmentType == (int)SegmentType.Workspan || x.segment.SegmentType == (int)SegmentType.Overtime).ToList();
                }
                else
                {
                    Times = _Times.Where(x => x.segment.SegmentType == segmentType).ToList();
                }

            }

            NoSchedule = Times.Where(x => x.segment.SegmentType != (int)SegmentType.Overtime).Count() == 0 || schedule == null;
            IsWorkDay = !NoSchedule && !IsRestDay;

            LeaveEq = getLeaveEq();
            Leave_WithoutPay = getLeaveWoPay();

            IsAbsent = IsWorkDay &&
                            !Times.Where(x => !x.IsAbsent).Any() &&
                            !IsOnTravel &&
                            !IsOnLeave &&
                            !IsOnHalfDayLeave;

            //if (IsRegularHoliday)
            //{
            //    etDay _etday = ImmediateWorkDay(d, d.AddMonths(-1));
            //    bool prevDayIsAbsent = _etday == null ? true : _etday.IsAbsent;
            //    bool prevDayIsOnLeave = _etday == null ? false : _etday.IsOnLeave || _etday.IsOnHalfDayLeave;
            //    IsAbsent = IsAbsent && prevDayIsAbsent && !prevDayIsOnLeave;
            //}
            //else if (IsSpecialHoliday)
            //{
            //    if (data.Career.IsPermanent || data.Career.IsTemporary || data.Career.IsCasual)
            //    {
            //        IsAbsent = IsAbsent && !data.settings.NoWorkRequiredOnSpecialHolidayPerm;
            //    }
            //}

            if (IsHoliday)
            {
                if (data.Career.IsPermanent || data.Career.IsTemporary || data.Career.IsCasual)
                {
                    IsAbsent = IsAbsent && !data.settings.NoWorkRequiredOnSpecialHolidayPerm;
                }
            }

            //if (data.Career.IsPermanent || data.Career.IsTemporary || data.Career.IsCasual)
            //{
            //    IsAbsent = IsAbsent && !data.settings.NoWorkRequiredOnSpecialHolidayPerm;
            //}

            Absent_DayEq = !IsAbsent ? 0 : Times.Where(x => x.IsAbsent).Sum(x => x.segment.WorkDayEq);

            WorkDayCount = IsWorkDay && !IsAbsent ? 1 : 0;

            TotalWorkHours = Times.Where(x => !x.OTSegment).Sum(x => x.TotalWorkHours);
            TotalHoursWorked = Times.Where(x => !x.IsAbsent && !x.OTSegment).Sum(x => x.TotalHoursWorked);
            TotalHoursWorked_DayEq = Times.Where(x => !x.IsAbsent && !x.OTSegment).Sum(x => x.TotalHoursWorked_DayEq);
            TotalMinsLate = Times.Where(x => !x.IsAbsent && !x.OTSegment).Sum(x => x.Log.minsLate);
            TotalMinsUndertime = Times.Where(x => !x.IsAbsent && !x.OTSegment).Sum(x => x.Log.minsUndertime);
            WorkDayEq = Times.Where(x => !x.OTSegment).Sum(x => x.segment.WorkDayEq);


            //if (!IsAbsent)
            //{
            //    if (Times.First().IsAbsent) TotalMinsLate += Times.First().TotalWorkHours * 60;
            //    if (Times.Last().IsAbsent) TotalMinsUndertime += Times.Last().TotalWorkHours * 60;
            //}

            otTimes = _Times.Where(x => x.segment.SegmentType == (int)SegmentType.Overtime).ToList();
            tmpOT = otTimes.Sum(x => x.TotalHoursWorked);
            tmpOT_NotOTRateOnly = otTimes.Where(x => !x.segment.OTRateOnly).ToList().Sum(x => x.TotalHoursWorked);

            getOT();
            getNP();
            getH();
            getSun();
            getRD();

            if (_lateTolerance > 0)
            {
                if (!IsAbsent && TotalMinsLate > 0)
                {
                    if (TotalMinsLate > _lateTolerance)
                    {
                        TotalMinsLate = roundOffLateUT(TotalMinsLate);
                    }
                    else
                    {
                        minsLateTolerance = TotalMinsLate;
                    }

                    TotalHoursWorked += (minsLateTolerance / 60);
                    TotalHoursWorked_DayEq = TotalHoursWorked / TotalWorkHours;
                }
            }
            else
            {
                TotalMinsLate = roundOffLateUT(TotalMinsLate);

                if (_flexiTime > 0)
                {
                    double mLate = TotalMinsLate - minsLateTolerance;

                    if (!IsAbsent && mLate > 0 && TotalMinsUndertime <= 0)
                    {
                        minsFlexiTime = Times.Last().Log.minsFlexiTime;
                        if (minsFlexiTime > _flexiTime)
                        {
                            minsFlexiTime = _flexiTime;
                        }

                        if (minsFlexiTime > mLate)
                        {
                            minsFlexiTime = mLate;
                        }

                        TotalHoursWorked += (minsFlexiTime / 60);
                        TotalHoursWorked_DayEq = TotalHoursWorked / TotalWorkHours;
                    }
                }
            }

            TotalMinsUndertime = roundOffLateUT(TotalMinsUndertime);

            if (!Faculty40)
            {
                if (TotalHoursWorked > TotalWorkHours) TotalHoursWorked = TotalWorkHours;
                if (TotalHoursWorked_DayEq > data.WorkHoursPerDay) TotalHoursWorked_DayEq = 1;
            }

            double _late = TotalMinsLate - minsLateTolerance - minsFlexiTime;
            IsReallyLate = _late > 0;

            IsOnTravel = data.tblEmployee_Travels.Any(x => d >= x.StartDate && d <= x.EndDate);

            if (IsOnTravel)
            {
                TotalHoursWorked = data.WorkHoursPerDay;
                TotalHoursWorked_DayEq = 1;
                TotalMinsLate = 0;
                TotalMinsUndertime = 0;
                OT = 0;
                OT_Eq = 0;
                NP = 0;
                NP_Eq = 0;
                RH_Eq = 0;
                SH_Eq = 0;
                RD_Eq = 0;
                RDRH_Eq = 0;
                RDSH_Eq = 0;
            }

        }

        private double roundOffLateUT(double mins)
        {
            double ret = mins;

            if (LateUTRoundOff > 0)
            {
                int wholeNo = (int)Math.Floor(mins / LateUTRoundOff);

                double rem = mins % LateUTRoundOff;
                if (rem != 0)
                {
                    ret = (wholeNo * LateUTRoundOff) + LateUTRoundOff;
                }
            }

            return ret;
        }

        private void getRD()
        {
            if (IsRestDay && !IsAbsent)
            {
                if (IsHoliday)
                {
                    if (IsRegularHoliday)
                    {
                        double RDRH_Rate = data.settings.RegularHoliday_RestDay / 100;
                        RDRH_Eq = (TotalHoursWorked_DayEq + (tmpOT_NotOTRateOnly / data.settings.WorkHoursPerDay)) * RDRH_Rate;

                        RH_Eq = 0;
                    }
                    else if (IsSpecialHoliday)
                    {
                        double RDSH_Rate = data.settings.SpecialHoliday_RestDay / 100;
                        RDSH_Eq = (TotalHoursWorked_DayEq + (tmpOT_NotOTRateOnly / data.settings.WorkHoursPerDay)) * RDSH_Rate;

                        SH_Eq = 0;
                    }
                }
                else
                {
                    double RD_Rate = data.settings.RestDay / 100;
                    RD_Eq = (TotalHoursWorked_DayEq + (tmpOT_NotOTRateOnly / data.settings.WorkHoursPerDay)) * RD_Rate;
                }

                if (RD_Eq < 0) RD_Eq = 0;
            }
        }

        private void getOT()
        {
            if (!excludeOT)
            {
                double OT_Rate = data.settings.OT / 100;
                OT = tmpOT;
                OT_Eq = otTimes.Sum(x => x.TotalHoursWorked_DayEq) * OT_Rate;
            }
        }

        private void getH()
        {
            if (IsHoliday)
            {
                bool cameToWork = IsWorkDay && Times.Where(x => !x.IsAbsent).Any();

                if (IsRegularHoliday)
                {
                    if (cameToWork)
                    {
                        double RH_Rate = data.settings.RegularHoliday / 100;
                        RH_Eq = ((TotalHoursWorked + tmpOT_NotOTRateOnly) / data.settings.WorkHoursPerDay) * RH_Rate;

                        if (RH_Eq < 0) RH_Eq = 0;
                    }
                }
                else if (IsSpecialHoliday)
                {
                    if (cameToWork)
                    {
                        double SH_Rate = data.settings.SpecialHoliday / 100;
                        SH_Eq = ((TotalHoursWorked + tmpOT_NotOTRateOnly) / data.settings.WorkHoursPerDay) * SH_Rate;

                        if (SH_Eq < 0) SH_Eq = 0;
                    }
                }
            }
        }

        private void getSun()
        {
            if (IsSunday && !IsAbsent)
            {
                double Sun_Rate = data.settings.Sunday / 100;
                Sun_Eq = ((TotalHoursWorked + tmpOT_NotOTRateOnly) / data.settings.WorkHoursPerDay) * Sun_Rate;

                if (Sun_Eq < 0) Sun_Eq = 0;
            }
        }

        private void getNP()
        {
            double NP_Rate = data.settings.NightPremium / 100;
            NP = _Times.Sum(x => x.NP);
            NP_Eq = (NP / data.settings.WorkHoursPerDay) * NP_Rate;

            double xRate = 0;

            if (IsSunday)
            {
                xRate = data.settings.Sunday;
            }

            if (IsHoliday)
            {
                if (IsRegularHoliday)
                {
                    if (IsRestDay)
                    {
                        if (xRate < data.settings.RegularHoliday_RestDay)
                        {
                            xRate = data.settings.RegularHoliday_RestDay;
                        }
                    }
                    else
                    {
                        if (xRate < data.settings.RegularHoliday)
                        {
                            xRate = data.settings.RegularHoliday;
                        }
                    }

                }
                else if (IsSpecialHoliday)
                {
                    if (IsRestDay)
                    {
                        if (xRate < data.settings.SpecialHoliday_RestDay)
                        {
                            xRate = data.settings.SpecialHoliday_RestDay;
                        }
                    }
                    else
                    {
                        if (xRate < data.settings.SpecialHoliday)
                        {
                            xRate = data.settings.SpecialHoliday;
                        }
                    }
                }
            }

            if (xRate > 0)
            {
                xRate = 1 + (xRate / 100);
                NP_Eq *= xRate;
            }
        }

        private void getTimes()
        {
            bool? prevIsAllAbsent = null;
            List<tblEmployee_Schedule_Segment> tmp, ots;

            List<tblEmployee_OT> _ots = data.tblEmployee_OTs.Where(x => d >= x.StartDate && d <= x.EndDate).ToList();

            ots = _ots
                .Select(x => new tblEmployee_Schedule_Segment(x) { OTBreaks = x.Breaks, OTRateOnly = x.OTRateOnly })
                .ToList();

            ots.ForEach(x => x.setBaseDate(d));
            ots = ots.OrderBy(x => x.TimeIn).ToList();


            if (schedule.IsOverride)
            {
                tmp = data.tblEmployee_ScheduleOverride_Segments.Where(x => x.ScheduleId == schedule.ScheduleId && x.inDays(d.DayOfWeek) && x.Enabled)
                    .Select(x => new tblEmployee_Schedule_Segment(x) { ScheduleId = schedule.ScheduleId, IsGlobal = x.IsGlobal })
                    .ToList();
            }
            else
            {
                tmp = data.tblEmployee_Schedule_Segments.Where(x => x.ScheduleId == schedule.ScheduleId && x.inDays(d.DayOfWeek) && x.Enabled).ToList();
            }

            tblEmployee_Schedule_Segment ws = tmp.Where(x => x.IsWorkSpan).FirstOrDefault();

            if (ws != null)
            {
                tmp = new List<tblEmployee_Schedule_Segment> { ws };
            }

            tmp.ForEach(x => x.setBaseDate(d));
            tmp = tmp.OrderBy(x => x.TimeIn).ToList();

            if (tmp.Any() && ots.Any())
            {
                if (!ots.First().MustTimeIn)
                {
                    tmp.Last().SkipLastLog = true;
                }
            }

            if (withException)
            {
                tmp.Clear();
            }

            tmp.AddRange(ots);

            foreach (tblEmployee_Schedule_Segment segment in tmp.OrderBy(x => x.TimeIn))
            {
                if (!segment.passedValidity(d) || !segment.passedException(d)) continue;

                if (segment.SegmentType_Desc == "Overtime")
                {
                    string[] OTDays = segment.Days.Split(' ');

                    string find = Array.Find(OTDays, element => element == ConvertToDateLetter(d));

                    if (find == null && segment.Days != "") continue;

                }

                etTime ett = new etTime(d, segment, ref data, lastLog, prevIsAllAbsent, IsHoliday);
                lastLog = ett.lastLog;

                if (prevIsAllAbsent != false)
                {
                    prevIsAllAbsent = ett.IsAbsent;
                }

                _Times.Add(ett);
            }

        }

        private string ConvertToDateLetter(DateTime d)
        {
            string text = "";

            int xd = (int)d.DayOfWeek;

            switch ((int)d.DayOfWeek)
            {
                case 0:
                    text = "Su";
                    break;
                case 1:
                    text = "M";
                    break;
                case 2:
                    text = "T";
                    break;
                case 3:
                    text = "W";
                    break;
                case 4:
                    text = "Th";
                    break;
                case 5:
                    text = "F";
                    break;
                case 6:
                    text = "S";
                    break;
            }

            return text;
        }

        private void getBreaks()
        {
            for (int i = 0; i < _Times.Count; i++)
            {
                if (i == 0) continue;

                etTime prev = _Times[i - 1];
                etTime curr = _Times[i];

                TimeSpan diff = curr.segment.TimeIn - prev.segment.TimeOut;

                if (diff.TotalMinutes > 0)
                {
                    Breaks.Add(new etBreak
                    {
                        startTime = prev.segment.TimeOut,
                        endTime = curr.segment.TimeIn
                    });
                }
            }
        }

    }

    public class etTime
    {
        public tblEmployee_Schedule_Segment segment;
        public etLog Log = new etLog();
        etData data;
        DateTime d;
        bool isHoliday;

        public double NP = 0;
        DateTime NP_Start, NP_End;
        double NP_Duration;

        public DateTime lastLog;

        public bool IsAbsent { get; set; }
        public double TotalWorkHours { get; set; }
        public double TotalHoursWorked { get; set; }
        public double TotalHoursWorked_DayEq { get; set; }

        public bool HolidayExcempted
        {
            get
            {
                if (data.Career.IsCasual || data.Career.IsPermanent || data.Career.IsTemporary && data.settings.NoWorkRequiredOnSpecialHolidayPerm)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public bool OTSegment
        {
            get
            {
                return segment.SegmentType == (int)SegmentType.Overtime;
            }
        }

        public bool Faculty40
        {
            get
            {
                return data.settings.Faculty40_Enable && (data.IsRegularFaculty || data.IsTemporaryFaculty) && segment.IsWorkSpan;
            }
        }

        public etTime(DateTime d, tblEmployee_Schedule_Segment segment, ref etData data, DateTime lastLog, bool? prevIsAllAbsent, bool isHoliday)
        {
            this.d = d;
            this.segment = segment;
            this.data = data;
            this.lastLog = lastLog;
            this.isHoliday = isHoliday;

            segment.setBaseDate(d);

            double workHoursLimit = Faculty40 ? 24 : segment.WorkDayEq * data.settings.WorkHoursPerDay;

            if (segment.IsWorkSpan)
            {
                TotalWorkHours = data.settings.WorkHoursPerDay;

                if (Faculty40)
                {
                    TotalWorkHours = data.settings.FacultyMaxHoursPerDay;
                    getWSLog_ForFaculty();
                }
                else
                {
                    getWSLog();
                }
            }
            else
            {
                TotalWorkHours = (segment.TimeOut - segment.TimeIn).TotalHours;
                if (segment.SegmentType == (int)SegmentType.Overtime) //deduct ot breaks if more than 4 hours
                {
                    if (TotalWorkHours > 4)
                    {
                        TotalWorkHours -= segment.OTBreaks;
                    }
                }

                if (TotalWorkHours > workHoursLimit) TotalWorkHours = workHoursLimit;

                getLog();
            }

            IsAbsent = Log.Computed_TimeIn == default(DateTime) || Log.Computed_TimeOut == default(DateTime);

            double t = (Log.Computed_TimeOut - Log.Computed_TimeIn).TotalHours;

            //double breakTime = Math.Floor(t / 5);

            TotalHoursWorked = IsAbsent ? 0 : t - (segment.IsWorkSpan && t > 4 ? 1 : 0); //deduct 1 hour for break if more than 4 hours
            //TotalHoursWorked = IsAbsent ? 0 : t - (segment.IsWorkSpan && breakTime > 0 ? breakTime : 0); //deduct 1 hour for break if more than 4 hours

            if (segment.SegmentType == (int)SegmentType.Overtime)   //deduct ot breaks if more than 4 hours
            {
                if (TotalHoursWorked > 4)
                {
                    TotalHoursWorked -= segment.OTBreaks;
                }
            }

            if (TotalHoursWorked > workHoursLimit) TotalHoursWorked = workHoursLimit;

            TotalHoursWorked_DayEq = getTotalHoursWorked_DayEq();

            if (segment.WorkDayEq <= 0)
            {
                TotalWorkHours = 0;
                TotalHoursWorked = 0;
                Log.minsLate = 0;
                Log.minsUndertime = 0;
            }

            if (!segment.MustTimeIn && !segment.MustTimeOut && prevIsAllAbsent == true && Log.autoLogin && Log.autoLogout && segment.SegmentType != (int)SegmentType.Overtime)
            {
                IsAbsent = true;
                TotalHoursWorked = 0;
                TotalHoursWorked_DayEq = 0;
            }

            if (segment.SegmentType == (int)SegmentType.Overtime)
            {
                Log.minsLate = 0;
                Log.minsUndertime = 0;
            }

            getNP();
        }

        private void getNP()
        {
            NP_Start = coreLib.Procs.getDate(d, data.settings.NightPremium_Start);
            NP_Duration = data.settings.NightPremium_Duration;
            NP_End = NP_Start.AddHours(NP_Duration);

            if (!IsAbsent && !segment.IsWorkSpan)
            {
                if (Log.Computed_TimeIn <= NP_Start)
                {
                    if (Log.Computed_TimeOut >= NP_End)
                    {
                        NP = NP_Duration;
                    }
                    else if (Log.Computed_TimeOut > NP_Start)
                    {
                        NP = (Log.Computed_TimeOut - NP_Start).TotalHours;
                    }
                }
                else if (Log.Computed_TimeIn < NP_End)
                {
                    if (Log.Computed_TimeOut >= NP_End)
                    {
                        NP = (NP_End - Log.Computed_TimeIn).TotalHours;
                    }
                    else
                    {
                        NP = (Log.Computed_TimeOut - Log.Computed_TimeIn).TotalHours;
                    }
                }
            }
        }

        private void getWSLog_ForFaculty()
        {
            string[] deviceReferences = (segment.DeviceIds ?? "").Split(',').Where(x => x != "").ToArray();
            bool hasDeviceRef = deviceReferences.Length > 0;

            //get time in
            tblEmployee_TimeLog tlIn = data.tblEmployee_TimeLogs
                                        .Where(x =>
                                            x.TimeLog >= d.Date && x.TimeLog <= d.Date.AddDays(1).AddSeconds(-1) &&
                                            x.IsTimeIn &&
                                            x.TimeLog > lastLog &&
                                            (!hasDeviceRef || deviceReferences.Contains(x.DeviceReference))
                                            )
                                        .OrderBy(x => x.TimeLog)
                                        .ThenBy(x => x.LogId)
                                        .FirstOrDefault();

            if (tlIn != null)
            {
                Log.Actual_TimeIn = tlIn.TimeLog;
                Log.Computed_TimeIn = tlIn.TimeLog;
                Log.minsLate = 0;

                lastLog = tlIn.TimeLog;
            }

            //get time out
            tblEmployee_TimeLog tlOut = data.tblEmployee_TimeLogs
                                        .Where(x =>
                                            x.TimeLog >= d.Date && x.TimeLog <= d.Date.AddDays(1).AddSeconds(-1) &&
                                            x.IsTimeOut &&
                                            x.TimeLog > lastLog &&
                                            (!hasDeviceRef || deviceReferences.Contains(x.DeviceReference))
                                        )
                                        .OrderBy(x => x.TimeLog)
                                        .ThenBy(x => x.LogId)
                                        .LastOrDefault();


            if (tlOut != null)
            {
                Log.Actual_TimeOut = tlOut.TimeLog;
                Log.Computed_TimeOut = tlOut.TimeLog;
                Log.minsFlexiTime = 0;

                lastLog = tlOut.TimeLog;
            }
        }

        private void getWSLog()
        {
            string[] deviceReferences = (segment.DeviceIds ?? "").Split(',').Where(x => x != "").ToArray();
            bool hasDeviceRef = deviceReferences.Length > 0;

            DateTime timeOut = d.Date;

            //get time in
            tblEmployee_TimeLog tlIn = data.tblEmployee_TimeLogs
                                        .Where(x =>
                                            x.TimeLog >= d.Date && x.TimeLog <= d.Date.AddDays(1).AddSeconds(-1) &&
                                            x.IsTimeIn &&
                                            x.TimeLog > lastLog &&
                                            (!hasDeviceRef || deviceReferences.Contains(x.DeviceReference))
                                            )
                                        .OrderBy(x => x.TimeLog)
                                        .ThenBy(x => x.LogId)
                                        .FirstOrDefault();

            if (tlIn != null)
            {
                Log.Actual_TimeIn = tlIn.TimeLog;
                Log.Computed_TimeIn = tlIn.TimeLog;
                Log.minsLate = 0;

                lastLog = tlIn.TimeLog;

                timeOut = tlIn.TimeLog.AddHours(data.settings.WorkHoursPerDay + 1); // add 1 hour for break time
            }

            //get time out
            tblEmployee_TimeLog tlOut = data.tblEmployee_TimeLogs
                                        .Where(x =>
                                            x.TimeLog >= d.Date && x.TimeLog <= d.Date.AddDays(1).AddSeconds(-1) &&
                                            x.IsTimeOut &&
                                            x.TimeLog > lastLog &&
                                            (!hasDeviceRef || deviceReferences.Contains(x.DeviceReference))
                                        )
                                        .OrderBy(x => x.TimeLog)
                                        .ThenBy(x => x.LogId)
                                        .LastOrDefault();


            if (tlOut != null)
            {
                Log.Actual_TimeOut = tlOut.TimeLog;

                TimeSpan diff = timeOut - Log.Actual_TimeOut;
                int minsUndertime = (int)diff.TotalMinutes;
                if (minsUndertime < 0) minsUndertime = 0;

                if (minsUndertime >= 1)
                {
                    Log.minsUndertime = minsUndertime;
                    Log.Computed_TimeOut = Log.Actual_TimeOut;
                }
                else
                {
                    Log.Computed_TimeOut = timeOut;
                    Log.minsFlexiTime = Math.Floor((tlOut.TimeLog - timeOut).TotalMinutes);
                }

                lastLog = tlOut.TimeLog;
            }
        }

        private void getLog()
        {
            string[] deviceReferences = (segment.DeviceIds ?? "").Split(',').Where(x => x != "").ToArray();
            bool hasDeviceRef = deviceReferences.Length > 0;


            //get time in
            tblEmployee_TimeLog tlIn = data.tblEmployee_TimeLogs
                                        .Where(x =>
                                            x.TimeLog >= segment.TimeInFrom && x.TimeLog <= segment.TimeInTo &&
                                            x.IsTimeIn &&
                                            x.TimeLog > lastLog &&
                                            (!hasDeviceRef || deviceReferences.Contains(x.DeviceReference))
                                            )
                                        .OrderBy(x => x.TimeLog)
                                        .ThenBy(x => x.LogId)
                                        .FirstOrDefault();

            if (tlIn != null)
            {
                Log.Actual_TimeIn = tlIn.TimeLog;

                TimeSpan diff = tlIn.TimeLog - segment.TimeIn;
                int minsLate = (int)diff.TotalMinutes;
                if (minsLate < 0) minsLate = 0;

                if (minsLate >= 1)
                {
                    Log.minsLate = minsLate;
                    Log.Computed_TimeIn = segment.TimeIn.AddMinutes(minsLate);
                }
                else
                {
                    Log.Computed_TimeIn = segment.TimeIn;
                }

                if (!segment.SkipLastLog && segment.SegmentType_Desc != "Overtime")
                {
                    lastLog = tlIn.TimeLog;
                }
            }
            else if (!segment.MustTimeIn)
            {
                Log.Computed_TimeIn = segment.TimeIn;
                Log.autoLogin = true;
            }

            //get time out
            tblEmployee_TimeLog tlOut = data.tblEmployee_TimeLogs
                                        .Where(x =>
                                            x.TimeLog >= segment.TimeOutFrom && x.TimeLog <= segment.TimeOutTo &&
                                            x.IsTimeOut &&
                                            x.TimeLog > lastLog &&
                                            (!hasDeviceRef || deviceReferences.Contains(x.DeviceReference))
                                        )
                                        .OrderBy(x => x.TimeLog)
                                        .ThenBy(x => x.LogId)
                                        .LastOrDefault();

            if (tlOut != null)
            {
                Log.Actual_TimeOut = tlOut.TimeLog;

                TimeSpan diff = segment.TimeOut - tlOut.TimeLog;
                int minsUndertime = (int)diff.TotalMinutes;
                if (minsUndertime < 0) minsUndertime = 0;

                if (minsUndertime >= 1)
                {
                    Log.minsUndertime = minsUndertime;
                    Log.Computed_TimeOut = segment.TimeOut.AddMinutes(-1 * minsUndertime);
                }
                else
                {
                    Log.Computed_TimeOut = segment.TimeOut;
                    Log.minsFlexiTime = Math.Floor((tlOut.TimeLog - segment.TimeOut).TotalMinutes);
                }

                if (!segment.SkipLastLog && segment.MustTimeOut && segment.SegmentType_Desc != "Overtime")
                {
                    lastLog = tlOut.TimeLog;
                }
            }
            else if (!segment.MustTimeOut)
            {
                Log.Computed_TimeOut = segment.TimeOut;
                Log.autoLogout = true;
            }
        }

        double getTotalHoursWorked_DayEq()
        {
            if (IsAbsent)
            {
                return 0;
            }
            else
            {
                double percentage = TotalHoursWorked / TotalWorkHours;

                return segment.WorkDayEq * percentage;
            }
        }
    }

    public class etLog
    {
        public tblEmployee_TimeLog tblEmployee_TimeLog;

        public DateTime Actual_TimeIn;
        public DateTime Actual_TimeOut;
        public DateTime Computed_TimeIn;
        public DateTime Computed_TimeOut;
        public int minsLate = 0;
        public int minsUndertime = 0;
        public bool autoLogin;
        public bool autoLogout;
        public double minsFlexiTime;

        public DateTime Display_TimeIn_Value
        {
            get
            {
                return autoLogin ? Computed_TimeIn : Actual_TimeIn;
            }
        }

        public DateTime Display_TimeOut_Value
        {
            get
            {
                return autoLogout ? Computed_TimeOut : Actual_TimeOut;
            }
        }
    }

    public class etBreak
    {
        public DateTime startTime;
        public DateTime endTime;

        public TimeSpan Duration()
        {
            return endTime - startTime;
        }

        public string Duration_Desc()
        {
            string ret = "";

            TimeSpan dur = Duration();
            if (dur.TotalMinutes < 60)
            {
                ret = string.Format("{0} minute{1}", (int)dur.TotalMinutes, dur.TotalMinutes > 1 ? "s" : "");
            }
            else
            {
                ret = string.Format("{0} hour{1}", dur.TotalHours.ToString("#0.#"), dur.TotalHours > 1 ? "s" : "");
            }

            return ret;
        }
    }

    public class faculty40_day
    {
        TimeSettingsModel settings;
        etDay day;

        public double HoursAbsent;
        public double TotalHoursWorked;
        public double TotalHoursWorkedActual;
        public double TotalHoursWorked_DayEq;
        public double WorkDaysCount;

        public bool IsSaturday;
        public bool IsSunday;
        public bool IsHoliday;

        public faculty40_day(TimeSettingsModel settings, etDay day)
        {
            this.settings = settings;
            this.day = day;

            IsSaturday = day.d.DayOfWeek == DayOfWeek.Saturday;
            IsSunday = day.d.DayOfWeek == DayOfWeek.Sunday;

            IsHoliday = day.IsHoliday;

            TotalHoursWorked = day.TotalHoursWorked;
            TotalHoursWorkedActual = day.TotalHoursWorked > settings.FacultyMaxHoursPerDay ? settings.FacultyMaxHoursPerDay : day.TotalHoursWorked;

            TotalHoursWorked_DayEq = day.TotalHoursWorked_DayEq;

            if (IsSaturday)
            {
                HoursAbsent = 0;
            }
            else if (IsSunday)
            {
                HoursAbsent = 0;
            }
            else if (IsHoliday)
            {
                HoursAbsent = 0;
            }
            else if (day.IsAbsent)
            {
                HoursAbsent = day.TotalWorkHours;
            }
            else
            {
                HoursAbsent = TotalHoursWorked < settings.Faculty40_MinHoursPerDay ? settings.WorkHoursPerDay - TotalHoursWorked : 0;
            }

            WorkDaysCount = day.IsWorkDay && (!day.IsAbsent) && !IsSaturday && !IsSunday ? 1 : 0;
            //WorkDaysCount = day.IsWorkDay && (!day.IsAbsent) ? 1 : 0;

            if (day.IsOnTravel)
            {
                TotalHoursWorked = settings.WorkHoursPerDay;
                TotalHoursWorked_DayEq = 1;
                HoursAbsent = 0;
            }
            else if (day.IsOnLeave)
            {
                TotalHoursWorked = settings.WorkHoursPerDay;
                TotalHoursWorked_DayEq = 1;
                HoursAbsent = 0;
            }
        }
    }

    public class faculty40_week
    {
        TimeSettingsModel settings;
        public int weekNo;
        public DateTime startDate;
        public DateTime endDate;
        public List<faculty40_day> days;

        public double TotalDays;
        public double TotalWorkHours;
        public double TotalMinWorkHours;
        public double TotalHoursAbsent;
        public double TotalHoursWorked;
        public double TotalHoursWorked_DayEq;
        public double TotalHoursOverload;

        public double TotalWorkDaysCount;

        public faculty40_week(TimeSettingsModel settings, int weekNo, DateTime startDate, DateTime endDate, List<etDay> etDays, double TotalOverloadHoursPerWeek)
        {
            this.settings = settings;
            this.weekNo = weekNo;
            this.startDate = startDate;
            this.endDate = endDate;

            days = new List<faculty40_day>();

            foreach (etDay etDay in etDays)
            {
                if (etDay.IsWorkDay)
                {
                    days.Add(new faculty40_day(settings, etDay));
                }
            }

            TotalWorkHours = days.Where(x => !x.IsSaturday && !x.IsSunday && !x.IsHoliday).Count() * settings.WorkHoursPerDay;

            double dayTotalHoursWorked = days.Sum(x => x.TotalHoursWorked);

            //double totalHoursWorked_1 = dayTotalHoursWorked > TotalWorkHours ? TotalWorkHours : dayTotalHoursWorked;

            double totalHoursWorked_1 = dayTotalHoursWorked > TotalWorkHours ? dayTotalHoursWorked : dayTotalHoursWorked;

            double dayUT = days.Sum(x => x.HoursAbsent);
            double _hrsDiff = totalHoursWorked_1 < TotalWorkHours ? TotalWorkHours - totalHoursWorked_1 : 0;
            TotalHoursAbsent = dayUT > _hrsDiff ? dayUT : _hrsDiff;

            double totalHoursWorked_2 = TotalWorkHours - TotalHoursAbsent;
            //TotalHoursWorked = totalHoursWorked_1 < totalHoursWorked_2 ? totalHoursWorked_1 : totalHoursWorked_2;
            TotalHoursWorked = totalHoursWorked_1;

            TotalHoursWorked_DayEq = TotalHoursWorked / settings.WorkHoursPerDay;

            double WeekOverload = dayTotalHoursWorked > TotalWorkHours ? dayTotalHoursWorked - TotalWorkHours : 0;

            TotalHoursOverload = WeekOverload > TotalOverloadHoursPerWeek ? TotalOverloadHoursPerWeek : WeekOverload;

            TotalWorkDaysCount = days.Sum(x => x.WorkDaysCount);
        }
    }


    public class faculty40
    {
        etPeriod period;
        public List<faculty40_week> weeks;

        public double TotalWorkHours;
        public double TotalDaysAbsent;
        public double TotalHoursWorked;
        public double TotalHoursWorked_DayEq;
        public double TotalHoursOverload;
        public double TotalWorkDaysCount;

        public faculty40(etPeriod period)
        {
            this.period = period;

            getWeeks(period.WithOverload ? period.WeeklyOverload : 0);

            TotalWorkHours = weeks.Sum(x => x.TotalWorkHours);
            TotalHoursWorked = weeks.Sum(x => x.TotalHoursWorked);
            TotalDaysAbsent = weeks.Sum(x => x.TotalHoursAbsent) / period.data.WorkHoursPerDay;
            TotalHoursWorked_DayEq = weeks.Sum(x => x.TotalHoursWorked_DayEq);

            TotalHoursOverload = weeks.Sum(x => x.TotalHoursOverload);

            TotalWorkDaysCount = weeks.Sum(x => x.TotalWorkDaysCount);
        }

        private void getWeeks(double TotalOverloadHoursPerWeek)
        {
            weeks = new List<faculty40_week>();

            int weekNo = 1;

            DateTime d = period.startDate;
            while (d <= period.endDate)
            {
                DateTime weekStart = d;
                DateTime weekEnd = d.AddDays(6 - (int)d.DayOfWeek);

                if (weekEnd > period.endDate) weekEnd = period.endDate;

                List<etDay> days = period.Days.Where(x => x.d >= weekStart && x.d <= weekEnd).ToList();

                weeks.Add(new faculty40_week(period.data.settings, weekNo, d, weekEnd, days, TotalOverloadHoursPerWeek));

                weekNo++;
                d = weekEnd.AddDays(2);
            }
        }
    }
}