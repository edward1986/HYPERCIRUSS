using coreApp.Areas.Procurement.DAL;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.Procurement.Models
{
    public class ItemModel
    {
        public tblItem Item { get; set; }
        public tblPPMPItem PPMPItem { get; set; }

        public string Data
        {
            get
            {
                if (PPMPItem == null)
                {
                    return "";
                }
                else
                {
                    return JsonConvert.SerializeObject(PPMPItem);
                }
            }
        }
    }
}