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

using System.Data;
using Xunit;

namespace UnitTests.IO.FileFormats
{
    public class Delimited
    {
        [Fact]
        public void Load()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38");
            Assert.Equal(3, TestObject.Count);
            Assert.Equal("\"Year\",\"Make\",\"Model\",\"Length\"" + System.Environment.NewLine + "\"1997\",\"Ford\",\"E350\",\"2.34\"" + System.Environment.NewLine + "\"2000\",\"Mercury\",\"Cougar\",\"2.38\"" + System.Environment.NewLine, TestObject.ToString());
        }

        [Fact]
        public void Load2()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year,Make,Model,Length\"\r\n\"1997,Ford,E350,2.34\"\r\n\"2000,Mercury,Cougar,2.38\"");
            Assert.Equal(3, TestObject.Count);
            Assert.Equal("\"Year,Make,Model,Length\"\r\n\"1997,Ford,E350,2.34\"\r\n\"2000,Mercury,Cougar,2.38\"\r\n", TestObject.ToString());
        }

        [Fact]
        public void Load3()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"\r\n", TestObject.ToString());
        }

        [Fact]
        public void Load4()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited();
            TestObject.Parse("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"\r\n", TestObject.ToString());
        }

        [Fact]
        public void Load5()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38\"\r\n", TestObject);
        }

        [Fact]
        public void PipeLoad()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("Year|Make|Model|Length\r\n1997|Ford|E350|2.34\r\n2000|Mercury|Cougar|2.38");
            Assert.Equal(3, TestObject.Count);
            Assert.Equal("\"Year\"|\"Make\"|\"Model\"|\"Length\"" + System.Environment.NewLine + "\"1997\"|\"Ford\"|\"E350\"|\"2.34\"" + System.Environment.NewLine + "\"2000\"|\"Mercury\"|\"Cougar\"|\"2.38\"" + System.Environment.NewLine, TestObject);
        }

        [Fact]
        public void PipeLoad2()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year|Make|Model|Length\"\r\n\"1997|Ford|E350|2.34\"\r\n\"2000|Mercury|Cougar|2.38\"");
            Assert.Equal(3, TestObject.Count);
            Assert.Equal("\"Year|Make|Model|Length\"\r\n\"1997|Ford|E350|2.34\"\r\n\"2000|Mercury|Cougar|2.38\"\r\n", TestObject);
        }

        [Fact]
        public void PipeLoad3()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year|Make|Model|Length\r\n1997|Ford|E350|2.34\r\n2000|Mercury|Cougar|2.38\"");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year|Make|Model|Length\r\n1997|Ford|E350|2.34\r\n2000|Mercury|Cougar|2.38\"\r\n", TestObject);
        }

        [Fact]
        public void PipeLoad4()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited();
            TestObject.Parse("\"Year|Make|Model|Length\r\n1997|Ford|E350|2.34\r\n2000|Mercury|Cougar|2.38\"");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year|Make|Model|Length\r\n1997|Ford|E350|2.34\r\n2000|Mercury|Cougar|2.38\"\r\n", TestObject);
        }

        [Fact]
        public void PipeToDataTable()
        {
            DataTable TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("Year|Make|Model|Length\r\n1997|Ford|E350|2.34\r\n2000|Mercury|Cougar|2.38").ToDataTable();
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

        [Fact]
        public void TabLoad()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38");
            Assert.Equal(3, TestObject.Count);
            Assert.Equal("\"Year\"\t\"Make\"\t\"Model\"\t\"Length\"" + System.Environment.NewLine + "\"1997\"\t\"Ford\"\t\"E350\"\t\"2.34\"" + System.Environment.NewLine + "\"2000\"\t\"Mercury\"\t\"Cougar\"\t\"2.38\"" + System.Environment.NewLine, TestObject);
        }

        [Fact]
        public void TabLoad2()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year\tMake\tModel\tLength\"\r\n\"1997\tFord\tE350\t2.34\"\r\n\"2000\tMercury\tCougar\t2.38\"", "\t");
            Assert.Equal(3, TestObject.Count);
            Assert.Equal("\"Year\tMake\tModel\tLength\"\r\n\"1997\tFord\tE350\t2.34\"\r\n\"2000\tMercury\tCougar\t2.38\"\r\n", TestObject);
        }

        [Fact]
        public void TabLoad3()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"", "\t");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"\r\n", TestObject);
        }

        [Fact]
        public void TabLoad4()
        {
            var TestObject = new Utilities.IO.FileFormats.Delimited.Delimited();
            TestObject.Parse("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"");
            Assert.Equal(1, TestObject.Count);
            Assert.Equal("\"Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38\"\r\n", TestObject);
        }

        [Fact]
        public void TabToDataTable()
        {
            DataTable TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("Year\tMake\tModel\tLength\r\n1997\tFord\tE350\t2.34\r\n2000\tMercury\tCougar\t2.38", "\t").ToDataTable();
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

        [Fact]
        public void ToDataTable()
        {
            DataTable TestObject = new Utilities.IO.FileFormats.Delimited.Delimited("Year,Make,Model,Length\r\n1997,Ford,E350,2.34\r\n2000,Mercury,Cougar,2.38").ToDataTable();
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