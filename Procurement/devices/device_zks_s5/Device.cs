using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using SB100BPCLib;

namespace Devices
{
    
    public class ZKS_S5 : clDevice
    {
        public SB100BPC SB100BPC1 = new SB100BPC();

        public ZKS_S5()
            : base(2, 5, 7, true)
        {
            setLogItemHandler += _SetLogItem;
            connectHandler += _Connect;
            disConnectHandler += _Disconnect;
            getLogsHandler += _GetLogs;
        }

        private bool _Connect(ref string errorMsg)
        {
            throw new Exception("Incomplete driver definition");

            //if (!SB100BPC1.ConnectTcpip(_iMachineNumber, IP, Port.Value, int.Parse(Password)))
            //{
            //    errorMsg = _getErrorMsg("Unable to connect the device");
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
        }

        private void _Disconnect()
        {
            throw new Exception("Incomplete driver definition");

            //SB100BPC1.Disconnect();
        }

        private void _GetLogs(ref TmpUploadResult uploadRes)
        {
            throw new Exception("Incomplete driver definition");
        }

        private void _SetLogItem(ref clLogItem logItem, Excel.Range cells, int r, int _idNo_ColNo, int _logDate_ColNo, int _mode_ColNo)
        {
            logItem.IdNo = (cells[r, _idNo_ColNo] as Excel.Range).Value;
            logItem.LogDate = (cells[r, _logDate_ColNo] as Excel.Range).Value;
            logItem.Mode = (cells[r, _mode_ColNo] as Excel.Range).Value;
        }

        private string _getErrorMsg(string msgPrefix)
        {
            int errorCode = 0;
            SB100BPC1.GetLastError(ref errorCode);

            string s = _getErrorDescription(errorCode);
            return msgPrefix + (string.IsNullOrEmpty(s) ? "" : ". " + s);
        }

        private string _getErrorDescription(int errorCode)
        {
            string ret = "";
            switch (errorCode)
            {
                case 0:
                    ret = "Operation was successfully completed";
                    break;
                case 1:
                    ret = "Cannot open the COM port";
                    break;
                case 2:
                    ret = "An error occured upon trasmitting the data";
                    break;
                case 3:
                    ret = "An error occured upon receiving the data";
                    break;
                case 4:
                    ret = "Invalid parameter";
                    break;
                case 5:
                    ret = "Operation failed";
                    break;
                case 6:
                    ret = "Completed reading all data from internal memory";
                    break;
            }
            return ret;
        }
    }

    
}

