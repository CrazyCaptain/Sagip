using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Greenpeace_Advisory.Startup))]
namespace Greenpeace_Advisory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
