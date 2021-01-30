using coreApp;
using coreApp.DAL;
using Module.Core;
using Module.DB;
using System.Linq;

namespace System.Web.Mvc
{

    public class EmployeeAuthorizeAttribute : AuthorizeAttribute
    {
        string idName = "employeeId";
        int id = -1;

        public EmployeeAuthorizeAttribute(string idName)
        {
            this.idName = idName;
        }

        public EmployeeAuthorizeAttribute()
        { }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            id = int.Parse(coreProcs.getRouteParam(httpContext.Request, idName));

            if (id == -1)
            {
                return true;
            }
            else
            {
                return Cache.Get().userAccess.AllowedEmployee(id);
            }
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

            string info = request.Url.AbsolutePath;

            using (dalDataContext context = new dalDataContext())
            {
                tblEmployee employee = context.tblEmployees.Where(x => x.EmployeeId == id).SingleOrDefault();
                if (employee != null)
                {
                    info = employee.FullName_FN;
                }
            }

            string msg = string.Format("{0}, \"{1}\"", ModuleConstants_Authorization.EMPLOYEEACCESS_UNAUTHORIZED_ACCESS_TO_RESOURCE, info);

            Procs.accessDenied(filterContext, msg);
        }

    }

}