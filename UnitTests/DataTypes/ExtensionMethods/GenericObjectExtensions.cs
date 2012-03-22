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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonUnit;
using MoonUnit.Attributes;
using Utilities.DataTypes.ExtensionMethods;
using System.Data;
using System.Collections;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class GenericObjectExtensions
    {
        [Test]
        public void If()
        {
            MyTestClass Temp = new MyTestClass();
            Assert.Same(Temp, Temp.If(x => x.B == 10));
            Assert.NotSame(Temp, Temp.If(x => x.B == 1));
        }

        [Test]
        public void NotIf()
        {
            MyTestClass Temp = new MyTestClass();
            Assert.NotSame(Temp, Temp.NotIf(x => x.B == 10));
            Assert.Same(Temp, Temp.NotIf(x => x.B == 1));
        }

        [Test]
        public void Execute1()
        {
            Func<int> Temp = () => 1;
            Assert.DoesNotThrow<Exception>(() => Temp.Execute());
        }

        [Test]
        public void Execute2()
        {
            Action Temp = () => Test();
            Assert.Throws<Exception>(() => Temp.Execute());
        }

        [Test]
        public void Chain()
        {
            DateTime Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp, Temp.Chain<DateTime>(x => x.AddSeconds(1)));
        }

        [Test]
        public void Chain2()
        {
            DateTime Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp.AddSeconds(1), Temp.Chain(x => x.AddSeconds(1)));
        }

        [Test]
        public void Do()
        {
            DateTime Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp, Temp.Do<DateTime>(x => x.AddSeconds(1)));
            Assert.Equal(DateTime.MaxValue, ((DateTime?)null).Do<DateTime?>(x => x.Value.AddSeconds(1), DateTime.MaxValue));
        }

        [Test]
        public void Do2()
        {
            DateTime Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp.AddSeconds(1), Temp.Do(x => x.AddSeconds(1)));
            Assert.Equal(DateTime.MaxValue, ((DateTime?)null).Do(x => x.Value.AddSeconds(1), DateTime.MaxValue));
        }

        [Test]
        public void Return()
        {
            Assert.Null(new MyTestClass().Return(x => x.A));
            Assert.NotNull(new MyTestClass().Return(x => x.A, new MyTestClass()));
            Assert.Equal(10, new MyTestClass().Return(x => x.A, new MyTestClass()).Return(x => x.B));
            Assert.Equal(0, new MyTestClass().Return(x => x.A).Return(x => x.B));
            Assert.Equal(0, ((MyTestClass)null).Return(x => x.A).Return(x => x.B));
        }

        [Test]
        public void ThrowIfTrue()
        {
            Assert.DoesNotThrow<Exception>(() => "ASDF".ThrowIfTrue(x => string.IsNullOrEmpty(x), new Exception()));
            Assert.Throws<Exception>(() => "ASDF".ThrowIfTrue(x => !string.IsNullOrEmpty(x), new Exception()));
        }

        [Test]
        public void ThrowIfFalse()
        {
            Assert.Throws<Exception>(() => "ASDF".ThrowIfFalse(x => string.IsNullOrEmpty(x), new Exception()));
            Assert.DoesNotThrow<Exception>(() => "ASDF".ThrowIfFalse(x => !string.IsNullOrEmpty(x), new Exception()));
        }

        public void Test()
        {
            throw new Exception();
        }
    }
}
