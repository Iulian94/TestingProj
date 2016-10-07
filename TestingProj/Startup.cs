using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestingProj.Startup))]
namespace TestingProj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
