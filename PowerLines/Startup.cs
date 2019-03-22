using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PowerLines.Startup))]
namespace PowerLines
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
