using coreApp.Areas.Procurement.DAL;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblPOMetaData
    {
        [Display(Name = "Created/Imported By")]
        public string CreatedBy_UserId { get; set; }

        [Required]
        [Display(Name = "P.O. No.")]
        public string PONo { get; set; }

        [Required]
        [Display(Name = "P.O. Date")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime PODate { get; set; }

        [Display(Name = "Supplier")]
        [Range(1, 99999, ErrorMessage = "The Supplier field is required")]
        public int SupplierId { get; set; }
                
        [Display(Name = "Place of Delivery")]
        public string Form_PlaceOfDelivery { get; set; }

        [Display(Name = "Date of Delivery")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime Form_DateOfDelivery { get; set; }

        [Display(Name = "Delivery Term")]
        public string Form_DeliveryTerm { get; set; }

        [Display(Name = "Payment Term")]
        public string Form_PaymentTerm { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string MOP { get; set; }

        [Display(Name = "P.R. No.")]
        public string PRNo { get; set; }

        [Display(Name = "P.R. Date")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime PRDate { get; set; }

        [Display(Name = "Office")]
        public int OfficeId { get; set; }
    }

    [MetadataType(typeof(tblPOMetaData))]
    public partial class tblPO
    {
        public bool FromProcurement { get; set; } = false;

        public bool IsWarehouseLocked(int? myWhId)
        {
            return myWhId != WarehouseId;
        }

        public tblPO (Procurement.SupplierPO supplierPO)
        {
            FromProcurement = true;
            PONo = supplierPO.PONo;
            PODate = supplierPO.PO.SubmitDate.Value;
            SupplierId = supplierPO.Supplier.Id;
            Procurement_POId = supplierPO.PO.Id;
            CreatedBy_UserId = supplierPO.PO.CreatedBy_UserId;
            Form_PlaceOfDelivery = supplierPO.PO.Form_PlaceOfDelivery;
            Form_DateOfDelivery = supplierPO.PO.Form_DateOfDelivery;
            Form_DeliveryTerm = supplierPO.PO.Form_DeliveryTerm;
            Form_PaymentTerm = supplierPO.PO.Form_PaymentTerm;
        }

        public void UpdateFields()
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblSupplier supplier = context.tblSuppliers.Where(x => x.Id == SupplierId).Single();

                _SupplierName = supplier.CompanyName;
                _SupplierAddress = supplier.Address;
                _SupplierTIN = supplier.TIN;
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

        public bool Locked
        {
            get
            {
                return Procurement_POId != null;
            }
        }

        public string Office_Desc
        {
            get
            {
                tblOffice office = GetOffice();
                return office == null ? "" : office.Office;
            }
        }

        public tblOffice GetOffice()
        {
            using (dalDataContext context = new dalDataContext())
            {
                return context.tblOffices.Where(x => x.OfficeId == OfficeId).SingleOrDefault();
            }
        }
    }
}