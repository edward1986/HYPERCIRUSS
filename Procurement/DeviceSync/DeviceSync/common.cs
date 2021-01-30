using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSync
{
    public static class Common
    {
        public static DateTime? LastDownloadDate()
        {
            DateTime? ret = null;
            
            DirectoryInfo di = new DirectoryInfo(Logger.ServiceLogsPath);
            foreach (string f in di.GetFiles().Select(x => x.Name).OrderByDescending(x => x))
            {
                DateTime dt = DateTime.Parse(f.Replace(".txt", ""));
                string[] data = Logger.GetServiceLog(dt);
                DateTime time = data
                                    .Where(x => x.Split('\t')[1].StartsWith("download data"))
                                    .Select(x => DateTime.Parse(x.Split('\t')[0]))
                                    .OrderByDescending(x => x)
                                    .FirstOrDefault();

                if (time != default(DateTime))
                {
                    string dtString = string.Format("{0} {1}", dt.ToString("MM/dd/yyyy"), time.ToString("h:mm tt"));
                    ret = DateTime.Parse(dtString);
                    break;
                }
            }

            return ret;
        }
    }

    public static class FixedSettings
    {
        public static string ServicePath { get; set; }        
        public static int RefreshInterval { get; set; }
        public static DateTime DownloadSchedule { get; set; }

        public static bool DownloadSchedule_IsDue(DateTime dt)
        {
            DateTime date = dt.Date.AddHours(DownloadSchedule.Hour).AddMinutes(DownloadSchedule.Minute).AddSeconds(DownloadSchedule.Second);
            return dt > date;
        }

        public static void Init()
        {
            ServicePath = Properties.Settings.Default.ServicePath;
            RefreshInterval = Properties.Settings.Default.RefreshInterval;
            DownloadSchedule = Properties.Settings.Default.DownloadSchedule;
        }
    }
}
