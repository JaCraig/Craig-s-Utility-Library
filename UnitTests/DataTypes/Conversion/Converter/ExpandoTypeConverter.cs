/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using Utilities.DataTypes.CodeGen.BaseClasses;
using Utilities.IO;
using Utilities.IO.FileSystem.Interfaces;
using Xunit;
using Utilities.DataTypes;
using System.Data;
using System.Dynamic;
using System.Collections.Generic;

namespace UnitTests.DataTypes.Conversion.Converter
{
    public class ExpandoTypeConverter
    {
        [Fact]
        public void CanConvertTo()
        {
            Utilities.DataTypes.Conversion.Converters.ExpandoTypeConverter Temp = new Utilities.DataTypes.Conversion.Converters.ExpandoTypeConverter();
            Assert.Equal(typeof(ExpandoObject), Temp.AssociatedType);
            Assert.True(Temp.CanConvertTo(typeof(ExpandoObject)));
            Assert.True(Temp.CanConvertFrom(typeof(ExpandoTypeConverter)));
        }

        [Fact]
        public void ConvertTo()
        {
            Utilities.DataTypes.Conversion.Converters.ExpandoTypeConverter Temp = new Utilities.DataTypes.Conversion.Converters.ExpandoTypeConverter();
            IDictionary<string,object> TestObject=new ExpandoObject();
            TestObject.Add("A","This is a test");
            TestObject.Add("B",10);
            TestClass Result = (TestClass)Temp.ConvertTo(TestObject, typeof(TestClass));
            Assert.Equal(10, Result.B);
            Assert.Equal("This is a test", Result.A);
        }
        
        [Fact]
        public void ConvertFrom()
        {
            Utilities.DataTypes.Conversion.Converters.ExpandoTypeConverter Temp = new Utilities.DataTypes.Conversion.Converters.ExpandoTypeConverter();
            IDictionary<string,object> Result=(ExpandoObject)Temp.ConvertFrom(new TestClass(){A="This is a test",B=10});
            Assert.Equal(10, Result["B"]);
            Assert.Equal("This is a test", Result["A"]);
        }

        public class TestClass
        {
            public string A { get; set; }
            public int B { get; set; }
        }
    }
}
