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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.Compression;

namespace Utilities.IO
{
    /// <summary>
    /// Defines the various compression types that are available
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        /// Deflate
        /// </summary>
        Deflate = 0,

        /// <summary>
        /// GZip
        /// </summary>
        GZip = 1
    }

    /// <summary>
    /// Extension methods dealing with compression
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CompressionExtensions
    {
        /// <summary>
        /// Compresses the data using the specified compression type
        /// </summary>
        /// <param name="Data">Data to compress</param>
        /// <param name="CompressionType">Compression type</param>
        /// <returns>The compressed data</returns>
        public static byte[] Compress(this byte[] Data, CompressionType CompressionType = CompressionType.Deflate)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Compress(Data, CompressionType.ToString());
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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Data), "Data");
            return Data.ToByteArray(EncodingUsing).Compress(CompressionType).ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Compresses the data using the specified compression type
        /// </summary>
        /// <param name="Data">Data to compress</param>
        /// <param name="CompressionType">Compression type</param>
        /// <returns>The compressed data</returns>
        public static byte[] Compress(this byte[] Data, string CompressionType)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(CompressionType), "CompressionType");
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Compress(Data, CompressionType.ToString());
        }

        /// <summary>
        /// Compresses a string of data
        /// </summary>
        /// <param name="Data">Data to Compress</param>
        /// <param name="EncodingUsing">Encoding that the data uses (defaults to UTF8)</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data Compressed</returns>
        public static string Compress(this string Data, Encoding EncodingUsing, string CompressionType)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Data), "Data");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(CompressionType), "CompressionType");
            return Data.ToByteArray(EncodingUsing).Compress(CompressionType).ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Decompresses the byte array that is sent in
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data decompressed</returns>
        public static byte[] Decompress(this byte[] Data, CompressionType CompressionType = CompressionType.Deflate)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Decompress(Data, CompressionType.ToString());
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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Data), "Data");
            return Data.FromBase64().Decompress(CompressionType).ToString(EncodingUsing);
        }

        /// <summary>
        /// Decompresses the byte array that is sent in
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data decompressed</returns>
        public static byte[] Decompress(this byte[] Data, string CompressionType)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(CompressionType), "CompressionType");
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Decompress(Data, CompressionType.ToString());
        }

        /// <summary>
        /// Decompresses a string of data
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="EncodingUsing">Encoding that the result should use (defaults to UTF8)</param>
        /// <param name="CompressionType">The compression type used</param>
        /// <returns>The data decompressed</returns>
        public static string Decompress(this string Data, Encoding EncodingUsing, string CompressionType)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Data), "Data");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(CompressionType), "CompressionType");
            return Data.FromBase64().Decompress(CompressionType).ToString(EncodingUsing);
        }
    }
}