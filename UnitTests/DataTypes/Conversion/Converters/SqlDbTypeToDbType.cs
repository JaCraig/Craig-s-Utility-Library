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

using System.Data;
using Xunit;

namespace UnitTests.DataTypes.Conversion
{
    public class SqlDbTypeToDbType
    {
        [Fact]
        public void Create()
        {
            Utilities.DataTypes.Conversion.Converters.SqlDbTypeToDbType Test = new Utilities.DataTypes.Conversion.Converters.SqlDbTypeToDbType(new Utilities.DataTypes.Conversion.Manager());
            Assert.Equal(true, Test.CanConvert(typeof(SqlDbType)));
            Assert.Equal(true, Test.CanConvert(typeof(DbType)));
            Assert.Equal(DbType.Int64, Test.To(SqlDbType.BigInt, typeof(DbType)));
            Assert.Equal(DbType.Binary, Test.To(SqlDbType.Binary, typeof(DbType)));
            Assert.Equal(DbType.Boolean, Test.To(SqlDbType.Bit, typeof(DbType)));
            Assert.Equal(DbType.DateTime2, Test.To(SqlDbType.DateTime2, typeof(DbType)));
            Assert.Equal(DbType.Int32, Test.To(SqlDbType.Int, typeof(DbType)));
        }
    }
}