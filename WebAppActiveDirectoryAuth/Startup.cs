using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppActiveDirectoryAuth.Startup))]
namespace WebAppActiveDirectoryAuth
{
    public partial class Startup
    {
     

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
