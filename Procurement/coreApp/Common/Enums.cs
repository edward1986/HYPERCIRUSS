using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace coreApp.Enums
{
        
    public enum PayrollType
    {
        Salary = 0,
        _13thMP = 1,
        Mid_Year = 2,
        Year_End = 3,
        Overtime = 4,
        ACA_PERA = 5,
        Overload = 6
    }

    public enum PSStatus
    {
        Draft = 0,
        Returned = 1,
        Submitted = 2,
        ReSubmitted = 3,
        Posted = 4
    }

    public enum PayMode
    {
        Monthly = 0,
        Semi_Monthly = 1,
        Daily = 2,
        Hourly = 3
    }
    public enum OverloadTypes
    {
        Doctoral = 0,
        Masteral = 1,
        Undergrad = 2,
        Special_Class = 3,
        Petepetition_Class = 4,
    }

public enum Contributions
    {
        GSIS = 0,
        SSS = 1,
        PH = 2,
        PAGIBIG = 3
    }

    public enum ContributionOptions
    {
        Auto = 0,
        None = 1,
        Full = 2,
        Half = 3
    }

    public enum UseRatesMode
    {
        UsePrimary = 0
        //UseSecondary = 1,
        //UseSecondaryForDeductionsOnly = 2
    }

    public enum BIRStatus
    {
        Z = 0,
        S = 1,
        S1 = 2,
        S2 = 3,
        S3 = 4,
        S4 = 5,
        ME = 6,
        ME1 = 7,
        ME2 = 8,
        ME3 = 9,
        ME4 = 10
    }
    
    
    public enum EducLevel
    {
        Elementary = 0,
        Secondary = 1,
        Vocational = 2,
        College = 3,
        Graduate_Studies = 4
    };

    public enum LDType
    {
        Managerial = 0,
        Supervisory = 1,
        Technical = 2
    };

    public enum StatusOfAppointment
    {
        Casual = 0,
        Contractual = 1,
        JobOrder = 2,
        Permanent = 3,
        Temporary = 4,
        Seasonal = 5
    }

    public enum CommonEntryType
    {
        Manual = 0,
        Auto = 1
    }
    
    public enum DeductionTypes
    {
        Deduction = 0,
        Contribution = 1
    }
    
    public enum DeductionScheduleTypes
    {
        First_Cutxoff = 0,
        Second_Cutxoff = 1,
        Every_Cutxoff = 2
    }


}