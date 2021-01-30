using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace coreLib
{
    public static class Procs
    {
        public static bool Ping(string ip)
        {
            try
            {
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(ip);

                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        public static string Ellipsis(string str, int maxNoOfCharacters)
        {
            if (str.Length <= maxNoOfCharacters)
            {
                return str;
            }
            else
            {
                return str.Substring(0, maxNoOfCharacters) + "...";
            }
        }

        public static DateTime getDate(DateTime? v)
        {
            if (v == null)
            {
                return getDate();
            }
            else
            {
                return getDate(v.Value.Hour, v.Value.Minute, v.Value.Second);
            }
        }

        public static DateTime getDate(DateTime baseDate, DateTime? v)
        {
            if (v == null)
            {
                return getDate();
            }
            else
            {
                return getDate(baseDate, v.Value.Hour, v.Value.Minute, v.Value.Second);
            }
        }

        public static DateTime getDate(int hours = 0, int minutes = 0, int seconds = 0)
        {
            return getDate(null, hours, minutes, seconds);
        }

        public static DateTime getDate(DateTime? baseDate, int hours = 0, int minutes = 0, int seconds = 0)
        {
            if (baseDate == null) baseDate = Constants.DEFAULT_DATETIME;
            return baseDate.Value.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }

        public static string getAddress(string address, string country, int? postalCode)
        {
            string ret = "";
            if (!string.IsNullOrEmpty(address)) ret += address;

            if (!string.IsNullOrEmpty(country))
            {
                ret += (string.IsNullOrEmpty(ret) ? "" : ", ") + country;
            }

            if (postalCode != null)
            {
                ret += (string.IsNullOrEmpty(ret) ? "" : ", ") + postalCode.Value.ToString();
            }

            return ret;
        }

        public static string getFullName(string LastName, string FirstName, string MiddleName, string NameExt)
        {
            string ret = "";

            if ((LastName ?? "").Trim() != "" || (FirstName ?? "").Trim() != "" || (MiddleName ?? "").Trim() != "" || (NameExt ?? "").Trim() != "")
            {
                string mn = "";
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    if (MiddleName.Trim() != "")
                    {
                        List<string> _mn = MiddleName.Trim().Split(' ').ToList();
                        mn = " " + _mn.Last().ToString()[0].ToString() + ".";
                    }
                }

                ret = string.Format("{0}{1}, {2}{3}",
                    LastName ?? "",
                    string.IsNullOrEmpty(NameExt) ? "" : (" " + NameExt),
                    FirstName ?? "",
                    mn
                    );
            }
            return ret;
        }

        public static string getFullName_FN(string LastName, string FirstName, string MiddleName, string NameExt)
        {
            string ret = "";

            if ((LastName ?? "").Trim() != "" || (FirstName ?? "").Trim() != "" || (MiddleName ?? "").Trim() != "" || (NameExt ?? "").Trim() != "")
            {
                string mn = "";
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    if (MiddleName.Trim() != "")
                    {
                        List<string> _mn = MiddleName.Trim().Split(' ').ToList();
                        mn = _mn.Last().ToString()[0].ToString() + ". ";
                    }                    
                }

                ret = string.Format("{0} {1}{2}{3}",
                    FirstName ?? "",
                    mn,
                    LastName ?? "",
                    string.IsNullOrEmpty(NameExt) ? "" : (" " + NameExt)
                    );
            }
            return ret;
        }

        public static string friendlyPeriod(DateTime d1, DateTime d2, bool fullMonth = false)
        {
            string ret = "";

            d1 = d1.Date;
            d2 = d2.Date;

            string mo = fullMonth ? "MMMM" : "MMM";

            if (d1 == d2)
            {
                ret = d1.ToString(mo + " dd, yyyy");
            }
            else if (d1.Month == d2.Month && d1.Year == d1.Year)
            {
                ret = string.Format("{0}-{1}", d1.ToString(mo + " d"), d2.ToString("d, yyyy"));
            }
            else if (d1.Year == d1.Year)
            {
                ret = string.Format("{0}-{1}", d1.ToString(mo + " d"), d2.ToString(mo + " d, yyyy"));
            }
            else
            {
                ret = string.Format("{0}-{1}", d1.ToString(mo + " d, yyyy"), d2.ToString(mo + " d, yyyy"));
            }

            return ret;
        }

        public static string friendlyDate(DateTime d)
        {
            string ret = "";

            DateTime n = DateTime.Now.Date;
            TimeSpan ts = n - d.Date;

            if (d < n.AddYears(-1))
            {
                ret = "Last " + d.ToString("yyyy");
            }
            else if (d < n.AddMonths(-1))
            {
                ret = "Last " + d.ToString("MMMM");
            }
            else if (ts.TotalDays > 1)
            {
                ret = ts.TotalDays + " days ago";
            }
            else if (ts.TotalDays == 1)
            {
                ret = "Yesterday";
            }
            else if (ts.TotalHours >= 1)
            {
                ret = ts.TotalHours + " hours ago";
            }
            else if (ts.TotalMinutes > 5)
            {
                ret = ts.TotalMinutes + " minutes ago";
            }
            else
            {
                ret = "Just now";
            }

            return ret;
        }

        public static string randomString(int length = 5, bool randomCase = true)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString = "";

            Random random = new Random(System.DateTime.Now.Millisecond);
            Random rCase = new Random();

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    char c = (char)intRandomNumber;
                    string s = "";

                    if (randomCase)
                    {
                        if (rCase.Next(0, 2) == 0) s = c.ToString().ToLower(); else s = c.ToString().ToUpper();
                    }

                    strCaptchaString = strCaptchaString + s;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString;
        }
    }
}