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

using Ironman.Core.Assets.Enums;
using Ironman.Core.Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Utilities.IO;

namespace Ironman.Core.Assets
{
    /// <summary>
    /// Contains data about an individual asset
    /// </summary>
    public class Asset : IAsset
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the asset</param>
        /// <param name="AssetManager">The asset manager.</param>
        public Asset(string Path, AssetManager AssetManager)
        {
            Contract.Requires<ArgumentNullException>(AssetManager != null, "AssetManager");
            this.Path = Path;
            this.Included = new List<IAsset>();
            this.Manager = AssetManager;
            this.Type = Manager.DetermineType(Path);
            var File = new FileInfo(Path);
            this.LastModified = File.Modified;
            this.Content = File.Read();
            this.Minified = File.FullName.ToUpperInvariant().Contains(".MIN.");
        }

        /// <summary>
        /// Content of the asset
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Included assets
        /// </summary>
        public IList<IAsset> Included { get; set; }

        /// <summary>
        /// Last date/time the asset was modified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Is the asset minified
        /// </summary>
        public bool Minified { get; set; }

        /// <summary>
        /// The path to the asset
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Asset type
        /// </summary>
        public AssetType Type { get; set; }

        /// <summary>
        /// URL to the asset
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Asset manager
        /// </summary>
        protected AssetManager Manager { get; private set; }

        /// <summary>
        /// Determines if the two objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are the same, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var TempAsset = obj as Asset;
            if (obj == null)
                return false;
            return TempAsset.Path.ToUpperInvariant() == Path.ToUpperInvariant();
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code for the object</returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        /// <summary>
        /// Gets the string version of the asset
        /// </summary>
        /// <returns>The string version of the asset</returns>
        public override string ToString()
        {
            return Content;
        }
    }
}