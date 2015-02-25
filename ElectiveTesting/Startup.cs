using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElectiveTesting.Startup))]
namespace ElectiveTesting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
