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
    public partial class zks_s5 : Form, deviceForm
    {
        public zks_s5()
        {
            InitializeComponent();
        }

        public int getErrorCode()
        {
            int errorCode = -1;
            axSB100BPC1.GetLastError(ref errorCode);
            
            return errorCode;
        }

        public TmpUploadResult GetLogs(int n, string ip, int port, int pw, DateTime startDate, DateTime endDate)
        {
            TmpUploadResult ret = new TmpUploadResult();

            int i = 1;

            int vDiv = 65536;
            int vAttStatus, vAntipass;
            //string stAttStatus, stAntipass;

            int vTMachineNumber = -1;
            int vSMachineNumber = -1;
            int vSEnrollNumber = -1;
            int vVerifyMode = -1;
            int vYear = -1;
            int vMonth = -1;
            int vDay = -1;
            int vHour = -1;
            int vMinute = -1;

            if (axSB100BPC1.ConnectTcpip(n, ref ip, port, pw))
            {

                axSB100BPC1.EnableDevice(n, 0);

                if (axSB100BPC1.ReadAllGLogData(n))
                {
                    while (axSB100BPC1.GetAllGLogData(n, ref vTMachineNumber, ref vSEnrollNumber, ref vSMachineNumber, ref vVerifyMode, ref vYear, ref vMonth, ref vDay, ref vHour, ref vMinute))
                    {
                        vAntipass = vVerifyMode / vDiv;
                        vVerifyMode = vVerifyMode % vDiv;
                        vAttStatus = vVerifyMode / 256;
                        vVerifyMode = vVerifyMode % 256;


                        DateTime l = new DateTime(vYear, vMonth, vDay, vHour, vMinute, 0);

                        if (l >= startDate && l <= endDate)
                        {
                            clLogItem log = new clLogItem
                            {
                                LineNo = i,
                                IdNo = vSEnrollNumber.ToString(),
                                LogDate = l.ToString(),
                                Mode = vAttStatus == 4 ? "0" : "1"
                            };

                            ret.LogItems.Add(log);
                        }

                        i++;
                    }
                }

                axSB100BPC1.EnableDevice(n, 1);
                axSB100BPC1.Disconnect();

            }

            return ret;
        }
    }
}
