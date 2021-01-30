
using coreLib.Encryption;
using Module.DB;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UareU.Enrollment_Post
{
    public static class Offline
    {
        public static OfflineDataSource Data;
    }

    public static class Common
    {
        public static bool FoceExit = false;

        public static bool SaveOnDB(int employeeId, int finger, string value)
        {
            string param = string.Format("employeeId={0}&finger={1}&value={2}",
                employeeId,
                finger,
                HttpUtility.UrlEncode(value)
                );

            try
            {
                new coreLib.Objects.WebData().postDataToUrl(Properties.Settings.Default.PostDataUrl, param);
                return true;
            }
            catch
            {
                return false;
            }           
        }
    }

    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public bool FP1 { get; set; }
        public bool FP2 { get; set; }
        public bool FP3 { get; set; }
        public bool FP4 { get; set; }
        public bool FP5 { get; set; }
        public bool FP6 { get; set; }
        public bool FP7 { get; set; }
        public bool FP8 { get; set; }
        public bool FP9 { get; set; }
        public bool FP10 { get; set; }
    }
    
    public class KioskDataManager
    {

        public void LoadDataFile()
        {
            string folderPath = Properties.Settings.Default.OfflineDataPath;
            string filePath = Path.Combine(folderPath, Properties.Settings.Default.DatasourceFilename);

            if (File.Exists(filePath))
            {
                string str;

                using (StreamReader sr = new StreamReader(filePath))
                {
                    str = sr.ReadToEnd();
                }

                str = new Sym().Decrypt(str);

                using (TextReader tr = new StringReader(str))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(OfflineDataSource));
                    Offline.Data = (OfflineDataSource)xmlSerializer.Deserialize(tr);
                }
            }
        }
    }

}
