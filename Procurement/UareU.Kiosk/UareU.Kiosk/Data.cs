using Module.DB;
using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using coreLib.Encryption;
using System.Windows.Forms;
using Module.Core;

namespace UareU.Kiosk
{
    public static class Offline
    {
        public static OfflineDataSource Data;
    }

    public class KioskDataManager
    {
        public string getServerTime()
        {
            string url = Common.GetTimeUrl();
            return new coreLib.Objects.WebData().getDataFromUrl(url);
        }

        public EmployeeModel getEmployeeModel(int employeeId)
        {
            EmployeeModel ret = new EmployeeModel { employeeId = employeeId };

            tblEmployee employee = Offline.Data.Employees.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
            if (employee != null)
            {
                ret.employee = employee.FullName;

                tblEmployee_Career career = new Module.DB.Procs.offline_procs_tblEmployee(employee, ref Offline.Data).LatestCareer();
                if (career != null)
                {
                    tblOffice office = career.Office;
                    if (office != null)
                    {
                        ret.office = office.Office;
                    }

                    tblPosition position = career.Position;
                    if (position != null)
                    {
                        ret.position = position.Position;
                    }
                }

                string photoPath = Path.Combine(Properties.Settings.Default.PhotosPath, employee.UserId + ".jpg");
                if (File.Exists(photoPath))
                {
                    ret.photo = Image.FromFile(photoPath);
                }

                //UserPhoto p = Offline.Data.Photos.Where(x => x.UserId == employee.UserId).SingleOrDefault();
                //if (p != null)
                //{
                //    if (p.Photo != null)
                //    {
                //        byte[] arr = p.Photo.ToArray();
                //        Image photo;

                //        using (MemoryStream mem = new MemoryStream(arr))
                //        {
                //            photo = Image.FromStream(mem);
                //        }

                //        ret.photo = photo;
                //    }
                //}
            }

            return ret;
        }

        public void LoadDataFile()
        {
            string folderPath = Common.GetOfflineDataPath();
            string filePath = Path.Combine(folderPath, Common.DATASOURCE_FILENAME);

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

    public class TimeLogManager
    {
        frmMain form;

        public TimeLogManager(frmMain form)
        {
            this.form = form;
        }
        
        public void SaveTimeLog(EmployeeModel em)
        {
            try
            {
                SaveOnFile(em);
            }
            catch { }

            bool success = false;

            if (form.IsOnline)
            {
                try
                {
                    success = PostTimeLog(em);
                }
                catch
                {
                    form.IsOnline = false;
                }
            }

            if (!form.IsOnline || !success)
            {
                SaveOnFile(em, true);
            }
            
        }

        private void SaveOnFile(EmployeeModel em, bool offlineLog = false)
        {
            string strTimelog_Date = em.timelog.ToString("yyyyMMdd");
            string strTimelog_Time = em.timelog.ToString("HH:mm:ss");
            string strMode = em.mode == (int)TimeLogMode.In ? "in" : "out";
            string strTimebase = em.localtime ? "local-time" : "server-time";
            string strTerminalId = FixedSettings.TerminalId.ToString();

            string tFilename = string.Format("{0}{1}-{2}.txt", offlineLog ? "offline-log-" : "", strTimelog_Date, strTerminalId);

            string folderPath = Common.GetTimeLogsPath();
            string filePath = Path.Combine(folderPath, tFilename);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string log = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                strTimelog_Time,
                em.employeeId,
                em.employee.ToUpper(),
                strMode,
                strTimebase,
                strTerminalId,
                em.devicetype
                );

            if (offlineLog)
            {
                saveEncryptedText(filePath, em.timelog, log);
            }
            else
            {
                savePlainText(filePath, em.timelog, log);
            }
        }

        private void saveEncryptedText(string filePath, DateTime timelog, string log)
        {
            string s = "";

            Sym sym = new Sym();

            if (File.Exists(filePath))
            {
                using(StreamReader sr = new StreamReader(filePath))
                {
                    s = sr.ReadToEnd();
                    s = sym.Decrypt(s);
                }
            }
            else
            {
                s = string.Format("[{0}]", timelog.ToString("MM/dd/yyyy"));
            }

            s += (s == "" ? "" : Environment.NewLine) + log;
            
            using (StreamWriter sw = File.CreateText(filePath))
            {
                s = sym.Encrypt(s);
                sw.Write(s);
            }
        }

        private void savePlainText(string filePath, DateTime timelog, string log)
        {
            StreamWriter sw;

            if (File.Exists(filePath))
            {
                sw = File.AppendText(filePath);
            }
            else
            {
                sw = File.CreateText(filePath);

                string dateInfo = string.Format("[{0}]", timelog.ToString("MM/dd/yyyy"));
                sw.WriteLine(dateInfo);
            }

            sw.WriteLine(log);

            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public bool PostTimeLog(EmployeeModel em)
        {
            string param = string.Format("terminalId={0}&employeeId={1}&timeLog={2}&mode={3}&deviceType={4}", 
                FixedSettings.TerminalId,
                em.employeeId,
                em.timelog,
                em.mode,
                em.devicetype
                );

            param = new EncryptDecryptQueryString().Encrypt(param, "r0b1nr0y");


            string url = Common.GetPostDataUrl() + "?" + param;
                        
            string s = new coreLib.Objects.WebData().getDataFromUrl(url);

            return s == "success";
        }

       
    }

}