/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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

using Xunit;

namespace UnitTests.DataTypes
{
    public class Table
    {
        [Fact]
        public void CreationTest()
        {
            Utilities.DataTypes.Table Table = new Utilities.DataTypes.Table("Column1", "Column2", "Column3");
            Assert.Equal(3, Table.ColumnNames.Length);
            Assert.Equal("Column1", Table.ColumnNames[0]);
            Assert.Equal("Column2", Table.ColumnNames[1]);
            Assert.Equal("Column3", Table.ColumnNames[2]);
        }

        [Fact]
        public void RowCreationTest()
        {
            Utilities.DataTypes.Table Table = new Utilities.DataTypes.Table("Column1", "Column2", "Column3");
            Table.AddRow(1, "A", 9.2f)
                 .AddRow(2, "B", 8.2f)
                 .AddRow(3, "C", 7.2f);
            Assert.Equal(3, Table.Rows.Count);
            Assert.Equal(1, Table[0][0]);
            Assert.Equal("A", Table[0][1]);
            Assert.Equal(9.2f, Table[0][2]);
            Assert.Equal(2, Table[1][0]);
            Assert.Equal("B", Table[1][1]);
            Assert.Equal(8.2f, Table[1][2]);
            Assert.Equal(3, Table[2][0]);
            Assert.Equal("C", Table[2][1]);
            Assert.Equal(7.2f, Table[2][2]);
            Assert.Equal("Column1", Table[0].ColumnNames[0]);
            Assert.Equal("Column2", Table[0].ColumnNames[1]);
            Assert.Equal("Column3", Table[0].ColumnNames[2]);
            Assert.Equal(1, Table[0]["Column1"]);
            Assert.Equal("A", Table[0]["Column2"]);
            Assert.Equal(9.2f, Table[0]["Column3"]);
            Assert.Equal(2, Table[1]["Column1"]);
            Assert.Equal("B", Table[1]["Column2"]);
            Assert.Equal(8.2f, Table[1]["Column3"]);
            Assert.Equal(3, Table[2]["Column1"]);
            Assert.Equal("C", Table[2]["Column2"]);
            Assert.Equal(7.2f, Table[2]["Column3"]);
        }
    }
}
