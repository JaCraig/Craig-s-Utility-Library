/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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

#region Usings
using System;
using Batman.Core.Bootstrapper.Interfaces;
using System.Web.Mvc;
using System.Collections.Generic;
using Batman.MVC.Assets.Interfaces;
using Batman.MVC.Assets.Enums;
using Batman.Core.FileSystem;
using Utilities.DataTypes.ExtensionMethods;
using System.Linq;
using System.Web.Optimization;
using System.IO;
using Batman.MVC.Assets.Utils;

using Batman.Core.FileSystem.Interfaces;
using System.Web;
using Utilities.DataTypes;
using Batman.MVC.Assets.Transformers;
using Batman.Core;
using System.Text;
using System.Web.Hosting;
using Batman.Core.Logging.BaseClasses;
using Batman.Core.Logging;
#endregion

namespace Batman.MVC.Assets
{
    /// <summary>
    /// Asset manager class
    /// </summary>
    public class AssetManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AssetManager()
        {
            Filters = AppDomain.CurrentDomain.GetAssemblies().Objects<IFilter>();
            ContentFilters = AppDomain.CurrentDomain.GetAssemblies().Objects<IContentFilter>();
            Translators = AppDomain.CurrentDomain.GetAssemblies().Objects<ITranslator>();
            FileTypes = new ListMapping<AssetType, string>();
            RunOrder = new System.Collections.Generic.List<RunTime>();
            Translators.ForEach(x => FileTypes.Add(x.TranslatesTo, x.FileTypeAccepts));
            FileTypes.Add(AssetType.CSS, "css");
            FileTypes.Add(AssetType.Javascript, "js");
            RunOrder.Add(RunTime.PostTranslate);
            RunOrder.Add(RunTime.PreMinified);
            RunOrder.Add(RunTime.Minify);
            RunOrder.Add(RunTime.PostMinified);
            RunOrder.Add(RunTime.PreCombine);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Filters
        /// </summary>
        protected IEnumerable<IFilter> Filters { get; private set; }

        /// <summary>
        /// Translators
        /// </summary>
        protected IEnumerable<ITranslator> Translators { get; private set; }

        /// <summary>
        /// Content filters
        /// </summary>
        protected IEnumerable<IContentFilter> ContentFilters { get; private set; }

        /// <summary>
        /// File types that the system recognizes
        /// </summary>
        protected ListMapping<AssetType, string> FileTypes { get; private set; }

        /// <summary>
        /// File manager
        /// </summary>
        protected FileManager FileManager { get { return BatComputer.Bootstrapper.Resolve<FileManager>(); } }

        /// <summary>
        /// Order that the filters are run in
        /// </summary>
        protected System.Collections.Generic.List<RunTime> RunOrder { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Determines the asset type
        /// </summary>
        /// <param name="Path">Path to the asset</param>
        /// <returns>The asset type</returns>
        public AssetType DetermineType(string Path)
        {
            AssetType Type = Translators.FirstOrDefault(x => Path.EndsWith(x.FileTypeAccepts))
                              .Chain(x => x.TranslatesTo, AssetType.Unknown);
            if (Type == AssetType.Unknown && Path.EndsWith("css"))
                return AssetType.CSS;
            else if (Type == AssetType.Unknown && Path.EndsWith("js"))
                return AssetType.Javascript;
            return Type;
        }

        /// <summary>
        /// Processes the assets
        /// </summary>
        /// <param name="Assets">Assets to process</param>
        /// <param name="Context">The bundle context</param>
        /// <param name="Response">The bundle response</param>
        public void Process(IList<IAsset> Assets, BundleContext Context, BundleResponse Response)
        {
            foreach (IFilter Filter in Filters.Where(x => x.TimeToRun == RunTime.PreTranslate))
            {
                Assets = Filter.Filter(Assets);
            }
            foreach (ITranslator Translator in Translators)
            {
                Assets = Translator.Translate(Assets);
            }
            foreach (RunTime Item in RunOrder)
            {
                foreach (IFilter Filter in Filters.Where(x => x.TimeToRun == Item))
                {
                    Assets = Filter.Filter(Assets);
                }
            }
            string Content = Assets.OrderBy(x => x.Path).ToString(x => x.ToString(), "");
            foreach (IContentFilter Filter in ContentFilters)
            {
                Content = Filter.Filter(Content);
            }
            System.Collections.Generic.List<BundleFile> Files = new System.Collections.Generic.List<BundleFile>();
            foreach (IAsset Asset in Assets)
            {
                if (Asset.Path.StartsWith("~")||Asset.Path.StartsWith("/"))
                {
                    Files.Add(new BundleFile(Asset.Path, new VirtualFileHack(Asset.Path)));
                    Files.Add(Asset.Included.Where(x => x.Path.StartsWith("~") || x.Path.StartsWith("/")).Select(x => new BundleFile(x.Path, new VirtualFileHack(x.Path))));
                }
            }
            Response.Content = Content;
            Response.Files = Files.OrderBy(x => x.VirtualFile.VirtualPath);
            Response.ContentType = Assets.First().Type == AssetType.CSS ? "text/css" : "text/javascript";
            Response.Cacheability = HttpCacheability.ServerAndPrivate;
        }

        /// <summary>
        /// Auto creates the bundles for the given directory
        /// </summary>
        public void CreateBundles()
        {
            CreateBundles(FileManager.Directory("~/Scripts"));
            CreateBundles(FileManager.Directory("~/Content"));
        }

        /// <summary>
        /// Auto creates the bundles for the given directory
        /// </summary>
        /// <param name="Directory">Directory to create bundles from</param>
        private void CreateBundles(IDirectory Directory)
        {
            string BundleDirectory = Directory.FullName.Replace(FileManager.Directory("~/").FullName, "~/").Replace("\\", "/");
            StyleBundle Bundle = new StyleBundle(BundleDirectory + "/bundle/css");
            Bundle.Transforms.Clear();
            Bundle.Transforms.Add(new Transformer());
            if (Directory.Exists)
            {
                foreach (string Value in FileTypes[AssetType.CSS])
                {
                    Bundle.IncludeDirectory(BundleDirectory, "*." + Value, true);
                }
            }
            ScriptBundle Bundle2 = new ScriptBundle(BundleDirectory + "/bundle/js");
            Bundle2.Transforms.Clear();
            Bundle2.Transforms.Add(new Transformer());
            if (Directory.Exists)
            {
                foreach (string Value in FileTypes[AssetType.Javascript])
                {
                    Bundle2.IncludeDirectory(BundleDirectory, "*." + Value, true);
                }
            }
            BundleTable.Bundles.Add(Bundle);
            BundleTable.Bundles.Add(Bundle2);
            foreach (IDirectory SubDirectory in Directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                CreateBundles(SubDirectory);
            }
        }

        public override string ToString()
        {
            return new StringBuilder().AppendLine()
                                      .AppendLineFormat("Filters: {0}", Filters.ToString(x => x.Name))
                                      .AppendLineFormat("Translators: {0}", Translators.ToString(x => x.Name))
                                      .AppendFormat("Content Filters: {0}", ContentFilters.ToString(x => x.Name))
                                      .ToString();
        }

        #endregion

        public class VirtualFileHack:VirtualFile
        {
            public VirtualFileHack(string Location)
                : base(Location)
            {
                this.File = BatComputer.Bootstrapper.Resolve<FileManager>().File(Location);
            }

            protected IFile File { get; set; }

            public override Stream Open()
            {
                return new MemoryStream(File.ReadBinary());
            }
        }
    }
}