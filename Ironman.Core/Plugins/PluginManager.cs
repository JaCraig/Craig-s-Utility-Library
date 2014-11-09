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

using Ironman.Core.Plugins.Interfaces;
using Ironman.Models.Plugins;
using NuGet;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.IO.Logging.Enums;

namespace Ironman.Core.Plugins
{
    /// <summary>
    /// Plugin manager
    /// </summary>
    public class PluginManager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManager" /> class.
        /// </summary>
        /// <param name="Repositories">The repositories.</param>
        public PluginManager(IEnumerable<string> Repositories)
        {
            Contract.Requires<ArgumentNullException>(Repositories != null, "Repositories");
            PackageRepositories = Repositories.ForEach(x => PackageRepositoryFactory.Default.CreateRepository(x));
            Initialize();
        }

        /// <summary>
        /// Gets or sets the plugin list.
        /// </summary>
        /// <value>The plugin list.</value>
        public PluginList PluginList { get; private set; }

        /// <summary>
        /// Gets the plugins available.
        /// </summary>
        /// <value>The plugins available.</value>
        public IEnumerable<Plugin> PluginsAvailable
        {
            get
            {
                List<Plugin> Results = new List<Plugin>();
                foreach (IPackageRepository Repo in PackageRepositories)
                {
                    Results.Add(Repo.GetPackages()
                        .Where(x => x.IsLatestVersion)
                        .ForEach(x => new Plugin(x))
                        .Where(x => !PluginList.Plugins.Any(y => string.Equals(y.PluginID, x.PluginID))));
                }
                return Results;
            }
        }

        /// <summary>
        /// Gets the package repositories.
        /// </summary>
        /// <value>The package repositories.</value>
        protected IEnumerable<IPackageRepository> PackageRepositories { get; private set; }

        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>The plugins.</value>
        protected IEnumerable<IPlugin> PluginsInstalled { get; set; }

        /// <summary>
        /// Restarts the system.
        /// </summary>
        public static void RestartSystem()
        {
            try
            {
                using (DirectoryEntry ApplicationPool = new DirectoryEntry(@"IIS://" + Environment.MachineName + "/W3SVC/AppPools/" + HttpContext.Current.Request.ServerVariables["APP_POOL_ID"]))
                {
                    ApplicationPool.Invoke("Recycle", null);
                }
                return;
            }
            catch { }
            File.SetLastWriteTimeUtc(AppDomain.CurrentDomain.BaseDirectory + "\\web.config", DateTime.UtcNow);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Delete(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/plugins/Loaded/"));
            new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/plugins/Loaded").Create();
            PluginList = PluginList.Load();
            foreach (Plugin TempPlugin in PluginList.Plugins)
            {
                TempPlugin.Initialize();
            }
            PluginsInstalled = AppDomain.CurrentDomain.GetAssemblies().Objects<IPlugin>();
            foreach (IPlugin TempPlugin in PluginsInstalled)
            {
                foreach (IPackageRepository Repo in PackageRepositories)
                {
                    IPackage Package = Repo.FindPackage(TempPlugin.PluginData.PluginID);
                    if (Package != null)
                    {
                        Plugin TempPluginData = PluginList.Get(Package.Id);
                        TempPluginData.OnlineVersion = Package.Version.ToString();
                    }
                }
            }
            PluginList.Save();
        }

        /// <summary>
        /// Installs the plugin associated with the ID
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns>Returns true if it is installed successfully, false otherwise</returns>
        public bool InstallPlugin(string ID)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(ID), "ID");
            string User = HttpContext.Current.Chain(x => x.User).Chain(x => x.Identity).Chain(x => x.Name, "");
            Utilities.IO.Log.Get().LogMessage("Plugin {0} is being installed by {1}", MessageType.Debug, ID, User);
            Plugin TempPlugin = PluginList.Get(ID);
            if (TempPlugin != null)
                UninstallPlugin(ID);
            foreach (IPackageRepository Repo in PackageRepositories)
            {
                IPackage Package = Repo.FindPackage(ID);
                if (Package != null)
                {
                    new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/packages/" + Package.Id + "." + Package.Version.ToString() + "/lib").Create();
                    new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/packages/" + Package.Id + "." + Package.Version.ToString() + "/content").Create();
                    new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/packages/" + Package.Id + "." + Package.Version.ToString() + "/tools").Create();
                    PackageManager Manager = new PackageManager(Repo,
                        new DefaultPackagePathResolver(Repo.Source),
                        new PhysicalFileSystem(new DirectoryInfo(HttpContext.Current != null ?
                            HttpContext.Current.Server.MapPath("~/App_Data/packages") :
                            "./App_Data/packages").FullName));
                    Manager.InstallPackage(Package, false, true);
                    PluginList.Add(new Plugin(Package));
                    Package.DependencySets.ForEach(x => x.Dependencies.ForEach(y => InstallPlugin(y.Id)));
                    PluginList.Save();
                    break;
                }
            }
            Utilities.IO.Log.Get().LogMessage("Plugin {0} has been installed by {1}", MessageType.Debug, ID, User);
            return true;
        }

        /// <summary>
        /// Uninstalls the plugin associated with the ID
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns>Returns true if it is uninstalled successfully, false otherwise</returns>
        public bool UninstallPlugin(string ID)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(ID), "ID");
            Plugin TempPlugin = PluginList.Get(ID);
            if (TempPlugin == null)
                return true;
            string User = HttpContext.Current.Chain(x => x.User).Chain(x => x.Identity).Chain(x => x.Name, "");
            Utilities.IO.Log.Get().LogMessage("Plugin {0} is being uninstalled by {1}", MessageType.Debug, ID, User);
            TempPlugin.Delete();
            Utilities.IO.Log.Get().LogMessage("Plugin {0} has been uninstalled by {1}", MessageType.Debug, ID, User);
            PluginList.Remove(TempPlugin);
            PluginList.Save();
            return true;
        }

        /// <summary>
        /// Updates the plugin associated with the ID
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns>Returns true if it is updated successfully, false otherwise</returns>
        public bool UpdatePlugin(string ID)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(ID), "ID");
            Plugin TempPlugin = PluginList.Get(ID);
            if (TempPlugin == null)
                return true;
            string User = HttpContext.Current.Chain(x => x.User).Chain(x => x.Identity).Chain(x => x.Name, "");
            bool Result = false;
            Utilities.IO.Log.Get().LogMessage("Plugin {0} is being updated by {1}", MessageType.Debug, ID, User);
            foreach (IPackageRepository Repo in PackageRepositories)
            {
                IPackage Package = Repo.FindPackage(ID);
                if (Package != null)
                {
                    TempPlugin = PluginList.Get(ID);
                    if (TempPlugin != null)
                    {
                        TempPlugin.OnlineVersion = Package.Version.ToString();
                        TempPlugin.Save();
                        if (TempPlugin.UpdateAvailable)
                            Result = UninstallPlugin(ID);
                        if (Result)
                            Result = InstallPlugin(ID);
                    }
                    break;
                }
            }
            return Result;
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">
        /// If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (PluginsInstalled != null)
            {
                PluginsInstalled.ForEach(x => x.Dispose());
                PluginsInstalled = null;
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="Directory">The directory.</param>
        private void Delete(DirectoryInfo Directory)
        {
            Contract.Requires<ArgumentNullException>(Directory != null, "Directory");
            if (!Directory.Exists)
                return;
            foreach (FileInfo File in Directory.EnumerateFiles())
            {
                File.Delete();
            }
            foreach (DirectoryInfo SubDirectory in Directory.EnumerateDirectories())
            {
                Delete(SubDirectory);
            }
            Directory.Delete();
        }
    }
}