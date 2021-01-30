using coreApp;
using coreApp.DAL;
using Module.Core;
using System.Linq;

namespace System.Web.Mvc
{

    public class KnownDeviceAuthorizeAttribute : AuthorizeAttribute
    {

        public KnownDeviceAuthorizeAttribute()
        { }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
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
            
            Procs.accessDenied(filterContext, ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
        }

    }

}