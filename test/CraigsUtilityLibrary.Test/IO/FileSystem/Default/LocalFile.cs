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

using System.Text;
using Xunit;

namespace UnitTests.IO.FileSystem.Default
{
    public class LocalFile
    {
        [Fact]
        public void Clone()
        {
            var Temp = new Utilities.IO.FileSystem.Default.LocalFile("./Test.txt");
            var Temp2 = (Utilities.IO.FileSystem.Default.LocalFile)Temp.Clone();
            Assert.True(Temp == Temp2);
            Assert.True(Temp.Equals(Temp2));
            Assert.Equal(0, Temp.CompareTo(Temp2));
            Assert.False(Temp < Temp2);
            Assert.False(Temp > Temp2);
            Assert.True(Temp <= Temp2);
            Assert.True(Temp >= Temp2);
            Assert.False(Temp != Temp2);
        }

        [Fact]
        public void Creation()
        {
            var File = new Utilities.IO.FileSystem.Default.LocalFile("./Test.txt");
            Assert.NotNull(File);
            Assert.False(File.Exists);
        }

        [Fact]
        public void ReadWrite()
        {
            var File = new Utilities.IO.FileSystem.Default.LocalFile("./Test.txt");
            File.Write("Testing this out");
            Assert.True(File.Exists);
            Assert.Equal("Testing this out", File.Read());
            Assert.Equal("Testing this out", File);
            Assert.Equal(Encoding.ASCII.GetBytes("Testing this out"), File.ReadBinary());
            Assert.Equal(Encoding.ASCII.GetBytes("Testing this out"), File);
            File.Delete();
        }
    }
}