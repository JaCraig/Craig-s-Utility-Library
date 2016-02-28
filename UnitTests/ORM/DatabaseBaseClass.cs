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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM
{
    [Collection("DatabaseCollection")]
    public abstract class DatabaseBaseClass : IDisposable
    {
        protected DatabaseBaseClass()
        {
            DatabaseSource = new Utilities.ORM.Manager.SourceProvider.Manager(new List<IDatabase>()).GetSource("Data Source=localhost;Integrated Security=SSPI;Pooling=false");
            LDAPSource = new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("LDAP");
            MasterDatabaseSource = new Utilities.ORM.Manager.SourceProvider.Manager(new List<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false");
            TestDatabaseSource = new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(DatabaseSource);
            try
            {
                Temp.AddCommand(null, null, CommandType.Text, "Create Database TestDatabase")
                    .Execute();

                Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(TestDatabaseSource);
                Temp.AddCommand(null, null, CommandType.Text, "Create Table TestTable(ID INT PRIMARY KEY IDENTITY,StringValue1 NVARCHAR(100),StringValue2 NVARCHAR(MAX),BigIntValue BIGINT,BitValue BIT,DecimalValue DECIMAL(12,6),FloatValue FLOAT,DateTimeValue DATETIME,GUIDValue UNIQUEIDENTIFIER)")
                    .Execute();
            }
            catch { }
        }

        protected ISourceInfo DatabaseSource { get; set; }

        protected ISourceInfo LDAPSource { get; set; }

        protected ISourceInfo MasterDatabaseSource { get; set; }

        protected ISourceInfo TestDatabaseSource { get; set; }

        public virtual void Dispose()
        {
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(MasterDatabaseSource);
            try
            {
                Temp.AddCommand(null, null, CommandType.Text, "ALTER DATABASE TestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE")
                        .AddCommand(null, null, CommandType.Text, "ALTER DATABASE TestDatabase SET ONLINE")
                        .AddCommand(null, null, CommandType.Text, "DROP DATABASE TestDatabase")
                        .Execute();
            }
            catch { }
        }
    }
}