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

using System;
using System.Collections.Generic;
using Ironman.Core.Assets.Enums;

#endregion Usings

namespace Ironman.Core.Assets.Interfaces
{
    /// <summary>
    /// Asset interface
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Content of the asset
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Included assets
        /// </summary>
        IList<IAsset> Included { get; set; }

        /// <summary>
        /// Last date/time the asset was modified
        /// </summary>
        DateTime LastModified { get; set; }

        /// <summary>
        /// Is the asset minified
        /// </summary>
        bool Minified { get; set; }

        /// <summary>
        /// The path to the asset
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Asset type
        /// </summary>
        AssetType Type { get; set; }

        /// <summary>
        /// URL to the asset
        /// </summary>
        string URL { get; set; }
    }
}