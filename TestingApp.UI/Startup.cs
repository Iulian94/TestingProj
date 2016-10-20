using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestingApp.UI.Startup))]
namespace TestingApp.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
