using Module.Device;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class uc_device_monitor : UserControl
    {        
        IDevice dev;
        bool enabled;

        public uc_device_monitor()
        {
            InitializeComponent();            
        }

        public void SetDevice(IDevice dev, int reconnectInterval)
        {
            this.dev = dev;
            this.dev.OnConnected += OnConnected;
            this.dev.OnDisconnected += OnDisconnected;
            this.dev.OnMessage += OnMessage;

            Clear();            
            SetType();

            SetReconnectInterval(reconnectInterval);
            
        }

        public void EnableDevice(bool enable)
        {
            this.enabled = enable;

            if (enable)
            {
                Reconnect();
            }
            else
            {
                if (dev.Type == Module.DB.Enums.TerminalDeviceType.LAN)
                {
                    ((uc_device_lan)dev).setTimer(false);
                }

                dev.Disconnect();
                CancelReconnectProcess();

                lblStatus.Text = "Disabled";
            }
        }

        private void InitDevice()
        {
            if (dev.Type == Module.DB.Enums.TerminalDeviceType.LAN)
            {
                ((uc_device_lan)dev).IP = FixedSettings.DeviceIP;
                ((uc_device_lan)dev).Port = FixedSettings.DevicePort;
                ((uc_device_lan)dev).DeviceIndex = FixedSettings.TerminalId;

                ((uc_device_lan)dev).setTimer(true);
            }
        }

        private void Clear()
        {
            lblType.Text = "";
            lblMessage.Text = "";
            lblStatus.Text = "";
            lblReconnectStatus.Text = "";
        }

        private void SetType()
        {
            string type = "";

            if (dev.Type == Module.DB.Enums.TerminalDeviceType.LAN)
            {
                type = "[LAN Device]";

                SetColors(Color.LightSkyBlue);
            }

            if (dev.Type == Module.DB.Enums.TerminalDeviceType.USB)
            {
                type = "[USB Device]";

                SetColors(Color.LightCoral);
            }

            lblType.Text = type;
        }

        private void SetColors(Color color)
        {
            foreach (Control ctl in Controls)
            {
                if (ctl.GetType() == typeof(Label))
                {
                    ctl.ForeColor = color;
                }
            }
        }

        delegate void OnConnectedCallback(int deviceIndex, int dbId);
        public void OnConnected(int deviceIndex, int dbId)
        {
            if (this.lblStatus.InvokeRequired)
            {
                OnConnectedCallback d = new OnConnectedCallback(OnConnected);
                this.Invoke(d, new object[] { deviceIndex });
            }
            else
            {
                lblStatus.Text = "Connected";
                CancelReconnectProcess();
            }
            
        }

        delegate void OnDisconnectedCallback(int deviceIndex, int dbId);
        public void OnDisconnected(int deviceIndex, int dbId)
        {
            if (this.lblStatus.InvokeRequired)
            {
                OnDisconnectedCallback d = new OnDisconnectedCallback(OnDisconnected);
                this.Invoke(d, new object[] { deviceIndex });
            }
            else 
            {
                if (enabled)
                {
                    lblStatus.Text = "Disconnected";
                    StartReconnectProcess();
                }
            }
        }

        delegate void OnMessageCallback(int deviceIndex, int dbId, string message);
        public void OnMessage(int deviceIndex, int dbId, string message)
        {
            if (this.lblMessage.InvokeRequired)
            {
                OnMessageCallback d = new OnMessageCallback(OnMessage);
                this.Invoke(d, new object[] { deviceIndex, message });
            }
            else
            {
                lblMessage.Text = message;
            }
        }

        int reconnectCount = 0;
        int reconnectInterval = -1;

        public void SetReconnectInterval(int reconnectInterval)
        {
            this.reconnectInterval = reconnectInterval;
        }

        private void tmrReconnect_Tick(object sender, EventArgs e)
        {
            reconnectCount++;

            if (reconnectCount >= reconnectInterval)
            {
                Reconnect();
            }
            else
            {
                int remainingTime = reconnectInterval - reconnectCount;
                lblReconnectStatus.Text = "Reconnect in " + remainingTime + " seconds...";
            }
        }

        private void StartReconnectProcess()
        {
            CancelReconnectProcess();
            tmrReconnect.Enabled = true;
        }

        private void CancelReconnectProcess()
        {
            tmrReconnect.Enabled = false;
            reconnectCount = 0;

            lblReconnectStatus.Text = "";
        }

        public void Reconnect()
        {
            lblReconnectStatus.Text = "Connecting...";
            Application.DoEvents();

            InitDevice();
            if (dev.Connect())
            {
                CancelReconnectProcess();
            }
            else
            {
                StartReconnectProcess();
            }
        }
    }
}
