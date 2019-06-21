using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeeManagementProject.Startup))]
namespace EmployeeManagementProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
