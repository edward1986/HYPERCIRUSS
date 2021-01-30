using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace device_zks_s5_app
{
    public partial class mdiMain : Form
    {
        frmDownloadData frmDownload = new frmDownloadData();

        public mdiMain()
        {
            InitializeComponent();
            
            frmDownload.MdiParent = this;
        }

        private void mdiMain_Load(object sender, EventArgs e)
        {
            getSettings();
            //connect();
        }

        private void getSettings()
        {
            textBoxMachineNo.Text = Properties.Settings.Default.MachineNo;
            textBoxIP.Text = Properties.Settings.Default.IP;
            textBoxPort.Text = Properties.Settings.Default.Port;
            textBoxPassword.Text = Properties.Settings.Default.PW;
        }

        private void connect()
        {
            lockConnection();

            int n = int.Parse(textBoxMachineNo.Text);
            string ip = textBoxIP.Text;
            int port = int.Parse(textBoxPort.Text);
            int pw = int.Parse(textBoxPassword.Text);

            clearLog();
            addLog("Connecting...");

            if (Lib.ping(ip))
            {
                if (axSB100BPC1.ConnectTcpip(n, ref ip, port, pw))
                {
                    toolStripStatusLabel1.Text = "Connected";
                    addLog("Connected", false);
                }
                else
                {
                    toolStripStatusLabel1.Text = "Disconnected";
                    addLog("Failed", false);

                    int errorCode = -1;
                    axSB100BPC1.GetLastError(ref errorCode);

                    string s = Lib.getErrorDescription(errorCode);
                    if (!string.IsNullOrEmpty(s))
                    {
                        addLog(s);
                    }
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Disconnected";
                addLog("Failed", false);
                addLog("IP address is unreachable");
            }

            lockConnection(true);
        }

        private void disconnect()
        {
            axSB100BPC1.Disconnect();
        }

        private void addLog(string log, bool newLine = true)
        {
            textBoxLog.Text += (newLine && textBoxLog.Text != "" ? Environment.NewLine : "") + log;
        }

        private void clearLog()
        {
            textBoxLog.Text = "";
        }

        private void lockConnection(bool enable = false)
        {
            buttonApply.Enabled = enable;

            textBoxMachineNo.Enabled = enable;
            textBoxIP.Enabled = enable;
            textBoxPort.Enabled = enable;
            textBoxPassword.Enabled = enable;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void mdiMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnect();
        }

        private void uploadDataFromDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDownload.Show();
        }
    }
}
