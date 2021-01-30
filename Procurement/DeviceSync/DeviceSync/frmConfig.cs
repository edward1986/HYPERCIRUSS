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
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            saveSettings();
            MessageBox.Show("Settings were successfully saved");

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void loadSettings()
        {
            tbServicePath.Text = Properties.Settings.Default.ServicePath;
            nudRefreshRate.Value = Properties.Settings.Default.RefreshInterval;            
            tbDailySchedule.Text = Properties.Settings.Default.DownloadSchedule.ToString("h:mm tt");   
        }

        private void saveSettings()
        {
            Properties.Settings.Default.ServicePath = tbServicePath.Text;
            Properties.Settings.Default.RefreshInterval = (int)nudRefreshRate.Value;
            Properties.Settings.Default.DownloadSchedule = Convert.ToDateTime(tbDailySchedule.Text);

            Properties.Settings.Default.Save();            
        }
    }
}
