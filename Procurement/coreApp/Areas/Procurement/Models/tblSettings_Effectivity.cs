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

    public partial class tblSettings_EffectivitiesMetaData
    { }
    [MetadataType(typeof(tblSettings_EffectivitiesMetaData))]
    public partial class tblSettings_Effectivity
    {
        public bool IsCustom
        {
            get
            {
                return Id != -1;
            }
        }

        public string Value
        {
            get
            {
                string ret = _Value;

                if (string.IsNullOrEmpty(_Value) && Setting != null)
                {
                    ret = Setting.SettingValue;
                }

                return ret;
            }
        }

        public tblSetting Setting
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    return context.tblSettings.Where(x => x.SettingId == SettingId).SingleOrDefault();
                }
            }
        }
    }
}