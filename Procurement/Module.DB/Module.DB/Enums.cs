
namespace Module.DB.Enums
{
    public enum CareerEvent
    {
        Hired = 0,
        ReAssigned = 1,
        Promoted = 2,
        Demoted = 3,
        SalaryIncrease = 4,
        Terminated = 5,
        Resigned = 6,
        Others = 7
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    };

    public enum CivilStatus
    {
        Single = 0,
        Married = 1,
        Widow = 2,
        Separated = 3,
        Divorced = 4,
        Widower = 5
    };

    public enum BloodType
    {
        A = 0,
        B = 1,
        AB = 2,
        O = 3
    };

    public enum LeaveTypeCategory
    {
        Others = 0,
        VL = 1,
        SL = 2,
        Maternity = 3
    }

    public enum LeaveMode
    {
        Monthly = 0,
        Yearly = 1
    }

    public enum RuleScopeType
    {
        Permanent = 1,
        Contractual = 2,
        JobOrder = 3,
        Seasonal = 4,
        Casual = 5,
        Temporary = 6,
        Faculty = 7,
        WithDesignation = 8,
        Male = 9,
        Female = 10
    }

    public enum LeaveEntryType
    {
        Auto = 0,
        Manual = 1
    }

    public enum ApplicationStatus
    {
        Draft = 0,
        Returned = 1,
        Submitted = 2,
        ReSubmitted = 3,
        Denied = 4,
        Approved = 5
    }

    public enum HolidayType
    {
        Regular = 0,
        Special = 1
    }

    public enum DurationType
    {
        Days = 0,
        Weeks = 1,
        Months = 2,
        Years = 3
    }

    public enum SegmentType
    {
        Regular = 0,
        Overtime = 1,
        Restday = 2, //Deprecated
        Workspan = 3
    }

    public enum DeviceLogMode
    {
        In = 0,
        Out = 1,
        Break = 2,
        Resume = 3,
        OTIn = 4,
        OTOut = 5,
        Other = 6
    }

    public enum TimeLogEntryType
    {
        Auto = 0,
        Manual = 1,
        GeoLocation = 2
    }

    public enum DeviceType
    {
        FP_FACE = 0
    }

    public enum TerminalDeviceType
    {
        USB = 0,
        LAN = 1
    }

    public enum EmploymentType
    {
        Permanent = 0,
        Contractual = 1,
        JobOrder = 2,
        Seasonal = 3,
        Casual = 4,
        Temporary = 5
    }

    public enum OverloadTypes
    {
        Doctoral = 0,
        Masteral = 1,
        Undergrad = 2,
        Special_Class = 3,
        Petepetition_Class = 4,
    }

}
