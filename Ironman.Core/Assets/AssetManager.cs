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

#region Usings

using Ironman.Core.Assets.Enums;
using Ironman.Core.Assets.Interfaces;
using Ironman.Core.Assets.Transformers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using Utilities.DataTypes;
using Utilities.IO;

#endregion Usings

namespace Ironman.Core.Assets
{
    /// <summary>
    /// Asset manager class
    /// </summary>
    public class AssetManager
    {
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

        /// <summary>
        /// Content filters
        /// </summary>
        protected IEnumerable<IContentFilter> ContentFilters { get; private set; }

        /// <summary>
        /// File types that the system recognizes
        /// </summary>
        protected ListMapping<AssetType, string> FileTypes { get; private set; }

        /// <summary>
        /// Filters
        /// </summary>
        protected IEnumerable<IFilter> Filters { get; private set; }

        /// <summary>
        /// Order that the filters are run in
        /// </summary>
        protected System.Collections.Generic.List<RunTime> RunOrder { get; private set; }

        /// <summary>
        /// Translators
        /// </summary>
        protected IEnumerable<ITranslator> Translators { get; private set; }

        /// <summary>
        /// Auto creates the bundles for the given directory
        /// </summary>
        public void CreateBundles()
        {
            CreateBundles(new DirectoryInfo("~/Scripts"));
            CreateBundles(new DirectoryInfo("~/Content"));
        }

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
        public void Process(IList<IAsset> Assets, BundleResponse Response)
        {
            Contract.Requires<ArgumentNullException>(Response != null, "Response");
            if (Assets == null || Assets.Count == 0)
                return;
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
                if (Asset.Path.StartsWith("~") || Asset.Path.StartsWith("/"))
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
        /// Exports info about the asset manager as a string
        /// </summary>
        /// <returns>String version of the asset manager</returns>
        public override string ToString()
        {
            return new StringBuilder().AppendLine()
                                      .AppendLineFormat("Filters: {0}", Filters.ToString(x => x.Name))
                                      .AppendLineFormat("Translators: {0}", Translators.ToString(x => x.Name))
                                      .AppendFormat("Content Filters: {0}", ContentFilters.ToString(x => x.Name))
                                      .ToString();
        }

        /// <summary>
        /// Auto creates the bundles for the given directory
        /// </summary>
        /// <param name="Directory">Directory to create bundles from</param>
        private void CreateBundles(DirectoryInfo Directory)
        {
            if (Directory == null || !Directory.Exists)
                return;
            string BundleDirectory = Directory.FullName.Replace(new DirectoryInfo("~/").FullName, "~/").Replace("\\", "/");
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
            foreach (DirectoryInfo SubDirectory in Directory.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly))
            {
                CreateBundles(SubDirectory);
            }
        }

        /// <summary>
        /// Implements virtual files since the asset optimizer uses it
        /// </summary>
        private class VirtualFileHack : VirtualFile
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="Location">File location</param>
            public VirtualFileHack(string Location)
                : base(Location)
            {
                this.File = new FileInfo(Location);
            }

            /// <summary>
            /// File object
            /// </summary>
            protected FileInfo File { get; set; }

            /// <summary>
            /// Opens the file as a stream
            /// </summary>
            /// <returns>The stream version of the file</returns>
            public override System.IO.Stream Open()
            {
                return new System.IO.MemoryStream(File.ReadBinary());
            }
        }
    }
}