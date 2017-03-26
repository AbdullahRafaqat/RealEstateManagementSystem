using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RealEstate.Startup))]
namespace RealEstate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
