using coreLib.Enums;
using coreLib.Objects;
using System;

namespace Module.Time
{
    public class deviceStatus
    {
        public bool IsConnected { get; set; }
        public Exception e { get; set; }
    }

    public class etPeriodModel : PeriodModel
    {
        public etPeriod Period { get; set; }

        public etPeriodModel()
            : base(PeriodModelInitType.ThisMonth)
        { }

        public etPeriodModel(etData data, DateTime startDate, DateTime endDate, int? segmentType, bool excludeOT = false)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            
            init(data, segmentType, excludeOT);
        }

        public etPeriodModel(int employeeId, string startDate, string endDate, int? segmentType, bool excludeOT = false)
            : base(startDate, endDate)
        {
            Period = new etPeriod(employeeId, StartDate, EndDate, segmentType, excludeOT);
        }

        public etPeriodModel(int employeeId, DateTime startDate, DateTime endDate, int? segmentType, bool excludeOT = false)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            
            Period = new etPeriod(employeeId, startDate, endDate, segmentType, excludeOT);
        }

        public void init(etData data, int? segmentType, bool excludeOT)
        {
            Period = new etPeriod(StartDate, EndDate, data, segmentType, excludeOT);
        }
    }
}