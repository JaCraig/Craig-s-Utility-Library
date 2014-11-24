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

using System.Collections.Generic;
using System.Linq;
using Utilities.IO;
using Xunit;

namespace UnitTests.IO.FileFormats.ExtensionMethods
{
    public class ExtensionMethods
    {
        [Fact]
        public void ToDelimitedFile()
        {
            List<ExportClass> Temp = new ExportClass[] { new ExportClass { ID = 1, Value = "A" }, new ExportClass { ID = 2, Value = "B" }, new ExportClass { ID = 3, Value = "C" } }.ToList();
            Temp.ToDelimitedFile();
            Utilities.IO.FileFormats.Delimited.Delimited TestObject = Temp.ToDelimitedFile();
            Assert.Equal(4, TestObject.Count);
            Assert.Equal("\"ID\"\t\"Value\"\r\n\"1\"\t\"A\"\r\n\"2\"\t\"B\"\r\n\"3\"\t\"C\"\r\n", TestObject.ToString());
        }

        public class ExportClass
        {
            public virtual int ID { get; set; }

            public virtual string Value { get; set; }
        }
    }
}