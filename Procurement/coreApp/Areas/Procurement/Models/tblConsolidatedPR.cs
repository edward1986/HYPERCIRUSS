using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using Module.DB;
using Module.DB.Procs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblConsolidatedPRMetaData
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

        [Display(Name = "Approved By")]
        public string ApprovedBy_UserId { get; set; }

        [Display(Name = "Date Approved")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime ApproveDate { get; set; }

        [Display(Name = "Returned By")]
        public string ReturnedBy_UserId { get; set; }

        [Display(Name = "Date Returned")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Return Message")]
        public string ReturnMessage { get; set; }

        [Display(Name = "PR No.")]
        public string Form_PRNo { get; set; }

        [Display(Name = "Purpose")]
        public string Form_Purpose { get; set; }
    }

    [MetadataType(typeof(tblConsolidatedPRMetaData))]
    public partial class tblConsolidatedPR
    {
        [Display(Name = "Submitted By")]
        public string SubmittedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(SubmittedBy_UserId);
            }
        }

        [Display(Name = "Created By")]
        public string CreatedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(CreatedBy_UserId);
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

        [Display(Name = "Approved By")]
        public string ApprovedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(ApprovedBy_UserId);
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

                if (ApproveDate != null)
                {
                    ret = (int)Status.Approved;
                }
                else if (ReturnDate != null)
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

        public bool HasBeenSubmitted
        {
            get
            {
                return DocStatus > (int)Status.Returned;
            }
        }

        public bool HasBeenServed
        {
            get
            {
                return IsApproved;
            }
        }

        public bool IsReturned
        {
            get
            {
                return DocStatus == (int)Status.Returned;
            }
        }

        public bool IsApproved
        {
            get
            {
                return DocStatus == (int)Status.Approved;
            }
        }

        public List<int> ContainedPRIds(List<int> prIds)
        {
            List<int> ret = new List<int>();

            if (!string.IsNullOrEmpty(PRIds))
            {
                List<int> myList = PRIds.Split(',').Select(x => int.Parse(x)).ToList();
                foreach (int x in myList)
                {
                    if (prIds.Contains(x))
                    {
                        ret.Add(x);
                    }
                }
            }

            return ret;
        }

        public List<int> ContainedCompanyPRIds(List<int> companyPRIds)
        {
            List<int> ret = new List<int>();

            if (!string.IsNullOrEmpty(CompanyPRIds))
            {
                List<int> myList = CompanyPRIds.Split(',').Select(x => int.Parse(x)).ToList();
                foreach (int x in myList)
                {
                    if (companyPRIds.Contains(x))
                    {
                        ret.Add(x);
                    }
                }
            }

            return ret;
        }

        public bool ContainsPR(int prId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(PRIds))
            {
                ret = PRIds.Split(',').Contains(prId.ToString());
            }

            return ret;
        }

        public bool ContainsCompanyPR(int prId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(CompanyPRIds))
            {
                ret = CompanyPRIds.Split(',').Contains(prId.ToString());
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

        public void RemovePRId(int prId)
        {
            if (string.IsNullOrEmpty(PRIds)) return;

            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> ids = PRIds.Split(',').Select(x => int.Parse(x)).ToList();
                int? id = ids.Where(x => x == prId).SingleOrDefault();

                if (id != null)
                {
                    ids.Remove(id.Value);
                }

                var pr = context.tblConsolidatedPRs.Where(x => x.Id == Id).Single();
                pr.PRIds = string.Join(",", ids);

                context.SubmitChanges();

                new CPRModel(Id).SaveItems();
            }
        }

        public List<tblRFQ> ReferringRFQs()
        {
            List<tblRFQ> ret = new List<tblRFQ>();

            using (procurementDataContext context = new procurementDataContext())
            {
                ret.AddRange(context.tblRFQs.Where(x => x.Year == Year).ToList().Where(x => x.ContainsCPR(Id)));
            }

            return ret;
        }

        public List<tblPR> ConsolidatedPRs()
        {
            List<tblPR> ret = new List<tblPR>();
            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(PRIds))
                {
                    int[] ids = PRIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblPRs.Where(x => ids.Contains(x.Id)).ToList());
                }
            }
            return ret;
        }

        public List<tblCompanyPR> ConsolidatedCompanyPRs()
        {
            List<tblCompanyPR> ret = new List<tblCompanyPR>();
            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(CompanyPRIds))
                {
                    int[] ids = CompanyPRIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblCompanyPRs.Where(x => ids.Contains(x.Id)).ToList());
                }
            }
            return ret;
        }

        public bool MOPSet()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblMOP> tmp = context.tblMOPs.Where(x => x.PRId == Id && x.Type == (int)MOPType.ConsolidatedPR).ToList();
                return tmp.Any() && !tmp.Any(x => x.ModeOfProcurement == -1);
            }
        }
    }
}