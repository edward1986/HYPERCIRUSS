using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Aspose.Cells;

namespace Devices
{
    public class clLogItem
    {
        public int LineNo;
        public Exception Exception;
        public string IdNo;
        public string LogDate;
        public string Mode;
    }

    public class TmpUploadResult
    {
        public List<clLogItem> LogItems = new List<clLogItem>();
        public List<Exception> Exceptions = new List<Exception>();

        public string getErrors()
        {
            string ret = "";
            if (Exceptions.Any())
            {
                foreach (Exception e in Exceptions)
                {
                    ret += (ret == "" ? "" : "<br />") + e.Message;
                }
            }

            var _err = LogItems.Where(x => x.Exception != null).ToList();
            if (_err.Any())
            {
                ret += (ret == "" ? "" : "<br />") + "<hr />Errors in items<hr />";
            }

            bool noBr = true;

            foreach (clLogItem item in _err)
            {
                ret += (ret == "" || noBr ? "" : "<br />") + "[" + item.LineNo + "] " + item.Exception.Message;
                noBr = false;
            }

            return ret;
        }
    }

    public class clDevice
    {
        int _idNo_ColNo, _logDate_ColNo, _mode_ColNo;
        public bool _skipFirstLine;
        public bool _useTextFile;

        protected delegate void setLogItemDelegate(ref clLogItem logItem, Range cells, int r, int _idNo_ColNo, int _logDate_ColNo, int _mode_ColNo);
        protected setLogItemDelegate setLogItemHandler;

        protected delegate void setTextLogItemDelegate(ref clLogItem logItem, string[] data, int r, int _idNo_ColNo, int _logDate_ColNo, int _mode_ColNo);
        protected setTextLogItemDelegate setTextLogItemHandler;

        protected delegate bool connectDelegate(ref string errorMsg);
        protected connectDelegate connectHandler;

        protected delegate void disConnectDelegate();
        protected disConnectDelegate disConnectHandler;

        protected delegate void getLogsDelegate(ref TmpUploadResult uploadRes, DateTime startDate, DateTime endDate, int idNo = -1);
        protected getLogsDelegate getLogsHandler;

        public event Action<int, int, DateTime, int> OnScanComplete;

        public int _iMachineNumber = 1;

        TmpUploadResult _uploadRes = new TmpUploadResult();
        bool _isConnected = false;
        string _lastError = "";

        public string IP { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public clDevice(int idNo_ColNo, int _logDate_ColNo, int _mode_ColNo, bool _skipFirstLine, bool _useTextFile)
        {
            this._idNo_ColNo = idNo_ColNo;
            this._logDate_ColNo = _logDate_ColNo;
            this._mode_ColNo = _mode_ColNo;
            this._skipFirstLine = _skipFirstLine;
            this._useTextFile = _useTextFile;
        }

        protected void ScanComplete(int deviceIndex, int enrollNo, DateTime timeLog, int mode)
        {
            OnScanComplete(deviceIndex, enrollNo, timeLog, mode);
        }

        public TmpUploadResult DownloadLogs(DateTime startDate, DateTime endDate, int idNo = -1)
        {
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            _uploadRes = new TmpUploadResult();

            if (isReachable())
            {
                try
                {
                    _connect();

                    if (IsConnected)
                    {
                        getLogsHandler(ref _uploadRes, startDate, endDate, idNo);
                    }

                }
                catch (Exception e)
                {
                    _lastError = e.Message;
                    _uploadRes.Exceptions.Add(e);
                }
                finally
                {
                    _disconnect();
                }
            }

            return _uploadRes;
        }

        //public TmpUploadResult UploadExcelFile(string path, DateTime startDate, DateTime endDate)
        //{
        //    endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);

        //    _uploadRes = new TmpUploadResult();

        //    FileInfo fi = new FileInfo(path);
        //    if (fi.Exists)
        //    {
        //        openExcelFile(fi, _skipFirstLine, startDate, endDate);
        //    }
        //    else
        //    {
        //        _uploadRes.Exceptions.Add(new Exception("File not found"));
        //    }

        //    return _uploadRes;
        //}

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public string LastError
        {
            get
            {
                return _lastError;
            }
        }

        public void Connect()
        {
            if (isReachable())
            {
                _connect();
            }
        }

        public void Disconnect()
        {
            if (isReachable())
            {
                _disconnect();
            }
        }

        private void _connect()
        {
            try
            {
                _isConnected = connectHandler(ref _lastError);
            }
            catch (Exception e)
            {
                _lastError = e.Message;
                throw e;
            }
        }

        private void _disconnect()
        {
            try
            {
                disConnectHandler();
                _isConnected = false;
            }
            catch (Exception e)
            {
                _lastError = e.Message;
            }
        }

        private bool isReachable()
        {
            _lastError = "";
            bool ret = ping(IP);

            if (!ret)
            {
                _lastError = "Device is unreachable";
            }

            return ret;
        }

        public clLogItem IterateTextRecord(int r, DateTime startDate, DateTime endDate, string[] data)
        {

            clLogItem logItem = new clLogItem
            {
                LineNo = r
            };

            try
            {
                setTextLogItemHandler(ref logItem, data, r, _idNo_ColNo, _logDate_ColNo, _mode_ColNo);
            }
            catch (Exception e)
            {
                logItem.Exception = e;
            }

            DateTime l = DateTime.Parse(logItem.LogDate); // DateTime.Parse((range.Cells[r, _logDate_ColNo] as Excel.Range).Value);
            if (l >= startDate && l <= endDate)
            {
                return logItem;
            }
            else
            {
                return null;
            }
        }

        public clLogItem IterateExcelRecord(int r, DateTime startDate, DateTime endDate, Range cells)
        {

            clLogItem logItem = new clLogItem
            {
                LineNo = r
            };

            try
            {
                setLogItemHandler(ref logItem, cells, r, _idNo_ColNo, _logDate_ColNo, _mode_ColNo);
            }
            catch (Exception e)
            {
                logItem.Exception = e;
            }

            DateTime l = DateTime.Parse(logItem.LogDate); // DateTime.Parse((range.Cells[r, _logDate_ColNo] as Excel.Range).Value);
            if (l >= startDate && l <= endDate)
            {
                return logItem;
            }
            else
            {
                return null;
            }
        }

        //private void openExcelFile(FileInfo fi, bool skipFirstLine, DateTime startDate, DateTime endDate)
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    Excel.Range range;

        //    int r = 0;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Open(fi.FullName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    range = xlWorkSheet.UsedRange;

        //    int total = range.Rows.Count;

        //    for (r = 1; r <= total; r++)
        //    {
        //        if (r == 1 && skipFirstLine)
        //        {
        //            continue;
        //        }

        //        clLogItem logItem = IterateExcelRecord(r, startDate, endDate, range.Cells);

        //        if (logItem != null)
        //        {
        //            _uploadRes.LogItems.Add(logItem);
        //        }

        //    }

        //    xlWorkBook.Close(true, null, null);
        //    xlApp.Quit();

        //    releaseExcelObject(xlWorkSheet);
        //    releaseExcelObject(xlWorkBook);
        //    releaseExcelObject(xlApp);

        //}

        //private void releaseExcelObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch (Exception e)
        //    {
        //        obj = null;
        //        _uploadRes.Exceptions.Add(new Exception("Unable to release the Object " + e.ToString()));
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}

        public bool ping(string ip)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                options.DontFragment = true;

                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 2000;
                PingReply reply = pingSender.Send(ip, timeout, buffer, options);

                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class Common
    {
        public static int getMode(string mode)
        {
            int ret;

            if (mode.ToLower().Trim() == "c/in" || mode.ToLower().Trim() == "in" || mode.ToLower().Trim() == "0")
            {
                ret = (int)CoreDeviceLogMode.In;
            }
            else if (mode.ToLower().Trim() == "c/out" || mode.ToLower().Trim() == "out" || mode.ToLower().Trim() == "1")
            {
                ret = (int)CoreDeviceLogMode.Out;
            }
            else if (mode.ToLower().Trim() == "break" || mode.ToLower().Trim() == "2")
            {
                ret = (int)CoreDeviceLogMode.Break;
            }
            else if (mode.ToLower().Trim() == "resume" || mode.ToLower().Trim() == "3")
            {
                ret = (int)CoreDeviceLogMode.Resume;
            }
            else if (mode.ToLower().Trim() == "ot" || mode.ToLower().Trim() == "4")
            {
                ret = (int)CoreDeviceLogMode.OTIn;
            }
            else if (mode.ToLower().Trim() == "done" || mode.ToLower().Trim() == "5")
            {
                ret = (int)CoreDeviceLogMode.OTOut;
            }
            else
            {
                ret = (int)CoreDeviceLogMode.Other;
            }

            return ret;
        }
    }

    public enum CoreDeviceLogMode
    {
        In = 0,
        Out = 1,
        Break = 2,
        Resume = 3,
        OTIn = 4,
        OTOut = 5,
        Other = 6
    }
}

