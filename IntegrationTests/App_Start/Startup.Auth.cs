using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Utilities.DataTypes;

namespace IntegrationTests
{
    /// <summary>
    /// OWIN Startup
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="app">The application builder</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            if (app == null)
                return;
            //Utilities.IoC.Manager.Bootstrapper.Register<IAppBuilder>(app);
            var Config = WebConfigurationManager.OpenWebConfiguration("/");
            var AuthSection = (AuthenticationSection)Config.GetSection("system.web/authentication");
            if (AuthSection.Mode == AuthenticationMode.Forms)
            {
                // Enable the application to use a cookie to store information for the signed in user
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    LogoutPath = new PathString("/Account/LogOff")
                });
                // Use a cookie to temporarily store information about a user logging in with a
                // third party login provider
                app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            }
        }
    }
}