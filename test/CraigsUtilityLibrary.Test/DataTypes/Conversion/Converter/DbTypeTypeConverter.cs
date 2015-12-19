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
using Xunit;

namespace UnitTests.DataTypes.Conversion.Converter
{
    public class DbTypeTypeConverter
    {
        [Fact]
        public void CanConvertTo()
        {
            var Temp = new Utilities.DataTypes.Conversion.Converters.DbTypeTypeConverter();
            Assert.Equal(typeof(DbType), Temp.AssociatedType);
            Assert.True(Temp.CanConvertTo(typeof(SqlDbType)));
            Assert.True(Temp.CanConvertTo(typeof(Type)));
        }

        [Fact]
        public void ConvertFrom()
        {
            var Temp = new Utilities.DataTypes.Conversion.Converters.DbTypeTypeConverter();
            Assert.Equal(DbType.Int16, Temp.ConvertFrom(SqlDbType.SmallInt));
            Assert.Equal(DbType.Int32, Temp.ConvertFrom(SqlDbType.Int));
            Assert.Equal(DbType.Int16, Temp.ConvertFrom(typeof(Int16)));
            Assert.Equal(DbType.Int32, Temp.ConvertFrom(typeof(Int32)));
        }

        [Fact]
        public void ConvertTo()
        {
            var Temp = new Utilities.DataTypes.Conversion.Converters.DbTypeTypeConverter();
            Assert.Equal(SqlDbType.SmallInt, Temp.ConvertTo(DbType.Int16, typeof(SqlDbType)));
            Assert.Equal(SqlDbType.Int, Temp.ConvertTo(DbType.Int32, typeof(SqlDbType)));
            Assert.Equal(typeof(int), Temp.ConvertTo(DbType.Int32, typeof(Type)));
            Assert.Equal(typeof(short), Temp.ConvertTo(DbType.Int16, typeof(Type)));
        }
    }
}