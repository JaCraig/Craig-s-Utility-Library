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

using NuGet;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Utilities.DataTypes;

namespace Ironman.Models.Plugins
{
    /// <summary>
    /// Plugin model
    /// </summary>
    [Serializable]
    public class Plugin
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Plugin()
        {
            Files = new List<PluginFile>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="Package">The package.</param>
        public Plugin(IPackage Package)
            : this()
        {
            Contract.Requires<ArgumentNullException>(Package != null, "Package");
            this.PluginID = Package.Id;
            this.Version = Package.Version.ToString();
            this.Author = Package.Authors.ToString(x => x);
            this.Description = Package.Description;
            this.LastUpdated = Package.Published.Value.DateTime;
            this.Name = Package.Title;
            this.OnlineVersion = Package.Version.ToString();
            this.Tags = Package.Tags;
            this.Website = Package.ProjectUrl.ToString();
            this.Active = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Plugin"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>The files.</value>
        public List<PluginFile> Files { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the online version.
        /// </summary>
        /// <value>The online version.</value>
        public string OnlineVersion { get; set; }

        /// <summary>
        /// Gets or sets the plugin identifier.
        /// </summary>
        /// <value>The plugin identifier.</value>
        public string PluginID { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets a value indicating whether an update is available.
        /// </summary>
        /// <value><c>true</c> if [update available]; otherwise, <c>false</c>.</value>
        public bool UpdateAvailable { get { return int.Parse(Version.Keep(StringFilter.Numeric)) < int.Parse(OnlineVersion.Keep(StringFilter.Numeric)); } }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        public string Website { get; set; }

        /// <summary>
        /// Deletes this instance and removes the files associated with it.
        /// </summary>
        public void Delete()
        {
            Files.OrderByDescending(x => x.Order).ForEach(x => x.Remove());
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            if (Active)
            {
                var LoadingDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/plugins/Loaded/" + PluginID);
                var PluginDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/plugins/" + PluginID);
                CopyTo(PluginDirectory, LoadingDirectory);
                LoadingDirectory.EnumerateFiles("*.dll", System.IO.SearchOption.AllDirectories)
                    .ForEach(x => AssemblyName.GetAssemblyName(x.FullName).Load());
            }
        }

        /// <summary>
        /// Saves this instance and copies the files associated with it.
        /// </summary>
        public void Save()
        {
            if (Files.Count == 0)
            {
                var LibDirectory = new DirectoryInfo(string.Format(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/packages/{0}.{1}/lib") : "./App_Data/packages/{0}.{1}/lib", PluginID, Version)).EnumerateDirectories().OrderByDescending(x => x.Name).First();
                var ContentDirectory = new DirectoryInfo(string.Format(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/packages/{0}.{1}/content") : "./App_Data/packages/{0}.{1}/content", PluginID, Version));
                int y = 0;
                var TempFiles = new List<PluginFile>();
                TempFiles.Add(LibDirectory.EnumerateFiles("*", System.IO.SearchOption.AllDirectories).ForEach(x => new PluginFile
                {
                    IsDirectory = false,
                    Order = y++,
                    Path = "~/App_Data/plugins/" + PluginID + x.FullName.Replace(LibDirectory.FullName, "").Replace("\\", "/")
                }));
                TempFiles.Add(ContentDirectory.EnumerateFiles("*", System.IO.SearchOption.AllDirectories).ForEach(x => new PluginFile
                {
                    IsDirectory = false,
                    Order = y++,
                    Path = "~" + x.FullName.Replace(ContentDirectory.FullName, "").Replace("\\", "/")
                }));
                Files = TempFiles;
                new DirectoryInfo(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/plugins/" + PluginID) : "./App_Data/plugins/" + PluginID).Create();
                CopyTo(LibDirectory, new DirectoryInfo(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/plugins/" + PluginID) : "./App_Data/plugins/" + PluginID));
                CopyTo(ContentDirectory, new DirectoryInfo(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/") : "./"));
                Delete(new DirectoryInfo(string.Format(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/packages/{0}/") : "./App_Data/packages/{0}/", PluginID)));
                Delete(new DirectoryInfo(string.Format(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/packages/{0}.{1}/") : "./App_Data/packages/{0}.{1}/", PluginID, Version)));
            }
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory1">The directory1.</param>
        /// <param name="Directory2">The directory2.</param>
        private void CopyTo(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return;
            Directory2.Create();
            foreach (FileInfo TempFile in Directory1.EnumerateFiles())
            {
                try
                {
                    TempFile.CopyTo(Path.Combine(Directory2.FullName, TempFile.Name), true);
                }
                catch { }
            }
            foreach (DirectoryInfo SubDirectory in Directory1.EnumerateDirectories())
                CopyTo(SubDirectory, new DirectoryInfo(Path.Combine(Directory2.FullName, SubDirectory.Name)));
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
                try
                {
                    File.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo SubDirectory in Directory.EnumerateDirectories())
            {
                Delete(SubDirectory);
            }
            try
            {
                Directory.Delete();
            }
            catch { }
        }
    }
}