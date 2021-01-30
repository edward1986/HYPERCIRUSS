using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Module.DB
{
    public partial class tblSalaryGradeMetaData
    {
        [Display(Name = "Salary Grade")]
        public int SalaryGrade { get; set; }

        [Display(Name = "Date Effective")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime DateEffective { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step1 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step2 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step3 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step4 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step5 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step6 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step7 { get; set; }

        [DataType(dataType: DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Step8 { get; set; }
    }

    [MetadataType(typeof(tblSalaryGradeMetaData))]
    public partial class tblSalaryGrade
    { }
}