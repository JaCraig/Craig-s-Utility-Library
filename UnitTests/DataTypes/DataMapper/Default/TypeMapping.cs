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

using Utilities.DataTypes.DataMapper.Default;
using Xunit;

namespace UnitTests.DataTypes.DataMapper.Default
{
    public class TypeMapping
    {
        [Fact]
        public void AutoMapping()
        {
            Utilities.DataTypes.DataMapper.Default.TypeMapping<MappingA, MappingB> TestObject = null;
            TestObject = new TypeMapping<MappingA, MappingB>();
            Assert.NotNull(TestObject);
            TestObject.AutoMap();
            var A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            var B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            TestObject.CopyLeftToRight(A, B);
            Assert.Equal(B.Item1, 12);
            Assert.Equal(B.Item2, "ASDF");
        }

        [Fact]
        public void CreationTest()
        {
            Utilities.DataTypes.DataMapper.Default.TypeMapping<MappingA, MappingB> TestObject = null;
            TestObject = new TypeMapping<MappingA, MappingB>();
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void Mapping()
        {
            Utilities.DataTypes.DataMapper.Default.TypeMapping<MappingA, MappingB> TestObject = null;
            TestObject = new TypeMapping<MappingA, MappingB>();
            Assert.NotNull(TestObject);
            TestObject.AddMapping(x => x.Item1, x => x.Item1);
            TestObject.AddMapping(x => x.Item2, x => x.Item2);
            var A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            var B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            TestObject.CopyLeftToRight(A, B);
            Assert.Equal(B.Item1, 12);
            Assert.Equal(B.Item2, "ASDF");
            A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            TestObject.CopyRightToLeft(B, A);
            Assert.Equal(A.Item1, 13);
            Assert.Equal(A.Item2, "ZXCV");
        }
    }
}