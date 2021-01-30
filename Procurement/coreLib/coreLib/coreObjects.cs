using coreLib.Enums;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace coreLib.Objects
{
    public class ItemIdsObject
    {
        public string Value { get; set; }
        public string Details { get; set; }
    }
    
    public class SelectListItemExt : SelectListItem
    {
        public Hashtable Data { get; set; }
        public bool CanSpecifyDetails { get; set; }
        public string Details { get; set; }
    }

    public delegate void ProgressIteration(int current, int total);

    public class ProgressData
    {
        public double Total { get; set; }
        public double Current { get; set; }
        public bool IsProgressUpdate { get; set; } = true;
        public int Level { get; set; }

        public double Percentage
        {
            get
            {
                if (Total == 0)
                {
                    return 0;
                }
                else
                {
                    return (Current / Total) * 100;
                }
            }
        }

        public ProgressData()
        {
            Current = 0;
            Total = 0;
            Level = 1;
        }

        public string JSONString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Raw_ActionResult : ActionResult
    {
        public string contents { get; set; }

        public Raw_ActionResult(string contents)
        {
            this.contents = contents;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.Write(contents);
            context.HttpContext.Response.Flush();
        }
    }

    public class MimeMappingStealer
    {
        // The get mime mapping method info
        private readonly MethodInfo _getMimeMappingMethod = null;

        /// <summary>
        /// Static constructor sets up reflection.
        /// </summary>
        public MimeMappingStealer()
        {
            // Load hidden mime mapping class and method from System.Web
            var assembly = Assembly.GetAssembly(typeof(HttpApplication));
            Type mimeMappingType = assembly.GetType("System.Web.MimeMapping");
            _getMimeMappingMethod = mimeMappingType.GetMethod("GetMimeMapping",
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        }

        /// <summary>
        /// Exposes the hidden Mime mapping method.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The mime mapping.</returns>
        public string GetMimeMapping(string fileName)
        {
            return (string)_getMimeMappingMethod.Invoke(null /*static method*/, new[] { fileName });
        }
    }

    public class WebData
    {
        public string getDataFromUrl(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                return wc.DownloadString(url);
            }
        }

        public string postDataToUrl(string url, string param)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return wc.UploadString(url, param);
            }

        }

        public void sendEmail(string sender, string receipient, string subject, string message, string host, int port, string username, string password, bool usedefaultcredentials, bool enablessl)
        {
            try
            {
                var loginInfo = new NetworkCredential(username, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient(host, port);

                msg.From = new MailAddress(sender);
                msg.To.Add(new MailAddress(receipient));
                msg.Subject = subject.ToString().Replace("\n", "");
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = enablessl;
                smtpClient.UseDefaultCredentials = usedefaultcredentials;
                smtpClient.Credentials = loginInfo;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(msg);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

    public class PeriodModel
    {
        [Required]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        [Display(Name = "End Date")]

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }

            set
            {
                _EndDate = value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
        }

        public PeriodModelInitType initType { get; set; }

        public PeriodModel()
        { }

        public PeriodModel(string startDate, string endDate)
        {
            initType = PeriodModelInitType.Custom;

            StartDate = DateTime.Parse(startDate);
            EndDate = DateTime.Parse(endDate);

        }

        public PeriodModel(DateTime startDate, DateTime endDate)
        {
            initType = PeriodModelInitType.Custom;

            StartDate = startDate;
            EndDate = endDate;

        }

        public PeriodModel(PeriodModelInitType initType)
        {
            this.initType = initType;
            DateTime n = DateTime.Now;

            bool IsDec = n.Month == 12;

            switch (initType)
            {
                case PeriodModelInitType.Custom:
                    StartDate = n;
                    EndDate = n;
                    break;
                case PeriodModelInitType.ThisMonth:
                    StartDate = new DateTime(n.Year, n.Month, 1);
                    if (IsDec)
                    {
                        EndDate = new DateTime(n.Year, 12, 31);
                    }
                    else
                    {
                        EndDate = new DateTime(n.Year, n.Month + 1, 1).AddDays(-1);
                    }

                    break;
                case PeriodModelInitType.ThisWeek:
                    StartDate = n.AddDays(-1 * (int)n.DayOfWeek);
                    EndDate = StartDate.AddDays(6);
                    break;
                case PeriodModelInitType.ThisYear:
                    StartDate = new DateTime(n.Year, 1, 1);
                    EndDate = new DateTime(n.Year, 12, 31);
                    break;
                case PeriodModelInitType.Last30Days:
                    StartDate = n.AddDays(-30);
                    EndDate = n;
                    break;
                case PeriodModelInitType.Today:
                    StartDate = n.Date;
                    EndDate = n.Date;
                    break;
                case PeriodModelInitType.Last2Months:
                    StartDate = n.AddMonths(-2);
                    EndDate = n;
                    break;
            }

            EndDate = EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public bool Within(DateTime dt)
        {
            return dt >= StartDate && dt <= EndDate;
        }

    }

    public class ImageTasks
    {
        public string ImageToString(string path)
        {
            Image im = Image.FromFile(path);

            using (MemoryStream ms = new MemoryStream())
            {
                im.Save(ms, im.RawFormat);
                byte[] array = ms.ToArray();

                return Convert.ToBase64String(array);
            }
        }

        public Image StringToImage(string imageString)
        {
            byte[] array = Convert.FromBase64String(imageString);

            using (MemoryStream ms = new MemoryStream(array))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
