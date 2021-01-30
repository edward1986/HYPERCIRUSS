using Aspose.Words;
using coreApp.DAL;
using Microsoft.AspNet.Identity;
using Module.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace coreApp
{
    public static class SeparableList
    {
        public static List<tblEmployee_Children> GetList(List<tblEmployee_Children> src, bool getExtraList = false)
        {
            int limit = 11;
            if (src.Count < limit) limit = src.Count;

            List<tblEmployee_Children> ret = new List<tblEmployee_Children>();

            if (src.Any(x => x.OnSeparatePage == true))
            {
                bool onextra = false;
                foreach(var rec in src)
                {
                    if (getExtraList)
                    {
                        if (onextra || rec.OnSeparatePage == true)
                        {
                            onextra = true;
                            ret.Add(rec);
                        }
                    }
                    else
                    {
                        if (rec.OnSeparatePage != true)
                        {
                            ret.Add(rec);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (getExtraList)
                {
                    for (int i = limit + 1; i <= src.Count; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
                else
                {
                    for (int i = 1; i <= limit; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
            }

            return ret;
        }
        public static List<tblEmployee_Educ> GetList(List<tblEmployee_Educ> src, bool getExtraList = false)
        {
            int limit = 4;
            if (src.Count < limit) limit = src.Count;

            List<tblEmployee_Educ> ret = new List<tblEmployee_Educ>();

            if (src.Any(x => x.OnSeparatePage == true))
            {
                bool onextra = false;
                foreach (var rec in src)
                {
                    if (getExtraList)
                    {
                        if (onextra || rec.OnSeparatePage == true)
                        {
                            onextra = true;
                            ret.Add(rec);
                        }
                    }
                    else
                    {
                        if (rec.OnSeparatePage != true)
                        {
                            ret.Add(rec);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (getExtraList)
                {
                    for (int i = limit + 1; i <= src.Count; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
                else
                {
                    for (int i = 1; i <= limit; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
            }

            return ret;
        }
        public static List<tblEmployee_CivilService> GetList(List<tblEmployee_CivilService> src, bool getExtraList = false)
        {
            int limit = 7;
            if (src.Count < limit) limit = src.Count;

            List<tblEmployee_CivilService> ret = new List<tblEmployee_CivilService>();

            if (src.Any(x => x.OnSeparatePage == true))
            {
                bool onextra = false;
                foreach (var rec in src)
                {
                    if (getExtraList)
                    {
                        if (onextra || rec.OnSeparatePage == true)
                        {
                            onextra = true;
                            ret.Add(rec);
                        }
                    }
                    else
                    {
                        if (rec.OnSeparatePage != true)
                        {
                            ret.Add(rec);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (getExtraList)
                {
                    for (int i = limit + 1; i <= src.Count; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
                else
                {
                    for (int i = 1; i <= limit; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
            }

            return ret;
        }
        public static List<tblEmployee_Training> GetList(List<tblEmployee_Training> src, bool getExtraList = false)
        {
            int limit = 21;
            if (src.Count < limit) limit = src.Count;

            List<tblEmployee_Training> ret = new List<tblEmployee_Training>();

            if (src.Any(x => x.OnSeparatePage == true))
            {
                bool onextra = false;
                foreach (var rec in src)
                {
                    if (getExtraList)
                    {
                        if (onextra || rec.OnSeparatePage == true)
                        {
                            onextra = true;
                            ret.Add(rec);
                        }
                    }
                    else
                    {
                        if (rec.OnSeparatePage != true)
                        {
                            ret.Add(rec);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (getExtraList)
                {
                    for (int i = limit + 1; i <= src.Count; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
                else
                {
                    for (int i = 1; i <= limit; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
            }

            return ret;
        }
        public static List<tblEmployee_WorkExp> GetList(List<tblEmployee_WorkExp> src, bool getExtraList = false)
        {
            int limit = 28;
            if (src.Count < limit) limit = src.Count;

            List<tblEmployee_WorkExp> ret = new List<tblEmployee_WorkExp>();

            if (src.Any(x => x.OnSeparatePage == true))
            {
                bool onextra = false;
                foreach (var rec in src)
                {
                    if (getExtraList)
                    {
                        if (onextra || rec.OnSeparatePage == true)
                        {
                            onextra = true;
                            ret.Add(rec);
                        }
                    }
                    else
                    {
                        if (rec.OnSeparatePage != true)
                        {
                            ret.Add(rec);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (getExtraList)
                {
                    for (int i = limit + 1; i <= src.Count; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
                else
                {
                    for (int i = 1; i <= limit; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
            }

            return ret;
        }
        public static List<tblEmployee_Voluntary> GetList(List<tblEmployee_Voluntary> src, bool getExtraList = false)
        {
            int limit = 7;
            if (src.Count < limit) limit = src.Count;

            List<tblEmployee_Voluntary> ret = new List<tblEmployee_Voluntary>();

            if (src.Any(x => x.OnSeparatePage == true))
            {
                bool onextra = false;
                foreach (var rec in src)
                {
                    if (getExtraList)
                    {
                        if (onextra || rec.OnSeparatePage == true)
                        {
                            onextra = true;
                            ret.Add(rec);
                        }
                    }
                    else
                    {
                        if (rec.OnSeparatePage != true)
                        {
                            ret.Add(rec);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (getExtraList)
                {
                    for (int i = limit + 1; i <= src.Count; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
                else
                {
                    for (int i = 1; i <= limit; i++)
                    {
                        ret.Add(src[i - 1]);
                    }
                }
            }

            return ret;
        }
    }

    public static class Procs
    {
        public static int GetNoOfMonthsCovered(DateTime startDate, DateTime endDate)
        {
            int ret = 1;

            DateTime d = startDate;
            int m = d.Month;

            while (d <= endDate)
            {
                if (d.Month != m)
                {
                    m = d.Month;
                    ret++;
                }

                d = d.AddDays(1);
            }

            return ret;
        }

        public static void setTick(bool value, string bookmark, Aspose.Words.Document wordDoc)
        {
            object saveWithDocument = true;
            object oMissing = System.Reflection.Missing.Value;

            string checkedPath = Path.Combine(getServerPath(), "Assets", "images", "checked.png");
            string unCheckedPath = Path.Combine(getServerPath(), "Assets", "images", "unchecked.png");

            DocumentBuilder builder = new DocumentBuilder(wordDoc);
            builder.MoveToBookmark(bookmark);
            builder.InsertImage(value ? checkedPath : unCheckedPath, 12, 12);
        }

       

        public static string getServerPath()
        {
            return HttpContext.Current.Server.MapPath("~/");
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

        public static void accessDenied(AuthorizationContext filterContext, string msg)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            RouteData rd = new RouteData();
            string url = "/";

            filterContext.Controller.TempData["GlobalError"] = msg;

            if (request.UrlReferrer != null)
            {
                url = request.UrlReferrer.AbsoluteUri;
            }
            else
            {
                rd.DataTokens.Add("area", "");
                rd.Values.Add("controller", "Home");
                rd.Values.Add("action", "Index");
            }

            filterContext.Controller.TempData["GlobalDestination"] = rd;
            filterContext.Result = new RedirectResult(url);
        }

        public static string getFullName(string LastName, string FirstName, string MiddleName, string NameExt)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(LastName) || !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(MiddleName) || !string.IsNullOrEmpty(NameExt))
            {
                string mn = "";
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    List<string> _mn = MiddleName.Split(' ').ToList();
                    mn = " " + _mn.Last().ToString()[0].ToString() + ".";
                }

                ret = string.Format("{0}{1}, {2}{3}",
                    LastName,
                    string.IsNullOrEmpty(NameExt) ? "" : (" " + NameExt),
                    FirstName,
                    mn
                    );
            }
            return ret;
        }

        public static string getFullName_FN(string LastName, string FirstName, string MiddleName, string NameExt)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(LastName) || !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(MiddleName) || !string.IsNullOrEmpty(NameExt))
            {
                string mn = "";
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    List<string> _mn = MiddleName.Split(' ').ToList();
                    mn = _mn.Last().ToString()[0].ToString() + ". ";
                }

                ret = string.Format("{0} {1}{2}{3}",
                    FirstName,
                    mn,
                    LastName,
                    string.IsNullOrEmpty(NameExt) ? "" : (" " + NameExt)
                    );
            }
            return ret;
        }

        public static string getAddress(string address, int? countryId, int? postalCode)
        {
            string ret = "";
            if (!string.IsNullOrEmpty(address)) ret += address;

            if (countryId != null)
            {
                ret += (string.IsNullOrEmpty(ret) ? "" : ", ") + getCountry(countryId.Value).Name;
            }

            if (postalCode != null)
            {
                ret += (string.IsNullOrEmpty(ret) ? "" : ", ") + postalCode.Value.ToString();
            }

            return ret;
        }

        public static _Country getCountry(int id)
        {
            return Cache.Get_Tables().Countries.Where(x => x.Id == id).SingleOrDefault();
        }
        
    }


    public static class Account
    {

        public static IdentityResult CreateUser(ApplicationUserManager userManager, Models.NewUserModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

            var result = userManager.Create(user, model.Password);

            if (result.Succeeded)
            {
                result = SaveUserRoles(userManager, user.Id, model.Roles);

                Module.DB.Procs.Account.DisableAccount(user.Id, model.Disabled);
            }

            return result;
        }

        public static IdentityResult EditUser(ApplicationUserManager userManager, Models.UserModel model)
        {
            IdentityResult result;

            var user = userManager.FindById(model.Id);
            if (user == null)
            {
                result = new IdentityResult(new string[] { "User Id not found" });
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                result = SaveUserRoles(userManager, user.Id, model.Roles);

                Module.DB.Procs.Account.DisableAccount(user.Id, model.Disabled);
            }

            return result;
        }

        public static IdentityResult DeleteUser(ApplicationUserManager userManager, string Id)
        {
            var result = RemoveUserRoles(userManager, Id);

            if (result.Succeeded)
            {
                var user = userManager.FindById(Id);

                result = userManager.Delete(user);

            }

            return result;
        }

        public static IdentityResult SaveUserRoles(ApplicationUserManager userManager, string Id, string[] newRoles)
        {
            var userRoles = userManager.GetRoles(Id);

            newRoles = newRoles ?? new string[] { };

            var result = userManager.AddToRoles(Id, newRoles.Except(userRoles).ToArray<string>());

            if (result.Succeeded)
            {
                result = userManager.RemoveFromRoles(Id, userRoles.Except(newRoles).ToArray<string>());
            }

            return result;
        }

        public static IdentityResult RemoveUserRoles(ApplicationUserManager userManager, string userId)
        {
            var userRoles = userManager.GetRoles(userId);
            var result = userManager.RemoveFromRoles(userId, userRoles.ToArray());

            return result;
        }
    }
}
