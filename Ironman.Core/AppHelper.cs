[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Ironman.Core.AppHelper), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Ironman.Core.AppHelper), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Ironman.Core.AppHelper), "End")]

namespace Ironman.Core
{
    #region Usings

    using System.Web.Mvc;
    using Ironman.Core.Tasks;
    using Ironman.Core.Tasks.Enums;
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
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}