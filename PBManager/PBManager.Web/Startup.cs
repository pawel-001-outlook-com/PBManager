using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PBManager.Web.Startup))]
namespace PBManager.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
