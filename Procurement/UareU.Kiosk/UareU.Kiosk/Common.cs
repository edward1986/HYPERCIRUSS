using Module.DB;
using Module.DB.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UareU.Common;

namespace UareU.Kiosk
{
    
    public static class Common
    {
        public const string DATASOURCE_FILENAME = "data.dat";

        public static bool FoceExit = false;

        public static string GetLogoPath()
        {
            string currentAppPath = Application.StartupPath;
            return Path.Combine(currentAppPath, "skyhr-client-logo.png");
        }

        public static string GetTimeLogsPath()
        {
            return string.IsNullOrEmpty(FixedSettings.TimeLogsPath) ? Application.StartupPath : FixedSettings.TimeLogsPath;
        }

        public static string GetOfflineDataPath()
        {
            return string.IsNullOrEmpty(FixedSettings.OfflineDataPath) ? Application.StartupPath : FixedSettings.OfflineDataPath;
        }

        public static string GetTimeUrl()
        {
            return FixedSettings.AppUrl + "/Kiosk/GetTime";
        }

        public static string GetDataUrl()
        {
            return FixedSettings.AppUrl + "/Kiosk/GetData";
        }

        public static string GetPostDataUrl()
        {
            return FixedSettings.AppUrl + "/Kiosk/PostData";
        }

        public static string GetPhotosUrl()
        {
            return FixedSettings.AppUrl + "/Kiosk/GetEmployeePhoto";
        }
    }

    public static class FixedSettings
    {
        public static int TerminalId { get; set; }
        public static string AppUrl { get; set; }
        public static Image Logo { get; set; }

        public static string AgencyName { get; set; }
        public static string AgencyCode { get; set; }
        public static string AgencyAddress { get; set; }

        public static int ServerTimeQueryInterval { get; set; }
        public static int DataDownloadInterval { get; set; }
        public static int PostOfflineLogsInterval { get; set; }

        public static bool DataDownloadEnabled { get; set; }

        public static bool LongTimeFormat { get; set; }

        public static string TimeLogsPath { get; set; }

        public static string OfflineDataPath { get; set; }
        public static string OfflineMsg { get; set; }

        public static string DeviceIP { get; set; }
        public static int DevicePort { get; set; }

        public static bool Fullview { get; set; }
        public static bool UseServerTime { get; set; }
        
        public static int MaxLogDispayTime { get; set; }

        public static int DeviceReconnectInterval { get; set; }
        
        public static bool EnableUSBDevice { get; set; }
        public static bool EnableLANDevice { get; set; }

        public static string getBaseModuleSetting(string setting, string category)
        {
            return Offline.Data.BaseSettings.Where(x => x.Setting == setting && x.Category == category).Single().SettingValue;
        }

        public static void Init()
        {            

            AppUrl = Properties.Settings.Default.AppUrl;

            string logoPath = Common.GetLogoPath();
            
            if (File.Exists(logoPath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Image tmp = Image.FromFile(logoPath);
                    tmp.Save(ms, ImageFormat.Png);

                    Logo = Image.FromStream(ms);
                    tmp.Dispose();
                }
            }
            else
            {
                Logo = null;
            }

            TerminalId = Properties.Settings.Default.TerminalId;

            ServerTimeQueryInterval = Properties.Settings.Default.ServerTimeQueryInterval;
            DataDownloadInterval = Properties.Settings.Default.DataDownloadInterval;
            PostOfflineLogsInterval = Properties.Settings.Default.PostOfflineLogsInterval;

            DataDownloadEnabled = Properties.Settings.Default.DataDownloadEnabled;

            LongTimeFormat = Properties.Settings.Default.LongTimeFormat;

            TimeLogsPath = Properties.Settings.Default.TimeLogsPath;
            OfflineDataPath = Properties.Settings.Default.OfflineDataPath;
            OfflineMsg = Properties.Settings.Default.OfflineMessage;
            
            Fullview = Properties.Settings.Default.Fullview;
            UseServerTime = Properties.Settings.Default.UseServerTime;

            MaxLogDispayTime = Properties.Settings.Default.MaxLogDispayTime;
            DeviceReconnectInterval = Properties.Settings.Default.DeviceReconnectInterval;

            EnableUSBDevice = Properties.Settings.Default.EnableUSBDevice;
            EnableLANDevice = Properties.Settings.Default.EnableLANDevice;
        }

        public static void InitDB(frmMain form)
        {
            AgencyName = getBaseModuleSetting("AgencyName", "General");
            AgencyCode = getBaseModuleSetting("AgencyCode", "General");
            AgencyAddress = getBaseModuleSetting("AgencyAddress", "General");

            tblDevice dev = Offline.Data.Devices.Where(x => x.TerminalId == TerminalId).SingleOrDefault();
            if (dev == null)
            {
                DeviceIP = "";
                DevicePort = 0;
            }
            else
            {
                DeviceIP = dev.IP;
                DevicePort = dev.Port.Value;
            }
        }
    }

    public class EmployeeModel
    {
        public int employeeId;
        public string employee;
        public string office;
        public string position;
        public Image photo;
        public DateTime timelog;
        public TimeLogMode mode;
        public TerminalDeviceType devicetype;
        public bool localtime;
    }

    public enum TimeLogMode
    {
        In = 0,
        Out = 1
    }

    public enum TimeLogEntryType
    {
        Auto = 0,
        Manual = 1
    }
}
