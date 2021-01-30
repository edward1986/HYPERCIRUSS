using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using System.Web;
using coreApp.Filters;
using coreApp.DAL;
using System.IO;
using System.Linq;
using Module.DB;

namespace coreApp.Controllers
{
    [ErrorFilter]
    //[LicenseFilter]
    public class Base : Controller
    {
        protected void AddErrors(IdentityResult result)
        {
            if (result != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
        }

        protected void AddError(string error)
        {
            ModelState.AddModelError("", error);
        }
        
        public Base()
        { }

        public Base(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        protected ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult GetPhoto(string type, int employeeId)
        {
            using (dalDataContext context = new dalDataContext())
            {
                string dir = Server.MapPath("~/Assets/images");
                string path = Path.Combine(dir, "anonymous.jpg");
                
                byte[] arr = new byte[] { };

                if (type == "pds-photo")
                {
                    tblEmployee_Info info = context.tblEmployee_Infos.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
                    if (info != null)
                    {
                        if (info.PDSPhoto != null)
                        {
                            arr = info.PDSPhoto.ToArray();
                        }                        
                    }
                }
                else if (type == "profile-photo")
                {
                    AspNetUsers_Photo p = context.AspNetUsers_Photos.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
                    if (p != null)
                    {
                        if (p.Photo != null)
                        {
                            arr = p.Photo.ToArray();
                        }                        
                    }
                }

                if (arr.Length > 0)
                {
                    path = Path.Combine(dir, "unauthorized.jpg");
                    bool allowedToView = Cache.Get().userAccess.IsAdmin || Cache.Get().userAccess.AllowedEmployee(employeeId);

                    if (allowedToView)
                    {
                        Response.Clear();
                        Response.ContentType = "image/jpeg";
                        //Response.AddHeader("Content-Disposition", "attachment; filename=myfile.docx");
                        Response.BinaryWrite(arr);
                        Response.Flush();
                        Response.Close();
                        Response.End();
                    }
                }

                return base.File(path, "image/jpeg");
            }
        }
              
    }
}
