using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace coreLib.Extensions
{

    public static class Extensions
    {
        public static bool WithIn(this DateTime dt, DateTime startDate, DateTime endDate)
        {
            return dt >= startDate && dt <= endDate;
        }

        public static bool WithIn(this DateTime? dt, DateTime startDate, DateTime endDate)
        {
            if (dt == null)
            {
                return false;
            }
            else
            {
                return dt.Value >= startDate && dt.Value <= endDate;
            }
        }

        public static string SentenceCase(this string str, bool firstCharacterOnly = false)
        {
            var lowerCase = str.ToLower();
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            return r.Replace(firstCharacterOnly ? str : lowerCase, s => s.Value.ToUpper());
        }

        public static string Right(this string str, int count)
        {
            string ret = str;

            if (count < str.Length)
            {
                ret = str.Substring(str.Length - count);
            }

            return ret;
        }

        public static string ToCurrencyString(this double obj, string altZero = "", bool negativeIsEncloseInParenthesis = false, bool blankIfZero = false)
        {
            return _ToCurrencyString(obj, altZero, negativeIsEncloseInParenthesis, blankIfZero);
        }

        public static string ToCurrencyString(this double? obj, string altZero = "", bool negativeIsEncloseInParenthesis = false, bool blankIfZero = false)
        {
            return _ToCurrencyString((obj ?? 0), altZero, negativeIsEncloseInParenthesis, blankIfZero);
        }

        private static string _ToCurrencyString(double v, string altZero = "", bool negativeIsEncloseInParenthesis = false, bool blankIfZero = false)
        {
            if (v == 0 && blankIfZero)
            {
                return "";
            }
            else if (v == 0 && altZero != "")
            {
                return altZero;
            }
            else
            {
                if (v < 0 && negativeIsEncloseInParenthesis)
                {
                    return string.Format("({0})", Math.Abs(v).ToString("#,##0.00"));
                }
                else
                {
                    return v.ToString("#,##0.00");
                }
            }
        }

        public static string ToCleanString(this object obj)
        {
            return (obj ?? "").ToString().ToCleanString();
        }

        public static string ToCleanString(this string str)
        {
            return (str ?? "").ToLower().Trim();
        }

        public static string ToMultilineString(this string str)
        {
            return (str ?? "").Replace("\n", "\r\n");
        }

        public static string RemoveCR_NewLine(this string str)
        {
            return (str ?? "").Replace("\r", "").Replace("\n", "");
        }

        public static string ToBytes(this long value, bool byOneThousand = false)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            int order = 0;
            while (value >= 1024 && order < sizes.Length - 1)
            {
                order++;
                value = value / (byOneThousand ? 1000 : 1024);
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0.##} {1}", value, sizes[order]);
        }
    }    

}
