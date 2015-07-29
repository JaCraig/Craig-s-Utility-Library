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

using System.Linq;

using Xunit;

namespace UnitTests.SQL.DataClasses
{
    public class Database
    {
        [Fact]
        public void AddFunction()
        {
            var Database = new Utilities.ORM.Manager.Schema.Default.Database.Database("TestDatabase");
            Database.AddFunction("TestFunction", "FunctionDefinition");
            Assert.Equal(1, Database.Functions.Count);
            Assert.Equal("TestFunction", Database.Functions.First().Name);
            Assert.Equal("FunctionDefinition", Database.Functions.First().Definition);
            Assert.Equal(Database, Database.Functions.First().Source);
        }

        [Fact]
        public void AddStoredProcedures()
        {
            var Database = new Utilities.ORM.Manager.Schema.Default.Database.Database("TestDatabase");
            Database.AddStoredProcedure("TestFunction", "FunctionDefinition");
            Assert.Equal(1, Database.StoredProcedures.Count);
            Assert.Equal("TestFunction", Database.StoredProcedures.First().Name);
            Assert.Equal("FunctionDefinition", ((Utilities.ORM.Manager.Schema.Interfaces.IFunction)Database.StoredProcedures.First()).Definition);
            Assert.Equal(Database, Database.StoredProcedures.First().Source);
        }

        [Fact]
        public void AddTable()
        {
            var Database = new Utilities.ORM.Manager.Schema.Default.Database.Database("TestDatabase");
            Database.AddTable("TestTable");
            Assert.Equal(1, Database.Tables.Count);
            Assert.Equal("TestTable", Database.Tables.First().Name);
            Assert.Equal(Database, Database.Tables.First().Source);
        }

        [Fact]
        public void AddView()
        {
            var Database = new Utilities.ORM.Manager.Schema.Default.Database.Database("TestDatabase");
            Database.AddView("TestTable");
            Assert.Equal(1, Database.Views.Count);
            Assert.Equal("TestTable", Database.Views.First().Name);
            Assert.Equal(Database, Database.Views.First().Source);
        }

        [Fact]
        public void Create()
        {
            var Database = new Utilities.ORM.Manager.Schema.Default.Database.Database("TestDatabase");
            Assert.Equal("TestDatabase", Database.Name);
            Assert.Equal(0, Database.Functions.Count);
            Assert.Equal(0, Database.StoredProcedures.Count);
            Assert.Equal(0, Database.Tables.Count);
            Assert.Equal(0, Database.Views.Count);
        }
    }
}