using coreApp.Areas.Procurement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace coreApp.Areas.Procurement.DAL
{
    public partial class tblNOAMetaData
    {
        [Required]
        public string Description { get; set; }

 
        [Display(Name = "Date Created")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateCreated { get; set; }

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

    }
    [MetadataType(typeof(tblNOAMetaData))]
    public partial class tblNOA
    {
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

        [Display(Name = "Consolidated PR")]
        public tblConsolidatedPR ConsolidatedPR
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblConsolidatedPRs.Where(x => x.Id == ConsolidatedPRId).SingleOrDefault();
                }
            }
        }

        public tblSupplier Supplier
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblSuppliers.Where(x => x.Id == SupplierId).SingleOrDefault();
                }
            }
        }


    }
}