using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{

    public partial class tblAREMetaData
    {
        [Required]
        [Display(Name = "Date")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime AREDate { get; set; }
        
        [Display(Name = "From")]
        public int From_Id { get; set; }

        [Display(Name = "To")]
        public int To_Id { get; set; }

        [Display(Name = "From Type")]
        public int From_Type { get; set; }

        [Display(Name = "To Type")]
        public int To_Type { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Position")]
        public string _To_Position { get; set; }

        [Display(Name = "Office")]
        public string _To_Office { get; set; }

        [Display(Name = "Department")]
        public string _To_Department { get; set; }
        
        [Display(Name = "Address")]
        public string _To_Address { get; set; }

        [Display(Name = "PAR No.")]
        public string _ARENo { get; set; }

    }

    [MetadataType(typeof(tblAREMetaData))]
    public partial class tblARE
    {
        public int _FromEmployee { get; set; }
        public int _FromContractor { get; set; }
        public int _ToEmployee { get; set; }
        public int _ToContractor { get; set; }

        public bool FromContractor
        {
            get
            {
                return From_Type == (int)iType.Contractor;
            }
        }

        public bool ToContractor
        {
            get
            {
                return To_Type == (int)iType.Contractor;
            }
        }

        public string PTRNo
        {
            get
            {
                return string.Format("PTR-{0}", _ARENo);
            }
        }

        public string RetNo
        {
            get
            {
                return string.Format("RET-{0}", _ARENo);
            }
        }

        public void UpdateARENo()
        {
            using (samDataContext context = new samDataContext())
            {
                tblARE item = context.tblAREs.Where(x => x.Id == Id).Single();
                item._ARENo = string.Format("{0}-{1}", AREDate.Year.ToString(), Id.ToString("0000"));

                context.SubmitChanges();
            }
            
        }
        
        public void CleanUp()
        {
            using (samDataContext context = new samDataContext())
            {
                List<tblAREItem> items = context.tblAREItems.Where(x => x.AREId == Id).ToList();
                context.tblAREItems.DeleteAllOnSubmit(items);
                context.SubmitChanges();
            }
        }

        public void UpdateFields()
        {
            using (dalDataContext _context = new dalDataContext())
            {
                using (samDataContext context = new samDataContext())
                {
                    if (From_Type == null)
                    {
                        _AREType = (int)AREType.PAR;
                    }
                    else if (To_Type == null)
                    {
                        _AREType = (int)AREType.Return;
                    }
                    else
                    {
                        _AREType = (int)AREType.PTR;
                    }

                    if (!FromContractor && _AREType != (int)AREType.PAR)
                    {
                        tblEmployee employee = _context.tblEmployees.Where(x => x.EmployeeId == From_Id).Single();
                       
                        _From_Name = employee.FullName_FN;
                    }
                    else if (FromContractor && _AREType != (int)AREType.PAR)
                    {
                        tblContractor contractor = context.tblContractors.Where(x => x.Id == From_Id).Single();

                        _From_Name = contractor.CompanyName;
                    }

                    if (!ToContractor && _AREType != (int)AREType.Return)
                    {
                        tblEmployee employee = _context.tblEmployees.Where(x => x.EmployeeId == To_Id).Single();
                       
                        _To_Name = employee.FullName_FN;
                        _To_Office = employee.Office.Office;
                        _To_Department = employee.Department;
                    }
                    else if (ToContractor && _AREType != (int)AREType.Return)
                    {
                        tblContractor contractor = context.tblContractors.Where(x => x.Id == To_Id).Single();

                        _To_Name = contractor.CompanyName;
                        _To_Address = contractor.Address;
                    }
                }
            }
        }
    }
}