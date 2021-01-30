using System;
using System.Data;
using System.Linq;
using Module.DB.Enums;
using Module.DB;
using System.Windows.Forms;
using zkemkeeper;

namespace Module.Device
{
    public class device_lan: IDevice
    {
        public Device_OnConnected OnConnected { get; set; }
        public Device_OnDisconnected OnDisconnected { get; set; }
        public Device_OnMessage OnMessage { get; set; }

        public TerminalDeviceType Type
        {
            get
            {
                return TerminalDeviceType.LAN;
            }
        }

        public event Action<int, int, DateTime, int> ScanComplete;

        CZKEM cz = new CZKEM();
        
        public string IP { get; set; }
        public int Port { get; set; }
        public int DeviceIndex { get; set; }
        public int DBId { get; set; }

        public bool Connect()
        {
            bool ret = false;

            Disconnect();

            if (coreLib.Procs.Ping(IP))
            {
                if (cz.Connect_Net(IP, Port))
                {
                    OnConnected(DeviceIndex, DBId);

                    if (cz.RegEvent(DeviceIndex, 65535))
                    {
                        cz.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx);
                    }
                    
                    ret = true;
                }
                else
                {
                    showMessage(getErrorMsg(), true);
                }
            }
            else
            {
                showMessage("Device not found", true);
            }

            return ret;
        }

        public void Disconnect()
        {
            cz.Disconnect();
        }

        private void OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerificationMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            showMessage("onatt - test");

            DateTime dt = new DateTime(Year, Month, Day, Hour, Minute, Second);
            int mode = Devices.Common.getMode(AttState.ToString());

            int enrollNo = int.Parse(EnrollNumber);

            ScanComplete(DeviceIndex, enrollNo, dt, mode);
        }

        private string getErrorMsg()
        {
            int errorCode = 0;
            cz.GetLastError(ref errorCode);

            return Common.getErrorDescription(errorCode);
        }

        private void showMessage(string errorMsg, bool isError = false)
        {
            if (isError)
            {
                OnDisconnected(DeviceIndex, DBId);
            }

            OnMessage(DeviceIndex, DBId, errorMsg);
        }
    }

    public partial class uc_device_lan : UserControl, IDevice
    {
        public TerminalDeviceType Type
        {
            get
            {
                return TerminalDeviceType.LAN;
            }
        }

        public void setTimer(bool enabled)
        {
            tmrCheck.Enabled = enabled;
        }

        public void setTimerInterval(int interval)
        {
            tmrCheck.Interval = interval;
        }

        public Device_OnConnected OnConnected { get; set; }
        public Device_OnDisconnected OnDisconnected { get; set; }
        public Device_OnMessage OnMessage { get; set; }

        public event Action<int, int, DateTime, int> ScanComplete;

        CZKEM cz = new CZKEM();
        public string IP { get; set; }
        public int Port { get; set; }
        public int DeviceIndex { get; set; }
        public int DBId { get; set; }

        bool isConnected = false;

        public uc_device_lan()
        {
            InitializeComponent();
        }

        public bool Connect()
        {
            bool ret = false;
            
            Disconnect();

            isConnected = false;

            if (coreLib.Procs.Ping(IP))
            {
                if (cz.Connect_Net(IP, Port))
                {
                    isConnected = true;
                    OnConnected(DeviceIndex, DBId);

                    if (cz.RegEvent(DeviceIndex, 65535))
                    {
                        cz.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx);
                    }

                    showMessage("Device is ready");

                    ret = true;
                }
                else
                {
                    showMessage(getErrorMsg(), true);
                }
            }
            else
            {
                showMessage("Device not found", true);
            }
            
            return ret;
        }

        public void Disconnect()
        {
            cz.Disconnect();
            isConnected = false;

            OnDisconnected(DeviceIndex, DBId);
        }

        private void OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerificationMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            DateTime dt = new DateTime(Year, Month, Day, Hour, Minute, Second);
            int mode = Devices.Common.getMode(AttState.ToString());

            int enrollNo = int.Parse(EnrollNumber);
            
            ScanComplete(DeviceIndex, enrollNo, dt, mode);
        }

        private string getErrorMsg()
        {
            int errorCode = 0;
            cz.GetLastError(ref errorCode);

            return Common.getErrorDescription(errorCode);
        }
        
        private void showMessage(string errorMsg, bool isError = false)
        {
            if (isError)
            {
                OnDisconnected(DeviceIndex, DBId);
            }

            OnMessage(DeviceIndex, DBId, errorMsg);
        }

        private void tmrCheck_Tick(object sender, EventArgs e)
        {
            if (!coreLib.Procs.Ping(IP))
            {
                if (isConnected)
                {
                    showMessage("Device not found", true);
                }
            }
        }
    }
}
