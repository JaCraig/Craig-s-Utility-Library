/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using Xunit;

namespace UnitTests.IO.FileFormats
{
    public class Excel
    {
        [Fact]
        public void Load()
        {
            Utilities.IO.FileFormats.Excel Docs = Utilities.IO.FileFormats.Excel.Load("../../Data/Testing/Test.xlsx", "Sheet1");
            Assert.Equal(4, Docs.Count);
            Assert.Equal(3, Docs.ColumnNames.Count);
            Assert.Equal("Header 1", Docs.ColumnNames[0]);
            Assert.Equal("Header 2", Docs.ColumnNames[1]);
            Assert.Equal("Header 3", Docs.ColumnNames[2]);
            Assert.Equal("This", Docs[0, "Header 1"]);
            Assert.Equal("is", Docs[0, "Header 2"]);
            Assert.Equal("a", Docs[0, "Header 3"]);
            Assert.Equal("simple", Docs[1, "Header 1"]);
            Assert.Equal("test", Docs[1, "Header 2"]);
            Assert.Equal("of", Docs[1, "Header 3"]);
            Assert.Equal("the", Docs[2, "Header 2"]);
            Assert.Equal("system", Docs[3, "Header 2"]);
            Assert.Equal("Header 1\tHeader 2\tHeader 3\r\nThis\tis\ta\r\nsimple\ttest\tof\r\n\tthe\t\r\n\tsystem\t", Docs.ToString());
        }
    }
}