using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.Procurement.Enums;
using Module.DB.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement
{
    public static class SelectItems
    {
        public static List<SelectListItem> getMonths(string defaultText = "", int defaultValue = -1, bool showBlankItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showBlankItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                for (int i = 1; i <= 12; i++)
                {
                    DateTime dt = new DateTime(2000, i, 1);

                    _Item.Add(new SelectListItem { Text = dt.ToString("MMMM"), Value = i.ToString() });
                }

                return _Item;
            }
        }

        public static List<SelectListItem> getOfficeFunds(int officeId, int year, string defaultText = "", int defaultValue = -1, bool showBlankItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showBlankItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                        context.tblOfficeAllocations
                        .Where(x => x.OfficeId == officeId && x.Year == year)
                        .Join(context.tblFunds, a => a.FundId, b => b.Id, (a, b) => new SelectListItem { Text = b.Fund, Value = b.Id.ToString() })
                        .ToList()
                        );

                return _Item;
            }
        }

        public static List<SelectListItem> getFunds(string defaultText = "", int defaultValue = -1, bool showBlankItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showBlankItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                    context.tblFunds.OrderBy(x => x.Fund).ToList().Select(x => new SelectListItem() { Text = x.Fund, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                    );

                return _Item;
            }
        }

        public static List<SelectListItem> getCategories(string defaultText = "", int defaultValue = -1)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };
                _Item.AddRange(
                    context.tblCategories.OrderBy(x => x.Category).ToList().Select(x => new SelectListItem() { Text = x.Category, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                    );

                return _Item;
            }
        }

        public static List<SelectListItem> getProcurementItems(string defaultText = "", int defaultValue = -1, int year = -1)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };
                _Item.AddRange(
                    context.tblItems.Where(x => year == -1 || x.Year == year).OrderBy(x => x.Name).ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                    );

                return _Item;
            }
        }

        public static List<SelectListItem> getUnits(string defaultText = "", int defaultValue = -1)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>() { new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 } };
                _Item.AddRange(
                    context.tblUnits.OrderBy(x => x.Unit).ToList().Select(x => new SelectListItem() { Text = x.Unit, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                    );

                return _Item;
            }
        }

        public static List<SelectListItem> getPPMPs(string defaultText = "", int defaultValue = -1, int year = -1, int officeId = -1, int deptId = -1, bool forOfficeOrSubmittedByDept = false, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                if (forOfficeOrSubmittedByDept)
                {
                    _Item.AddRange(
                   context.tblPPMPs
                       .Where(x => year == -1 || x.Year == year)
                       .Where(x => officeId == -1 || x.OfficeId == officeId)
                       .ToList()
                       .Where(x => !x.IsDepartmentPPMP || (x.IsDepartmentPPMP && x.HasBeenSubmitted))
                       .OrderBy(x => x.Description).ToList()
                       .Select(x => new SelectListItem() { Text = x.Description + (x.Fund == null ? "" : string.Format(" ({0})", x.Fund.Fund)), Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                   );
                }
                else
                {
                    _Item.AddRange(
                   context.tblPPMPs
                       .Where(x => year == -1 || x.Year == year)
                       .Where(x => officeId == -1 || x.OfficeId == officeId)
                       .Where(x => deptId == -1 || x.DepartmentId == deptId)
                       .OrderBy(x => x.Description).ToList()
                       .Select(x => new SelectListItem() { Text = x.Description + (x.Fund == null ? "" : string.Format(" ({0})", x.Fund.Fund)), Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                   );
                }


                return _Item;
            }
        }

        public static List<SelectListItem> getAPPs(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblAPPs
                      .Where(x => year == -1 || x.Year == year)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getCPRs(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblConsolidatedPRs
                      .Where(x => year == -1 || x.Year == year)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getPRs(string defaultText = "", int defaultValue = -1, int year = -1, int officeId = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblPRs
                      .Where(x => year == -1 || x.Year == year)
                      .Where(x => officeId == -1 || x.OfficeId == officeId)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getAOBs(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblAOBs
                      .Where(x => year == -1 || x.Year == year)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getPOs(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblPOs
                      .Where(x => year == -1 || x.Year == year)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getCompanyPRs(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblCompanyPRs
                      .Where(x => year == -1 || x.Year == year)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getAPRs(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblAPRs
                      .Where(x => year == -1 || x.Year == year)
                      .OrderBy(x => x.Description).ToList()
                      .Select(x => new SelectListItem() { Text = x.Description, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        public static List<SelectListItem> getModesOfProcurement(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Item = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            int i = 0;
            string myText = "";

            foreach (string s in Enum.GetNames(typeof(ModeOfProcurement)))
            {
                switch (i)
                {
                    case 0:
                        myText = "NP-53.9 - Small Value Procurement";
                        break;
                    case 1:
                        myText = "Shopping";
                        break;
                    case 2:
                        myText = "Limited Source Bidding";
                        break;
                    case 3:
                        myText = "Competitive Bidding";
                        break;
                    case 4:
                        myText = "Direct Contracting";
                        break;
                    case 5:
                        myText = "Repeat Order";
                        break;
                    case 6:
                        myText = "NP-53.1 Two Failed Biddings";
                        break;
                    case 7:
                        myText = "NP-53.2 Emergency Cases";
                        break;
                    case 8:
                        myText = "NP-53.3 Take-Over of Contracts";
                        break;
                    case 9:
                        myText = "NP-53.4 Adjacent or Contiguous";
                        break;
                    case 10:
                        myText = "NP-53.5 Agency-to-Agency";
                        break;
                    case 11:
                        myText = "NP-53.6 Scientific, Scholarly, Artistic Work, Exclusive Technology and Media Services";
                        break;
                    case 12:
                        myText = "NP-53.7 Highly Technical Consultants";
                        break;
                    case 13:
                        myText = "NP-53.8 Defense Cooperation Agreement";
                        break;
                    case 14:
                        myText = "NP-53.10 Lease of Real Property and Venues";
                        break;
                    case 15:
                        myText = "NP-53.11 NGO Participation";
                        break;
                    case 16:
                        myText = "NP-53.12 Community Participation";
                        break;
                    case 17:
                        myText = "NP-53.13 UN Agencies, Int'l Organizations or International Financing Institutions";
                        break;
                    case 18:
                        myText = "Others - Foreign-funded procurement";
                        break;

                }
                _Item.Add(new SelectListItem() { Text = myText, Value = i.ToString(), Selected = defaultValue == i });
                i++;
            }

            return _Item;
        }

        public static List<SelectListItem> getMFO(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Item = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            int i = 0;
            foreach (string s in Enum.GetNames(typeof(MFO)))
            {
                _Item.Add(new SelectListItem() { Text = s, Value = i.ToString(), Selected = defaultValue == i });
                i++;
            }

            return _Item;
        }

        public static List<SelectListItem> getVAT(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Item = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            int i = 0;
            foreach (string s in System.Enum.GetNames(typeof(VAT)))
            {
                _Item.Add(new SelectListItem() { Text = s, Value = i.ToString(), Selected = defaultValue == i });
                i++;
            }

            return _Item;
        }

        public static List<SelectListItem> getSuppliers(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblSuppliers
                      .OrderBy(x => x.CompanyName).ToList()
                      .Select(x => new SelectListItem() { Text = x.CompanyName, Value = x.Id.ToString(), Selected = x.Id == defaultValue })
                  );

                return _Item;
            }
        }

        //public static List<SelectListItem> getEmployees(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true, int employmentType = -1)
        //{
        //    using (Module.DB.dalDataContext context = new Module.DB.dalDataContext())
        //    {
        //        List<SelectListItem> _Item = new List<SelectListItem>();

        //        if (showEmptyItem)
        //        {
        //            _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "", Selected = defaultValue == -1 });
        //        }

        //        _Item.AddRange(
        //          context.tblEmployees
        //            .Join(context.tblEmployee_Careers.Where(x => employmentType == -1 || x.EmploymentType == employmentType), a => a.EmployeeId, b => b.EmployeeId, (a, b) => a)
        //            .ToList()
        //            .Select(x => new SelectListItem() { Text = x.FullName, Value = x.EmployeeId.ToString(), Selected = x.EmployeeId == defaultValue })
        //            .OrderBy(x => x.Text)
        //            .ToList()
        //          );

        //        return _Item;
        //    }
        //}

        public static List<SelectListItem> getEmployees(string defaultText = "", int defaultValue = -1, int year = -1, bool showEmptyItem = true, int employmentType = -1)
        {
            using (Module.DB.dalDataContext context = new Module.DB.dalDataContext())
            {
                List<SelectListItem> _Item = new List<SelectListItem>();

                if (showEmptyItem)
                {
                    _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "", Selected = defaultValue == -1 });
                }

                _Item.AddRange(
                  context.tblEmployees
                    .ToList()
                    .Select(x => new SelectListItem() { Text = x.FullName, Value = x.EmployeeId.ToString(), Selected = x.EmployeeId == defaultValue })
                    .OrderBy(x => x.Text)
                    .ToList()
                  );

                return _Item;
            }
        }



        public static List<SelectListItem> getFundCluster(string defaultText = "", int defaultValue = -1, bool showEmptyItem = true)
        {
            List<SelectListItem> _Item = new List<SelectListItem>();

            if (showEmptyItem)
            {
                _Item.Add(new SelectListItem() { Text = defaultText == "" ? "" : defaultText, Value = "-1", Selected = defaultValue == -1 });
            }

            _Item.Add(new SelectListItem() { Text = "Fund 01: Regular Agency Fund/GF", Value = "Fund 01: Regular Agency Fund/GF", Selected = defaultText == "Fund 01: Regular Agency Fund/GF" });
            _Item.Add(new SelectListItem() { Text = "Fund 05: Internally Generated Funds/STF", Value = "Fund 05: Internally Generated Funds/STF", Selected = defaultText == "Fund 05: Internally Generated Funds/STF" });
            _Item.Add(new SelectListItem() { Text = "Fund 06: Business Type Income/Revolving Funds", Value = "Fund 06: Business Type Income/Revolving Funds", Selected = defaultText == "FUND 06: Business Type Income/Revolving Funds" });
            _Item.Add(new SelectListItem() { Text = "Fund 07: Trust Receipts", Value = "Fund 07: Trust Receipts", Selected = defaultText == "Fund 07: Trust Receipts" });

            return _Item;
        }

    }
}