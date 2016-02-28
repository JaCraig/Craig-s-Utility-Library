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
using Xunit;

namespace UnitTests.DataTypes.Conversion
{
    public class Manager
    {
        public Manager()
        {
            TestObject = new Utilities.DataTypes.Conversion.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<IConverter>());
        }

        private Utilities.DataTypes.Conversion.Manager TestObject { get; set; }

        [Fact]
        public void Create()
        {
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void To()
        {
            Assert.Equal(10, Utilities.DataTypes.Conversion.Manager.To(10.0f, 5));
            Assert.Equal(5.0f, Utilities.DataTypes.Conversion.Manager.To(5, 10.0f));
            Assert.Equal(5.0f, Utilities.DataTypes.Conversion.Manager.To("5", 10.0f));
            Assert.Equal("5", Utilities.DataTypes.Conversion.Manager.To(5, ""));
            Assert.Equal(DbType.Guid, Utilities.DataTypes.Conversion.Manager.To(typeof(Guid), DbType.Int32));
            Assert.Equal(DbType.Int32, Utilities.DataTypes.Conversion.Manager.To(typeof(int), DbType.Int16));
        }

        [Fact]
        public void ToClasses()
        {
            var Object1 = new TestA { A = 10, B = "ASDF" };
            var Object2 = new TestB { A = 20, B = "ZXCV" };
            TestB Result1 = Utilities.DataTypes.Conversion.Manager.To<TestA, TestB>(Object1, null);
            Assert.Equal(10, Result1.A);
            Assert.Equal("ASDF", Result1.B);
            TestA Result2 = Utilities.DataTypes.Conversion.Manager.To<TestB, TestA>(Object2, null);
            Assert.Equal(20, Result2.A);
            Assert.Equal("ZXCV", Result2.B);
        }

        public class TestA
        {
            public int A { get; set; }

            public string B { get; set; }
        }

        public class TestB
        {
            public int A { get; set; }

            public string B { get; set; }
        }
    }
}