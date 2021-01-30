using Module.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSync
{
    public static class Logger
    {
        static List<string> locked;
        public static string ServicePath, ServiceLogsPath, TimeLogsPath;
        
        public static void Init()
        {
            locked = new List<string>();

            //set paths                        
            ServicePath = FixedSettings.ServicePath;
            ServiceLogsPath = Path.Combine(ServicePath, "Service Logs");
            TimeLogsPath = Path.Combine(ServicePath, "Time Logs");

            if (!Directory.Exists(ServicePath)) Directory.CreateDirectory(ServicePath);
            if (!Directory.Exists(ServiceLogsPath)) Directory.CreateDirectory(ServiceLogsPath);
            if (!Directory.Exists(TimeLogsPath)) Directory.CreateDirectory(TimeLogsPath);
        }

        public static void WriteLog(string Message, DateTime? LogDate = null)
        {
            if (LogDate == null) LogDate = DateTime.Now;

            string[] lines = new string[]
            {
                Message
            };

            WriteLog(lines, LogDate);       
        }

        public static void WriteLog(string[] Messages, DateTime? LogDate = null)
        {
            if (LogDate == null) LogDate = DateTime.Now;

            List<string> lines = new List<string>();
            foreach(string msg in Messages)
            {
                lines.Add(string.Format("{0}\t{1}", LogDate.Value.ToString("hh:mm tt"), msg));
            }

            string fileName = Path.Combine(ServiceLogsPath, LogDate.Value.ToString("yyyy-MM-dd") + ".txt");

            while (locked.Any(x => x == fileName)) { }

            locked.Add(fileName);
            File.AppendAllLines(fileName, lines);
            locked.Remove(fileName);
        }

        public static void WriteTimeLog(tblDeviceLog log, tblDevice device)
        {
            DateTime LogDate = DateTime.Now;

            string[] lines = new string[]
            {
                string.Format("{0}\t{1}\t{2}\t{3}\t{4}", log.LogDate.ToString("hh:mm tt"), log.Mode_Desc, log.IdNo, device.DeviceId, device.DeviceName)
            };

            string fileName = Path.Combine(TimeLogsPath, LogDate.ToString("yyyy-MM-dd") + ".txt");

            while (locked.Any(x => x == fileName)) { }

            locked.Add(fileName);
            File.AppendAllLines(fileName, lines);
            locked.Remove(fileName);
        }

        public static string[] GetServiceLog(DateTime dt)
        {
            string fileName = Path.Combine(ServiceLogsPath, dt.ToString("yyyy-MM-dd") + ".txt");
            return File.ReadAllLines(fileName);
        }
    }
}
