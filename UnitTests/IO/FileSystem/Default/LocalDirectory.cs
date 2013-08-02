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

using Utilities.IO.FileSystem.Interfaces;
using Xunit;

namespace UnitTests.IO.FileSystem.Default
{
    public class LocalDirectory
    {
        [Fact]
        public void Creation()
        {
            Utilities.IO.FileSystem.Default.LocalDirectory Temp = new Utilities.IO.FileSystem.Default.LocalDirectory(".");
            Assert.NotNull(Temp);
            Assert.True(Temp.Exists);
            Temp = new Utilities.IO.FileSystem.Default.LocalDirectory(new System.IO.DirectoryInfo("."));
            Assert.NotNull(Temp);
            Assert.True(Temp.Exists);
        }

        [Fact]
        public void Enumeration()
        {
            Utilities.IO.FileSystem.Default.LocalDirectory Temp = new Utilities.IO.FileSystem.Default.LocalDirectory(".");
            Assert.DoesNotThrow(() => { foreach (IFile File in Temp) { } });
        }

        [Fact]
        public void Equality()
        {
            Utilities.IO.FileSystem.Default.LocalDirectory Temp = new Utilities.IO.FileSystem.Default.LocalDirectory(".");
            Utilities.IO.FileSystem.Default.LocalDirectory Temp2 = new Utilities.IO.FileSystem.Default.LocalDirectory(".");
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
        public void Clone()
        {
            Utilities.IO.FileSystem.Default.LocalDirectory Temp = new Utilities.IO.FileSystem.Default.LocalDirectory(".");
            Utilities.IO.FileSystem.Default.LocalDirectory Temp2 = (Utilities.IO.FileSystem.Default.LocalDirectory)Temp.Clone();
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
        public void CreateAndDelete()
        {
            Utilities.IO.FileSystem.Default.LocalDirectory Temp = new Utilities.IO.FileSystem.Default.LocalDirectory("./Test");
            Temp.Create();
            Assert.True(Temp.Exists);
            Temp.Delete();
            Assert.False(Temp.Exists);
        }


        [Fact]
        public void Move()
        {
            Utilities.IO.FileSystem.Default.LocalDirectory Temp = new Utilities.IO.FileSystem.Default.LocalDirectory("./Test");
            Utilities.IO.FileSystem.Default.LocalDirectory Temp2 = new Utilities.IO.FileSystem.Default.LocalDirectory("./Test2");
            Temp.Create();
            Temp2.Create();
            Temp2.MoveTo(Temp);
            Assert.True(Temp.Exists);
            Assert.True(Temp2.Exists);
            Assert.Equal(Temp, Temp2.Parent);
            Temp.Delete();
            Assert.False(Temp.Exists);
        }
    }
}
