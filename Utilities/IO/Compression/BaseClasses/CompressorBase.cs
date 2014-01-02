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
using System.IO;
using System.IO.Compression;
using Utilities.IO.Compression.Interfaces;
#endregion


namespace Utilities.IO.Compression.BaseClasses
{
    /// <summary>
    /// Compressor base class
    /// </summary>
    public abstract class CompressorBase : ICompressor
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected CompressorBase()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Compressor name
        /// </summary>
        public abstract string Name { get; }

        #endregion

        #region Functions

        /// <summary>
        /// Compresses the byte array
        /// </summary>
        /// <param name="Data">Data to compress</param>
        /// <returns>Compressed data</returns>
        public byte[] Compress(byte[] Data)
        {
            if (Data == null)
                throw new ArgumentNullException("Data");
            using (MemoryStream Stream = new MemoryStream())
            {
                using (Stream ZipStream = GetStream(Stream, CompressionMode.Compress))
                {
                    ZipStream.Write(Data, 0, Data.Length);
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the stream used to compress/decompress the data
        /// </summary>
        /// <param name="Stream">Memory stream used</param>
        /// <param name="compressionMode">Compression mode</param>
        /// <returns>The stream used to compress/decompress the data</returns>
        protected abstract Stream GetStream(MemoryStream Stream, CompressionMode compressionMode);

        /// <summary>
        /// Decompresses the data
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <returns>The decompressed data</returns>
        public byte[] Decompress(byte[] Data)
        {
            if (Data == null)
                throw new ArgumentNullException("Data");
            using (MemoryStream Stream = new MemoryStream())
            {
                using (MemoryStream DataStream = new MemoryStream(Data))
                {
                    using (Stream ZipStream = GetStream(DataStream, CompressionMode.Decompress))
                    {
                        byte[] Buffer = new byte[4096];
                        while (true)
                        {
                            int Size = ZipStream.Read(Buffer, 0, Buffer.Length);
                            if (Size > 0) Stream.Write(Buffer, 0, Size);
                            else break;
                        }
                        ZipStream.Close();
                        return Stream.ToArray();
                    }
                }
            }
        }

        #endregion
    }
}