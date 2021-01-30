using System;

namespace coreLib
{
    public static class Constants
    {
        public static DateTime DEFAULT_DATETIME = new DateTime(1900, 1, 1);

        public const string DEVICE_REFERENCE_FORMAT = "DEVICE_ID-{0}";
        public const string KIOSK_REFERENCE_FORMAT = "KIOSK-{0}-{1}";
        public const string KIOSK_REFERENCE_USB_FORMAT = "KIOSK-{0}-USB";
        public const string KIOSK_REFERENCE_LAN_FORMAT = "KIOSK-{0}-LAN";
        public const string GEO_REFERENCE_FORMAT = "GEO-{0}";
    }
}