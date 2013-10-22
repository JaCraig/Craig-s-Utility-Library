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
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.Dynamic
{
    public class Dynamo
    {
        [Fact]
        public void SetAndGetMember()
        {
            dynamic Temp = new TestClass();
            Assert.Equal<string>((string)null, Temp.A);
            Temp.A = "Testing";
            Temp.B = 1;
            int B = Temp.B;
            Assert.Equal<string>("Testing", Temp.A);
            Assert.Equal<int>(1, B);
        }

        [Fact]
        public void ConvertToClass()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            TestClass2 Temp2 = Temp;
            Assert.Equal("Testing", Temp2.A);
            Assert.Equal(1, Temp2.B);

            Temp = new Utilities.DataTypes.Dynamo();
            Temp.A = "Testing";
            Temp.B = 1;
            Temp2 = Temp;
            Assert.Equal("Testing", Temp2.A);
            Assert.Equal(1, Temp2.B);
        }


        [Fact]
        public void CopyBetweenItems()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            Utilities.DataTypes.Dynamo Temp2 = Temp;
            Assert.Equal("Testing", Temp2["A"]);
            Assert.Equal(1, Temp2["B"]);

            Temp = new Utilities.DataTypes.Dynamo();
            Temp.A = "Testing2";
            Temp.B = 2;
            Temp2 = Temp;
            Assert.Equal("Testing2", Temp2["A"]);
            Assert.Equal(2, Temp2["B"]);
        }

        [Fact]
        public void CallMethod()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = new Func<string>(() => Temp.A);
            Assert.Equal("Testing", Temp.B());
            Assert.Equal("Testing", Temp.A);
        }

        public class TestClass : Utilities.DataTypes.Dynamo<TestClass>
        {
            public string A { get; set; }
        }

        public class TestClass2
        {
            public string A { get; set; }
            public int B { get; set; }
        }
    }
}
