/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted\t free of charge\t to any person obtaining a copy
of this software and associated documentation files (the "Software")\t to deal
in the Software without restriction\t including without limitation the rights
to use\t copy\t modify\t merge\t publish\t distribute\t sublicense\t and/or sell
copies of the Software\t and to permit persons to whom the Software is
furnished to do so\t subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS"\t WITHOUT WARRANTY OF ANY KIND\t EXPRESS OR
IMPLIED\t INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY\t
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM\t DAMAGES OR OTHER
LIABILITY\t WHETHER IN AN ACTION OF CONTRACT\t TORT OR OTHERWISE\t ARISING FROM\t
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using System.Data;

namespace UnitTests.FileFormats.GenericDelimited
{
    public class GenericDelimited
    {
        [Fact]
        public void Load()
        {
            Utilities.FileFormats.GenericDelimited.GenericDelimited TestObject = new Utilities.FileFormats.GenericDelimited.GenericDelimited("Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38","\t");
            Assert.Equal(3, TestObject.NumberOfRows);
            Assert.Equal("\"Year\"\t\"Make\"\t\"Model\"\t\"Length\"" + System.Environment.NewLine + "\"1997\"\t\"Ford\"\t\"E350\"\t\"2.34\"" + System.Environment.NewLine + "\"2000\"\t\"Mercury\"\t\"Cougar\"\t\"2.38\"" + System.Environment.NewLine, TestObject.ToString());
        }

        [Fact]
        public void Load2()
        {
            Utilities.FileFormats.GenericDelimited.GenericDelimited TestObject = new Utilities.FileFormats.GenericDelimited.GenericDelimited("\"Year\tMake\tModel\tLength\"\r\n\"1997\tFord\tE350\t2.34\"\r\n\"2000\tMercury\tCougar\t2.38\"","\t");
            Assert.Equal(3, TestObject.NumberOfRows);
            Assert.Equal("\"Year\tMake\tModel\tLength\"\r\n\"1997\tFord\tE350\t2.34\"\r\n\"2000\tMercury\tCougar\t2.38\"\r\n", TestObject.ToString());
        }

        [Fact]
        public void Load3()
        {
            Utilities.FileFormats.GenericDelimited.GenericDelimited TestObject = new Utilities.FileFormats.GenericDelimited.GenericDelimited("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"","\t");
            Assert.Equal(1, TestObject.NumberOfRows);
            Assert.Equal("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"\r\n", TestObject.ToString());
        }

        [Fact]
        public void Load4()
        {
            Utilities.FileFormats.GenericDelimited.GenericDelimited TestObject = new Utilities.FileFormats.GenericDelimited.GenericDelimited("\t");
            TestObject.Parse("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"");
            Assert.Equal(1, TestObject.NumberOfRows);
            Assert.Equal("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"\r\n", TestObject.ToString());
        }

        [Fact]
        public void ToDataTable()
        {
            DataTable TestObject = new Utilities.FileFormats.GenericDelimited.GenericDelimited("Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38", "\t").ToDataTable();
            Assert.Equal(2, TestObject.Rows.Count);
            Assert.Equal("Year", TestObject.Columns[0].ColumnName);
            Assert.Equal("Make", TestObject.Columns[1].ColumnName);
            Assert.Equal("Model", TestObject.Columns[2].ColumnName);
            Assert.Equal("Length", TestObject.Columns[3].ColumnName);
            Assert.Equal("1997", TestObject.Rows[0].ItemArray[0]);
            Assert.Equal("Ford", TestObject.Rows[0].ItemArray[1]);
            Assert.Equal("E350", TestObject.Rows[0].ItemArray[2]);
            Assert.Equal("2.34", TestObject.Rows[0].ItemArray[3]);
            Assert.Equal("2000", TestObject.Rows[1].ItemArray[0]);
            Assert.Equal("Mercury", TestObject.Rows[1].ItemArray[1]);
            Assert.Equal("Cougar", TestObject.Rows[1].ItemArray[2]);
            Assert.Equal("2.38", TestObject.Rows[1].ItemArray[3]);
        }
    }
}
