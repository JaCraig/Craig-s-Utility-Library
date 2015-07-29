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
using Xunit;

namespace UnitTests.ORM.Manager.QueryProvider.Default
{
    public class Command
    {
        [Fact]
        public void Create()
        {
            var Bootstrapper = Utilities.IoC.Manager.Bootstrapper;
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.Command(null, null, "SELECT * FROM A", CommandType.Text, "@", new object[] { 1, "ASDF", 2.0f, Guid.NewGuid() });
            Assert.Equal(CommandType.Text, Temp.CommandType);
            Assert.Equal(4, Temp.Parameters.Count);
            IParameter Parameter = Temp.Parameters.ElementAt(0);
            Assert.Equal(DbType.Int32, Parameter.DatabaseType);
            Assert.Equal(ParameterDirection.Input, Parameter.Direction);
            Assert.Equal("0", Parameter.ID);
            Parameter = Temp.Parameters.ElementAt(1);
            Assert.Equal(DbType.String, Parameter.DatabaseType);
            Assert.Equal(ParameterDirection.Input, Parameter.Direction);
            Assert.Equal("1", Parameter.ID);
            Parameter = Temp.Parameters.ElementAt(2);
            Assert.Equal(DbType.Single, Parameter.DatabaseType);
            Assert.Equal(ParameterDirection.Input, Parameter.Direction);
            Assert.Equal("2", Parameter.ID);
            Parameter = Temp.Parameters.ElementAt(3);
            Assert.Equal(DbType.Guid, Parameter.DatabaseType);
            Assert.Equal(ParameterDirection.Input, Parameter.Direction);
            Assert.Equal("3", Parameter.ID);
        }
    }
}