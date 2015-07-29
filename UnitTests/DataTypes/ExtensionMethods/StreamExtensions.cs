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

using UnitTests.Fixtures;
using Utilities.DataTypes;
using Utilities.IO;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class StreamExtensions : TestingDirectoryFixture
    {
        public StreamExtensions()
        {
            new DirectoryInfo(@"..\..\Data\Testing").CopyTo(new DirectoryInfo(@".\Testing"));
        }

        [Fact]
        public void ReadAll()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (System.IO.FileStream Test = File.OpenRead())
            {
                Assert.Equal("This is a test", Test.ReadAll());
            }
        }

        [Fact]
        public void ReadAllBinary()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (System.IO.FileStream Test = File.OpenRead())
            {
                byte[] Content = Test.ReadAllBinary();
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        [Fact]
        public void ReadAllBinary2()
        {
            using (System.IO.MemoryStream Test = new System.IO.MemoryStream())
            {
                Test.Write("This is a test".ToByteArray(), 0, "This is a test".Length);
                byte[] Content = Test.ReadAllBinary();
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
        }
    }
}