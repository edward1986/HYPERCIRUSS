using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using Module.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblAPRMetaData
    {
        [Required]
        public string Description { get; set; }

        [Range(0, 99999, ErrorMessage = "The Year field is required")]
        public int Year { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy_UserId { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime SubmitDate { get; set; }

        [Display(Name = "Agency Control No.")]
        public string Form_AgencyControlNo { get; set; }

        [Display(Name = "PS APR No.")]
        public string Form_PSAPRNo { get; set; }

        [Display(Name = "Check No.")]
        public string Form_FundDeposit_CheckNo { get; set; }

        [Display(Name = "Amount")]
        public string Form_FundDeposit_Amount { get; set; }

        [Display(Name = "Amount in words")]
        public string Form_FundDeposit_AmountInWords { get; set; }

        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime Form_IssueCommonUse_PriceListNo_Date { get; set; }

        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime Form_InsufficientFunds_Charge_Date { get; set; }
    }

    [MetadataType(typeof(tblAPRMetaData))]
    public partial class tblAPR
    {
        [Display(Name = "Submitted By")]
        public string SubmittedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(SubmittedBy_UserId);
            }
        }

        [Display(Name = "Returned By")]
        public string ReturnedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(ReturnedBy_UserId);
            }
        }

        [Display(Name = "Status")]
        public string DocStatus_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(Status), DocStatus);
            }
        }

        public int DocStatus
        {
            get
            {
                int ret = (int)Status.Draft;

                if (ReturnDate != null)
                {
                    if (SubmitDate != null)
                    {
                        if (ReturnDate > SubmitDate)
                        {
                            ret = (int)Status.Returned;
                        }
                        else
                        {
                            ret = (int)Status.ReSubmitted;
                        }
                    }
                }
                else if (SubmitDate != null)
                {
                    ret = (int)Status.Submitted;
                }

                return ret;
            }
        }

        public bool IsReturned
        {
            get
            {
                return DocStatus == (int)Status.Returned;
            }
        }

        public bool HasBeenSubmitted
        {
            get
            {
                return DocStatus > (int)Status.Returned;
            }
        }
        
        public List<tblAPP> ConsolidatedAPPs()
        {
            List<tblAPP> ret = new List<tblAPP>();
            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(APPIds))
                {
                    int[] ids = APPIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblAPPs.Where(x => ids.Contains(x.Id)).ToList());
                }
            }
            return ret;
        }

        public List<int> ContainedAPPIds(List<int> appIds)
        {
            List<int> ret = new List<int>();

            if (!string.IsNullOrEmpty(APPIds))
            {
                List<int> myList = APPIds.Split(',').Select(x => int.Parse(x)).ToList();
                foreach (int x in myList)
                {
                    if (appIds.Contains(x))
                    {
                        ret.Add(x);
                    }
                }
            }

            return ret;
        }

        public bool ContainsAPP(int appId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(APPIds))
            {
                ret = APPIds.Split(',').Contains(appId.ToString());
            }

            return ret;
        }

        public bool ContainsCategory(int categoryId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(CategoryIds))
            {
                ret = CategoryIds.Split(',').Contains(categoryId.ToString());
            }

            return ret;
        }

        public bool ContainsFund(int fundId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(FundIds))
            {
                ret = FundIds.Split(',').Contains(fundId.ToString());
            }

            return ret;
        }

        public bool ContainsMonth(int month)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(Months))
            {
                ret = Months.Split(',').Contains(month.ToString());
            }

            return ret;
        }
        
        public List<tblCategory> Categories()
        {
            List<tblCategory> ret = new List<tblCategory>();

            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(CategoryIds))
                {
                    int[] ids = CategoryIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblCategories.Where(x => ids.Contains(x.Id)).ToList());
                }
            }

            return ret;
        }

        public List<tblFund> Funds()
        {
            List<tblFund> ret = new List<tblFund>();

            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(FundIds))
                {
                    int[] ids = FundIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblFunds.Where(x => ids.Contains(x.Id)).ToList());
                }
            }

            return ret;
        }

        public List<SelectListItem> GetMonths()
        {
            List<SelectListItem> ret = new List<SelectListItem>();

            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(Months))
                {
                    int[] m = Months.Split(',').Select(x => int.Parse(x)).ToArray();

                    ret = SelectItems.getMonths(showBlankItem: false).Where(x => m.Contains(int.Parse(x.Value))).ToList();
                }
            }

            return ret;
        }

        public void RemoveAPPId(int appId)
        {
            if (string.IsNullOrEmpty(APPIds)) return;

            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> ids = APPIds.Split(',').Select(x => int.Parse(x)).ToList();
                int? id = ids.Where(x => x == appId).SingleOrDefault();

                if (id != null)
                {
                    ids.Remove(id.Value);
                }

                var apr = context.tblAPRs.Where(x => x.Id == Id).Single();
                apr.APPIds = string.Join(",", ids);

                context.SubmitChanges();
            }
        }
        
        public string ModeOfDelivery_Desc
        {
            get
            {
                string ret = "";

                if (Form_IssueCommonUse_Pickup_FastLane == true) ret = "Pickup (Fast Lane)";
                if (Form_IssueCommonUse_Pickup_Schedule == true) ret = "Pickup (Schedule)";
                if (Form_IssueCommonUse_Pickup_Delivery == true) ret = "Delivery (door-to-door)";

                return ret;
            }
        }

        public string InsufficientFunds_Desc
        {
            get
            {
                string ret = "";

                if (Form_InsufficientFunds_ReduceQty == true) ret = "Reduce Quantity";
                if (Form_InsufficientFunds_Bill == true) ret = "Bill Us";
                if (Form_InsufficientFunds_Charge == true) ret = "Charge to Unutilized Deposit";

                return ret;
            }
        }

        public bool ContainsAPPsInSubmittedAPRs()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> _appIds = APPIds.Split(',').Select(y => int.Parse(y)).ToList();

                return context.tblAPRs
                    .Where(x => x.Year == Year && x.Id != Id)
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .Any(x => x.ContainedAPPIds(_appIds).Any());
            }
        }

        public bool InSubmittedCompanyPR(int? excludeCompanyPRId = null)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblCompanyPRs
                    .Where(x => x.Year == Year && x.APRId == Id)
                    .Where(x => excludeCompanyPRId == null || x.Id != excludeCompanyPRId)
                    .ToList()
                    .Any(x => x.HasBeenSubmitted);
            }
        }

        public List<tblCompanyPR> ReferringAgencyPRs()
        {
            List<tblCompanyPR> ret = new List<tblCompanyPR>();

            using (procurementDataContext context = new procurementDataContext())
            {
                ret.AddRange(context.tblCompanyPRs.Where(x => x.APRId == Id));
            }

            return ret;
        }

        public bool HasBeenIncludedInSubmittedAgencyPR()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                bool ret = context.tblCompanyPRs
                    .Where(x => x.Year == Year && x.APRId == Id)
                    .ToList()
                    .Any(x => x.HasBeenSubmitted);

                return ret;
            }
        }

        public List<tblAPR_OO> OOSItems()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblAPR_OOs.Where(x => x.APRId == Id).ToList();

            }
        }
    }
}