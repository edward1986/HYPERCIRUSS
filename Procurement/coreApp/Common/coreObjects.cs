using coreApp.DAL;
using coreApp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace coreApp
{
    public class ModuleUrlParameter
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public ModuleUrlParameter_RouteValue RouteValues { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ModuleUrlParameter_RouteValue
    {
        public string parentUrl { get; set; }
        public string area { get; set; }
        public string id { get; set; }
    }

    public class Breadcrumb
    {
        public string Description { get; set; }
        public string Link { get; set; }
    }


    public class WebData
    {
        public string getDataFromUrl(string url)
        {
            string res = "";

            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                res = webClient.DownloadString(url);
            }

            return res;
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

    public static class LayoutUtility
    {
        public static string MenuSelector(bool hasSubMenu, string areas, string includedControllers, string excludedControllers = "")
        {
            string ret = "";

            string currentArea = "[NONE]";
            string currentController = "";

            if (HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"] != null)
            {
                currentArea = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            }

            if (HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] != null)
            {
                currentController = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }

            bool controllersOk = (includedControllers == "" || includedControllers.Split(',').Contains(currentController)) &&
                (excludedControllers == "" || !excludedControllers.Split(',').Contains(currentController));


            if (areas.Split(',').Contains(currentArea) && controllersOk)
            {
                ret = "active";

                //if (hasSubMenu)
                //{
                //    ret += " open";
                //}
            }

            return ret;
        }
    }

    public class objPaging
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public int totalCount { get; set; }
        public int currentCount { get; set; }
        public int totalPages { get; set; }
        public string url { get; set; }

        public objPaging(int pageNo, int pageSize, int totalCount)
        {
            totalPages = Decimal.ToInt32(Math.Ceiling((decimal)totalCount / pageSize));

            if (pageNo < 1) this.pageNo = 1;
            else if (pageNo > totalPages && totalPages > 0) this.pageNo = totalPages;
            else this.pageNo = pageNo;

            this.pageSize = pageSize;
            this.totalCount = totalCount;

            skip = pageSize * (this.pageNo - 1);
            url = HttpContext.Current.Request.Url.AbsolutePath;
        }
    }

    public class List
    {
        public string url_details { get; set; }
        public objPaging paging { get; set; }
        public object list { get; set; }
    }

    public class queryResult
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
        public string Remarks { get; set; }
        public string Err { get; set; }
        public bool IsProgressUpdate { get; set; } = false;
    }

    public static class FixedSettings
    {
        public static string ApplicationName = "HR Integrated System";
        public static string DefaultPassword;
        public static string AgencyName;
        public static string AgencyAddress;
        public static string AgencyCode;
        public static long PhotoFileSize;
        public static string PhotoFileTypes;
        public static long SupportFileSize;
        public static string SupportFileTypes;
        public static bool UseAspose;
        public static bool UseMFO;
        public static bool UseStartInGovService;
        public static long EmployeeFileSize;
        public static string EmployeeFixedFolders;
        public static bool RatesBySalaryGrade;

        public static bool DTRByCutOff;
        public static bool TwinDTR;
        public static bool DTRHeader;
        public static bool DTROTCols;
        public static bool DTRTotalCols;

        public static DateTime LeaveCalculationReferenceDate;

        public static bool IncludeContractors;
        public static bool UseAgencyShare;
        public static int EquipmentLife;
        public static int SemiExpendableLife;
        public static bool UseCommissionDefs;

        public static int AutoLogout;

        public static bool ProcurementForPrivate;

        public static void Init()
        {
            DefaultPassword = coreProcs.getBaseModuleSetting("DefaultPassword", "General");
            AgencyName = coreProcs.getBaseModuleSetting("AgencyName", "General");
            AgencyCode = coreProcs.getBaseModuleSetting("AgencyCode", "General");
            AgencyAddress = coreProcs.getBaseModuleSetting("AgencyAddress", "General");

            PhotoFileSize = Convert.ToInt32(coreProcs.getBaseModuleSetting("PhotoFileSize", "General"));
            PhotoFileTypes = coreProcs.getBaseModuleSetting("PhotoFileTypes", "General");

            SupportFileSize = Convert.ToInt32(coreProcs.getBaseModuleSetting("SupportFileSize", "General"));
            SupportFileTypes = coreProcs.getBaseModuleSetting("SupportFileTypes", "General");

            UseAspose = Convert.ToBoolean(coreProcs.getBaseModuleSetting("UseAspose", "General"));
            UseMFO = Convert.ToBoolean(coreProcs.getBaseModuleSetting("UseMFO", "General"));
            UseStartInGovService = Convert.ToBoolean(coreProcs.getBaseModuleSetting("UseStartInGovService", "General"));

            EmployeeFileSize = Convert.ToInt32(coreProcs.getBaseModuleSetting("EmployeeFileSize", "General"));
            EmployeeFixedFolders = coreProcs.getBaseModuleSetting("EmployeeFixedFolders", "General");

            RatesBySalaryGrade = Convert.ToBoolean(coreProcs.getBaseModuleSetting("RatesBySalaryGrade", "General"));

            DTRByCutOff = Convert.ToBoolean(coreProcs.getBaseModuleSetting("DTRByCutOff", "Reports"));
            TwinDTR = Convert.ToBoolean(coreProcs.getBaseModuleSetting("TwinDTR", "Reports"));
            DTRHeader = Convert.ToBoolean(coreProcs.getBaseModuleSetting("DTRHeader", "Reports"));
            DTROTCols = Convert.ToBoolean(coreProcs.getBaseModuleSetting("DTROTCols", "Reports"));
            DTRTotalCols = Convert.ToBoolean(coreProcs.getBaseModuleSetting("DTRTotalCols", "Reports"));

            IncludeContractors = Convert.ToBoolean(coreProcs.getBaseModuleSetting("IncludeContractors", "SAMS"));

            LeaveCalculationReferenceDate = Convert.ToDateTime(coreProcs.getBaseModuleSetting("LeaveCalculationReferenceDate", "General"));
            UseAgencyShare = Convert.ToBoolean(coreProcs.getBaseModuleSetting("UseAgencyShare", "General"));
            UseCommissionDefs = Convert.ToBoolean(coreProcs.getBaseModuleSetting("UseCommissionDefs", "General"));
            EquipmentLife = Convert.ToInt32(coreProcs.getBaseModuleSetting("EquipmentLife", "SAMS"));
            SemiExpendableLife = Convert.ToInt32(coreProcs.getBaseModuleSetting("SemiExpendableLife", "SAMS"));


           AutoLogout = Convert.ToInt32(coreProcs.getBaseModuleSetting("AutoLogout", "General"));

            ProcurementForPrivate = false;
        }
    }

    public class objAgencyShare
    {
        public bool HasShare { get; set; }
        public double AgencyShare { get; set; }
        public double WTax { get; set; }
        public double Accrued { get; set; }

        public objAgencyShare(tblPayrollSummary ps, List<tblPayrollSummary_Employee> psEmployees, double agencyShareWTaxRate)
        {
            AgencyShare = 0;
            WTax = 0;
            Accrued = 0;

            HasShare = ps.AgencyShare != null;

            if (HasShare)
            {
                AgencyShare = psEmployees.Sum(x => x._TotalWorkingDaysCount) * (ps.AgencyShare ?? 0);
                WTax = AgencyShare * (agencyShareWTaxRate / 100);
                Accrued = AgencyShare - WTax;
            }
        }
    }

    public class objSummary
    {
        public double Salaries { get; set; }
        public double OT_SalAdj_Others { get; set; }
        public Dictionary<string, double> Allowances { get; set; }
        public double SSS_ER { get; set; }
        public double PH_ER { get; set; }
        public double HDMF_ER { get; set; }
        public double SSS { get; set; }
        public double PH { get; set; }
        public double HDMF { get; set; }
        public Dictionary<string, double> Deductions { get; set; }
        public double CA { get; set; }
        public double OD { get; set; }
        public double WTax { get; set; }

        public double CashInBank
        {
            get
            {
                return Total1 - _Total2;
            }
        }

        public double Total1
        {
            get
            {
                return Salaries + OT_SalAdj_Others + Allowances.Sum(x => x.Value) + SSS_ER + PH_ER + HDMF_ER;
            }
        }

        public double _Total2
        {
            get
            {
                return SSS + PH + HDMF + Deductions.Sum(x => x.Value) + CA + OD + WTax;
            }
        }

        public double Total2
        {
            get
            {
                return _Total2 + CashInBank;
            }
        }

        public objSummary(tblPayrollSummary ps, List<tblPayrollSummary_Employee> psEmployees, List<string> tAllowances, List<double> vAllowances, List<List<string>> tDeductions, List<List<double>> vDeductions)
        {
            Salaries = psEmployees.Sum(x => (x._fin_GrossSalary ?? 0) - (x._fin_AbsencesTardiness ?? 0) - (x._fin_Breaks ?? 0));
            OT_SalAdj_Others = psEmployees.Sum(x => x._fin_OT ?? 0) + psEmployees.Sum(x => x._fin_OtherIncome ?? 0) + psEmployees.Sum(x => x._fin_SA ?? 0);
            SSS_ER = psEmployees.Sum(x => (x._fin_SSS_ER ?? 0) + (x._fin_SSS_EC ?? 0));
            PH_ER = psEmployees.Sum(x => x._fin_PH_ER ?? 0);
            HDMF_ER = psEmployees.Sum(x => x._fin_PAGIBIG_ER ?? 0);
            SSS = SSS_ER + psEmployees.Sum(x => x._fin_SSS ?? 0);
            PH = PH_ER + psEmployees.Sum(x => x._fin_PH ?? 0);
            HDMF = HDMF_ER + psEmployees.Sum(x => x._fin_PAGIBIG ?? 0);
            CA = psEmployees.Sum(x => x._fin_CA ?? 0);
            OD = psEmployees.Sum(x => x._fin_Overdraft ?? 0);
            WTax = psEmployees.Sum(x => x._fin_WTax ?? 0);

            Allowances = new Dictionary<string, double>();
            Deductions = new Dictionary<string, double>();

            for (int i = 0; i < tAllowances.Count; i++)
            {
                Allowances.Add(tAllowances[i], vAllowances[i]);
            }

            for (int t = 0; t < tDeductions.Count; t++)
            {
                for (int i = 0; i < tDeductions[t].Count; i++)
                {
                    Deductions.Add(tDeductions[t][i], vDeductions[t][i]);
                }
            }
        }
    }

    //public static class RouteUtils
    //{
    //    public static RouteData GetRouteDataByUrl(string url)
    //    {
    //        return RouteTable.Routes.GetRouteData(new RewritedHttpContextBase(url));
    //    }

    //    private class RewritedHttpContextBase : HttpContextBase
    //    {
    //        private readonly HttpRequestBase mockHttpRequestBase;

    //        public RewritedHttpContextBase(string appRelativeUrl)
    //        {
    //            this.mockHttpRequestBase = new MockHttpRequestBase(appRelativeUrl);
    //        }


    //        public override HttpRequestBase Request
    //        {
    //            get
    //            {
    //                return mockHttpRequestBase;
    //            }
    //        }

    //        private class MockHttpRequestBase : HttpRequestBase
    //        {
    //            private readonly string appRelativeUrl;

    //            public MockHttpRequestBase(string appRelativeUrl)
    //            {
    //                this.appRelativeUrl = appRelativeUrl;
    //            }

    //            public override string AppRelativeCurrentExecutionFilePath
    //            {
    //                get { return appRelativeUrl; }
    //            }

    //            public override string PathInfo
    //            {
    //                get { return ""; }
    //            }
    //        }
    //    }
    //}
}
