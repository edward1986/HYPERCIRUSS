using System.Web.Mvc;

namespace coreApp.Areas.AdminModule
{
    public class AdminModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
          
           

            context.MapRoute(
                "AdminModule_default",
                "AdminModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}