using coreLib.Encryption;
using Module.DB;
using Module.DB.Enums;
using Module.Time;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UareU.Common;

namespace UareU.Kiosk
{
    public partial class frmMain : Form, IForm
    {

        KioskDataManager dataManager;
        TimeLogManager tlManager;

        etPeriod etp;

        public DateTime? _officialTime { get; set; }
        bool _isOnline = false;
        
        public bool IsOnline
        {
            get
            {
                return _isOnline;
            }
            set
            {
                _isOnline = value;
                setWarning();
            }
        }

        bool _posting = false;

        public frmMain()
        {
            InitializeComponent();

            dataManager = new KioskDataManager();
            tlManager = new TimeLogManager(this);

            uc_employee1.Clear();
            lblNoRecordFound.Visible = false;

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _officialTime = DateTime.Now;

            loadSettings();

            if (getData(true, true, false))
            {
                frmAuthentication frmAuth = new frmAuthentication(Offline.Data.BaseSettings);

                FixedSettings.InitDB(this);

                uc_header1.ShowHeader();
                uc_announcements1.Init(Offline.Data.BulletinBoard);

                uc_device_monitor1.SetDevice(uc_device_usb1, FixedSettings.DeviceReconnectInterval);
                uc_device_monitor2.SetDevice(uc_device_lan1, FixedSettings.DeviceReconnectInterval);

                uc_device_monitor1.EnableDevice(FixedSettings.EnableUSBDevice);
                uc_device_monitor2.EnableDevice(FixedSettings.EnableLANDevice);

                tmrDateTime.Enabled = true;
                tmrData.Enabled = FixedSettings.DataDownloadEnabled;
                tmrPostOfflineLogs.Enabled = true;
            }
            else
            {
                MessageBox.Show("No data found. Application will exit");

                Common.FoceExit = true;
                Application.Exit();
            }
            
        }

        private void loadSettings()
        {
            FixedSettings.Init();

            gbDeviceStatus.Text = string.Format("Device Status [Terminal Id: {0}]", FixedSettings.TerminalId);
            
            tmrDateTime.Interval = FixedSettings.ServerTimeQueryInterval * 60000;
            tmrData.Interval = FixedSettings.DataDownloadInterval * 60000;
            tmrPostOfflineLogs.Interval = FixedSettings.PostOfflineLogsInterval * 60000;

            uc_employee1.MaxLogDisplayTime = FixedSettings.MaxLogDispayTime;


            if (FixedSettings.Fullview)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.TopMost = true;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
            }

            getServerTimeAndCheckConnectivity();

            lblInfo.Text = FixedSettings.AppUrl;
            lblVersion.Text = "Attendance Kiosk version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public bool getData(bool showDialog, bool forceDownload, bool overWrite)
        {
            frmDownloadDS dl = new frmDownloadDS(this, forceDownload, overWrite, Common.GetOfflineDataPath(), Common.DATASOURCE_FILENAME, Common.GetDataUrl());

            if (!File.Exists(dl.FilePath) || overWrite)
            {
                if (showDialog)
                {
                    dl.ShowDialog();
                }
                else
                {
                    dl.Download(forceDownload, overWrite);
                }
            }

            dataManager.LoadDataFile();

            bool ok = Offline.Data != null;

            if (ok)
            {
                Module.DB.Procs.Common.Global_CachedTables = new cachedTables(Offline.Data);
            }

            return ok;
        }

        delegate void setWarningCallback();
        private void setWarning()
        {
            if (uc_greeting_and_time1.InvokeRequired)
            {
                setWarningCallback d = new setWarningCallback(setWarning);
                this.Invoke(d);
            }
            else
            {
                uc_greeting_and_time1.SetWarning(IsOnline);
            }
        }
        
        delegate void ProcessCallback(EmployeeModel em);
        private void Process(EmployeeModel em)
        {
            if (uc_employee1.InvokeRequired)
            {
                ProcessCallback d = new ProcessCallback(Process);
                this.Invoke(d, new object[] { em });
            }
            else
            {
                uc_employee1.Clear();
                lblNoRecordFound.Visible = false;

                if (em != null)
                {
                    if (em.employeeId == -1)
                    {
                        lblNoRecordFound.Visible = true;
                    }
                    else
                    {
                        tlManager.SaveTimeLog(em);

                        DateTime n = DateTime.Today;
                        etData data;

                        data = new etData(em.employeeId, n, n, ref Offline.Data);
                        etp = new etPeriod(n, n, data);

                        uc_employee1.ShowEmployee(em, etp);
                        
                        uc_recent_logs1.Add(em);
                    }
                }                
            }

        }

       
        
        public string getServerTimeAndCheckConnectivity()
        {
            string ret = "";
            uc_greeting_and_time1.EnableTimer = false;

            string s = "";
            try
            {
                s = dataManager.getServerTime();
                _officialTime = DateTime.Parse(s);

                IsOnline = true;

                ret = "online - " + _officialTime.ToString();
            }
            catch (Exception ex)
            {
                IsOnline = false;
                ret = "offline - [response: " + s + "], [exception: " + ex.Message + "]";
            }            

            if (!FixedSettings.UseServerTime)
            {
                _officialTime = DateTime.Now;
            }

            uc_greeting_and_time1.EnableTimer = true;
            return ret;
        }
        
        private void tmrDateTime_Tick(object sender, EventArgs e)
        {
            getServerTimeAndCheckConnectivity();
        }

        private void tmrData_Tick(object sender, EventArgs e)
        {
            getData(false, false, true);
        }

        private void tmrPostOfflineLogs_Tick(object sender, EventArgs e)
        {
            try
            {
                postOfflineLogs();
            }
            catch { }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Common.FoceExit) return;

            frmAuthentication auth = new frmAuthentication(Offline.Data.BaseSettings);
            auth.ApplyButtonText = "Ok";

            if (auth.ShowDialog() == DialogResult.OK)
            {
                uc_device_usb1.Disconnect();
            }
            else
            {
                e.Cancel = true;
            }
        }

        public void postOfflineLogs()
        {
            // if (_posting || !IsOnline) return;
            
            _posting = true;

            string folderPath = Common.GetTimeLogsPath();
            string searchString = "offline-log-*";
                        
            foreach (string path in Directory.GetFiles(folderPath, searchString))
            {
                bool lineFailed = false;

                string strTmp;

                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        strTmp = sr.ReadToEnd();
                        strTmp = new Sym().Decrypt(strTmp);
                    }

                    string[] lines = strTmp.Replace("\r\n", "\n").Split('\n');

                    int col_time = 0;
                    int col_employeeId = 1;
                    int col_mode = 3;
                    int col_deviceType = 6;

                    DateTime dt = default(DateTime);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string ln = lines[i];

                        if (i == 0)
                        {
                            string strDate = ln.Replace("[", "").Replace("]", "");
                            dt = DateTime.Parse(strDate);
                            continue;
                        }

                        string[] tmp = ln.Split('\t');

                        string strTime = tmp[col_time];
                        int h = int.Parse(strTime.Split(':')[0]);
                        int m = int.Parse(strTime.Split(':')[1]);
                        int s = int.Parse(strTime.Split(':')[2]);

                        DateTime _dt = dt.AddHours(h).AddMinutes(m).AddSeconds(s);

                        EmployeeModel em = new EmployeeModel
                        {
                            employeeId = int.Parse(tmp[col_employeeId]),
                            timelog = _dt,
                            mode = tmp[col_mode] == "in" ? TimeLogMode.In : TimeLogMode.Out,
                            devicetype = tmp[col_deviceType] == "USB" ? TerminalDeviceType.USB : TerminalDeviceType.LAN
                        };

                        if (!tlManager.PostTimeLog(em))
                        {
                            lineFailed = true;
                        }
                        
                    }

                    if (!lineFailed)
                    {
                        FileInfo fi = new FileInfo(path);
                        string newPath = Path.Combine(folderPath, "posted-" + fi.Name);

                        File.AppendAllLines(newPath, lines);
                        
                        File.Delete(path);
                    }

                }
                catch
                {
                    continue;
                }
            }

            _posting = false;
        }

        private void uc_device_usb1_ScanComplete(int employeeId)
        {
            EmployeeModel em = dataManager.getEmployeeModel(employeeId);
            em.timelog = _officialTime.Value;
            em.localtime = !IsOnline;
            em.mode = uc_timelog_mode1.Mode;
            em.devicetype = TerminalDeviceType.USB;

            Process(em);
        }

        private void uc_device_lan1_ScanComplete(int deviceIndex, int enrollNo, DateTime timeLog, int mode)
        {
            int employeeId = GetEmployeeId(deviceIndex, enrollNo);
            EmployeeModel em = dataManager.getEmployeeModel(employeeId);
            em.timelog = _officialTime.Value;
            em.localtime = !IsOnline;
            em.mode = uc_timelog_mode1.Mode;
            em.devicetype = TerminalDeviceType.LAN;

            Process(em);
        }

        private int GetEmployeeId(int deviceIndex, int enrollNo)
        {
            int ret = -1;

            tblDevice dev = Offline.Data.Devices.Where(x => x.TerminalId == deviceIndex).SingleOrDefault();
            if (dev != null) {

                tblEmployee_DeviceIdNo eid = Offline.Data.EmployeeDeviceIdNos.Where(x => x.DeviceId == dev.DeviceId && x.IdNo == enrollNo).SingleOrDefault();
                if (eid != null)
                {
                    ret = eid.EmployeeId;
                }
            }

            return ret;
        }

        private void pbConfig_Click(object sender, EventArgs e)
        {
            frmAuthentication auth = new frmAuthentication(Offline.Data.BaseSettings);
            auth.ApplyButtonText = "Ok";

            if (auth.ShowDialog() == DialogResult.OK)
            {
                frmConfig config = new frmConfig(this);
                config.ShowDialog();

                loadSettings();
                FixedSettings.InitDB(this);

                uc_announcements1.Init(Offline.Data.BulletinBoard);

                uc_device_monitor1.SetReconnectInterval(FixedSettings.DeviceReconnectInterval);
                uc_device_monitor2.SetReconnectInterval(FixedSettings.DeviceReconnectInterval);

                uc_device_monitor1.EnableDevice(FixedSettings.EnableUSBDevice);
                uc_device_monitor2.EnableDevice(FixedSettings.EnableLANDevice);

            }
        }

        private void pbConfig_MouseEnter(object sender, EventArgs e)
        {
            pbConfig.BorderStyle = BorderStyle.FixedSingle;            
        }

        private void pbConfig_MouseLeave(object sender, EventArgs e)
        {
            pbConfig.BorderStyle = BorderStyle.None;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BorderStyle = BorderStyle.None;
        }
    }
}
