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

namespace UnitTests.IO.FileSystem
{
    public class Manager
    {
        [Fact]
        public void Creation()
        {
            Utilities.IO.FileSystem.Manager Temp = new Utilities.IO.FileSystem.Manager();
            Assert.NotNull(Temp);
        }

        [Fact]
        public void Directory()
        {
            Utilities.IO.FileSystem.Manager Temp = new Utilities.IO.FileSystem.Manager();
            IDirectory Dir = Temp.Directory(@"C:\");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalDirectory>(Dir);
            Assert.True(Dir.Exists);
            Dir = Temp.Directory(@"\\localhost\C$\");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalDirectory>(Dir);
            Assert.True(Dir.Exists);
            Dir = Temp.Directory(@"./");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalDirectory>(Dir);
            Assert.True(Dir.Exists);
            Dir = Temp.Directory(@"../");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalDirectory>(Dir);
            Assert.True(Dir.Exists);
            Dir = Temp.Directory(@"~/");
            Assert.NotNull(Dir);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalDirectory>(Dir);
            Assert.True(Dir.Exists);
        }

        [Fact]
        public void File()
        {
            Utilities.IO.FileSystem.Manager Temp = new Utilities.IO.FileSystem.Manager();
            IFile File = Temp.File(@"C:\Test.txt");
            Assert.NotNull(File);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalFile>(File);
            Assert.False(File.Exists);
            File = Temp.File(@"~\Test.txt");
            Assert.NotNull(File);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalFile>(File);
            Assert.False(File.Exists);
            File = Temp.File(@"\\localhost\C$\Test.txt");
            Assert.NotNull(File);
            Assert.IsType<Utilities.IO.FileSystem.Default.LocalFile>(File);
            Assert.False(File.Exists);
        }
    }
}
