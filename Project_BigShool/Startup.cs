using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_BigShool.Startup))]
namespace Project_BigShool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
