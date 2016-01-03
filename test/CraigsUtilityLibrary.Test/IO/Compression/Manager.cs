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
using Utilities.DataTypes;
using Utilities.IO.Compression.Interfaces;
using Xunit;

namespace UnitTests.IO.Compression
{
    public class Manager
    {
        [Fact]
        public void Compress()
        {
            var Temp = new Utilities.IO.Compression.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<ICompressor>());
            string Data = "This is a bit of data that I want to compress";
            Assert.NotEqual("This is a bit of data that I want to compress", Temp.Compress(Data.ToByteArray(), "Deflate").ToString(null));
            Assert.Equal("This is a bit of data that I want to compress", Temp.Decompress(Temp.Compress(Data.ToByteArray(), "Deflate"), "Deflate").ToString(null));
        }

        [Fact]
        public void Creation()
        {
            var Temp = new Utilities.IO.Compression.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<ICompressor>());
            Assert.NotNull(Temp);
            Assert.Equal(2, Temp.Compressors.Count);
            Assert.Equal("Compressors: Deflate, GZip\r\n", Temp.ToString());
        }

        [Fact]
        public void Decompress()
        {
            var Temp = new Utilities.IO.Compression.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<ICompressor>());
            string Data = "This is a bit of data that I want to compress";
            Assert.NotEqual("This is a bit of data that I want to compress", Temp.Compress(Data.ToByteArray(), "GZip").ToString(null));
            Assert.Equal("This is a bit of data that I want to compress", Temp.Decompress(Temp.Compress(Data.ToByteArray(), "GZip"), "GZip").ToString(null));
        }
    }
}