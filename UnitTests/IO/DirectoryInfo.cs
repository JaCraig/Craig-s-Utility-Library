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
using System.Linq;
using UnitTests.Fixtures;
using Utilities.IO;
using Utilities.IO.FileSystem.Interfaces;
using Xunit;

namespace UnitTests.IO
{
    public class DirectoryInfo : TestingDirectoryFixture
    {
        [Fact]
        public void Clone()
        {
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo(".");
            Utilities.IO.DirectoryInfo Temp2 = (Utilities.IO.DirectoryInfo)Temp.Clone();
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
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo("./Test");
            Temp.Create();
            Assert.True(Temp.Exists);
            Temp.Delete();
            Assert.False(Temp.Exists);
        }

        [Fact]
        public void Creation()
        {
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo(".");
            Assert.NotNull(Temp);
            Assert.True(Temp.Exists);
            Temp = new Utilities.IO.DirectoryInfo(new Utilities.IO.DirectoryInfo("."));
            Assert.NotNull(Temp);
            Assert.True(Temp.Exists);
        }

        [Fact]
        public void DeleteExtension()
        {
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo("./Test");
            Temp.Create();
            for (int x = 0; x < 10; ++x)
            {
                new Utilities.IO.DirectoryInfo("./Test/" + x).Create();
            }
            Temp.EnumerateDirectories().Delete();
            Temp.Delete();
        }

        [Fact]
        public void Enumeration()
        {
            new Utilities.IO.DirectoryInfo("~/Logs/").Delete();
            new Utilities.IO.DirectoryInfo("~/App_Data/").Delete();
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo(".");
            foreach (IFile File in Temp) { }
            Assert.Equal(1, Temp.EnumerateDirectories().Count());
            Assert.Equal(39, Temp.EnumerateFiles().Count());
            Assert.Equal(1, Temp.EnumerateDirectories(x => x.Created < DateTime.UtcNow).Count());
            Assert.Equal(39, Temp.EnumerateFiles(x => x.Created < DateTime.UtcNow).Count());
        }

        [Fact]
        public void Equality()
        {
            Utilities.IO.DirectoryInfo Temp = new Utilities.IO.DirectoryInfo(".");
            Utilities.IO.DirectoryInfo Temp2 = new Utilities.IO.DirectoryInfo(".");
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
        public void Move()
        {
            IDirectory Temp = new Utilities.IO.DirectoryInfo("./Test");
            IDirectory Temp2 = new Utilities.IO.DirectoryInfo("./Test2");
            Temp.Create();
            Temp2.Create();
            Temp2 = Temp2.MoveTo(Temp);
            Assert.True(Temp.Exists);
            Assert.True(Temp2.Exists);
            Assert.Equal(Temp, Temp2.Parent);
            Temp.Delete();
            Assert.False(Temp.Exists);
        }
    }
}