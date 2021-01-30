using System;

namespace Module.Core
{
    public class KioskPostData
    {
        public int terminalId { get; set; }
        public int employeeId { get; set; }
        public DateTime timeLog { get; set; }
        public int mode { get; set; }
        public int deviceType { get; set; }
    }
}
