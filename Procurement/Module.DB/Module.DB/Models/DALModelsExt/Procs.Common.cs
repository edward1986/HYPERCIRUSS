using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.DB.Procs
{
    
    public static class Common
    {
        private static cachedTables _Global_CachedTables;

        public static cachedTables Global_CachedTables
        {
            get
            {
                if (_Global_CachedTables == null)
                {
                    return new cachedTables();
                }
                else
                {
                    return _Global_CachedTables;
                }
            }
            set
            {
                _Global_CachedTables = value;
            }
        }
                    

        public static string getAddress(string address, int? countryId, int? postalCode)
        {
            _Country country = getCountry(countryId ?? -1);
            string strCountry = country == null ? "" : country.Name;

            return coreLib.Procs.getAddress(address, strCountry, postalCode);
        }

        public static _Country getCountry(int id)
        {
            return Global_CachedTables.Countries.Where(x => x.Id == id).SingleOrDefault();
        }
              
        public static string getUser(string userId)
        {
            string ret = "";

            using (dalDataContext context = new dalDataContext())
            {
                var tmp = context.tblEmployees.Where(x => x.UserId == userId).SingleOrDefault();
                if (tmp == null)
                {
                    var tmp2 = Account.GetUser(userId);
                    if (tmp2 != null)
                    {
                        ret = "[" + tmp2.UserName + "]";
                    }
                }
                else
                {
                    ret = tmp.FullName;
                }
            }

            return ret;
        }


        public static string LeavePeriod(DateTime StartDate, DateTime EndDate, bool StartDate_IsHalfDay, bool EndDate_IsHalfDay, bool IncludeRestDays)
        {
            string ret = "";

            if (StartDate.Date == EndDate.Date)
            {
                ret = StartDate.ToString("MM/dd/yyyy");

                if (StartDate_IsHalfDay)
                {
                    ret += " [Half Day]";
                }
            }
            else
            {
                ret = string.Format("{0}{1} - {2}{3}",
                    StartDate.ToString("MM/dd/yyyy"),
                    StartDate_IsHalfDay ? " [Half Day]" : "",
                    EndDate.ToString("MM/dd/yyyy"),
                    EndDate_IsHalfDay ? " [Half Day]" : ""
                    );
            }

            ret += IncludeRestDays ? " [Including Rest Days]" : "";

            return ret;
        }

        public static string PeriodText(DateTime StartDate, DateTime EndDate)
        {
            string ret = "";

            if (StartDate.Date == EndDate.Date)
            {
                ret = StartDate.ToString("MM/dd/yyyy");
            }
            else
            {
                ret = string.Format("{0} - {1}",
                    StartDate.ToString("MM/dd/yyyy"),
                    EndDate.ToString("MM/dd/yyyy")
                    );
            }

            return ret;
        }
    }

    public static class Account
    {
        public static AspNetUser GetUser(string id = "", string email = "", string userName = "")
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.AspNetUsers.Where(x => x.Id == id || x.Email == email || x.UserName == userName).SingleOrDefault();
            }
        }

        public static void DisableAccount(string id, bool disable)
        {
            using (dalDataContext context = new dalDataContext())
            {
                AspNetUser u = context.AspNetUsers.Where(x => x.Id == id).SingleOrDefault();
                if (u != null)
                {
                    u.Disabled = disable;
                    context.SubmitChanges();
                }
            }
        }

        public static bool IsAccountDisabled(string id = "", string email = "", string userName = "")
        {
            bool ret = true;

            AspNetUser u = GetUser(id, email, userName);
            if (u != null)
            {
                ret = u.Disabled == true;
            }

            return ret;
        }
    }
}