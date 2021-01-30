using coreApp.Areas.SAM.Enums;
using System.ComponentModel.DataAnnotations;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblCategoryMetaData
    {
        
        [Required]
        public string Code { get; set; }

        [Required]
        public string Category { get; set; }
        
        public string Description { get; set; }

    }

    [MetadataType(typeof(tblCategoryMetaData))]
    public partial class tblCategory
    {
        public string Type_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(CategoryType), Type);
            }
        }
    }
}