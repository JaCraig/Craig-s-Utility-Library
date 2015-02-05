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
using System.IO;
using UnitTests.Fixtures;
using Utilities.IO.ExtensionMethods;
using Xunit;

namespace UnitTests.IO.ExtensionMethods
{
    public class File : TestingDirectoryFixture
    {
        public File()
        {
            new DirectoryInfo(@"..\..\Data\Testing").CopyTo(@".\Testing");
        }

        [Fact]
        public void Append()
        {
            new FileInfo(@".\Testing\Test.txt").Save("\r\nYay, this is appended text", Mode: FileMode.Append);
            Assert.True(new FileInfo(@".\Testing\Test.txt").Exists);
            Assert.Equal(48, new FileInfo(@".\Testing\Test.txt").Length);
        }

        [Fact]
        public void CompareTo()
        {
            Assert.True(new FileInfo(@".\Testing\Test.txt").CompareTo(new FileInfo(@"..\..\Data\Testing\Test.txt")));
        }

        [Fact]
        public void DriveInfo()
        {
            Assert.NotNull(new FileInfo(@".\Testing\Test.txt").DriveInfo());
        }

        [Fact]
        public void Execute()
        {
            new FileInfo(@"..\..\Data\Test.bat").Execute(WindowStyle: System.Diagnostics.ProcessWindowStyle.Hidden);
        }

        [Fact]
        public void Read()
        {
            Assert.Equal("This is a test file.", new FileInfo(@".\Testing\Test.txt").Read());
        }

        [Fact]
        public void Read2()
        {
            Assert.Equal("This is a test file.", @"~\Testing\Test.txt".Read());
        }

        [Fact]
        public void ReadBinary()
        {
            byte[] Content = new FileInfo(@".\Testing\Test.txt").ReadBinary();
            Assert.Equal("This is a test file.", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
        }

        [Fact]
        public void Save()
        {
            new FileInfo(@".\Testing\Test2.txt").Save("This is yet another test");
            Assert.Equal("This is yet another test", new FileInfo(@".\Testing\Test2.txt").Read());
        }

        [Fact]
        public void Save2()
        {
            @"~\Testing\Test2.txt".Save("This is yet another test");
            Assert.Equal("This is yet another test", new FileInfo(@".\Testing\Test2.txt").Read());
        }

        [Fact]
        public void SaveAsync()
        {
            new FileInfo(@".\Testing\Test2.txt").SaveAsync("This is yet another test", SaveAsyncCallback, null);
        }

        public void SaveAsyncCallback(IAsyncResult Result)
        {
            Assert.True(Result.IsCompleted);
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
        }
    }
}