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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.IO.Messaging.BaseClasses;
using Utilities.IO.Messaging.Interfaces;
using Utilities.DataTypes;
using Utilities.IO.Compression.Interfaces;
using System.IO;
using System.Diagnostics.Contracts;
using System.IO.Compression;
using Utilities.IO.Compression.BaseClasses;
#endregion

namespace Utilities.IO.Compression.Default
{
    /// <summary>
    /// Deflate compressor
    /// </summary>
    public class DeflateCompressor : CompressorBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DeflateCompressor()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        public override string Name
        {
            get { return "Deflate"; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the stream
        /// </summary>
        /// <param name="Stream">Memory stream</param>
        /// <param name="compressionMode">Compression mode</param>
        /// <returns>The compressor stream</returns>
        protected override Stream GetStream(MemoryStream Stream, CompressionMode compressionMode)
        {
            return new DeflateStream(Stream, compressionMode);
        }

        #endregion
    }
}