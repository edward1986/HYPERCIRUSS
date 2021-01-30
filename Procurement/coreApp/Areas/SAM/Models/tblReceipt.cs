using coreApp.Areas.Procurement;
using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblReceiptMetaData
    {
        [Display(Name = "Invoice No.")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Invoice Date")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Delivery Receipt No.")]
        public string DRNo { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedBy_UserId { get; set; }
        
        [Required]
        [Display(Name = "Date Received")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime ReceivedDate { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime DateSubmitted { get; set; }

        [Display(Name = "Date Returned")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime DateReturned { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy_UserId { get; set; }

        [Display(Name = "Returned By")]
        public string ReturnedBy_UserId { get; set; }

        [Display(Name = "Items Last Updated")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime? ItemsLastUpdated { get; set; }

        [Display(Name = "Inspection Date")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime? InspectionDate { get; set; }

        [Display(Name = "Inspection Remarks")]
        public string InspectionRemarks { get; set; }

        
    }

    [MetadataType(typeof(tblReceiptMetaData))]
    public partial class tblReceipt
    {
        public string RFINo
        {
            get
            {
                string ret = "";

                if (HasBeenSubmitted)
                {
                    ret = string.Format("{0}-{1}", DateSubmitted.Value.Year, Id.ToString("0000"));
                }

                return ret;
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
        public string Status_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(ReceiptStatus), Status);
            }
        }

        public int Status
        {
            get
            {
                int ret = (int)ReceiptStatus.Draft;

                if (InspectionDate != null)
                {
                    ret = (int)ReceiptStatus.Inspected;
                }
                else if (DateReturned != null)
                {
                    if (DateSubmitted != null)
                    {
                        if (DateReturned > DateSubmitted)
                        {
                            ret = (int)ReceiptStatus.Returned;
                        }
                        else
                        {
                            ret = (int)ReceiptStatus.ReSubmitted;
                        }
                    }
                }
                else if (DateSubmitted != null)
                {
                    ret = (int)ReceiptStatus.Submitted;
                }

                return ret;
            }
        }

        public bool HasBeenSubmitted
        {
            get
            {
                return Status > (int)ReceiptStatus.Returned;
            }
        }

        public bool IsReturned
        {
            get
            {
                return Status == (int)ReceiptStatus.Returned;
            }
        }

        public bool IsInspected
        {
            get
            {
                return Status == (int)ReceiptStatus.Inspected;
            }
        }

        [Display(Name = "IAR No.")]
        public string IARNo
        {
            get
            {
                return IsInspected ? string.Format("{0}-{1}", InspectionDate.Value.ToString("yyyyMMdd"), Id.ToString("00000")) : "";
            }
        }

        [Display(Name = "Received By")]
        public string ReceivedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(ReceivedBy_UserId);
            }
        }

        public void UpdateFields(bool isInitial)
        {
            using (samDataContext context = new samDataContext())
            {
                using (dalDataContext _context = new dalDataContext())
                {
                    if (isInitial)
                    {
                        tblPO po = context.tblPOs.Where(x => x.Id == POId).Single();
                        
                        _PONo = po.PONo;
                        _SupplierId = po.SupplierId;
                        _SupplierName = po._SupplierName;
                    }

                    tblDepartment  department = _context.tblDepartments.Where(x => x.DepartmentId == DepartmentId).SingleOrDefault();
                   
                    if (department == null)
                    {
                        _OfficeId = null;
                        _OfficeName = null;
                        _DepartmentName = null;
                        _RCC = null;
                    }
                    else
                    {
                        _OfficeId = department.OfficeId;
                        _OfficeName = department.Office.Office;
                        _DepartmentName = department.Department;
                        _RCC = department.Office.RCC;
                    }
                }
            }

        }

        [Display(Name = "Office / Department")]
        public string Office_Department
        {
            get
            {
                return _OfficeId == null ? "" : string.Format("{0} / {1}", _OfficeName, _DepartmentName);
            }
        }

        public tblPO GetPO()
        {
            using (samDataContext context = new samDataContext())
            {
                return context.tblPOs.Where(x => x.Id == POId).SingleOrDefault();
            }
        }
        
        public string Delivery
        {
            get
            {
                string ret = "No item received";

                if (IsCompleteDelivery())
                {
                    ret = "Complete";
                }
                else
                {
                    using (samDataContext context = new samDataContext())
                    {
                        if (context.tblReceiptItems.Any(x => x.ReceiptId == Id))
                        {
                            ret = "Partial";
                        }
                    }
                }

                return ret;
            }
        }

        public bool IsCompleteDelivery()
        {
            bool ret = false;

            using (samDataContext context = new samDataContext())
            {
                tblPO po = GetPO();
                List<tblPOItem> poItems = context.tblPOItems.Where(x => x.POId == po.Id).ToList();
                List<tblReceiptItem> receiptItems = context.tblReceiptItems.Where(x => x.ReceiptId == Id).ToList();

                ret = !poItems.Any(x => !receiptItems.Any(y => y.POItemId == x.Id && y.Qty >= x.Qty));

            }

            return ret;
        }
    }
}