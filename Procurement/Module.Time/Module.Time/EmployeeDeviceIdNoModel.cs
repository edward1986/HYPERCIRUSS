using Module.DB;

namespace Module.Time
{
    public class EmployeeDeviceIdNoModel
    {
        public int employeeId { get; set; }
        public tblDevice device { get; set; }
        public tblEmployee_DeviceIdNo deviceIdNo { get; set; }

        public int? IdNo
        {
            get
            {
                if (deviceIdNo == null)
                {
                    return null;
                }
                else
                {
                    return deviceIdNo.IdNo;
                }
            }
        }
    }
}