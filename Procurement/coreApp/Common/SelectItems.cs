using coreApp.DAL;
using coreApp.Enums;
using Module.DB.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp
{
    public static class SelectItems
    {
        public static List<SelectListItem> getCountries()
        {
            List<SelectListItem> _countries = new List<SelectListItem>();
            _countries.AddRange(Cache.Get_Tables().Countries.ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }));

            return _countries;
        }

        public static List<SelectListItem> getCareerEvents(bool forEndEvent = false)
        {
            List<SelectListItem> _events = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(CareerEvent)))
            {
                if (forEndEvent)
                {
                    if (i < (int)CareerEvent.Terminated)
                    {
                        i++;
                        continue;
                    }
                }
                else
                {
                    if (i >= (int)CareerEvent.Terminated) break;
                }

                _events.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _events;
        }

        public static List<SelectListItem> getEducLevel()
        {
            List<SelectListItem> _level = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(EducLevel)))
            {
                _level.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _level;
        }

        public static List<SelectListItem> getLDTypes()
        {
            List<SelectListItem> _level = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(LDType)))
            {
                _level.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _level;
        }

        public static List<SelectListItem> getHolidayTypes()
        {
            List<SelectListItem> _holidays = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(HolidayType)))
            {
                _holidays.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _holidays;
        }

        public static List<SelectListItem> getMonthDays()
        {
            List<SelectListItem> _days = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };

            for (int i = 1; i <= 31; i++)
            {
                _days.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return _days;
        }

        public static List<SelectListItem> getMonths()
        {
            List<SelectListItem> _months = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };

            for (int i = 1; i <= 12; i++)
            {
                _months.Add(new SelectListItem() { Text = new DateTime(1900, i, 1).ToString("MMMM"), Value = i.ToString() });
            }

            return _months;
        }

        public static List<SelectListItem> getYears_ForHolidays()
        {
            List<SelectListItem> _years = new List<SelectListItem>() { new SelectListItem() { Text = "[REPEAT EVERY YEAR]", Value = "-1", Selected = true } };

            for (int i = 2010; i <= 2030; i++)
            {
                _years.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return _years;
        }

        public static List<SelectListItem> getMaxLevel(int max = -1)
        {
            if (max == -1)
            {
                max = 10;
            }

            List<SelectListItem> _levels = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };

            for (int i = 1; i <= max; i++)
            {
                _levels.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return _levels;
        }

        public static List<SelectListItem> getSalaryGrades()
        {
            List<SelectListItem> _levels = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "", Selected = true } };

            for (int i = 1; i <= 33; i++)
            {
                _levels.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return _levels;
        }

        public static List<SelectListItem> getMFOs(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _MFOs = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _MFOs.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            _MFOs.AddRange(
                Cache.Get_Tables().MFOs.OrderBy(x => x.Description).ToList().Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                );

            return _MFOs;
        }

        public static List<SelectListItem> getCampuses(string defaultText = "", bool showAll = false, int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _campuses = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _campuses.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            _campuses.AddRange(
                Cache.Get_Tables().Campuses.OrderBy(x => x.Campus).ToList().Where(x => showAll || Cache.Get().userAccess.AllowedCampus(x.CampusID)).Select(x => new SelectListItem() { Text = x.Campus, Value = x.CampusID.ToString(), Selected = x.CampusID == defaultValue })
                );

            return _campuses;
        }

        public static List<SelectListItem> getOffices(string defaultText = "", bool showAll = false, int? defaultValue = -1, bool showEmptyItem = true, bool showCampus = true)
        {
            List<SelectListItem> _Offices = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Offices.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }



            _Offices.AddRange(
                Cache.Get_Tables().Offices.OrderBy(x => x.Office).ToList().Where(x => showAll || Cache.Get().userAccess.AllowedOffice(x.OfficeId)).Select(x => new SelectListItem() { Text = x.Office,  Value = (showCampus ? x.CampusID.ToString() + "," : "") + x.OfficeId, Selected = x.OfficeId == defaultValue })
                );

            return _Offices;
        }

        public static List<SelectListItem> getDepartments(string defaultText = "", bool showAll = false, int defaultOffice = -1, int defaultValue = -1,  bool showEmptyItem = true)
        {
            List<SelectListItem> _Departments = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Departments.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            _Departments.AddRange(
                Cache.Get_Tables().Departments.OrderBy(x => x.Department).ToList().Select(x => new SelectListItem() { Text = x.Department, Value = x.OfficeId.ToString() + "," + x.DepartmentId.ToString(), Selected = x.DepartmentId == defaultValue })
                );

            return _Departments;
        }

        public static List<SelectListItem> getGroups(string defaultText = "", bool showAll = false, int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Groups = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Groups.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            _Groups.AddRange(
                Cache.Get_Tables().Groups.OrderBy(x => x.GroupName).Select(x => new SelectListItem() { Text = x.GroupName, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                );

            return _Groups;
        }

        public static List<SelectListItem> getPositions(string defaultText = "", bool showAll = false, int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Positions = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Positions.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            _Positions.AddRange(
                Cache.Get_Tables().Positions.OrderBy(x => x.Position).Select(x => new SelectListItem() { Text = x.Position, Value = x.PositionId.ToString(), Selected = x.PositionId == defaultValue })
                );

            return _Positions;
        }

        public static List<SelectListItem> getPayrollTypes()
        {
            List<SelectListItem> _payrolltypes = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(PayrollType)))
            {
                string txt = s;
                if (s == "_13thMP") txt = "13th Month Pay";
                _payrolltypes.Add(new SelectListItem() { Text = txt.Replace("_", " "), Value = i.ToString()});

                i++;
            }

            return _payrolltypes;
        }

        public static List<SelectListItem> getUseRatesMode()
        {
            List<SelectListItem> _modes = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(UseRatesMode)))
            {
                _modes.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _modes;
        }

        public static List<SelectListItem> getYears(int? maxYear = null, int? minYear = null, bool showEmptyItem = true)
        {
            List<SelectListItem> _years = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _years.Add(new SelectListItem() { Text = "", Value = "-1", Selected = true });
            }

            int max = maxYear == null ? DateTime.Today.Year : maxYear.Value;
            int min = minYear == null ? 1940 : minYear.Value;

            for (int i = max; i >= min; i--)
            {
                _years.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return _years;
        }

        public static List<SelectListItem> getGender()
        {
            List<SelectListItem> _gender = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(Gender)))
            {
                _gender.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _gender;
        }

        public static List<SelectListItem> getBIRStatus()
        {
            List<SelectListItem> _birstatus = new List<SelectListItem>();

            foreach (string s in System.Enum.GetNames(typeof(BIRStatus)))
            {
                _birstatus.Add(new SelectListItem() { Text = s, Value = s });
            }

            return _birstatus;
        }

        public static List<SelectListItem> getCivilStatus()
        {
            List<SelectListItem> _status = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(CivilStatus)))
            {
                _status.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _status;
        }

        public static List<SelectListItem> getBloodType()
        {
            List<SelectListItem> _bloodtype = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(BloodType)))
            {
                _bloodtype.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _bloodtype;
        }

        public static List<SelectListItem> getEmploymentType(string defaultText = "", bool showAll = false, int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _employmenttype = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _employmenttype.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(EmploymentType)))
            {
                _employmenttype.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _employmenttype;
        }

        public static List<SelectListItem> getActive(string defaultText = "", bool showAll = false, string defaultValue = "", bool showEmptyItem = true)
        {
            List<SelectListItem> _items = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _items.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "", Selected = defaultValue == "" });
            }

            _items.Add(new SelectListItem() { Text = "Active", Value = "active" });
            _items.Add(new SelectListItem() { Text = "In-Active", Value = "inactive" });

            return _items;
        }

        public static List<SelectListItem> getNationalities()
        {
            List<SelectListItem> _nationalities = new List<SelectListItem>();
            _nationalities.AddRange(Cache.Get_Tables().Countries.ToList().Where(x => !string.IsNullOrEmpty(x.Nationality)).OrderBy(x => x.Nationality).Select(x => new SelectListItem() { Text = x.Nationality, Value = x.Id.ToString() }));

            return _nationalities;
        }

        public static List<SelectListItem> getPositions()
        {
            List<SelectListItem> _positions = new List<SelectListItem>();
            _positions.AddRange(Cache.Get_Tables().Positions.OrderBy(x => x.Position).Select(x => new SelectListItem() { Text = x.Position, Value = x.PositionId.ToString() }));

            return _positions;
        }

        public static List<SelectListItem> getStatusOfAppointment()
        {
            List<SelectListItem> _status = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(StatusOfAppointment)))
            {
                _status.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _status;
        }

        public static List<SelectListItem> getPayModes(int selectedValue = -1)
        {
            List<SelectListItem> _modes = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(PayMode)))
            {
                _modes.Add(new SelectListItem() { Text = s.Replace("_", " "), Value = i.ToString(), Selected = selectedValue == i });
                i++;
            }

            return _modes;
        }
        public static List<SelectListItem> getOverloadTypes(int selectedValue = 0)
        {
            List<SelectListItem> _modes = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(Enums.OverloadTypes)))
            {
                _modes.Add(new SelectListItem() { Text = s.Replace("_", " "), Value = i.ToString(), Selected = selectedValue == i });
                i++;
            }

            return _modes;
        }

        public static List<SelectListItem> getContributionOptions(int selectedValue = 0)
        {
            List<SelectListItem> _modes = new List<SelectListItem>();

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(ContributionOptions)))
            {
                _modes.Add(new SelectListItem() { Text = s, Value = i.ToString(), Selected = selectedValue == i });
                i++;
            }

            return _modes;
        }

        public static List<SelectListItem> getContributions(int selectedValue = -1)
        {
            List<SelectListItem> _modes = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(Contributions)))
            {
                _modes.Add(new SelectListItem() { Text = s, Value = i.ToString(), Selected = selectedValue == -1 });
                i++; 
            }

            return _modes;
        }

        public static List<SelectListItem> getLoans()
        {
            using (hr2017DataContext context = new hr2017DataContext())
            {
                List<SelectListItem> loans = new List<SelectListItem>();
                //List<SelectListItem> loans = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "-1", Selected = true } };
                loans.AddRange(context.tblFin_LoanDefs.OrderBy(x => x.Description).Select(x => new SelectListItem() { Text = x.Description, Value = x.LoanId.ToString() }));

                return loans;
            }
        }
              
        public static List<SelectListItem> getAllowances(string defaultText = "", bool showAll = false, int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Items = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Items.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            using (hr2017DataContext context = new hr2017DataContext())
            {
                _Items.AddRange(
                    context.tblFin_AllowanceDefs.OrderBy(x => x.Description).Select(x => new SelectListItem() { Text = x.Description, Value = x.AllowanceId.ToString(), Selected = x.AllowanceId == defaultValue })
                );
            }
            return _Items;
        }

        public static List<SelectListItem> getDeductions(string defaultText = "", bool showAll = false, int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Items = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Items.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            using (hr2017DataContext context = new hr2017DataContext())
            {
                _Items.AddRange(
                    context.tblFin_DeductionDefs.OrderBy(x => x.Description).Select(x => new SelectListItem() { Text = x.Description, Value = x.DeductionId.ToString(), Selected = x.DeductionId == defaultValue })
                );
            }
            return _Items;
        }

        public static List<SelectListItem> getDeductionType()
        {
            List<SelectListItem> _DeductionType = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(DeductionTypes)))
            {
                _DeductionType.Add(new SelectListItem() { Text = s, Value = i.ToString() });
                i++;
            }

            return _DeductionType;
        }

        public static List<SelectListItem> getDeductionSchedule()
        {
            List<SelectListItem> _DeductionSchedule = new List<SelectListItem>();

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(DeductionScheduleTypes)))
            {
                _DeductionSchedule.Add(new SelectListItem() { Text = s.Replace("_", " ").Replace("x", "-"), Value = i.ToString() });
                i++;
            }

            return _DeductionSchedule;
        }
    }

}
