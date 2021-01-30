using Module.DB;
using Module.DB.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Module.Leave
{
    public static class Procs
    {
        public static bool IsLeaveRuleApplicable(LeaveRuleModel rule, tblEmployee_Career career, tblEmployee_Info info)
        {
            return evaluate(rule.Scope, career, info);
        }

        private static bool evaluate(string scope, tblEmployee_Career career, tblEmployee_Info info)
        {
            bool ret = true;

            if (!string.IsNullOrEmpty(scope))
            {
                scope = scope.Trim();
                Regex regex = new Regex("\\((?!.*\\().*?\\)"); //Get last inner-most group in parenthesis
                Match match = regex.Match(scope);
                string matchedValue = match.Value.Replace("(", "").Replace(")", "");

                while (match.Success)
                {
                    string value = evaluate(matchedValue, career, info) ? "true" : "false";
                    scope = regex.Replace(scope, value);

                    match = regex.Match(scope);
                    matchedValue = match.Value.Replace("(", "").Replace(")", "");
                }

                if (scope.Contains("&"))
                {
                    foreach (string s in scope.Split('&'))
                    {
                        if (string.IsNullOrEmpty(s) || s == "true") continue;

                        if (s == "false")
                        {
                            ret = false;
                            break;
                        }
                        else if (!evaluate(s, career, info))
                        {
                            ret = false;
                            break;
                        }
                    }
                }
                else if (scope.Contains("|"))
                {
                    ret = false;

                    foreach (string s in scope.Split('|'))
                    {
                        if (string.IsNullOrEmpty(s) || s == "false") continue;

                        if (s == "true")
                        {
                            ret = true;
                            break;
                        }
                        else if (evaluate(s, career, info))
                        {
                            ret = true;
                            break;
                        }
                    }
                }
                else
                {
                    ret = false;

                    bool not = scope.StartsWith("!");
                    string s = not ? scope.Substring(1) : scope;
                    int v;

                    if (int.TryParse(s, out v))
                    {
                        switch ((RuleScopeType)v)
                        {
                            case RuleScopeType.Permanent:
                                ret = career.EmploymentType == (int)EmploymentType.Permanent;
                                break;
                            case RuleScopeType.Contractual:
                                ret = career.EmploymentType == (int)EmploymentType.Contractual;
                                break;
                            case RuleScopeType.JobOrder:
                                ret = career.EmploymentType == (int)EmploymentType.JobOrder;
                                break;
                            case RuleScopeType.Seasonal:
                                ret = career.EmploymentType == (int)EmploymentType.Seasonal;
                                break;
                            case RuleScopeType.Casual:
                                ret = career.EmploymentType == (int)EmploymentType.Casual;
                                break;
                            case RuleScopeType.Temporary:
                                ret = career.EmploymentType == (int)EmploymentType.Temporary;
                                break;
                            case RuleScopeType.Faculty:
                                ret = career.IsFaculty;
                                break;
                            case RuleScopeType.WithDesignation:
                                ret = career.IsDesignated;
                                break;
                            case RuleScopeType.Male:
                                ret = (info.Gender ?? 0) == (int)Gender.Male;
                                break;
                            case RuleScopeType.Female:
                                ret = (info.Gender ?? 0) == (int)Gender.Female;
                                break;
                        }

                        if (not)
                        {
                            ret = !ret;
                        }
                    }
                }
            }

            return ret;
        }

        public static tblLeaveApplication LeaveApplication(int? leaveApplicationId)
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblLeaveApplications.Where(x => x.Id == leaveApplicationId).SingleOrDefault();
            }
        }
    }

    public class LeaveRuleModel
    {
        public bool ApplicationRequiresSupport { get; set; }
        public bool CarryOver { get; set; }
        public bool WithoutPay { get; set; }
        public LeaveMode Mode { get; set; }
        public int Effectivity { get; set; }
        public double Credits { get; set; }
        public string Scope { get; set; }

        public LeaveRuleModel()
        { }

        public LeaveRuleModel(List<tblLeaveTypeRule> rules)
        {
            ApplicationRequiresSupport = rules.Where(x => x.Rule.Name == Constants.RULE_APPLICATION_REQUIRES_SUPPORT).Single().Value == "1";
            CarryOver = rules.Where(x => x.Rule.Name == Constants.RULE_CARRY_OVER).Single().Value == "1";
            WithoutPay = rules.Where(x => x.Rule.Name == Constants.RULE_WITHOUT_PAY).Single().Value == "1";
            Mode = (LeaveMode)Convert.ToInt32(rules.Where(x => x.Rule.Name == Constants.RULE_MODE).Single().Value);
            Effectivity = Convert.ToInt32(rules.Where(x => x.Rule.Name == Constants.RULE_EFFECTIVITY).Single().Value);
            Credits = Convert.ToDouble(rules.Where(x => x.Rule.Name == Constants.RULE_NO_OF_CREDITS).Single().Value);

            Scope = rules.Where(x => x.Rule.Name == Constants.RULE_SCOPE).Single().Value;

        }
    }

    public class LeaveChargesModel
    {
        public tblLeaveType leaveType { get; set; }
        public double charge { get; set; }
    }

    public class employeeLeaveData
    {
        public tblEmployee Employee;
        public List<tblEmployee_LeaveCredit> LeaveCredits;
        public List<tblEmployee_Career> Careers;
        public List<tblLeaveType> LeaveTypes;

        public employeeLeaveData(int employeeId, ref OfflineDataSource offlineDS)
        {
            Employee = offlineDS.Employees.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
            Careers = offlineDS.EmployeeCareers.Where(x => x.EmployeeId == employeeId).ToList();
            LeaveCredits = offlineDS.EmployeeLeaveCredits.Where(x => x.EmployeeId == employeeId).ToList();
            LeaveTypes = offlineDS.LeaveTypes.ToList();
        }

        public employeeLeaveData(int employeeId)
        {
            using (dalDataContext context = new dalDataContext())
            {
                Employee = context.tblEmployees.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
                Careers = context.tblEmployee_Careers.Where(x => x.EmployeeId == employeeId).ToList();
                LeaveCredits = context.tblEmployee_LeaveCredits.Where(x => x.EmployeeId == employeeId).ToList();
                LeaveTypes = context.tblLeaveTypes.ToList();
            }
        }
    }

    public class employeeLeave
    {
        employeeLeaveData elData;
        DateTime? LeaveCalculationReferenceDate;

        int employeeId;
        public int leaveTypeId;

        LeaveRuleModel ruleModel;

        public bool hasApplicableCareer { get; set; }

        public List<tblEmployee_LeaveCredit> table { get; set; }

        public DateTime table_date { get; set; }

        //public employeeLeave(int employeeId, int leaveTypeId, List<tblEmployee_LeaveCredit> data, DateTime? table_date = null)
        //{
        //    init(employeeId, leaveTypeId, data, table_date);
        //}

        public bool LeaveWithoutPay { get; set; }

        public employeeLeave(int employeeId, int leaveTypeId, ref OfflineDataSource offlineDS, DateTime? table_date = null, DateTime? leaveCalculationReferenceDate = null)
        {
            this.elData = new employeeLeaveData(employeeId, ref offlineDS);
            this.LeaveCalculationReferenceDate = leaveCalculationReferenceDate;

            init(employeeId, leaveTypeId, table_date);
        }

        public employeeLeave(int employeeId, int leaveTypeId, DateTime? table_date = null, DateTime? leaveCalculationReferenceDate = null)
        {
            this.elData = new employeeLeaveData(employeeId);
            this.LeaveCalculationReferenceDate = leaveCalculationReferenceDate;

            init(employeeId, leaveTypeId, table_date);
        }

        private void init(int employeeId, int leaveTypeId, DateTime? table_date = null)
        {
            this.employeeId = employeeId;
            this.leaveTypeId = leaveTypeId;

            if (table_date == null) table_date = latestDate();
            //if (!table_date.HasValue)
            //table_date = new DateTime?(this.latestDate());

            tblLeaveType lt = elData.LeaveTypes.Where(x => x.Id == leaveTypeId).SingleOrDefault();
            ruleModel = new LeaveRuleModel(lt.Rules());

            LeaveWithoutPay = ruleModel.WithoutPay;

            this.table_date = table_date.Value;
            this.table = getLeaveTable(table_date.Value);

        }

        public string LeaveName()
        {
            tblLeaveType lt = elData.LeaveTypes.Where(x => x.Id == leaveTypeId).SingleOrDefault();

            return lt.Description;
        }

        public double leaveEq(DateTime d)
        {
            double ret = 0;

            List<tblEmployee_LeaveCredit> l = table.Where(x => x.IsDrEntry && x.Match(d)).ToList();

            if (l.Any())
            {
                ret = 1;

                if (l.Count(x => d.Date == x.StartDate.Date && x.StartDate_IsHalfDay || d.Date == x.EndDate.Date && x.EndDate_IsHalfDay) == 1)
                {
                    ret = .5;
                }
            }

            return ret;
        }

        public double LeaveBalance(DateTime? dt = null, int excludeId = -1)
        {
            if (dt == null)
            {
                dt = latestDate();
            }

            List<tblEmployee_LeaveCredit> tmp =
                table.Where(x => x.EndDate <= dt && x.CreditId != excludeId).ToList();


            if (ruleModel.CarryOver)
            {
                double cr = tmp.Sum(x => x.Cr);
                double dr = tmp.Sum(x => x.Dr);

                return cr - dr;
            }
            else
            {
                DateTime sd = default(DateTime);

                var t = tmp.Where(x => x.EntryType == (int)LeaveEntryType.Auto && x.EndDate <= dt).OrderByDescending(x => x.EndDate).FirstOrDefault();
                if (t != null)
                {
                    sd = t.StartDate;
                }

                double cr = tmp.Where(x => x.EndDate >= sd && x.EndDate <= dt).Sum(x => x.Cr);
                double dr = tmp.Where(x => x.EndDate >= sd && x.EndDate <= dt).Sum(x => x.Dr);

                return cr - dr;
            }
        }

        public double TotalDrCr(bool Dr, DateTime? dt = null, int excludeId = -1)
        {
            List<tblEmployee_LeaveCredit> tmp = table;
            if (dt == null)
            {
                dt = latestDate();
            }
            else
            {
                tmp = table.Where(x => x.EndDate <= dt).ToList();
            }

            tmp = tmp.Where(x => x.CreditId != excludeId).ToList();

            if (ruleModel.CarryOver)
            {
                return tmp.Sum(x => Dr ? x.Dr : x.Cr);
            }
            else
            {
                DateTime sd = default(DateTime);

                var t = tmp.Where(x => x.EntryType == (int)LeaveEntryType.Auto && x.EndDate <= dt).OrderByDescending(x => x.EndDate).FirstOrDefault();
                if (t != null)
                {
                    sd = t.StartDate;
                }

                return tmp.Where(x => x.EndDate >= sd && x.EndDate <= dt).Sum(x => Dr ? x.Dr : x.Cr);
            }
        }

        public List<tblEmployee_LeaveCredit> getLeaveTable(DateTime dt)
        {
            var tmp = elData.LeaveCredits.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
            DateTime _dt = LeaveCalculationReferenceDate == null ? default(DateTime) : LeaveCalculationReferenceDate.Value;

            return (tmp
                .Union(generate(dt)))
                .Where(x => x.StartDate >= _dt)
                .OrderBy(x => x.EndDate).ThenBy(x => x.CreditId)
                .ToList();
        }

        private DateTime latestDate()
        {
            DateTime ret = DateTime.Today;

            tblEmployee_LeaveCredit tmp = elData.LeaveCredits.OrderByDescending(x => x.EndDate).FirstOrDefault();
            if (tmp != null)
            {
                if (tmp.EndDate > ret)
                {
                    ret = tmp.EndDate;
                }
            }

            return ret;
        }

        DateTime preDate;

        private List<tblEmployee_LeaveCredit> generate(DateTime dt)
        {
            List<tblEmployee_LeaveCredit> ret = new List<tblEmployee_LeaveCredit>();

            tblEmployee_Info info = new Module.DB.Procs.procs_tblEmployee(elData.Employee).Info();

            List<tblEmployee_Career> careers = elData.Careers.OrderBy(x => x.DateEffective).ThenBy(x => x.CareerId).ToList();
            careers = careers.Where(x => Module.Leave.Procs.IsLeaveRuleApplicable(ruleModel, x, info)).ToList();

            hasApplicableCareer = careers.Any();

            if (hasApplicableCareer)
            {
                bool firstPass = true;
                tblEmployee_Career career = careers.First();

                while (career != null)
                {
                    DateTime sDate = career.DateEffective;
                    DateTime eDate = dt;

                    if (firstPass)
                    {
                        sDate = sDate.AddMonths(ruleModel.Effectivity);
                        firstPass = false;
                    }

                    if (career.hasEndEvent())
                    {
                        eDate = career.EndEventDate.Value;

                        if (career.EndEvent == (int)CareerEvent.Terminated || career.EndEvent == (int)CareerEvent.Resigned)
                        {
                            firstPass = true;
                        }

                    }
                    else
                    {
                        eDate = DateTime.Now;
                    }

                    if (ruleModel.Credits != 0)
                    {
                        ret.AddRange(getCredits(sDate, eDate));

                    }

                    if (career.hasEndEvent())
                    {

                        preDate = career.EndEventDate.Value;

                    }

                    career = careers.Where(x => x.DateEffective > career.DateEffective).FirstOrDefault();
                }
            }

            return ret;
        }

        private List<tblEmployee_LeaveCredit> getCredits(DateTime sDt, DateTime eDt)
        {
            List<tblEmployee_LeaveCredit> ret = new List<tblEmployee_LeaveCredit>();

            DateTime d;

            if (sDt.Month == preDate.Month && sDt.Year == preDate.Year)
            {
                d = sDt.AddMonths(1);
            }
            else
            {
                d = sDt;
            }

            while (d <= eDt)
            {
                ret.Add(new tblEmployee_LeaveCredit
                {
                    EmployeeId = employeeId,
                    StartDate = d,
                    EndDate = d,
                    StartDate_IsHalfDay = false,
                    EndDate_IsHalfDay = false,
                    EntryType = (int)LeaveEntryType.Auto,
                    LeaveTypeId = leaveTypeId,
                    Cr = ruleModel.Credits,
                    Remarks = "[earned credits]",
                    WithoutPay = ruleModel.WithoutPay
                });


                if (ruleModel.Mode == LeaveMode.Yearly)
                {
                    d = d.AddYears(1);
                }
                else
                {
                    d = d.AddMonths(1);
                }
            }

            return ret;
        }


    }
}