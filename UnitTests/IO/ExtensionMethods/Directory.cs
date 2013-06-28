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

using System.IO;
using UnitTests.Fixtures;
using Utilities.IO.ExtensionMethods;
using Xunit;

namespace UnitTests.IO.ExtensionMethods
{
    public class Directory : IUseFixture<TestingDirectoryFixture>
    {
        [Fact]
        public void DeleteAll()
        {
            new DirectoryInfo(@".\Testing").DeleteAll();
            Assert.False(new DirectoryInfo(@".\Testing").Exists);
        }

        [Fact]
        public void CopyTo()
        {
            new DirectoryInfo(@"..\..\Data\Testing").CopyTo(@".\Testing");
            Assert.True(new DirectoryInfo(@".\Testing").Exists);
            Assert.Equal(new DirectoryInfo(@".\Testing").Size(), new DirectoryInfo(@"..\..\Data\Testing").Size());
        }

        [Fact]
        public void Size()
        {
            Assert.Equal(20, new DirectoryInfo(@"..\..\Data\Testing").Size());
        }

        [Fact]
        public void DeleteFiles()
        {
            new DirectoryInfo(@"..\..\Data\Testing").CopyTo(@".\Testing");
            new DirectoryInfo(@".\Testing").DeleteFiles();
            Assert.Equal(0, new DirectoryInfo(@".\Testing").Size());
        }

        [Fact]
        public void DriveInfo()
        {
            Assert.NotNull(new DirectoryInfo(@".\Testing").DriveInfo());
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
            
        }
    }
}