using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreApp
{
    public static class coreProcs
    {
        public static bool hasPermission(string controller = "", string controllers = "")
        {
            if (controller == "")
            {
                controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }

            return (controllers ?? "").Split(',').Contains(controller) ||
                controller == "EmployeePhotos" ||
                controller == "MyPhotos";
        }

        public static string getRouteParam(HttpRequestBase Request, string paramName = "id", string defaultValue = "-1")
        {
            string id = defaultValue;
            if (Request.RequestContext.RouteData.Values[paramName] != null)
            {
                id = Request.RequestContext.RouteData.Values[paramName].ToString();
            }
            else if (Request.QueryString[paramName] != null)
            {
                id = Request.QueryString[paramName].ToString();
            }

            return id;
        }

        public static string ShowErrors(ModelStateDictionary ModelState, string delimeter = "\n")
        {
            string ret = "";

            if (ModelState.Values.ToArray().Any(x => x.Errors.Count > 0))
            {
                for (int i = ModelState.Values.Count - 1; i >= 0; i--)
                {
                    foreach (var err in ModelState.Values.ToArray()[i].Errors)
                    {
                        ret += (!String.IsNullOrEmpty(ret) ? delimeter : "") + err.ErrorMessage;
                    }
                }
            }

            return ret;
        }

        public static string ShowErrors(Exception e, string delimeter = "\n")
        {
            string ret = "";

            if (e != null)
            {
                ret += e.Message;
                ret += (!String.IsNullOrEmpty(ret) ? delimeter : "") + ShowErrors(e.InnerException, delimeter);
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

       
        public static string getBaseModuleSetting(string setting, string category)
        {
            return Cache.Get_Tables().BaseSettings.Where(x => x.Setting == setting && x.Category == category).FirstOrDefault().SettingValue;
            
            
        }

        public static void sendEmail(string receipientEmail, string senderEmail, string subject, string message)
        {

            string host = getBaseModuleSetting("Host", "Mail");
            int port = int.Parse(getBaseModuleSetting("Port", "Mail"));
            string username = getBaseModuleSetting("User", "Mail");
            string password = getBaseModuleSetting("Password", "Mail");
            bool usedefaultcredentials = bool.Parse(getBaseModuleSetting("UseDefaultCredentials", "Mail"));
            bool enablessl = bool.Parse(getBaseModuleSetting("EnableSSL", "Mail"));

            (new WebData()).sendEmail(senderEmail, receipientEmail, subject, message, host, port, username, password, usedefaultcredentials, enablessl);
        }
    }
}
