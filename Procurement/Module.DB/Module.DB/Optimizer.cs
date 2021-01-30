using Module.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.DB
{
    public class cachedTables : ICache, ICacheTables
    {
        OfflineDataSource offlineDS;
        bool useOffline
        {
            get
            {
                return offlineDS != null;
            }
        }

        public DateTime cacheDate { get; set; }

        public List<tblSettings_BaseModule> BaseSettings { get; set; } = new List<tblSettings_BaseModule>();
        public List<_Country> Countries { get; set; } = new List<_Country>();
        public List<tblCampus> Campuses { get; set; } = new List<tblCampus>();
        public List<tblOffice> Offices { get; set; } = new List<tblOffice>();
        public List<tblMFO> MFOs { get; set; } = new List<tblMFO>();
        public List<tblDepartment> Departments { get; set; } = new List<tblDepartment>();
        public List<tblPosition> Positions { get; set; } = new List<tblPosition>();
        public List<tblGroup> Groups { get; set; } = new List<tblGroup>();

        public cachedTables(OfflineDataSource offlineDS = null)
        {
            this.offlineDS = offlineDS;
            Refresh();
        }

        public void Refresh(bool _settings = false, bool _countries = false, bool _offices = false, bool _campuses = false, bool _departments = false,
            bool _positions = false, bool _groups = false)
        {
            bool hasSpecified = _settings || _countries || _offices || _campuses || _departments || _positions || _groups;

            if (useOffline)
            {
                if (!hasSpecified || _settings) BaseSettings = offlineDS.BaseSettings;
                if (!hasSpecified || _countries) Countries = offlineDS.Countries;
                if (!hasSpecified || _offices) Offices = offlineDS.Offices;
                if (!hasSpecified || _campuses) Campuses = offlineDS.Campuses;
                if (!hasSpecified || _departments) Departments = offlineDS.Departments;
                if (!hasSpecified || _positions) Positions = offlineDS.Positions;
                if (!hasSpecified || _groups) Groups = offlineDS.Groups;

                cacheDate = DateTime.Now;
            }
            else
            {

                using (dalDataContext context = new dalDataContext())
                {
                    if (!hasSpecified || _settings) BaseSettings = context.tblSettings_BaseModule.OrderBy(x => x.Setting).ToList();
                    if (!hasSpecified || _countries) Countries = context._Countries.ToList();
                    if (!hasSpecified || _offices) Offices = context.tblOffices.ToList();
                    if (!hasSpecified || _campuses) Campuses = context.tblCampus.ToList();
                    if (!hasSpecified || _departments) Departments = context.tblDepartments.ToList();
                    if (!hasSpecified || _positions) Positions = context.tblPositions.ToList();
                    if (!hasSpecified || _groups) Groups = context.tblGroups.ToList();

                    cacheDate = context.getDateTime().Value;
                }


            }
        }
    }
}
