using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using zkemkeeper;

namespace Devices
{

    public class X628 : clDevice
    {
        public zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();

        public X628()
            : base(1, 2, 4, false)
        {
            setLogItemHandler += _SetLogItem;
            connectHandler += _Connect;
            disConnectHandler += _Disconnect;
            getLogsHandler += _GetLogs;
        }


        private bool _Connect(ref string errorMsg)
        {
            if (!axCZKEM1.Connect_Net(IP, Port.Value))
            {
                errorMsg = _getErrorMsg("Unable to connect the device");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void _Disconnect()
        {
            axCZKEM1.Disconnect();
        }

        private void _GetLogs(ref TmpUploadResult uploadRes, DateTime startDate, DateTime endDate, int idNo = -1)
        {
            uploadRes = new TmpUploadResult();

            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;

            int i = 1;

            endDate = endDate.AddHours(11).AddMinutes(59).AddSeconds(59);

            axCZKEM1.EnableDevice(_iMachineNumber, false);
            if (axCZKEM1.ReadGeneralLogData(_iMachineNumber))
            {
                while (axCZKEM1.SSR_GetGeneralLogData(_iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                {
                    DateTime l = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);

                    if (l >= startDate && l <= endDate && (idNo == -1 || (idNo == int.Parse(sdwEnrollNumber))))
                    {
                        clLogItem log = new clLogItem
                        {
                            LineNo = i,
                            IdNo = sdwEnrollNumber,
                            LogDate = l.ToString(),
                            Mode = idwInOutMode.ToString()
                        };

                        uploadRes.LogItems.Add(log);
                    }

                    i++;
                }
            }
            else
            {
                uploadRes.Exceptions.Add(new Exception(_getErrorMsg("Unable to get general log data")));
            }

            axCZKEM1.EnableDevice(_iMachineNumber, true);
        }

        private void _SetLogItem(ref clLogItem logItem, Excel.Range cells, int r, int _idNo_ColNo, int _logDate_ColNo, int _mode_ColNo)
        {
            logItem.IdNo = ((double)(cells[r, _idNo_ColNo] as Excel.Range).Value).ToString();
            logItem.LogDate = ((DateTime)(cells[r, _logDate_ColNo] as Excel.Range).Value).ToString();
            logItem.Mode = ((double)(cells[r, _mode_ColNo] as Excel.Range).Value).ToString();
        }

        private string _getErrorMsg(string msgPrefix)
        {
            int errorCode = 0;
            axCZKEM1.GetLastError(ref errorCode);

            string s = _getErrorDescription(errorCode);
            return msgPrefix + (string.IsNullOrEmpty(s) ? "" : ". " + s);
        }

        private string _getErrorDescription(int errorCode)
        {
            string ret = "";
            switch (errorCode)
            {
                case -100:
                    ret = "Operation failed or data not exist";
                    break;
                case -10:
                    ret = "Transmitted data length is incorrect";
                    break;
                case -5:
                    ret = "Data already exists";
                    break;
                case -4:
                    ret = "Space is not enough";
                    break;
                case -3:
                    ret = "Error size";
                    break;
                case -2:
                    ret = "Error in file read/write";
                    break;
                case -1:
                    ret = "SDK is not initialized and needs to be reconnected";
                    break;
                case 0:
                    ret = "Data not found or data repeated";
                    break;
                case 1:
                    ret = "Operation is correct";
                    break;
                case 4:
                    ret = "Parameter is incorrect";
                    break;
                case 101:
                    ret = "Error in allocating buffer";
                    break;
            }
            return ret;
        }
    }
}

