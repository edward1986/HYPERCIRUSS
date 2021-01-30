using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM.Enums;
using coreApp.Areas.SAM.Interfaces;
using coreLib.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace coreApp.Areas.SAM
{
    public class RISItemModel
    {
        public tblRI RIS { get; set; }
        public tblRISItem Item { get; set; }
    }
}