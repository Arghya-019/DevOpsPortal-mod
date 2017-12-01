using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevOpsPortal.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile="Web.config", Watch=true)]
namespace DevOpsPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
