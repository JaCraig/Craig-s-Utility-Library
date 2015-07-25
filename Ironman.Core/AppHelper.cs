/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Ironman.Core.AppHelper), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Ironman.Core.AppHelper), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Ironman.Core.AppHelper), "End")]

namespace Ironman.Core
{
    #region Usings

    using Ironman.Core.Tasks;
    using Ironman.Core.Tasks.Enums;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Utilities.IO;
    using Utilities.IO.Logging.Enums;
    using Utilities.IO.Logging.Interfaces;
    using Utilities.IoC.Interfaces;

    #endregion Usings

    /// <summary>
    /// App helper, used to setup the application
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// Bootstrapper holder
        /// </summary>
        public static IBootstrapper Bootstrapper = null;

        /// <summary>
        /// End task
        /// </summary>
        public static void End()
        {
            if (Bootstrapper != null)
            {
                ILog Logger = Utilities.IO.Log.Get();
                if (Logger != null)
                {
                    Logger.LogMessage("Application ending", MessageType.Info);
                    Logger.LogMessage("Starting end tasks", MessageType.Info);
                }
                Bootstrapper.Resolve<TaskManager>().Run(RunTime.End);
                Bootstrapper.Dispose();
                Bootstrapper = null;
            }
        }

        /// <summary>
        /// Post start task
        /// </summary>
        public static void PostStart()
        {
            ILog Log = Utilities.IO.Log.Get();
            Log.LogMessage("Starting post start tasks", MessageType.Info);
            Bootstrapper.Resolve<TaskManager>().Run(RunTime.PostStart);
        }

        /// <summary>
        /// Pre start task
        /// </summary>
        public static void PreStart()
        {
            Bootstrapper = Utilities.IoC.Manager.Bootstrapper;
            ILog Log = Utilities.IO.Log.Get();
            Log.LogMessage("Ironman starting", MessageType.Info);
            Log.LogMessage("{0}", MessageType.Info, Bootstrapper.ToString());
            Log.LogMessage("Starting pre start tasks", MessageType.Info);
            Bootstrapper.Resolve<TaskManager>().Run(RunTime.PreStart);
            DependencyResolver.SetResolver(new Bootstrapper.DependencyResolver(Bootstrapper));
            if (ViewEngines.Engines != null)
            {
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine());
            }
        }

        /// <summary>
        /// Registers the API.
        /// </summary>
        /// <param name="Routes">The routes.</param>
        /// <param name="ControllerName">Name of the controller.</param>
        /// <param name="AreaName">Name of the area.</param>
        public static void RegisterAPI(RouteCollection Routes, string ControllerName, string AreaName)
        {
            Bootstrapper.Resolve<API.Manager.Manager>().RegisterRoutes(Routes, ControllerName, AreaName);
        }
    }
}