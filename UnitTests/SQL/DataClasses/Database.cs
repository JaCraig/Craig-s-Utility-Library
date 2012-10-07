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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

namespace UnitTests.SQL.DataClasses
{
    public class Database
    {
        [Fact]
        public void Create()
        {
            Utilities.SQL.DataClasses.Database Database = new Utilities.SQL.DataClasses.Database("TestDatabase");
            Assert.Equal("TestDatabase", Database.Name);
            Assert.Equal(0, Database.Functions.Count);
            Assert.Equal(0, Database.StoredProcedures.Count);
            Assert.Equal(0, Database.Tables.Count);
            Assert.Equal(0, Database.Views.Count);
        }

        [Fact]
        public void AddFunction()
        {
            Utilities.SQL.DataClasses.Database Database = new Utilities.SQL.DataClasses.Database("TestDatabase");
            Database.AddFunction("TestFunction", "FunctionDefinition");
            Assert.Equal(1, Database.Functions.Count);
            Assert.Equal("TestFunction", Database.Functions[0].Name);
            Assert.Equal("FunctionDefinition", Database.Functions[0].Definition);
            Assert.Equal(Database, Database.Functions[0].ParentDatabase);
        }

        [Fact]
        public void AddStoredProcedures()
        {
            Utilities.SQL.DataClasses.Database Database = new Utilities.SQL.DataClasses.Database("TestDatabase");
            Database.AddStoredProcedure("TestFunction", "FunctionDefinition");
            Assert.Equal(1, Database.StoredProcedures.Count);
            Assert.Equal("TestFunction", Database.StoredProcedures[0].Name);
            Assert.Equal("FunctionDefinition", Database.StoredProcedures[0].Definition);
            Assert.Equal(Database, Database.StoredProcedures[0].ParentDatabase);
        }

        [Fact]
        public void AddTable()
        {
            Utilities.SQL.DataClasses.Database Database = new Utilities.SQL.DataClasses.Database("TestDatabase");
            Database.AddTable("TestTable");
            Assert.Equal(1, Database.Tables.Count);
            Assert.Equal("TestTable", Database.Tables[0].Name);
            Assert.Equal(Database, Database.Tables[0].ParentDatabase);
        }

        [Fact]
        public void AddView()
        {
            Utilities.SQL.DataClasses.Database Database = new Utilities.SQL.DataClasses.Database("TestDatabase");
            Database.AddView("TestTable");
            Assert.Equal(1, Database.Views.Count);
            Assert.Equal("TestTable", Database.Views[0].Name);
            Assert.Equal(Database, Database.Views[0].ParentDatabase);
        }
    }
}