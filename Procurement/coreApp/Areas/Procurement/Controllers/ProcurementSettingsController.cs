using System.Web.Mvc;
using System.Linq;
using System;
using coreApp.Controllers;
using System.Collections.Generic;
using Module.DB;
using Module.Core;
using coreApp.Filters;
using coreApp.Interfaces;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using coreApp.Areas.Procurement.DAL;
using coreLib.Objects;

namespace coreApp.Areas.Procurement.Controllers
{
    [UserAccessAuthorize(allowedAccess: "procurement_item_settings")]
    [EffectivityInfoFilter("Settings_Effectivities")]
    public class ProcurementSettingsController : Base_NoCoreAuthorizedController, IEffectivityController
    {
        public string effectivity { get; set; }

        public ActionResult Index()
        {
            return View(Common.Settings(effectivity));
        }

        public ActionResult Edit(int id)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                tblSetting s = context.tblSettings.Where(x => x.SettingId == id).SingleOrDefault();
                if (s == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                if (effectivity != ModuleConstants.DEFAULT_EFFECTIVITY)
                {
                    DateTime _dt = Convert.ToDateTime(effectivity);
                    List<DateTime> dates = (List<DateTime>)ViewBag.Effectivities;

                    foreach (DateTime dt in dates.Where(x => x <= _dt))
                    {
                        tblSettings_Effectivity eff = context.tblSettings_Effectivities.Where(x => x.DateEffective == dt && x.SettingId == id).SingleOrDefault();
                        if (eff != null)
                        {
                            s.SettingValue = eff._Value;
                            break;
                        }
                    }
                }
                
                return PartialView(s);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "Effectivity")] string Effectivity, tblSetting model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };


            if (ModelState.IsValid)
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    if (Effectivity == ModuleConstants.DEFAULT_EFFECTIVITY)
                    {
                        tblSetting setting = context.tblSettings.Where(x => x.SettingId == model.SettingId).Single();
                        if (model.ValueType == "bool")
                        {
                            setting.SettingValue = model.SettingValue == "on" ? "true" : "false";
                        }
                        else
                        {
                            setting.SettingValue = model.SettingValue;
                        }

                    }
                    else
                    {
                        DateTime dt = Convert.ToDateTime(Effectivity);

                        tblSettings_Effectivity eff = context.tblSettings_Effectivities.Where(x => x.DateEffective == dt && x.SettingId == model.SettingId).SingleOrDefault();
                        if (eff == null)
                        {
                            eff = new tblSettings_Effectivity
                            {
                                DateEffective = dt,
                                SettingId = model.SettingId
                            };

                            if (model.ValueType == "bool")
                            {
                                eff._Value = model.SettingValue == "on" ? "true" : "false";
                            }
                            else
                            {
                                eff._Value = model.SettingValue;
                            }

                            context.tblSettings_Effectivities.InsertOnSubmit(eff);
                        }
                        else
                        {
                            if (model.ValueType == "bool")
                            {
                                eff._Value = model.SettingValue == "on" ? "true" : "false";
                            }
                            else
                            {
                                eff._Value = model.SettingValue;
                            }
                        }

                    }

                    context.SubmitChanges();

                    res.Remarks = "Record was successfully updated";

                    Cache.Update();
                    Cache.Update_Tables();
                }

                FixedSettings.Init();
            }
            else
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ModelState);
            }

            return Json(res);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            using (procurementDataContext context = new procurementDataContext())
            {
                if (ModelState.IsValid)
                {
                    DateTime dt = Convert.ToDateTime(effectivity);
                    tblSettings_Effectivity eff = context.tblSettings_Effectivities.Where(x => x.SettingId == id && x.DateEffective == dt).SingleOrDefault();
                    if (eff == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    else
                    {
                        context.tblSettings_Effectivities.DeleteOnSubmit(eff);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";

                        Cache.Update();
                        Cache.Update_Tables();
                    }

                }
                else
                {
                    res.IsSuccessful = false;
                    res.Err = coreProcs.ShowErrors(ModelState);
                }

                return Json(res);
            }
        }
    }
}