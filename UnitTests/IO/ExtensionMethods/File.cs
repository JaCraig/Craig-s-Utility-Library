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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MoonUnit;
using MoonUnit.Attributes;
using Utilities.IO.ExtensionMethods;

namespace UnitTests.IO.ExtensionMethods
{
    public class File : IDisposable
    {
        public File() { new DirectoryInfo(@"..\..\Data\Testing").CopyTo(@".\Testing"); }

        [Test]
        public void Append()
        {
            new FileInfo(@".\Testing\Test.txt").Append("\r\nYay, this is appended text");
            Assert.True(new FileInfo(@".\Testing\Test.txt").Exists);
            Assert.Equal(48, new FileInfo(@".\Testing\Test.txt").Length);
        }

        [Test]
        public void CompareTo()
        {
            Assert.True(new FileInfo(@".\Testing\Test.txt").CompareTo(new FileInfo(@"..\..\Data\Testing\Test.txt")));
        }

        [Test]
        public void Read()
        {
            Assert.Equal("This is a test file.", new FileInfo(@".\Testing\Test.txt").Read());
        }

        [Test]
        public void ReadBinary()
        {
            byte[] Content = new FileInfo(@".\Testing\Test.txt").ReadBinary();
            Assert.Equal("This is a test file.", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
        }

        [Test]
        public void DriveInfo()
        {
            Assert.NotNull(new FileInfo(@".\Testing\Test.txt").DriveInfo());
        }

        [Test]
        public void Save()
        {
            new FileInfo(@".\Testing\Test2.txt").Save("This is yet another test");
            Assert.Equal("This is yet another test", new FileInfo(@".\Testing\Test2.txt").Read());
        }

        [Test]
        public void SaveAsync()
        {
            new FileInfo(@".\Testing\Test2.txt").SaveAsync("This is yet another test", SaveAsyncCallback, null);
        }

        [Test]
        public void Execute()
        {
            Assert.DoesNotThrow<Exception>(() => new FileInfo(@"..\..\Data\Test.bat").Execute(WindowStyle: System.Diagnostics.ProcessWindowStyle.Hidden));
        }

        public void SaveAsyncCallback(IAsyncResult Result)
        {
            Assert.True(Result.IsCompleted);
        }


        public void Dispose()
        {
            new DirectoryInfo(@".\Testing").DeleteAll();
        }
    }
}
