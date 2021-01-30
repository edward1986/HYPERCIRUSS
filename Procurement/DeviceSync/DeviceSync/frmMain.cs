using coreLib.Enums;
using coreLib.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceSync
{
    public partial class frmMain : Form
    {
        DeviceWrapper deviceWrapper;

        public frmMain()
        {
            InitializeComponent();
        }
        
        private void trmRefresher_Tick(object sender, EventArgs e)
        {
            doRefresh();
        }

        private void tmrDownloader_Tick(object sender, EventArgs e)
        {
            doDownload();
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfig config = new frmConfig();

            if (config.ShowDialog() == DialogResult.OK)
            {
                loadSettings();
            }
        }

        private void doRefresh()
        {
            deviceWrapper = new DeviceWrapper();
            deviceWrapper.Disconnect();
            deviceWrapper.Connect();
        }

        private void doDownload(bool force = false)
        {
            DateTime now = DateTime.Now;
            bool doit = false;

            DateTime? lastDL = Common.LastDownloadDate();
            if (lastDL == null)
            {
                doit = FixedSettings.DownloadSchedule_IsDue(now);
            }
            else
            {
                lblLastDL.Text = lastDL.ToString();

                if (now.Date > lastDL.Value.Date)
                {
                    doit = FixedSettings.DownloadSchedule_IsDue(now);
                }
            }

            if (doit || force)
            {
                PeriodModel pm = new PeriodModel(PeriodModelInitType.ThisMonth);

                deviceWrapper.DownloadLogs(pm.StartDate.AddDays(-1), pm.EndDate);
                deviceWrapper.RelateLogs(pm.StartDate.AddDays(-1), pm.EndDate);
            }
        }

        private void loadSettings()
        {
            FixedSettings.Init();
            Logger.Init();

            trmRefresher.Interval = FixedSettings.RefreshInterval * 60000;

            trmRefresher.Enabled = true;
            tmrDownloader.Enabled = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            doRefresh();
            MessageBox.Show("done");
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            doDownload(true);
            MessageBox.Show("done");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            loadSettings();

            this.Text = "Device Sync version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            deviceWrapper = new DeviceWrapper();
            deviceWrapper.Connect();
            
            this.Visible = true;
            this.Show();
            this.Activate();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            deviceWrapper.Disconnect();
        }
    }
}
