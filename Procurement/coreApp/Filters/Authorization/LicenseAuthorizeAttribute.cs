using coreApp;
using Module.Core;

namespace System.Web.Mvc
{

    public class LicenseAuthorizeAttribute : AuthorizeAttribute
    {
        string moduleName;

        public LicenseAuthorizeAttribute(string moduleName)
        {
            this.moduleName = moduleName;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool ret = true;

            switch (moduleName)
            {
                case "Procurement":
                    ret = MvcApplication.ApplicationLicense.Info.Modules_Procurement;
                    break;
                case "SAM":
                    ret = MvcApplication.ApplicationLicense.Info.Modules_SAM;
                    break;
            }

            return ret;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            string msg = string.Format("{0}, \"{1}\"", ModuleConstants_Authorization.MODULEACCESS_MODULE_IS_NOT_ENABLED, request.Url.AbsolutePath);

            Procs.accessDenied(filterContext, msg);
        }
    }
}