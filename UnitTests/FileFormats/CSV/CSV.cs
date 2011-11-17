/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using MoonUnit.Attributes;
using MoonUnit;

namespace UnitTests.FileFormats.CSV
{
    public class CSV
    {
        [Test]
        public void Load()
        {
            Utilities.FileFormats.CSV.CSV TestObject = new Utilities.FileFormats.CSV.CSV("Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38");
            Assert.Equal(3, TestObject.NumberOfRows);
            Assert.Equal("\"Year\",\"Make\",\"Model\",\"Length\"" + System.Environment.NewLine + "\"1997\",\"Ford\",\"E350\",\"2.34\"" + System.Environment.NewLine + "\"2000\",\"Mercury\",\"Cougar\",\"2.38\"" + System.Environment.NewLine, TestObject.ToString());
        }

        [Test]
        public void Load2()
        {
            Utilities.FileFormats.CSV.CSV TestObject = new Utilities.FileFormats.CSV.CSV("\"Year,Make,Model,Length\"\r\n\"1997,Ford,E350,2.34\"\r\n\"2000,Mercury,Cougar,2.38\"");
            Assert.Equal(3, TestObject.NumberOfRows);
            Assert.Equal("\"Year,Make,Model,Length\"\r\n\"1997,Ford,E350,2.34\"\r\n\"2000,Mercury,Cougar,2.38\"\r\n", TestObject.ToString());
        }

        [Test]
        public void Load3()
        {
            Utilities.FileFormats.CSV.CSV TestObject = new Utilities.FileFormats.CSV.CSV("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"");
            Assert.Equal(1, TestObject.NumberOfRows);
            Assert.Equal("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"\r\n", TestObject.ToString());
        }
    }
}
