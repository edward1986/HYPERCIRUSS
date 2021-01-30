using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DPUruNet;
using System.Threading;
using Module.DB;
using Module.DB.Enums;
using Module.Device;

namespace UareU.Kiosk
{
    public partial class uc_device_usb : UserControl, IDevice
    {
        public TerminalDeviceType Type
        {
            get
            {
                return TerminalDeviceType.USB;
            }
        }

        public Device_OnConnected OnConnected { get; set; }
        public Device_OnDisconnected OnDisconnected { get; set; }
        public Device_OnMessage OnMessage { get; set; }

        Reader currentReader;
        private const int PROBABILITY_ONE = 0x7fffffff;

        public event Action<int> ScanComplete;

        public uc_device_usb()
        {
            InitializeComponent();
        }

        private bool GetReader()
        {
            ReaderCollection _readers = ReaderCollection.GetReaders();
            if (_readers.Any())
            {
                currentReader = _readers.First();

                showMessage("");

                return true;
            }
            else
            {
                currentReader = null;
                showMessage("No reader found", true);

                return false;
            }
        }

        public bool Connect()
        {
            Disconnect();

            if (GetReader())
            {
                Constants.ResultCode result = currentReader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);

                if (result == Constants.ResultCode.DP_SUCCESS)
                {
                    OnConnected(1, 1);

                    if (StartCaptureAsync(this.OnCaptured))
                    {
                        showMessage("Device is ready");
                    }

                    return true;
                }
                else
                {
                    showMessage(result.ToString(), true);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool StartCaptureAsync(Reader.CaptureCallback OnCaptured)
        {
            currentReader.On_Captured += new Reader.CaptureCallback(OnCaptured);
            return CaptureFingerAsync();
        }

        private bool CaptureFingerAsync()
        {
            try
            {
                getStatus();

                Constants.ResultCode captureResult = currentReader.CaptureAsync(Constants.Formats.Fid.ANSI, Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, currentReader.Capabilities.Resolutions[0]);
                if (captureResult != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception("" + captureResult);
                }

                return true;
            }
            catch (Exception ex)
            {
                showMessage(ex.Message, true);
                return false;
            }
        }

        public void Disconnect()
        {
            if (currentReader != null)
            {
                currentReader.CancelCapture();
                
                currentReader.Dispose();
                currentReader = null;
            }
        }

        public DeviceStatus GetStatus()
        {
            Constants.ResultCode result = currentReader.GetStatus();
            if (result == Constants.ResultCode.DP_SUCCESS)
            {
                return DeviceStatus.Connected;
            }
            else
            {
                return DeviceStatus.Disconnected;
            }
        }

        private void getStatus()
        {  

            Constants.ResultCode result = currentReader.GetStatus();

            if ((result != Constants.ResultCode.DP_SUCCESS))
            {

                throw new Exception("" + result);
            }

            if ((currentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_BUSY))
            {
                Thread.Sleep(50);
            }
            else if ((currentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_NEED_CALIBRATION))
            {
                currentReader.Calibrate();
            }
            else if ((currentReader.Status.Status != Constants.ReaderStatuses.DP_STATUS_READY))
            {
                throw new Exception("Reader Status - " + currentReader.Status.Status);
            }
        }

        private void OnCaptured(CaptureResult captureResult)
        {
            try
            {
                if (!CheckCaptureResult(captureResult)) return;

                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(resultConversion.ResultCode.ToString());
                }

                Console.Beep();

                int employeeId = GetEmployeeId(resultConversion.Data);
                ScanComplete(employeeId);
            }
            catch (Exception ex)
            {
                showMessage(ex.Message, true);
            }
        }

        private bool CheckCaptureResult(CaptureResult captureResult)
        {
            if (captureResult.Data == null)
            {
                if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(captureResult.ResultCode.ToString());
                }

                if ((captureResult.Quality != Constants.CaptureQuality.DP_QUALITY_CANCELED))
                {
                    throw new Exception("Quality - " + captureResult.Quality);
                }
                return false;
            }

            return true;
        }

        private void showMessage(string errorMsg, bool isError = false)
        {
            if (isError)
            {
                OnDisconnected(1, 1);
            }

            OnMessage(1, 1, errorMsg);
        }

        private int GetEmployeeId(Fmd finger)
        {
            int ret = -1;
            
            List<Fmd> fmds = new List<Fmd>();

            foreach (tblEmployee_FP fp in Offline.Data.EmployeeFPs)
            {
                for (int i = 0; i <= 9; i++)
                {
                    string data = "";
                    if (i == 0) data = fp.FP1;
                    if (i == 1) data = fp.FP2;
                    if (i == 2) data = fp.FP3;
                    if (i == 3) data = fp.FP4;
                    if (i == 4) data = fp.FP5;
                    if (i == 5) data = fp.FP6;
                    if (i == 6) data = fp.FP7;
                    if (i == 7) data = fp.FP8;
                    if (i == 8) data = fp.FP9;
                    if (i == 9) data = fp.FP10;

                    if (!string.IsNullOrEmpty(data))
                    {
                        Fmd _fmd = Fmd.DeserializeXml(data);

                        CompareResult compareResult = Comparison.Compare(finger, 0, _fmd, 0);
                        if (compareResult.ResultCode == Constants.ResultCode.DP_SUCCESS && compareResult.Score <= 100)
                        {
                            ret = fp.EmployeeId;
                            break;
                        }
                    }
                }

                if (ret != -1) break;
            }

            return ret;
        }
    }
}
