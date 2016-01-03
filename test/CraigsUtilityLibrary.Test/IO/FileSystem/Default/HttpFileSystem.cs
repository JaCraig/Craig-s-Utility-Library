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

using Utilities.IO.FileSystem.Interfaces;
using Xunit;

namespace UnitTests.IO.FileSystem.Default
{
    public class HttpFileSystem
    {
        [Fact]
        public void CanHandle()
        {
            var Temp = new Utilities.IO.FileSystem.Default.HttpFileSystem();
            Assert.True(Temp.CanHandle(@"http://www.google.com"));
            Assert.True(Temp.CanHandle(@"https://www.google.com"));
            Assert.True(Temp.CanHandle(@"www.google.com"));
        }

        [Fact]
        public void Creation()
        {
            var Temp = new Utilities.IO.FileSystem.Default.HttpFileSystem();
            Assert.NotNull(Temp);
            Assert.Equal("HTTP", Temp.Name);
        }

        [Fact]
        public void Directory()
        {
            var Temp = new Utilities.IO.FileSystem.Default.HttpFileSystem();
            IDirectory Dir = Temp.Directory(@"http://www.google.com");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.WebDirectory>(Dir);
            Assert.True(Dir.Exists);
        }

        [Fact]
        public void File()
        {
            var Temp = new Utilities.IO.FileSystem.Default.HttpFileSystem();
            IFile Dir = Temp.File(@"http://www.google.com");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.WebFile>(Dir);
            Assert.True(Dir.Exists);
        }
    }
}