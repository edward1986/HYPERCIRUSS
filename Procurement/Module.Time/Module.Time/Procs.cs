using Module.Core;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Time
{
    public static class Procs
    {

        public static List<tblTMSetting> TMSettings(string effectivity, int officeId)
        {
            if (effectivity == ModuleConstants.DEFAULT_EFFECTIVITY)
            {
                return Module.DB.Procs.Common.Global_CachedTables.TMSettings.ToList();
            }
            else
            {
                DateTime dt = Convert.ToDateTime(effectivity);

                return TMSettings(dt, officeId);
            }
        }

        public static List<tblTMSetting> TMSettings(DateTime dt, int officeId)
        {
            List<tblTMSetting> ret = Module.DB.Procs.Common.Global_CachedTables.TMSettings.Select(x => x.Clone()).ToList();

            bool inTable = Module.DB.Procs.Common.Global_CachedTables.TMSettings_Effectivities.Any(x => x.OfficeId == officeId);

            List<DateTime> dates = Module.DB.Procs.Common.Global_CachedTables.TMSettings_Effectivities
                .Where(x => ((officeId == -1 || !inTable) && x.OfficeId == null) || (officeId != -1 && x.OfficeId == officeId))
                .Select(x => x.DateEffective)
                .Distinct()
                .OrderByDescending(x => x)
                .Where(x => x <= dt)
                .ToList();

            if (dates.Count == 0)
            {
                return ret;
            }

            foreach (tblTMSetting setting in ret)
            {
                foreach (DateTime _dt in dates)
                {
                    tblTMSettings_Effectivity eff = Module.DB.Procs.Common.Global_CachedTables.TMSettings_Effectivities
                        .Where(x => (((officeId == -1 || !inTable) && x.OfficeId == null) || (officeId != -1 && x.OfficeId == officeId)) && x.DateEffective == _dt && x.SettingId == setting.SettingId)
                        .OrderByDescending(x => x.OfficeId)
                        .FirstOrDefault();

                    if (eff != null)
                    {
                        setting.SettingValue = eff._Value;
                        setting.DateUpdated = _dt;
                        break;
                    }
                }
            }

            return ret;
        }

        public static void UpdateDr(ref tblEmployee_LeaveCredit lc)
        {
            double ret = 0;

            if (lc.IsPSAutoEntry)
            {
                ret = lc.Dr;
            }
            else if (lc.IsDrEntry)
            {
                DateTime startDate = lc.StartDate;
                DateTime endDate = lc.EndDate;
                bool includeRestDays = lc.IncludeRestDays;

                etPeriod period = new etPeriod(lc.EmployeeId, lc.StartDate, lc.EndDate);
                etDay day = period.Days.Where(x => x.d == startDate).Single();

                if (day.IsWorkDay || (day.IsRestDay && includeRestDays))
                {
                    ret += (lc.StartDate_IsHalfDay ? .5 : 1);
                }

                if (lc.StartDate < lc.EndDate)
                {
                    ret += period.Days.Where(x => x.d > startDate && x.d < endDate && (x.IsWorkDay || (day.IsRestDay && includeRestDays))).Count();

                    day = period.Days.Where(x => x.d == endDate).Single();

                    if (day.IsWorkDay || (day.IsRestDay && includeRestDays))
                    {
                        ret += (lc.EndDate_IsHalfDay ? .5 : 1);
                    }
                }
            }

            lc.Dr = ret;
        }
    }
}