using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zks_console
{
    class Program
    {       
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                if (path.StartsWith("file:///"))
                {
                    path = path.Substring(8);
                }

                System.Windows.Forms.Clipboard.SetText(path);
                
                Console.Write(path);
                Console.ReadKey();
            }
            else
            {
                List<string> ret = new List<string>();
                Form1 f = new Form1();

                int n = int.Parse(args[0]);
                string ip = args[1];
                int port = int.Parse(args[2]);
                int pw = int.Parse(args[3]);
                int deviceId = int.Parse(args[4]);

                DateTime now = DateTime.Now;

                DateTime startDate = new DateTime(now.Year, now.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                if (args.Length > 5)
                {
                    startDate = DateTime.Parse(args[5]);
                    endDate = DateTime.Parse(args[6]);
                }

                TmpUploadResult res = f.GetLogs(n, ip, port, pw, startDate, endDate);

                saveLogs(res, deviceId);
                relateLogs(deviceId, startDate, endDate);

                f.Dispose();
                f = null;
            }
        }

        static List<tblEmployee_TimeLog> relateLogs(int deviceId, DateTime startDate, DateTime endDate)
        {
            using (dalDataContext context = new dalDataContext())
            {
                endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                List<tblEmployee_TimeLog> logs = context.vwDeviceLogs
                                                    .Where(x => x.DeviceId == deviceId && x.LogDate >= startDate && x.LogDate <= endDate)
                                                    .ToList()
                                                    .Where(x =>
                                                        !context.tblEmployee_TimeLogs.Where(y => y.DeviceLogId == x.Id).Any()
                                                        )
                                                    .Select(x => new tblEmployee_TimeLog
                                                    {
                                                        EmployeeId = x.EmployeeId,
                                                        EntryType = (int)TimeLogEntryType.Auto,
                                                        TimeLog = x.LogDate,
                                                        Mode = x.Mode,
                                                        DeviceLogId = x.Id
                                                    })
                                                    .ToList();

                context.tblEmployee_TimeLogs.InsertAllOnSubmit(logs);
                context.SubmitChanges();

                return logs;
            }
        }

        static List<tblDeviceLog> saveLogs(TmpUploadResult res, int deviceId)
        {
            using (dalDataContext context = new dalDataContext())
            {
                List<tblDeviceLog> logs = new List<tblDeviceLog>();
                List<tblDeviceLog> dbLogs = context.tblDeviceLogs.Where(x => x.DeviceId == deviceId).ToList();

                var tmp = res.LogItems.Where(x => x.Exception == null).ToList();
                foreach (clLogItem logItem in tmp)
                {
                    int mode = getMode(logItem.Mode);

                    logs.Add(new tblDeviceLog
                    {
                        DeviceId = deviceId,
                        IdNo = int.Parse(logItem.IdNo),
                        LogDate = DateTime.Parse(logItem.LogDate),
                        Mode = mode == 1 || mode == 2 || mode == 5 ? 1 : 0
                    });
                }

                List<tblDeviceLog> newLogs = logs.Where(x => !dbLogs.Where(y => y.IdNo == x.IdNo && y.LogDate == x.LogDate && y.Mode == x.Mode).Any()).ToList();

                context.tblDeviceLogs.InsertAllOnSubmit(newLogs);
                context.SubmitChanges();

                return newLogs;
            }
        }

        static int getMode(string mode)
        {
            int ret;

            if (mode.ToLower().Trim() == "in" || mode.ToLower().Trim() == "0")
            {
                ret = (int)DeviceLogMode.In;
            }
            else if (mode.ToLower().Trim() == "out" || mode.ToLower().Trim() == "1")
            {
                ret = (int)DeviceLogMode.Out;
            }
            else if (mode.ToLower().Trim() == "break" || mode.ToLower().Trim() == "2")
            {
                ret = (int)DeviceLogMode.Break;
            }
            else if (mode.ToLower().Trim() == "resume" || mode.ToLower().Trim() == "3")
            {
                ret = (int)DeviceLogMode.Resume;
            }
            else if (mode.ToLower().Trim() == "ot" || mode.ToLower().Trim() == "4")
            {
                ret = (int)DeviceLogMode.OTIn;
            }
            else if (mode.ToLower().Trim() == "done" || mode.ToLower().Trim() == "5")
            {
                ret = (int)DeviceLogMode.OTOut;
            }
            else
            {
                ret = (int)DeviceLogMode.Other;
            }

            return ret;
        }

        enum DeviceLogMode
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
            Manual = 1
        }
    }
}
