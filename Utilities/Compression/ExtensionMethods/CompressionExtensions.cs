/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System;
using Utilities.Compression.ExtensionMethods.Enums;
using Utilities.DataTypes.ExtensionMethods;
using System.Text;
#endregion

namespace Utilities.Compression.ExtensionMethods
{
    /// <summary>
    /// Extension methods dealing with compression
    /// </summary>
    public static class CompressionExtensions
    {
        #region Functions

        #region Compress

        /// <summary>
        /// Compresses the data using the specified compression type
        /// </summary>
        /// <param name="Data">Data to compress</param>
        /// <param name="CompressionType">Compression type</param>
        /// <returns>The compressed data</returns>
        public static byte[] Compress(this byte[] Data, CompressionType CompressionType = CompressionType.Deflate)
        {
            Data.ThrowIfNull("Data");
            using (MemoryStream Stream = new MemoryStream())
            {
                using (Stream ZipStream = GetStream(Stream, CompressionMode.Compress, CompressionType))
                {
                    ZipStream.Write(Data, 0, Data.Length);
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Compresses a string of data
        /// </summary>
        /// <param name="Data">Data to Compress</param>
        /// <param name="EncodingUsing">Encoding that the data uses (defaults to UTF8)</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data Compressed</returns>
        public static string Compress(this string Data, Encoding EncodingUsing = null, CompressionType CompressionType = CompressionType.Deflate)
        {
            Data.ThrowIfNullOrEmpty("Data");
            return Data.ToByteArray(EncodingUsing).Compress(CompressionType).ToBase64String();
        }

        #endregion

        #region Decompress

        /// <summary>
        /// Decompresses the byte array that is sent in
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data decompressed</returns>
        public static byte[] Decompress(this byte[] Data, CompressionType CompressionType = CompressionType.Deflate)
        {
            Data.ThrowIfNull("Data");
            using (MemoryStream Stream = new MemoryStream())
            {
                using (Stream ZipStream = GetStream(new MemoryStream(Data), CompressionMode.Decompress, CompressionType))
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

        /// <summary>
        /// Decompresses a string of data
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="EncodingUsing">Encoding that the result should use (defaults to UTF8)</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data decompressed</returns>
        public static string Decompress(this string Data, Encoding EncodingUsing = null, CompressionType CompressionType = CompressionType.Deflate)
        {
            Data.ThrowIfNullOrEmpty("Data");
            return Data.FromBase64().Decompress(CompressionType).ToEncodedString(EncodingUsing);
        }

        #endregion

        #region GetStream

        private static Stream GetStream(MemoryStream Stream, CompressionMode Mode, CompressionType CompressionType)
        {
            if (CompressionType == CompressionType.Deflate)
                return new DeflateStream(Stream, Mode, true);
            else
                return new GZipStream(Stream, Mode, true);
        }

        #endregion

        #endregion
    }
}