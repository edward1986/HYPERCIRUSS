using Module.DB;
using Module.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreApp
{
    public class ForceUpdateItem
    {
        public string userName;
        public string cacheType;
        public DateTime dt;
    }

    public class ForceUpdateManager
    {
        List<ForceUpdateItem> ForceUpdates = new List<ForceUpdateItem>();

        public void Register(string userName, string cacheType, DateTime dt)
        {
            ForceUpdateItem item = ForceUpdates.Where(x => x.userName == userName && x.cacheType == cacheType).FirstOrDefault();
            if (item == null)
            {
                item = new ForceUpdateItem
                {
                    userName = userName,
                    cacheType = cacheType,
                    dt = dt
                };

                ForceUpdates.Add(item);
            }
            else
            {
                item.dt = dt;
            }
        }

        public void Update(string cacheType, DateTime dt)
        {
            foreach (ForceUpdateItem item in ForceUpdates.Where(x => x.cacheType == cacheType))
            {
                item.dt = dt;
            }
        }

        public bool IsOutdated(string userName, string cacheType, DateTime cacheDate)
        {
            bool ret = true;

            ForceUpdateItem item = ForceUpdates.Where(x => x.userName == userName && x.cacheType == cacheType).FirstOrDefault();
            if (item == null)
            {
                Register(userName, cacheType, cacheDate);
            }
            else
            {
                ret = cacheDate < item.dt;
            }

            return ret;
        }
    }

    public static class Cache
    {
        public static ForceUpdateManager UpdateManager = new ForceUpdateManager();

        public static cachedUserAccess Get(string userName = "")
        {
            cachedUserAccess ret;

            if (HttpContext.Current.Session["UserAccess"] == null)
            {
                ret = new cachedUserAccess(userName);
                HttpContext.Current.Session["UserAccess"] = ret;

                UpdateManager.Register(ret.userName, "useraccess", ret.cacheDate);
            }
            else
            {
                ret = (cachedUserAccess)HttpContext.Current.Session["UserAccess"];

                if (UpdateManager.IsOutdated(ret.userName, "useraccess", ret.cacheDate))
                {
                    ret.Refresh();
                    ret.Cache();
                }
            }

            return ret;
        }

        public static void Clear()
        {
            HttpContext.Current.Session["UserAccess"] = null;
        }

        public static void Update()
        {
            UpdateManager.Update("useraccess", DateTime.Now);
        }

        public static cachedTables Get_Tables()
        {
            cachedTables ret;

            if (MvcApplication.CachedTables == null)
            {
                ret = new cachedTables();

                MvcApplication.CachedTables = ret;
                Module.DB.Procs.Common.Global_CachedTables = ret;
            }
            else
            {
                ret = MvcApplication.CachedTables;
            }

            return ret;
        }

        public static void Clear_Tables()
        {
            MvcApplication.CachedTables = null;
            Module.DB.Procs.Common.Global_CachedTables = null;
        }

        public static void Update_Tables(bool _settings = false, bool _countries = false, bool _offices = false, bool _Campuses = false, bool _departments = false, bool _positions = false, bool _groups = false)
        {
            cachedTables tables = Get_Tables();
            tables.Refresh(_settings, _countries, _offices, _Campuses, _departments, _positions, _groups);

            MvcApplication.CachedTables = tables;
            Module.DB.Procs.Common.Global_CachedTables = tables;
        }
    }

    public class cachedUserAccess : ICache
    {
        public DateTime cacheDate { get; set; }

        public string userName { get; set; }
        public UserAccess userAccess { get; set; }

        public cachedUserAccess(string _userName = "")
        {
            if (_userName == "")
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = _userName;
            }

            Refresh();
        }

        public void Refresh(bool _userAccess = false)
        {
            bool hasSpecified = _userAccess;

            if (!hasSpecified || _userAccess) userAccess = new UserAccess(userName);

            cacheDate = DateTime.Now;
        }

        public void Cache()
        {
            HttpContext.Current.Session["UserAccess"] = this;
        }
    }

}
