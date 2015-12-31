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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.Compression.Interfaces;

namespace Utilities.IO.Compression
{
    /// <summary>
    /// Compression manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Compressors">The compressors.</param>
        public Manager(IEnumerable<ICompressor> Compressors)
        {
            Contract.Requires<ArgumentNullException>(Compressors != null, "Compressors");
            this.Compressors = Compressors.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Compressors
        /// </summary>
        public IDictionary<string, ICompressor> Compressors { get; private set; }

        /// <summary>
        /// Compresses the data
        /// </summary>
        /// <param name="Data">Data to compress</param>
        /// <param name="Compressor">Compressor name</param>
        /// <returns>The compressed data</returns>
        public byte[] Compress(byte[] Data, string Compressor)
        {
            Contract.Requires<NullReferenceException>(Compressors != null, "Compressors");
            return Compressors.ContainsKey(Compressor) ? Compressors[Compressor].Compress(Data) : Data;
        }

        /// <summary>
        /// Decompresses the data
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="Compressor">Compressor name</param>
        /// <returns>The decompressed data</returns>
        public byte[] Decompress(byte[] Data, string Compressor)
        {
            Contract.Requires<NullReferenceException>(Compressors != null, "Compressors");
            return Compressors.ContainsKey(Compressor) ? Compressors[Compressor].Decompress(Data) : Data;
        }

        /// <summary>
        /// String info for the manager
        /// </summary>
        /// <returns>The string info that the manager contains</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("Compressors: {0}", Compressors.OrderBy(x => x.Key).ToString(x => x.Key, ", "));
            return Builder.ToString();
        }
    }
}