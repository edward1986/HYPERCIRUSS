using System.Web.Routing;
using coreApp;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Module.Core;

namespace System.Web.Mvc
{

    public class UserAccessAuthorizeAttribute : AuthorizeAttribute
    {
        string allowedRoles = "";
        string allowedAccess = "";
        
        public UserAccessAuthorizeAttribute(string allowedRoles = "", string allowedAccess = "")
        {
            this.allowedRoles = allowedRoles;
            this.allowedAccess = allowedAccess;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!EvaluateRoles()) return false;
            if (!EvaluateAccess()) return false;

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
            string msg = string.Format("{0}, \"{1}\"", ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACCESS_TO_RESOURCE, request.Url.AbsolutePath);

            Procs.accessDenied(filterContext, msg);
        }

        private bool EvaluateRoles()
        {
            bool ret = true;

            UserAccess access = Cache.Get().userAccess;

            if (allowedRoles != "")
            {
                ret = false;

                foreach (string role in allowedRoles.Split(','))
                {
                    if (role == "") continue;

                    if ((role == "admin" && access.IsAdmin) || (role == "employee" && access.IsEmployee) || 
                        (role == "staff" && access.IsStaff) || 
                        (role == "hr-staff" && access.IsHRStaff) || (role == "finance-staff" && access.IsFinanceStaff))
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }

        private bool EvaluateAccess()
        {
            bool ret = true;

            UserAccess access = Cache.Get().userAccess;

            if (allowedAccess != "")
            {
                ret = false;

                foreach (string role in allowedAccess.Split(','))
                {
                    if (role == "") continue;

                    if (access.HasAccess(role))
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }
    }
}