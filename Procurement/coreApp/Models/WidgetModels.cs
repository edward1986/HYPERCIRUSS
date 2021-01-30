
using coreLib.Objects.GraphModels.JqueryFlot;

namespace coreApp.Models
{
    public class EmployeeAttendanceStatisticsModel
    {
        public GraphModel WorkHours;
        public PieModel AbsencesTardiness;
    }

    public class DailyAttendanceMonitorModel
    {
        public PieModel AbsencesTardiness;
    }

    public class ApplicationsModel
    {
        public int Leave { get; set; }
        public int OT { get; set; }
        public int Travel { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Leave == 0 && OT == 0 && Travel == 0;
            }
        }

        public ApplicationsModel()
        {
            Leave = 0;
            OT = 0;
            Travel = 0;
        }
    }
}
