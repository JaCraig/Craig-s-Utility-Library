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
using Utilities.ORM.Manager.Schema.Enums;
using Xunit;

namespace UnitTests.SQL.DataClasses
{
    public class Trigger
    {
        [Fact]
        public void Create()
        {
            var Database = new Utilities.ORM.Manager.Schema.Default.Database.Database("TestDatabase");
            Utilities.ORM.Manager.Schema.Interfaces.ITable Table = Database.AddTable("TestTable");
            Utilities.ORM.Manager.Schema.Interfaces.ITrigger TempTrigger = Table.AddTrigger("Trigger", "Definition", TriggerType.Insert);
            Assert.Equal(TempTrigger, Table.Triggers.First());
            Assert.Equal("Trigger", TempTrigger.Name);
            Assert.Equal("Definition", TempTrigger.Definition);
            Assert.Equal(TriggerType.Insert, TempTrigger.Type);
            Assert.Equal(Table, TempTrigger.ParentTable);
        }
    }
}