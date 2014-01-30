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

using System;
using System.Data;
using System.Linq;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Xunit;

namespace UnitTests.ORM.Manager.QueryProvider.Default.SQLServer
{
    public class SQLServerQueryProvider : DatabaseBaseClass
    {
        public SQLServerQueryProvider()
            : base()
        {
        }

        [Fact]
        public void Batch()
        {
            Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerQueryProvider Temp = new Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerQueryProvider();
            IBatch Batch = Temp.Batch("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(0, Batch.CommandCount);
            Assert.Equal(0, Batch.Execute().First().Count());
            Assert.Equal(typeof(Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch), Batch.GetType());
        }

        [Fact]
        public void Create()
        {
            Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerQueryProvider Temp = new Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerQueryProvider();
            Assert.NotNull(Temp);
            Assert.Equal("System.Data.SqlClient", Temp.ProviderName);
        }

        [Fact]
        public void Generate()
        {
            Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerQueryProvider Temp = new Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerQueryProvider();
            IGenerator<TestClass> Generator = Temp.Generate<TestClass>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(typeof(Utilities.ORM.Manager.QueryProvider.Default.SQLServer.SQLServerGenerator<TestClass>), Generator.GetType());
        }

        private class TestClass
        {
            public int A { get; set; }
        }
    }
}