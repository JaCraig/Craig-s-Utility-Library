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

namespace UnitTests.FileFormats.INI
{
    public class INI : IUseFixture<TestingDirectoryFixture>
    {
        public INI()
        {
            new DirectoryInfo(@"..\..\Data\FileFormats").CopyTo(@".\Testing");
        }

        [Fact]
        public void Load()
        {
            Utilities.FileFormats.INI.INI TestObject = new Utilities.FileFormats.INI.INI(@".\Testing\TestFile.ini");
            Assert.Equal("Value1", TestObject.ReadFromINI("Section1", "Key1"));
            Assert.Equal("Value2", TestObject.ReadFromINI("Section1", "Key2"));
            Assert.Equal("Value3", TestObject.ReadFromINI("Section2", "Key1"));
            Assert.Equal("Value4", TestObject.ReadFromINI("Section2", "Key2"));
        }

        [Fact]
        public void Write()
        {
            Utilities.FileFormats.INI.INI TestObject = new Utilities.FileFormats.INI.INI(@".\Testing\TestFile.ini");
            TestObject.WriteToINI("Section1", "Key3", "SpecialValue");

            TestObject = new Utilities.FileFormats.INI.INI(@".\Testing\TestFile.ini");
            Assert.Equal("Value1", TestObject.ReadFromINI("Section1", "Key1"));
            Assert.Equal("Value2", TestObject.ReadFromINI("Section1", "Key2"));
            Assert.Equal("SpecialValue", TestObject.ReadFromINI("Section1", "Key3"));
            Assert.Equal("Value3", TestObject.ReadFromINI("Section2", "Key1"));
            Assert.Equal("Value4", TestObject.ReadFromINI("Section2", "Key2"));
        }

        [Fact]
        public void Delete()
        {
            Utilities.FileFormats.INI.INI TestObject = new Utilities.FileFormats.INI.INI(@".\Testing\TestFile.ini");
            Assert.True(TestObject.DeleteFromINI("Section1"));
            Assert.Equal("", TestObject.ReadFromINI("Section1", "Key1"));
            Assert.Equal("", TestObject.ReadFromINI("Section1", "Key2"));
            Assert.True(TestObject.DeleteFromINI("Section2","Key1"));
            Assert.Equal("", TestObject.ReadFromINI("Section2", "Key1"));
            Assert.Equal("Value4", TestObject.ReadFromINI("Section2", "Key2"));
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
            
        }
    }
}
