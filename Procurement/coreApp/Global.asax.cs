using Aspose.Words;
using Licenses;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace coreApp
{
    public class MvcApplication : HttpApplication
    {
        public static cachedTables CachedTables;
        public static Licenses.License ApplicationLicense;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Cache.Get_Tables();
                       
            ApplicationLicense = new Licenses.License(Server.MapPath("~/application.lic"), FixedSettings.ApplicationName);

            Aspose.Words.License Wordslicense = new Aspose.Words.License();
            Wordslicense.SetLicense("Aspose.total.lic");

             Aspose.Cells.License Cellslicense = new Aspose.Cells.License();
            Cellslicense.SetLicense("Aspose.total.lic");

        }

        protected void Session_Start()
        {
            FixedSettings.Init();
           
            ApplicationLicense.Validate();
           
        }

        void Application_Error(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var exception = context.Server.GetLastError();

            context.Session["ApplicationErrorMessage"] = coreProcs.ShowErrors(exception);

            Response.Redirect("/Error");

        }
    }
}
