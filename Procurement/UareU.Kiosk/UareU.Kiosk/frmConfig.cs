using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class frmConfig : Form
    {
        frmMain form;

        public frmConfig(frmMain form)
        {
            this.form = form;

            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void loadSettings()
        {
            nudTerminalId.Value = Properties.Settings.Default.TerminalId;

            tbAppUrl.Text = Properties.Settings.Default.AppUrl;

            showLogo();

            nudServerTimeQueryInterval.Value = Properties.Settings.Default.ServerTimeQueryInterval;
            nudDataDownloadInterval.Value = Properties.Settings.Default.DataDownloadInterval;
            nudPostOfflineLogsInterval.Value = Properties.Settings.Default.PostOfflineLogsInterval;

            cbLongTimeFormat.Checked = Properties.Settings.Default.LongTimeFormat;

            tbOfflineMsg.Text = Properties.Settings.Default.OfflineMessage;

            tbTLL.Text = Properties.Settings.Default.TimeLogsPath;
            tbODL.Text = Properties.Settings.Default.OfflineDataPath;
            tbPL.Text = Properties.Settings.Default.PhotosPath;

            cbFullview.Checked = Properties.Settings.Default.Fullview;

            cbUseServerTime.Checked = Properties.Settings.Default.UseServerTime;

            cbDataDownloadEnabled.Checked = Properties.Settings.Default.DataDownloadEnabled;

            nudMaxLogDisplayTime.Value = Properties.Settings.Default.MaxLogDispayTime;
            nudDeviceReconnectInterval.Value = Properties.Settings.Default.DeviceReconnectInterval;

            cbEnableUSBDevice.Checked = Properties.Settings.Default.EnableUSBDevice;
            cbEnableLANDevice.Checked = Properties.Settings.Default.EnableLANDevice;

            setDatasourceVersion();
            setPhotosVersion();
        }

        private void saveSettings()
        {
            Properties.Settings.Default.TerminalId = (int)nudTerminalId.Value;

            Properties.Settings.Default.AppUrl = tbAppUrl.Text;

            Properties.Settings.Default.ServerTimeQueryInterval = (int)nudServerTimeQueryInterval.Value;
            Properties.Settings.Default.DataDownloadInterval = (int)nudDataDownloadInterval.Value;
            Properties.Settings.Default.PostOfflineLogsInterval = (int)nudPostOfflineLogsInterval.Value;

            Properties.Settings.Default.LongTimeFormat = cbLongTimeFormat.Checked;

            Properties.Settings.Default.OfflineMessage = tbOfflineMsg.Text;

            Properties.Settings.Default.TimeLogsPath = tbTLL.Text;
            Properties.Settings.Default.OfflineDataPath = tbODL.Text;
            Properties.Settings.Default.PhotosPath = tbPL.Text;

            Properties.Settings.Default.Fullview = cbFullview.Checked;

            Properties.Settings.Default.DataDownloadEnabled = cbDataDownloadEnabled.Checked;

            Properties.Settings.Default.UseServerTime = cbUseServerTime.Checked;

            Properties.Settings.Default.MaxLogDispayTime = (int)nudMaxLogDisplayTime.Value;
            Properties.Settings.Default.DeviceReconnectInterval= (int)nudDeviceReconnectInterval.Value;

            Properties.Settings.Default.EnableUSBDevice = cbEnableUSBDevice.Checked;
            Properties.Settings.Default.EnableLANDevice = cbEnableLANDevice.Checked;
            
            Properties.Settings.Default.Save();

            try
            {
                string logoPath = Common.GetLogoPath();

                if (File.Exists(logoPath))
                {
                    File.Delete(logoPath);
                }

                if (pbLogo.Image != null)
                {
                    pbLogo.Image.Save(logoPath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save logo. " + ex.Message);
            }
            
        }

        private void showLogo()
        {
            string logoPath = Common.GetLogoPath();

            if (File.Exists(logoPath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Image tmp = Image.FromFile(logoPath);
                    tmp.Save(ms, ImageFormat.Png);

                    pbLogo.Image = Image.FromStream(ms);
                    tmp.Dispose();
                }
            }
            else
            {
                pbLogo.Image = null;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            saveSettings();
            loadSettings();

            MessageBox.Show("Settings were successfully saved");
        }

        private void btnApplyAndClose_Click(object sender, EventArgs e)
        {
            saveSettings();
            loadSettings();

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDownloadLogo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.AppUrl))
            {
                MessageBox.Show("SkyHR url is invalid");
            }
            else
            {
                byte[] pic;
                Image logo;

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string logoUrl = Properties.Settings.Default.AppUrl + "/Assets/images/company-logo.png";
                        pic = client.DownloadData(logoUrl);
                    }

                    using (MemoryStream mem = new MemoryStream(pic))
                    {
                        logo = Image.FromStream(mem);
                    }

                    pbLogo.Image = logo;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to download logo. " + ex.Message);
                }
            }
        }

        private void btnBrowseTLL_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbTLL.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowseODL_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbODL.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowsePL_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbPL.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void setDatasourceVersion()
        {
            if (Offline.Data == null)
            {
                lblDSVersion.Text = "[unknown]";
                lblDSVersion.ForeColor = Color.Red;
            }
            else
            {
                lblDSVersion.Text = "Data source latest version: " + Offline.Data.Version.ToLongTimeString();
                lblDSVersion.ForeColor = Color.Blue; ;
            }
            
        }

        private void setPhotosVersion()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.PhotosVersion))
            {
                lblPhotosVersion.Text = "[unknown]";
                lblPhotosVersion.ForeColor = Color.Red;
            }
            else
            {
                lblPhotosVersion.Text = "Photos latest version: " + Properties.Settings.Default.PhotosVersion;
                lblPhotosVersion.ForeColor = Color.Blue; ;
            }

        }

        private void btnForceDownload_Click(object sender, EventArgs e)
        {
            form.getData(true, true, true);
            setDatasourceVersion();

            MessageBox.Show("Data was successfully downloaded");
        }
        
        private void btnForcePost_Click(object sender, EventArgs e)
        {
            form.postOfflineLogs();
            MessageBox.Show("Posting was successfully executed");
        }

        private void btnForceQueryTime_Click(object sender, EventArgs e)
        {
            MessageBox.Show(form.getServerTimeAndCheckConnectivity());
        }
        
        private void btnDownloadPhotos_Click(object sender, EventArgs e)
        {
            List<string> userIds = Offline.Data.Employees.Select(x => x.UserId).ToList();

            frmDownloadPhotos frm = new frmDownloadPhotos(userIds);
            frm.ShowDialog();

            setPhotosVersion();

            MessageBox.Show("Photos were successfully downloaded");
        }

        
    }
}
