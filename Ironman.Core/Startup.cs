[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Batman.Core.AppHelper), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Batman.Core.AppHelper), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Batman.Core.AppHelper), "End")]

namespace Ironman.Core
{
    #region Usings

    using Batman.Core.Bootstrapper.Interfaces;
    using Batman.Core.Logging;
    using Batman.Core.Logging.BaseClasses;
    using Batman.Core.Serialization;
    using Batman.MVC.Assets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Utilities.IO;
    using Utilities.IO.Logging.Enums;

    #endregion Usings

    /// <summary></summary>
    public static class AppHelper
    {
        public static void End()
        {
            BatComputer.End();
        }

        public static void PostStart()
        {
            BatComputer.PostStart();
        }

        public static void PreStart()
        {
            BatComputer.PreStart();
            Log.Get().LogMessage("Current asset helpers: {0}", MessageType.Debug, BatComputer.Bootstrapper.Resolve<AssetManager>().ToString());
            Log.Get().LogMessage("Current serializers: {0}", MessageType.Debug, BatComputer.Bootstrapper.Resolve<SerializationManager>().ToString());
            DependencyResolver.SetResolver(new Bootstrapper.DependencyResolver(BatComputer.Bootstrapper));
        }
    }
}