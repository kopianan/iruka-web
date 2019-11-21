using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Iruka.Startup))]
namespace Iruka
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
