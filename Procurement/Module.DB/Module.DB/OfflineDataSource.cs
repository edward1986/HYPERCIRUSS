
using coreLib.Enums;
using coreLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Module.DB
{
    public class UserPhoto
    {
        public int EmployeeId;
        public string PhotoString;

        public byte[] Photo
        {
            get
            {
                if (string.IsNullOrEmpty(PhotoString))
                {
                    return null;
                }
                else
                {
                    return Convert.FromBase64String(PhotoString);
                }
            }
        }
    }

    public class OfflineDataSource
    {
        public DateTime Version;
        public List<AspNetUserRole> AspNetUserRoles;
        public List<tblEmployee> Employees;
        public List<vwEmployee_Info_NoPhoto> EmployeeInfos = new List<vwEmployee_Info_NoPhoto>();
        public List<tblEmployee_Access> Accesses;
        public List<_HRPermission> Permissions;
        public List<tblEmployee_FP> EmployeeFPs;
        public List<UserPhoto> Photos = new List<UserPhoto>();
       
        public List<tblSettings_BaseModule> BaseSettings;
        public List<_Country> Countries;
        public List<tblOffice> Offices;
        public List<tblCampus> Campuses;
        public List<tblMFO> MFOs;
        public List<tblDepartment> Departments;
        public List<tblPosition> Positions;
        public List<tblSalaryGrade> SalaryGrades;
        public List<tblGroup> Groups;

        public List<TblPayrollAllowance> Allowance { get; set; }


        public PeriodModel pm = null;

        public OfflineDataSource() { }

        public OfflineDataSource(bool fill, PeriodModel period = null, bool _roles = false, bool _employees = false, bool _employee_careers = false, bool _emloyee_overloads = false, bool _employee_infos = false, bool _employee_accesses = false, bool _employee_permissions = false, bool _employee_fps = false, bool _employee_photos = false,
            bool _devices = false, bool _employee_device_idnos = false, bool _employee_schedules = false, bool _employee_schedule_segments = false, bool _employee_scheduleoverrides = false, bool _employee_scheduleoverride_segments = false,
            bool _employee_timelogs = false, bool _bulletinboard = false, bool _employee_leavecredits = false, bool _vwemployee_schedules = false,
            bool _basesettings = false, bool _countries = false, bool _offices = false, bool _departments = false, bool _positions = false, bool _salarygrades = false, bool _rates = false,
            bool _leaverules = false, bool _leavetypes = false, bool _leavetyperules = false, bool _tmsettings = false, bool _tmsettings_effectivities = false, bool _holidays = false, bool _scheduletemplates = false, bool _segmenttemplates = false,
            bool _groups = false, bool _globalschedule_segments = false, bool _gsis = false, bool _sss = false, bool _ph = false, bool _pagibig = false, bool _bir = false, bool _mfos = false, bool _employee_travels = false, bool _employee_ot = false, bool _employee_rd = false)
        {
            pm = period;

            if (fill)
            {
                this.Fill(_roles, _employees, _employee_careers, _emloyee_overloads, _employee_infos, _employee_accesses, _employee_permissions, _employee_fps, _employee_photos,
            _devices, _employee_device_idnos, _employee_schedules, _employee_schedule_segments, _employee_scheduleoverrides, _employee_scheduleoverride_segments,
            _employee_timelogs, _bulletinboard, _employee_leavecredits, _vwemployee_schedules,
            _basesettings, _countries, _offices, _departments, _positions, _salarygrades, _rates,
            _leaverules, _leavetypes, _leavetyperules, _tmsettings, _tmsettings_effectivities, _holidays, _scheduletemplates, _segmenttemplates,
            _groups, _globalschedule_segments, _gsis, _sss, _ph, _pagibig, _bir, _mfos, _employee_travels, _employee_ot, _employee_rd);
            }
        }

        public void Fill_Inverse(bool _roles = true, bool _employees = true, bool _employee_careers = true, bool _employee_overloads = true, bool _employee_infos = true, bool _employee_accesses = true, bool _employee_permissions = true, bool _employee_fps = true, bool _employee_photos = true,
            bool _devices = true, bool _employee_device_idnos = true, bool _employee_schedules = true, bool _employee_schedule_segments = true, bool _employee_scheduleoverrides = true, bool _employee_scheduleoverride_segments = true,
            bool _employee_timelogs = true, bool _bulletinboard = true, bool _employee_leavecredits = true, bool _vwemployee_schedules = true,
            bool _basesettings = true, bool _countries = true, bool _offices = true, bool _departments = true, bool _positions = true, bool _salarygrades = true, bool _rates = true,
            bool _leaverules = true, bool _leavetypes = true, bool _leavetyperules = true, bool _tmsettings = true, bool _tmsettings_effectivities = true, bool _holidays = true, bool _scheduletemplates = true, bool _segmenttemplates = true,
            bool _groups = true, bool _globalschedule_segments = true, bool _gsis = true, bool _sss = true, bool _ph = true, bool _pagibig = true, bool _bir = true, bool _mfos = true, bool _employee_travels = true, bool _employee_ot = true, bool _employee_rd = true)
        {
            this.Fill(_roles, _employees, _employee_careers, _employee_infos, _employee_accesses, _employee_permissions, _employee_fps, _employee_photos,
            _devices, _employee_device_idnos, _employee_schedules, _employee_schedule_segments, _employee_scheduleoverrides, _employee_scheduleoverride_segments,
            _employee_timelogs, _bulletinboard, _employee_leavecredits, _vwemployee_schedules,
            _basesettings, _countries, _offices, _departments, _positions, _salarygrades, _rates,
            _leaverules, _leavetypes, _leavetyperules, _tmsettings, _tmsettings_effectivities, _holidays, _scheduletemplates, _segmenttemplates,
            _groups, _globalschedule_segments, _gsis, _sss, _ph, _pagibig, _bir, _mfos, _employee_travels, _employee_ot, _employee_rd);
        }

        public void Fill(bool _roles = false, bool _employees = false, bool _employee_careers = false, bool _employee_overloads = false, bool _employee_infos = false, bool _employee_accesses = false, bool _employee_permissions = false, bool _employee_fps = false, bool _employee_photos = false,
            bool _devices = false, bool _employee_device_idnos = false, bool _employee_schedules = false, bool _employee_schedule_segments = false, bool _employee_scheduleoverrides = false, bool _employee_scheduleoverride_segments = false,
            bool _employee_timelogs = false, bool _bulletinboard = false, bool _employee_leavecredits = false, bool _vwemployee_schedules = false,
            bool _basesettings = false, bool _countries = false, bool _offices = false, bool _departments = false, bool _positions = false, bool _salarygrades = false, bool _rates = false,
            bool _leaverules = false, bool _leavetypes = false, bool _leavetyperules = false, bool _tmsettings = false, bool _tmsettings_effectivities = false, bool _holidays = false, bool _scheduletemplates = false, bool _segmenttemplates = false,
            bool _groups = false, bool _globalschedule_segments = false, bool _gsis = false, bool _sss = false, bool _ph = false, bool _pagibig = false, bool _bir = false, bool _mfos = false, bool _employee_travels = false, bool _employee_ot = false, bool _employee_rd = false)
        {

            bool hasSpecified = _roles || _employees || _employee_careers || _employee_overloads || _employee_infos || _employee_accesses || _employee_permissions || _employee_fps || _employee_photos ||
            _devices || _employee_device_idnos || _employee_schedules || _employee_schedule_segments || _employee_scheduleoverrides || _employee_scheduleoverride_segments ||
            _employee_timelogs || _bulletinboard || _employee_leavecredits || _vwemployee_schedules ||
            _basesettings || _countries || _offices || _departments || _positions || _salarygrades || _rates ||
            _leaverules || _leavetypes || _leavetyperules || _tmsettings || _tmsettings_effectivities || _holidays || _scheduletemplates || _segmenttemplates ||
            _groups || _globalschedule_segments || _gsis || _sss || _ph || _pagibig || _bir || _mfos || _employee_travels || _employee_ot || _employee_rd;

            using (dalDataContext context = new dalDataContext())
            {
                if (!hasSpecified || _roles) AspNetUserRoles = context.AspNetUserRoles.ToList();
                if (!hasSpecified || _employees) Employees = context.tblEmployees.ToList();
                if (!hasSpecified || _employee_infos) EmployeeInfos = context.tblEmployee_Infos.Select(x => x.vw()).ToList();
                if (!hasSpecified || _employee_accesses) Accesses = context.tblEmployee_Accesses.ToList();
                if (!hasSpecified || _employee_permissions) Permissions = context._HRPermissions.ToList();
                if (!hasSpecified || _employee_fps) EmployeeFPs = context.tblEmployee_FPs.ToList();
                if (!hasSpecified || _employee_photos) Photos = context.AspNetUsers_Photos.Select(x => new UserPhoto { EmployeeId = x.EmployeeId, PhotoString = x.GetPhotoString() }).ToList();
               
                if (!hasSpecified || _basesettings) BaseSettings = context.tblSettings_BaseModule.OrderBy(x => x.Setting).ToList();
                if (!hasSpecified || _countries) Countries = context._Countries.ToList();
                if (!hasSpecified || _offices) Offices = context.tblOffices.ToList();
                if (!hasSpecified || _mfos) MFOs = context.tblMFOs.ToList();
                if (!hasSpecified || _departments) Departments = context.tblDepartments.ToList();
                if (!hasSpecified || _positions) Positions = context.tblPositions.ToList();
                if (!hasSpecified || _groups) Groups = context.tblGroups.ToList();
                                
                Version = context.getDateTime().Value;
            }
        }
    }
}