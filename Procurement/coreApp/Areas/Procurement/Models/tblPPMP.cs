using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using Module.DB;
using Module.DB.Procs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblPPMPMetaData
    {
        [Display(Name = "PPMP Number")]
        public string PPMPNumber { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 99999, ErrorMessage = "The Office field is required")]
        [Display(Name = "Office")]
        public int OfficeId { get; set; }
        
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

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

        [Display(Name = "Fund")]
        public int FundId { get; set; }
    }

    [MetadataType(typeof(tblPPMPMetaData))]
    public partial class tblPPMP
    {
        public PPMPModel Model()
        {
            return new PPMPModel(Id);
        }

        public double _TotalAmount
        {
            get
            {
                return (_TotalAmount_InDBM ?? 0) + (_TotalAmount_NotInDBM ?? 0);
            }
        }

        public tblFund Fund
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblFunds.Where(x => x.Id == FundId).SingleOrDefault();
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

        public bool IsDepartmentPPMP
        {
            get
            {
                return DepartmentId != null;
            }
        }

        public tblDepartment  Department
        {
            get
            {
                return Cache.Get_Tables().Departments.Where(x => x.DepartmentId == DepartmentId).SingleOrDefault();
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

        public List<tblAPP> ReferringAPPs()
        {
            List<tblAPP> ret = new List<tblAPP>();

            using (procurementDataContext context = new procurementDataContext())
            {
                ret.AddRange(context.tblAPPs.ToList().Where(x => x.ContainsPPMP(Id)));
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
        

        public void RemoveCategoryId(int categoryId)
        {
            if (string.IsNullOrEmpty(CategoryIds)) return;

            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> ids = CategoryIds.Split(',').Select(x => int.Parse(x)).ToList();
                int? id = ids.Where(x => x == categoryId).SingleOrDefault();

                if (id != null)
                {
                    ids.Remove(id.Value);
                }

                var app = context.tblAPPs.Where(x => x.Id == Id).Single();
                app.CategoryIds = string.Join(",", ids);

                context.SubmitChanges();
            }
        }

        public bool HasBeenIncludedInSubmittedAPP(int? excludeAPPId = null)
        {
            using(procurementDataContext context = new procurementDataContext())
            {
                bool ret = context.tblAPPs
                    .Where(x => x.Year == Year && (excludeAPPId == null || x.Id != excludeAPPId))
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .Any(x => x.PPMPIds.Split(',').Select(y => int.Parse(y)).Contains(Id));

                return ret;
            }
        }
        
    }
}