using coreApp.Areas.Procurement.Models;
using System.Collections.Generic;
using System.Web;

namespace coreApp.Areas.Procurement
{   
    public static class ModelOptimizer
    {
        public static ModelCache Get(bool forceNew = false)
        {
            ModelCache ret;

            if (HttpContext.Current.Session["ModelOptimizer"] == null || forceNew)
            {
                ret = new ModelCache();
                HttpContext.Current.Session["ModelOptimizer"] = ret;
            }
            else
            {
                ret = (ModelCache)HttpContext.Current.Session["ModelOptimizer"];
            }

            return ret;
        }
    }

    public class ModelCache
    {
        Dictionary<string, CompanyAPPModel> CompanyAPPModels { get; set; }

        public ModelCache()
        {
            CompanyAPPModels = new Dictionary<string, CompanyAPPModel>();
        }

        public bool CompanyAPPModels_Contains(string key)
        {
            return CompanyAPPModels.ContainsKey(key);
        }

        public CompanyAPPModel GetCompanyAPPModel(string key)
        {
            CompanyAPPModel ret = null;

            if (CompanyAPPModels.ContainsKey(key))
            {
                ret = CompanyAPPModels[key];
            }

            return ret;
        }

        public void SaveCompanyAPPModel(string key, CompanyAPPModel model)
        {
            if (CompanyAPPModels.ContainsKey(key))
            {
                CompanyAPPModels[key] = model;
            }
            else
            {
                CompanyAPPModels.Add(key, model);
            }

            HttpContext.Current.Session["ModelOptimizer"] = this;
        }
    }
}
