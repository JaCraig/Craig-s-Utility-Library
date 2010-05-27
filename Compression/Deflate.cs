/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using System.IO;
using System.IO.Compression;
#endregion

namespace Utilities.Compression
{
    /// <summary>
    /// Utility class used for compressing data
    /// using deflate.
    /// </summary>
    public static class Deflate
    {
        #region Static Functions

        /// <summary>
        /// Compresses data
        /// </summary>
        /// <param name="Bytes">The byte array to be compressed</param>
        /// <returns>A byte array of compressed data</returns>
        public static byte[] Compress(byte[] Bytes)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                using (DeflateStream ZipStream = new DeflateStream(Stream, CompressionMode.Compress, true))
                {
                    ZipStream.Write(Bytes, 0, Bytes.Length);
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresses data
        /// </summary>
        /// <param name="Bytes">The byte array to be decompressed</param>
        /// <returns>A byte array of uncompressed data</returns>
        public static byte[] Decompress(byte[] Bytes)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                using (DeflateStream ZipStream = new DeflateStream(new MemoryStream(Bytes), CompressionMode.Decompress, true))
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

        #endregion
    }
}