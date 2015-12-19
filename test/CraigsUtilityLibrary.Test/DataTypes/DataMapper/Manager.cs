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
using UnitTests.DataTypes.DataMapper.Default;
using Utilities.DataTypes;
using Utilities.DataTypes.DataMapper.Interfaces;
using Xunit;

namespace UnitTests.DataTypes.DataMapper
{
    public class Manager
    {
        [Fact]
        public void CreationTest()
        {
            Utilities.DataTypes.DataMapper.Manager TestObject = null;
            TestObject = new Utilities.DataTypes.DataMapper.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<IDataMapper>(), AppDomain.CurrentDomain.GetAssemblies().Objects<IMapperModule>());
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void TypeMappingTest()
        {
            var TestObject = new Utilities.DataTypes.DataMapper.Manager(AppDomain.CurrentDomain.GetAssemblies().Objects<IDataMapper>(), AppDomain.CurrentDomain.GetAssemblies().Objects<IMapperModule>());
            Assert.NotNull(TestObject.Map<MappingA, MappingB>());
            Assert.IsType<Utilities.DataTypes.DataMapper.Default.TypeMapping<MappingA, MappingB>>(TestObject.Map<MappingA, MappingB>());
        }
    }
}