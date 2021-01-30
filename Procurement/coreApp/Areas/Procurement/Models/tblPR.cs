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

    public partial class tblPRMetaData
    {
        [Required]
        public string Description { get; set; }

        [Range(0, 99999, ErrorMessage = "The Year field is required")]
        public int Year { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy_UserId { get; set; }

        [Display(Name = "Date Created")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime CreateDate { get; set; }

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

    [MetadataType(typeof(tblPRMetaData))]
    public partial class tblPR
    {
        public string ppmpDescription
        {
            get
            {
                tblPPMP _ppmp = ppmp;
                return _ppmp == null ? "" : _ppmp.Description;
            }
        }

        public tblPPMP ppmp
        {
            get
            {
                using(procurementDataContext context = new procurementDataContext())
                {
                    return context.tblPPMPs.Where(x => x.Id.ToString() == PPMPIds.Split(',')[0]).SingleOrDefault();
                }
            }
        }

        public tblOffice Office
        {
            get
            {
                return Cache.Get_Tables().Offices.Where(x => x.OfficeId == OfficeId).SingleOrDefault();
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

        public tblEmployee CreatorCareer()
        {
            tblEmployee ret = null;

            using (dalDataContext context = new dalDataContext())
            {
                var tmp = context.tblEmployees.Where(x => x.UserId == CreatedBy_UserId).SingleOrDefault();
                if (tmp != null)
                {
                    ret = tmp;
                }
            }

            return ret;
        }


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

        public List<tblPPMP> ConsolidatedPPMPs()
        {
            List<tblPPMP> ret = new List<tblPPMP>();
            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(PPMPIds))
                {
                    int[] ids = PPMPIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblPPMPs.Where(x => ids.Contains(x.Id)).ToList());
                }
            }
            return ret;
        }

        public bool ContainsPPMP(int ppmpId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(PPMPIds))
            {
                ret = PPMPIds.Split(',').Contains(ppmpId.ToString());
            }

            return ret;
        }

        public bool ContainsCategory(int CategoryId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(CategoryIds))
            {
                ret = CategoryIds.Split(',').Contains(CategoryId.ToString());
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


        public List<tblConsolidatedPR> ReferringCPRs()
        {
            List<tblConsolidatedPR> ret = new List<tblConsolidatedPR>();

            using (procurementDataContext context = new procurementDataContext())
            {
                ret.AddRange(context.tblConsolidatedPRs.ToList().Where(x => x.ContainsPR(Id)));
            }

            return ret;
        }

        public void RemovePPMPId(int ppmpId)
        {
            if (string.IsNullOrEmpty(PPMPIds)) return;

            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> ids = PPMPIds.Split(',').Select(x => int.Parse(x)).ToList();
                int? id = ids.Where(x => x == ppmpId).SingleOrDefault();

                if (id != null)
                {
                    ids.Remove(id.Value);
                }

                var pr = context.tblPRs.Where(x => x.Id == Id).Single();
                pr.PPMPIds = string.Join(",", ids);

                context.SubmitChanges();
            }
        }

        public bool MOPSet()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblMOP> tmp = context.tblMOPs.Where(x => x.PRId == Id && x.Type == (int)MOPType.OfficePR).ToList();
                return tmp.Any() && !tmp.Any(x => x.ModeOfProcurement == -1);
            }
        }
    }
}