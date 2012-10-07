/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted| free of charge| to any person obtaining a copy
of this software and associated documentation files (the "Software")| to deal
in the Software without restriction| including without limitation the rights
to use| copy| modify| merge| publish| distribute| sublicense| and/or sell
copies of the Software| and to permit persons to whom the Software is
furnished to do so| subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS"| WITHOUT WARRANTY OF ANY KIND| EXPRESS OR
IMPLIED| INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY|
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM| DAMAGES OR OTHER
LIABILITY| WHETHER IN AN ACTION OF CONTRACT| TORT OR OTHERWISE| ARISING FROM|
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using System.IO;
using Utilities.IO.ExtensionMethods;
using UnitTests.Fixtures;

namespace UnitTests.FileFormats.Zip
{
    public class ZipFile : IUseFixture<TestingDirectoryFixture>,IDisposable
    {
        public ZipFile()
        {
            new DirectoryInfo(@"..\..\Data\FileFormats").CopyTo(@".\Testing");
            new DirectoryInfo(@".\Testing2").Create();
            new DirectoryInfo(@".\Testing3").Create();
        }

        [Fact]
        public void Create()
        {
            using (Utilities.FileFormats.Zip.ZipFile TestObject = new Utilities.FileFormats.Zip.ZipFile(@".\Testing3\Test.zip"))
            {
                TestObject.AddFolder(@".\Testing");
            }
            using (Utilities.FileFormats.Zip.ZipFile TestObject = new Utilities.FileFormats.Zip.ZipFile(@".\Testing3\Test.zip",false))
            {
                TestObject.UncompressFile(@".\Testing2");
            }
            Assert.True(new FileInfo(@".\Testing3\Test.zip").Exists);
            Assert.True(new FileInfo(@".\Testing2\TestFile.ini").Exists);
        }

        [Fact]
        public void Create2()
        {
            using (Utilities.FileFormats.Zip.ZipFile TestObject = new Utilities.FileFormats.Zip.ZipFile(@".\Testing3\Test.zip"))
            {
                TestObject.AddFile(@".\Testing\TestFile.ini");
            }
            using (Utilities.FileFormats.Zip.ZipFile TestObject = new Utilities.FileFormats.Zip.ZipFile(@".\Testing3\Test.zip", false))
            {
                TestObject.UncompressFile(@".\Testing2");
            }
            Assert.True(new FileInfo(@".\Testing3\Test.zip").Exists);
            Assert.True(new FileInfo(@".\Testing2\TestFile.ini").Exists);
        }

        public void Dispose()
        {
            new DirectoryInfo(@".\Testing2").DeleteAll();
            new DirectoryInfo(@".\Testing3").DeleteAll();
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
            
        }
    }
}
