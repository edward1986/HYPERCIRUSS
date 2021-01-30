using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(coreApp.Startup))]
namespace coreApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
