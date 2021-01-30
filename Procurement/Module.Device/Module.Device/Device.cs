

using Module.DB.Enums;

namespace Module.Device
{
    public delegate void Device_OnConnected(int deviceIndex, int dbId);
    public delegate void Device_OnDisconnected(int deviceIndex, int dbId);
    public delegate void Device_OnMessage(int deviceIndex, int dbId, string message);

    public enum DeviceStatus
    {
        Connected = 0,
        Disconnected = 1
    }

    public interface IDevice
    {
        Device_OnConnected OnConnected { get; set; }
        Device_OnDisconnected OnDisconnected { get; set; }
        Device_OnMessage OnMessage { get; set; }
        bool Connect();
        void Disconnect();
        TerminalDeviceType Type { get; }
    }

    public static class Common
    {
        //public static int getMode(string mode)
        //{
        //    int ret;

        //    if (mode.ToLower().Trim() == "in" || mode.ToLower().Trim() == "0")
        //    {
        //        ret = (int)DeviceLogMode.In;
        //    }
        //    else if (mode.ToLower().Trim() == "out" || mode.ToLower().Trim() == "1")
        //    {
        //        ret = (int)DeviceLogMode.Out;
        //    }
        //    else if (mode.ToLower().Trim() == "break" || mode.ToLower().Trim() == "2")
        //    {
        //        ret = (int)DeviceLogMode.Break;
        //    }
        //    else if (mode.ToLower().Trim() == "resume" || mode.ToLower().Trim() == "3")
        //    {
        //        ret = (int)DeviceLogMode.Resume;
        //    }
        //    else if (mode.ToLower().Trim() == "ot" || mode.ToLower().Trim() == "4")
        //    {
        //        ret = (int)DeviceLogMode.OTIn;
        //    }
        //    else if (mode.ToLower().Trim() == "done" || mode.ToLower().Trim() == "5")
        //    {
        //        ret = (int)DeviceLogMode.OTOut;
        //    }
        //    else
        //    {
        //        ret = (int)DeviceLogMode.Other;
        //    }

        //    return ret;
        //}

        public static string getErrorDescription(int errorCode)
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