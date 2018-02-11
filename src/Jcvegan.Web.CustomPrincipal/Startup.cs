using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jcvegan.Web.CustomPrincipal.Startup))]
namespace Jcvegan.Web.CustomPrincipal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
