using Microsoft.Owin;
using Owin;
using ToDoList;

[assembly: OwinStartupAttribute(typeof(Startup))]

namespace ToDoList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}