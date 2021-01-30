using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devices;

namespace zks_console
{
    public partial class ino1_a : Form, deviceForm
    {
        public ino1_a()
        {
            InitializeComponent();
        }

        public int getErrorCode()
        {
            int errorCode = -1;
            axCZKEM1.GetLastError(ref errorCode);
            
            return errorCode;
        }

        public TmpUploadResult GetLogs(int n, string ip, int port, int pw, DateTime startDate, DateTime endDate)
        {
            TmpUploadResult ret = new TmpUploadResult();

            int i = 1;

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
            
            axCZKEM1.EnableDevice(n, false);
            if (axCZKEM1.ReadGeneralLogData(n))
            {
                while (axCZKEM1.SSR_GetGeneralLogData(n, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                {
                    DateTime l = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);

                    if (l >= startDate && l <= endDate)
                    {
                        clLogItem log = new clLogItem
                        {
                            LineNo = i,
                            IdNo = sdwEnrollNumber,
                            LogDate = l.ToString(),
                            Mode = idwInOutMode.ToString()
                        };
                                                
                        ret.LogItems.Add(log);
                    }

                    i++;
                }
            }

            axCZKEM1.EnableDevice(n, true);
            axCZKEM1.Disconnect();

            return ret;
        }
    }
}
