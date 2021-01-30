using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.DB.Interfaces
{
    public interface ICacheTables
    {
        List<tblSettings_BaseModule> BaseSettings { get; set; }
        List<_Country> Countries { get; set; }
        List<tblOffice> Offices { get; set; }
        List<tblDepartment> Departments { get; set; }
        List<tblPosition> Positions { get; set; }
       

    }

    public interface ICache
    {
        DateTime cacheDate { get; set; }

    }

    public interface ISegment
    {
        int GlobalId { get; set; }
        string GlobalAppliesTo { get; set; }

        int ScheduleId { get; set; }
        string Description { get; set; }
        DateTime TimeInFrom { get; set; }
        DateTime TimeInTo { get; set; }

        [Display(Name = "Time-In")]
        DateTime TimeIn { get; set; }

        DateTime TimeOutFrom { get; set; }
        DateTime TimeOutTo { get; set; }

        [Display(Name = "Time-Out")]
        DateTime TimeOut { get; set; }

        bool MustTimeIn { get; set; }
        bool MustTimeOut { get; set; }
        bool TimeInFrom_IsPrev { get; set; }
        bool TimeInTo_IsNext { get; set; }
        bool TimeOut_IsNext { get; set; }
        bool TimeOutFrom_IsNext { get; set; }
        bool TimeOutTo_IsNext { get; set; }

        bool Days_Sun { get; set; }
        bool Days_Mon { get; set; }
        bool Days_Tue { get; set; }
        bool Days_Wed { get; set; }
        bool Days_Thu { get; set; }
        bool Days_Fri { get; set; }
        bool Days_Sat { get; set; }

        [Display(Name = "Work-Day Equivalent")]
        double WorkDayEq { get; set; }
        int SegmentType { get; set; }
        bool SkipLastLog { get; set; }
        bool Enabled { get; set; }
        string Validity { get; set; }
        string Exception { get; set; }

        string DeviceIds { get; set; }
    }

    
}
