using coreApp.Areas.Procurement.Enums;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblMOPMetaData
    { }

    [MetadataType(typeof(tblMOPMetaData))]
    public partial class tblMOP
    {
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double _Amount { get; set; }

        public string Category_Desc
        {
            get
            {
                string ret = "";

                using (procurementDataContext context = new procurementDataContext())
                {
                    var tmp = context.tblCategories.Where(x => x.Id == CategoryId).SingleOrDefault();
                    if (tmp != null)
                    {
                        ret = tmp.Category;
                    }
                }

                return ret;
            }
        }

        public string ModeOfProcurement_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(ModeOfProcurement), ModeOfProcurement);
            }
        }

        public string MOPType_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(MOPType), Type);
            }
        }
    }
}