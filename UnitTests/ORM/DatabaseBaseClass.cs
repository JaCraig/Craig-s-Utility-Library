﻿/*
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
using UnitTests.ORM.Test2.Models;
using Xunit;

namespace UnitTests.ORM
{
    [Collection("DatabaseCollection")]
    public abstract class DatabaseBaseClass : IDisposable
    {
        public DatabaseBaseClass()
            : base()
        {
            new Utilities.ORM.ORM(false, typeof(Task).Assembly);
        }

        public virtual void Dispose()
        {
            Utilities.ORM.ORM.Destroy();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", CommandType.Text, "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.Batch().AddCommand("ALTER DATABASE ORMTestDatabase3 SET OFFLINE WITH ROLLBACK IMMEDIATE", CommandType.Text)
                    .AddCommand("ALTER DATABASE ORMTestDatabase3 SET ONLINE", CommandType.Text)
                    .AddCommand("DROP DATABASE ORMTestDatabase3", CommandType.Text);
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", CommandType.Text, "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.Batch().AddCommand("ALTER DATABASE ORMTestDatabase2 SET OFFLINE WITH ROLLBACK IMMEDIATE", CommandType.Text)
                    .AddCommand("ALTER DATABASE ORMTestDatabase2 SET ONLINE", CommandType.Text)
                    .AddCommand("DROP DATABASE ORMTestDatabase2", CommandType.Text);
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", CommandType.Text, "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.Batch().AddCommand("ALTER DATABASE ORMTestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE", CommandType.Text)
                    .AddCommand("ALTER DATABASE ORMTestDatabase SET ONLINE", CommandType.Text)
                    .AddCommand("DROP DATABASE ORMTestDatabase", CommandType.Text);
                Helper.ExecuteNonQuery();
            }
        }
    }
}