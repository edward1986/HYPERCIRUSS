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

    public partial class tblRFQMetaData
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

        [Display(Name = "PHILGEPS Ref. No.")]
        public string Form_PHILGEPS { get; set; }

        [Display(Name = "Deadline")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime Form_Deadline { get; set; }

        [Display(Name = "Canvasser")]
        public DateTime Form_Canvasser { get; set; }

        [Display(Name = "Suppliers")]
        public string SupplierIds { get; set; }
    }

    [MetadataType(typeof(tblRFQMetaData))]
    public partial class tblRFQ
    {
        public List<tblSupplier> Suppliers()
        {
            List<tblSupplier> ret = new List<tblSupplier>();

            if (!string.IsNullOrEmpty(SupplierIds)) {

                using (procurementDataContext context = new procurementDataContext())
                {
                    ret = context.tblSuppliers.Where(x => SupplierIds.Split(',').Select(y => int.Parse(y)).Contains(x.Id)).ToList();
                }
            }

            return ret;
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
        
        public List<tblConsolidatedPR> ConsolidatedCPRs()
        {
            List<tblConsolidatedPR> ret = new List<tblConsolidatedPR>();
            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(CPRIds))
                {
                    int[] ids = CPRIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblConsolidatedPRs.Where(x => ids.Contains(x.Id)).ToList());
                }
            }
            return ret;
        }

        public List<int> ContainedCPRIds(List<int> cprIds)
        {
            List<int> ret = new List<int>();

            if (!string.IsNullOrEmpty(CPRIds))
            {
                List<int> myList = CPRIds.Split(',').Select(x => int.Parse(x)).ToList();
                foreach (int x in myList)
                {
                    if (cprIds.Contains(x))
                    {
                        ret.Add(x);
                    }
                }
            }

            return ret;
        }

        public bool ContainsCPR(int cprId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(CPRIds))
            {
                ret = CPRIds.Split(',').Contains(cprId.ToString());
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

        
        public void RemoveCPRId(int cprId)
        {
            if (string.IsNullOrEmpty(CPRIds)) return;

            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> ids = CPRIds.Split(',').Select(x => int.Parse(x)).ToList();
                int? id = ids.Where(x => x == cprId).SingleOrDefault();

                if (id != null)
                {
                    ids.Remove(id.Value);
                }

                var rfq = context.tblRFQs.Where(x => x.Id == Id).Single();
                rfq.CPRIds = string.Join(",", ids);

                context.SubmitChanges();
            }
        }

        public List<tblAOB> ReferringAOBs()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                return context.tblAOBs.Where(x => x.Year == Year).ToList().Where(x => x.ContainsRFQ(Id)).ToList();
            }
        }
    }
}