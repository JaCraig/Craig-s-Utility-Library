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
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using Batman.Core.FileSystem.Interfaces;
using Batman.Core;
#endregion

namespace Batman.MVC.Assets.Filters
{
    /// <summary>
    /// Goes through and sets up the asset tree for JS files
    /// </summary>
    public class JSSetupImports : IFilter
    {
        /// <summary>
        /// Filter name
        /// </summary>
        public string Name { get { return "JS reference fix"; } }

        /// <summary>
        /// Time to run the filter
        /// </summary>
        public RunTime TimeToRun { get { return RunTime.PostTranslate; } }

        /// <summary>
        /// Used to determine what files to remove
        /// </summary>
        private Regex FileImportRegex = new Regex(@"/// <reference path=""(?<File>[^""]*)"" />", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Filters the assets
        /// </summary>
        /// <param name="Assets">Assets to filter</param>
        /// <returns>The filtered assets</returns>
        public IList<IAsset> Filter(IList<IAsset> Assets)
        {
            if (Assets == null || Assets.Count == 0)
                return new List<IAsset>();
            if (Assets.FirstOrDefault().Type != AssetType.Javascript)
                return Assets;
            FileManager FileSystem = BatComputer.Bootstrapper.Resolve<FileManager>();
            List<IAsset> TempAssets = new List<IAsset>();
            foreach (IAsset Asset in Assets)
            {
                bool Done = false;
                while (!Done)
                {
                    Done = true;
                    foreach (Match Import in FileImportRegex.Matches(Asset.Content))
                    {
                        Done = false;
                        string TempFile = Import.Groups["File"].Value;
                        string MatchString = Import.Value;
                        IFile File = FileSystem.File(TempFile);
                        if (File == null || !File.Exists)
                        {
                            IFile AssetFile = FileSystem.File(Asset.Path);
                            File = FileSystem.File(AssetFile.Directory.FullName + "\\" + TempFile);
                        }
                        IAsset SubAsset = Assets.FirstOrDefault(x => x.Path.ToUpperInvariant() == File.FullName.ToUpperInvariant());
                        if (SubAsset == null)
                        {
                            SubAsset = new Asset(File.FullName.Replace(FileSystem.Directory("~/").FullName, "~/").Replace("\\", "/"));
                        }
                        Asset.Included.Add(SubAsset);
                        Asset.Content = Asset.Content.Replace(MatchString, SubAsset.Content);
                    }
                }
                TempAssets.Add(Asset);
            }
            foreach (IAsset Asset in TempAssets)
            {
                foreach (IAsset IncludedAsset in Asset.Included)
                {
                    Assets.Remove(IncludedAsset);
                }
            }
            return Assets;
        }
    }
}