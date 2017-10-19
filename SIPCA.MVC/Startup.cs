using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIPCA.MVC.Startup))]
namespace SIPCA.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
