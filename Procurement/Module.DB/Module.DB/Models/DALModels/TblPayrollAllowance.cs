using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace Module.DB
{
    public partial class TblPayrollAllowancesMetaData
    {
        [Required]
        [Display(Name = "Id")]
        public int AllowanceID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string AllowanceName { get; set; }

        [Display(Name ="Default Amount")]
        [Range(0, 999999, ErrorMessage = "Invalid Amount value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public decimal DefaultAmount { get; set; }
               

    }

    [MetadataType(typeof(TblPayrollAllowancesMetaData))]
    public partial class TblPayrollAllowance
    {
       
    }
}