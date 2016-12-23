using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAnMonHoc.Startup))]
namespace DoAnMonHoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
