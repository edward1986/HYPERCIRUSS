using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Time
{
    public class EmployeeScheduleSegments
    {
        int scheduleId;
        OfflineDataSource offlineDS;
        List<tblGlobalSchedule_Segment> globalSegments;


        public List<tblEmployee_Schedule_Segment> tblEmployee_Schedule_Segments = new List<tblEmployee_Schedule_Segment>();

        public EmployeeScheduleSegments(int scheduleId)
        {
            this.scheduleId = scheduleId;
            this.offlineDS = null;

            init();
        }

        public EmployeeScheduleSegments(int scheduleId, ref OfflineDataSource offlineDS)
        {
            this.scheduleId = scheduleId;
            this.offlineDS = offlineDS;

            init();
        }

        public EmployeeScheduleSegments(int scheduleId, List<tblGlobalSchedule_Segment> globalSegments)
        {
            this.scheduleId = scheduleId;
            this.offlineDS = null;
            this.globalSegments = globalSegments;
            
            init();
        }

        public EmployeeScheduleSegments(int scheduleId, List<tblGlobalSchedule_Segment> globalSegments, ref OfflineDataSource offlineDS)
        {
            this.scheduleId = scheduleId;
            this.offlineDS = offlineDS;
            this.globalSegments = globalSegments;

            init();
        }

        public void init()
        {

            List<tblEmployee_Schedule_Segment> global, seg;

            tblEmployee employee;
            tblEmployee_Career career;
            tblEmployee_Schedule schedule;

            if (offlineDS == null)
            {
                using (dalDataContext context = new dalDataContext())
                {
                    schedule = context.tblEmployee_Schedules.Where(x => x.ScheduleId == scheduleId).Single();

                    if (globalSegments == null)
                    {
                        employee = context.tblEmployees.Where(x => x.EmployeeId == schedule.EmployeeId).Single();
                        career = new Module.DB.Procs.procs_tblEmployee(employee).LatestCareer();

                        globalSegments = context.tblGlobalSchedule_Segments.ToList().Where(x => x.AllowEmployee(career)).ToList();
                    }

                    global = globalSegments.Select(x => new tblEmployee_Schedule_Segment(x) { ScheduleId = scheduleId }).ToList();
                    seg = context.tblEmployee_Schedule_Segments.Where(x => x.ScheduleId == scheduleId).ToList();
                }
            }
            else
            {
                schedule = offlineDS.EmployeeSchedules.Where(x => x.ScheduleId == scheduleId).Single();

                if (globalSegments == null)
                {
                    employee = offlineDS.Employees.Where(x => x.EmployeeId == schedule.EmployeeId).Single();
                    career = new Module.DB.Procs.offline_procs_tblEmployee(employee, ref offlineDS).LatestCareer();

                    globalSegments = offlineDS.GlobalScheduleSegments.ToList().Where(x => x.AllowEmployee(career)).ToList();
                }

                global = globalSegments.Select(x => new tblEmployee_Schedule_Segment(x) { ScheduleId = scheduleId }).ToList();
                seg = offlineDS.EmployeeSchedule_Segments.Where(x => x.ScheduleId == scheduleId).ToList();                
            }

            tblEmployee_Schedule_Segments.AddRange(global);
            tblEmployee_Schedule_Segments.AddRange(seg);

            tblEmployee_Schedule_Segments.AddRange(
                Module.DB.Procs.Common.Global_CachedTables.SegmentTemplates
                    .Where(x => x.ScheduleTemplateId == schedule.ScheduleTemplateId)
                    .Select(x => new tblEmployee_Schedule_Segment(x, scheduleId))
                );

            foreach (tblEmployee_Schedule_Segment segment in tblEmployee_Schedule_Segments)
            {
                segment.setBaseDate(default(DateTime));
            }

            tblEmployee_Schedule_Segments = tblEmployee_Schedule_Segments.OrderBy(x => x.TimeIn).ToList();
        }
    }

}