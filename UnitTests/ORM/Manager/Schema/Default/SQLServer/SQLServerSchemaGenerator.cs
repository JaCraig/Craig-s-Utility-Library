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
using System.Data;
using System.Linq;
using Utilities.ORM.Manager.Schema.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager.Schema.Default.SQLServer
{
    public class SQLServerSchemaGenerator : DatabaseBaseClass
    {
        [Fact]
        public void Create()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.Database.SQLServer.SQLServerSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            Assert.Equal("System.Data.SqlClient", Temp.ProviderName);
        }

        [Fact]
        public void GenerateSchema()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.Database.SQLServer.SQLServerSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            ISource Source = Temp.GetSourceStructure(TestDatabaseSource);
            Source.Tables.First().AddColumn<string>("A", DbType.Int16);
            ITable Table = Source.AddTable("TestTable2");
            Table.AddColumn<string>("A", DbType.Int16);
            IEnumerable<string> Commands = Temp.GenerateSchema(Source, TestDatabaseSource);
            Assert.Equal(2, Commands.Count());
            Assert.Equal("EXEC dbo.sp_executesql @statement = N'ALTER TABLE TestTable ADD A SmallInt'", Commands.First());
            Assert.Equal("EXEC dbo.sp_executesql @statement = N'CREATE TABLE TestTable2(A SmallInt)'", Commands.Last());
        }

        [Fact]
        public void GetSourceStructure()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.Database.SQLServer.SQLServerSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            ISource Source = Temp.GetSourceStructure(TestDatabaseSource);
            Assert.Equal(0, Source.Functions.Count);
            Assert.Equal("TestDatabase", Source.Name);
            Assert.Equal(0, Source.StoredProcedures.Count);
            Assert.Equal(1, Source.Tables.Count);
            Assert.Equal(0, Source.Views.Count);
            ITable TempTable = Source.Tables.First();
            Assert.Equal(9, TempTable.Columns.Count);
            Assert.Equal("TestTable", TempTable.Name);
            Assert.Equal(Source, TempTable.Source);
            Assert.Equal(0, TempTable.Triggers.Count);
        }

        [Fact]
        public void SourceExists()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.Database.SQLServer.SQLServerSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            Assert.True(Temp.SourceExists("TestDatabase", DatabaseSource));
        }

        [Fact]
        public void TableExists()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.Database.SQLServer.SQLServerSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            Assert.True(Temp.TableExists("TestTable", TestDatabaseSource));
        }
    }
}