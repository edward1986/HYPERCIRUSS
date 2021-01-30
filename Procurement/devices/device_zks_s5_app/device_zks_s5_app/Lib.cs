using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace device_zks_s5_app
{
    public class clLogItem
    {
        public int LineNo;
        public Exception Exception;
        public string IdNo;
        public string LogDate;
        public string Mode;
    }

    public static class Lib
    {
        public static string getErrorDescription(int errorCode)
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

        
        public static bool ping(string ip)
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
    }
}
