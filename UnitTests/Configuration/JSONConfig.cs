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

using Utilities.IO;
using Xunit;

namespace UnitTests.Configuration
{
    public class JSONConfig
    {
        [Fact]
        public void Load()
        {
            new FileInfo("./Test.config").Delete();
            TestClass Temp = Utilities.Configuration.ConfigurationManager.Get<TestClass>("Test1");
            Assert.Equal("A", Temp.A);
            Assert.Equal("B", Temp.B);
            new FileInfo("./Test.config").Delete();
        }

        [Fact]
        public void Save()
        {
            new FileInfo("./Test.config").Delete();
            TestClass Temp = Utilities.Configuration.ConfigurationManager.Get<TestClass>("Test1");
            Assert.Equal("A", Temp.A);
            Assert.Equal("B", Temp.B);
            Temp.A = "C";
            Temp.B = "D";
            Temp.Save();
            Temp.Load();
            Assert.Equal("C", Temp.A);
            Assert.Equal("D", Temp.B);
            Temp.A = "A";
            Temp.B = "B";
            Temp.Save();
            new FileInfo("./Test.config").Delete();
        }

        public class TestClass : Utilities.Configuration.JSONConfig<TestClass>
        {
            public TestClass()
            {
                A = "A";
                B = "B";
            }

            public string A { get; set; }

            public string B { get; set; }

            public override string Name
            {
                get { return "Test1"; }
            }

            protected override string ConfigFileLocation
            {
                get
                {
                    return "./Test.config";
                }
            }
        }
    }
}