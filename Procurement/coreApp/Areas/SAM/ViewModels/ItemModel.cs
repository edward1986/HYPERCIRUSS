using coreApp.Areas.SAM.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class AREItemModel
    {
        public tblAREItem Item { get; set; }
        public tblARE ARE { get; set; }

        public AREItemModel(tblAREItem Item, tblARE ARE)
        {
            this.Item = Item;
            this.ARE = ARE;
        }
    }
}