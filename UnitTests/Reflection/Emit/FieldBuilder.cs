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

using Xunit;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Reflection.Emit.Interfaces;
using Utilities.Reflection.Emit.BaseClasses;

namespace UnitTests.Reflection.Emit
{
    public class FieldBuilder
    {
        [Fact]
        public void Create()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.FieldBuilder Field = TestType.CreateField("Field1", typeof(int));
            Assert.NotNull(Field);
            Assert.Equal(typeof(int), Field.DataType);
            Assert.Equal("Field1", Field.Name);
            Assert.Equal(FieldAttributes.Public, Field.Attributes);
            Assert.NotNull(Field.Builder);
        }

        [Fact]
        public void Assign()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            IMethodBuilder Method = TestType.CreateMethod("TestMethod");
            Utilities.Reflection.Emit.FieldBuilder Field = TestType.CreateField("Field1", typeof(int));
            Assert.DoesNotThrow(() => Field.Assign(12));
        }

        [Fact]
        public void Call()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            IMethodBuilder Method = TestType.CreateMethod("TestMethod");
            Utilities.Reflection.Emit.FieldBuilder Field = TestType.CreateField("Field1", typeof(int));
            Assert.DoesNotThrow(() => Field.Call("ToString"));
        }

        [Fact]
        public void GetDefinition()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.FieldBuilder Field = TestType.CreateField("Field1", typeof(int));
            Assert.NotEmpty(Field.GetDefinition());
        }

        [Fact]
        public void Load()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.FieldBuilder Field = TestType.CreateField("Field1", typeof(int));
            IMethodBuilder Method = TestType.CreateMethod("TestMethod");
            Assert.DoesNotThrow(() => Field.Load(Method.Generator));
        }

        [Fact]
        public void Save()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.FieldBuilder Field = TestType.CreateField("Field1", typeof(int));
            IMethodBuilder Method = TestType.CreateMethod("TestMethod");
            Assert.DoesNotThrow(() => Field.Save(Method.Generator));
        }
    }
}
