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
using Utilities.DataTypes;
using Utilities.DataTypes.Conversion.Converters.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Xunit;

namespace UnitTests.ORM.Manager.QueryProvider.Default
{
    public class Parameter
    {
        public Parameter()
        {
            var TestObject = new Utilities.DataTypes.Conversion.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<IConverter>());
        }

        [Fact]
        public void Create()
        {
            var Parameter = new Utilities.ORM.Manager.QueryProvider.Default.Parameter<int>("Test", 101);
            Assert.Equal("Test", Parameter.ID);
            Assert.Equal(101, Parameter.Value);
            Assert.Equal("@", Parameter.ParameterStarter);
            Assert.Equal(ParameterDirection.Input, Parameter.Direction);
            Assert.Equal(DbType.Int32, Parameter.DatabaseType);
        }

        [Fact]
        public void CreateCopy()
        {
            var Parameter = new Utilities.ORM.Manager.QueryProvider.Default.Parameter<int>("Test", 101);
            var Parameter2 = (Utilities.ORM.Manager.QueryProvider.Default.Parameter<int>)Parameter.CreateCopy("0");
            Assert.Equal("Test0", Parameter2.ID);
            Assert.Equal(101, Parameter2.Value);
            Assert.Equal("@", Parameter2.ParameterStarter);
            Assert.Equal(ParameterDirection.Input, Parameter2.Direction);
            Assert.Equal(DbType.Int32, Parameter2.DatabaseType);
        }
    }
}