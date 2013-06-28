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
using System.Data;
using Xunit;

namespace UnitTests.DataTypes.Conversion
{
    public class GenericConverter
    {
        [Fact]
        public void Create()
        {
            Utilities.DataTypes.Conversion.Converters.GenericConverter Test = new Utilities.DataTypes.Conversion.Converters.GenericConverter(new Utilities.DataTypes.Conversion.Manager());
            Assert.Equal(true, Test.CanConvert(typeof(DbType)));
            Assert.Equal(true, Test.CanConvert(typeof(Type)));
            Assert.Equal(true, Test.CanConvert(typeof(object)));
            Assert.Equal(10f, Test.To(10, typeof(float)));
            Assert.Equal(10, Test.To(10.5f, typeof(int)));
            Assert.Equal(new DateTime(1900, 1, 1), Test.To("1/1/1900", typeof(DateTime)));
        }
    }
}