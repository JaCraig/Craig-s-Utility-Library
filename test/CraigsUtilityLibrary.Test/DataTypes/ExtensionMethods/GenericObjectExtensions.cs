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
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class GenericObjectExtensions
    {
        [Fact]
        public void Async()
        {
            new Action(() => string.IsNullOrEmpty("")).Async();
        }

        [Fact]
        public void Chain()
        {
            var Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp, Temp.Chain<DateTime>(x => x.AddSeconds(1)));
        }

        [Fact]
        public void Chain2()
        {
            var Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp.AddSeconds(1), Temp.Chain(x => x.AddSeconds(1)));
        }

        [Fact]
        public void Chain3()
        {
            var Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp, Temp.Chain<DateTime>(x => x.AddSeconds(1)));
            Assert.Equal(default(DateTime?), ((DateTime?)null).Chain<DateTime?>(x => x.Value.AddSeconds(1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => ((DateTime?)null).Chain<DateTime?>(x => x.Value.AddSeconds(1), DateTime.MaxValue));
        }

        [Fact]
        public void Chain4()
        {
            var Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp.AddSeconds(1), Temp.Chain(x => x.AddSeconds(1)));
            Assert.Equal(DateTime.MaxValue, ((DateTime?)null).Chain(x => x.Value.AddSeconds(1), DateTime.MaxValue));
        }

        [Fact]
        public void Chain5()
        {
            Assert.Null(new MyTestClass().Chain(x => x.A));
            Assert.NotNull(new MyTestClass().Chain(x => x.A, new MyTestClass()));
            Assert.Equal(10, new MyTestClass().Chain(x => x.A, new MyTestClass()).Chain(x => x.B));
            Assert.Equal(0, new MyTestClass().Chain(x => x.A).Chain(x => x.B));
            Assert.Equal(0, ((MyTestClass)null).Chain(x => x.A).Chain(x => x.B));
        }

        [Fact]
        public void Execute1()
        {
            Func<int> Temp = () => 1;
            Temp.Execute();
        }

        [Fact]
        public void Execute2()
        {
            Action Temp = Test;
            Assert.Throws<Exception>(() => Temp.Execute());
        }

        [Fact]
        public void If()
        {
            var Temp = new MyTestClass();
            Assert.Same(Temp, Temp.Check(x => x.B == 10));
            Assert.NotSame(Temp, Temp.Check(x => x.B == 1));
        }

        [Fact]
        public void IsDefault()
        {
            Assert.False(new DateTime(1999, 1, 1).Is(default(DateTime)));
            object TestObject = null;
            Assert.True(TestObject.Is(default(object)));
        }

        [Fact]
        public void IsNull()
        {
            Assert.False(new DateTime(1999, 1, 1).Is(default(DateTime)));
            object TestObject = null;
            Assert.True(TestObject.Is(default(object)));
        }

        [Fact]
        public void IsNullOrDBNull()
        {
            Assert.False(new DateTime(1999, 1, 1).Is(default(DateTime)));
            object TestObject = null;
            Assert.True(TestObject.Is(default(object)));
            Assert.True(DBNull.Value.Is(Convert.DBNull));
        }

        [Fact]
        public void NotIf()
        {
            var Temp = new MyTestClass();
            Assert.NotSame(Temp, Temp.Check(x => x.B != 10));
            Assert.Same(Temp, Temp.Check(x => x.B != 1));
        }

        [Fact]
        public void NullCheck()
        {
            object TestObject = new DateTime(1999, 1, 1);
            Assert.Equal(TestObject, TestObject.Check());
            Assert.Same(TestObject, TestObject.Check());
            TestObject = null;
            Assert.Equal(new DateTime(1999, 1, 2), TestObject.Check(new DateTime(1999, 1, 2)));
            Assert.Equal(new DateTime(1999, 1, 2), TestObject.Check(x => x != null, new DateTime(1999, 1, 2)));
        }

        public void Test()
        {
            throw new Exception();
        }

        [Fact]
        public void ThrowIf()
        {
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIf(x => x == null, new ArgumentNullException("TempName")));
            TempObject.ThrowIf(x => x != null, new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfDefault()
        {
            Assert.Throws<ArgumentNullException>(() => default(DateTime).ThrowIfDefault("TempName"));
            Assert.Throws<ArgumentNullException>(() => default(DateTime).ThrowIfDefault(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfFalse()
        {
            Assert.Throws<Exception>(() => "ASDF".ThrowIfNot(x => string.IsNullOrEmpty(x), new Exception()));
            "ASDF".ThrowIfNot(x => !string.IsNullOrEmpty(x), new Exception());
        }

        [Fact]
        public void ThrowIfNotDefault()
        {
            default(DateTime).ThrowIfNotDefault("TempName");
            default(DateTime).ThrowIfNotDefault(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNotNull()
        {
            object TempObject = null;
            TempObject.ThrowIfNotNull("TempName");
            TempObject.ThrowIfNotNull(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNotNullOrDBNull()
        {
            DBNull.Value.ThrowIfNotNull("TempName");
            object TempObject = null;
            TempObject.ThrowIfNotNull("TempName");
            DBNull.Value.ThrowIfNotNull(new ArgumentNullException("TempName"));
            TempObject.ThrowIfNotNull(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNotNullOrEmpty()
        {
            string TempObject = "";
            TempObject.ThrowIfNotNullOrEmpty("TempName");
            TempObject.ThrowIfNotNullOrEmpty(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNull()
        {
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull("TempName"));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfNullOrDBNull()
        {
            Assert.Throws<ArgumentNullException>(() => DBNull.Value.ThrowIfNull("TempName"));
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull("TempName"));
            Assert.Throws<ArgumentNullException>(() => DBNull.Value.ThrowIfNull(new ArgumentNullException("TempName")));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfNullOrEmpty()
        {
            string TempObject = "";
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNullOrEmpty("TempName"));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNullOrEmpty(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfTrue()
        {
            "ASDF".ThrowIf(x => string.IsNullOrEmpty(x), new Exception());
            Assert.Throws<Exception>(() => "ASDF".ThrowIf(x => !string.IsNullOrEmpty(x), new Exception()));
        }

        [Fact]
        public void Times()
        {
            Assert.Equal(new int[] { 0, 1, 2, 3, 4 }.ToList(), 5.Times(x => x));
            var Builder = new StringBuilder();
            5.Times(x => { Builder.Append(x); });
            Assert.Equal("01234", Builder.ToString());
        }
    }
}