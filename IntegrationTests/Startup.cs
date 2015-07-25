using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IntegrationTests.Startup))]

namespace IntegrationTests
{
    /// <summary>
    /// OWIN Startup
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configuration for OWIN
        /// </summary>
        /// <param name="app">App builder</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}