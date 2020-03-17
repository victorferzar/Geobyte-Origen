using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(geologycmcc.Startup))]
namespace geologycmcc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
