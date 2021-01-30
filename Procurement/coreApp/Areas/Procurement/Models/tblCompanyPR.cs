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

    public partial class tblCompanyPRMetaData
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

        [Display(Name = "PR No.")]
        public string Form_PRNo { get; set; }

        [Display(Name = "Purpose")]
        public string Form_Purpose { get; set; }
    }

    [MetadataType(typeof(tblCompanyPRMetaData))]
    public partial class tblCompanyPR
    {
        public string aprDescription
        {
            get
            {
                tblAPR _apr = apr;
                return _apr == null ? "" : _apr.Description;
            }
        }

        public tblAPR apr
        {
            get
            {
                using(procurementDataContext context = new procurementDataContext())
                {
                    return context.tblAPRs.Where(x => x.Id == APRId).SingleOrDefault();
                }
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

                if (SubmitDate != null)
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

        public List<tblConsolidatedPR> ReferringCPRs()
        {
            List<tblConsolidatedPR> ret = new List<tblConsolidatedPR>();

            using (procurementDataContext context = new procurementDataContext())
            {
                ret.AddRange(context.tblConsolidatedPRs.ToList().Where(x => x.ContainsPR(Id)));
            }

            return ret;
        }

        public void RemoveAPRId()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblCompanyPR pr = context.tblCompanyPRs.Where(x => x.Id == Id).Single();
                pr.APRId = null;

                context.SubmitChanges();
            }
        }

        public bool MOPSet()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                List<tblMOP> tmp = context.tblMOPs.Where(x => x.PRId == Id && x.Type == (int)MOPType.AgencyPR).ToList();
                return tmp.Any() && !tmp.Any(x => x.ModeOfProcurement == -1);
            }
        }
    }
}