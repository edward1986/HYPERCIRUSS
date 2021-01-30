using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using Module.DB;
using Module.DB.Procs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.DAL
{
    public partial class tblSettingsMetaData
    {
        [Display(Name = "Id")]
        public int SettingId { get; set; }

        [Display(Name = "Value")]
        public string SettingValue { get; set; }

        [Display(Name = "Value Type")]
        public string ValueType { get; set; }

    }

    [MetadataType(typeof(tblSettingsMetaData))]
    public partial class tblSetting
    {
        public DateTime DateUpdated { get; set; }
    }
}