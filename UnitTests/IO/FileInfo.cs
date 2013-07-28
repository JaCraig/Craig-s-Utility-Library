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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Utilities.IO;
using System.Threading;

namespace UnitTests.IO
{
    public class FileInfo
    {
        [Fact]
        public void Creation()
        {
            Utilities.IO.FileInfo File = new Utilities.IO.FileInfo("./Test.txt");
            Assert.NotNull(File);
            Assert.False(File.Exists);
        }

        [Fact]
        public void ReadWrite()
        {
            Utilities.IO.FileInfo File = new Utilities.IO.FileInfo("./Test.txt");
            File.Write("Testing this out").Wait();
            Assert.True(File.Exists);
            Assert.Equal("Testing this out", File.Read());
            Assert.Equal("Testing this out", File);
            Assert.Equal(ASCIIEncoding.ASCII.GetBytes("Testing this out"), File.ReadBinary());
            Assert.Equal(ASCIIEncoding.ASCII.GetBytes("Testing this out"), File);
            File.Delete().Wait();
        }

        [Fact]
        public void DeleteExtension()
        {
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo("./Test");
            Temp.Create().Wait();
            for (int x = 0; x < 10; ++x)
            {
                new Utilities.IO.FileInfo("./Test/" + x + ".txt").Write("Testing this out").Wait();
            }
            Temp.EnumerateFiles().Delete().Wait();
            Temp.Delete().Wait();
        }

        [Fact]
        public void Clone()
        {
            Utilities.IO.FileInfo Temp = new Utilities.IO.FileInfo("./Test.txt");
            Utilities.IO.FileInfo Temp2 = (Utilities.IO.FileInfo)Temp.Clone();
            Assert.True(Temp == Temp2);
            Assert.True(Temp.Equals(Temp2));
            Assert.Equal(0, Temp.CompareTo(Temp2));
            Assert.False(Temp < Temp2);
            Assert.False(Temp > Temp2);
            Assert.True(Temp <= Temp2);
            Assert.True(Temp >= Temp2);
            Assert.False(Temp != Temp2);
        }
    }
}
