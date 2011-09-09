/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using MoonUnit.Attributes;
using MoonUnit;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Reflection.Emit.Interfaces;

namespace UnitTests.Reflection.Emit
{
    public class TypeBuilder
    {
        [Test]
        public void Create()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Assert.NotNull(TestType);
            Assert.DoesNotThrow<Exception>(() => Assembly.Create());
            Assert.NotNull(Activator.CreateInstance(TestType.DefinedType));
        }

        [Test]
        public void CreateMethod()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.Interfaces.IMethodBuilder TestMethod = TestType.CreateMethod("TestMethod");
            Assert.NotNull(TestMethod);
            Assert.Equal("TestMethod", TestMethod.Name);
            Assert.Equal(typeof(void), TestMethod.ReturnType);
            Assert.NotNull(TestMethod.Generator);
            Assert.NotNull(TestMethod.This);
            Assert.Equal(1, TestMethod.Parameters.Count);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, TestMethod.Attributes);
        }

        [Test]
        public void CreateField()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.FieldBuilder TestField = TestType.CreateField("TestField", typeof(int));
            Assert.NotNull(TestField);
            Assert.Equal("TestField", TestField.Name);
            Assert.Equal(typeof(int), TestField.DataType);
            Assert.NotNull(TestField.Builder);
            Assert.Equal(FieldAttributes.Public, TestField.Attributes);
        }

        [Test]
        public void CreateProperty()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            IPropertyBuilder TestProperty=TestType.CreateProperty("TestProperty", typeof(int));
            Assert.NotNull(TestProperty);
            Assert.Equal("TestProperty", TestProperty.Name);
            Assert.Equal(typeof(int), TestProperty.DataType);
            Assert.NotNull(TestProperty.Builder);
            Assert.Equal(PropertyAttributes.SpecialName, TestProperty.Attributes);
            Assert.NotNull(TestProperty.GetMethod);
            Assert.NotNull(TestProperty.SetMethod);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, TestProperty.GetMethodAttributes);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, TestProperty.SetMethodAttributes);
        }

        [Test]
        public void CreateDefaultProperty()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            IPropertyBuilder TestProperty = TestType.CreateDefaultProperty("TestProperty", typeof(int));
            Assert.NotNull(TestProperty);
            Assert.Equal("TestProperty", TestProperty.Name);
            Assert.Equal(typeof(int), TestProperty.DataType);
            Assert.NotNull(TestProperty.Builder);
            Assert.Equal(PropertyAttributes.SpecialName, TestProperty.Attributes);
            Assert.NotNull(TestProperty.GetMethod);
            Assert.NotNull(TestProperty.SetMethod);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, TestProperty.GetMethodAttributes);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, TestProperty.SetMethodAttributes);
        }

        [Test]
        public void CreateConstructor()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            IMethodBuilder TestConstructor = TestType.CreateConstructor();
            Assert.NotNull(TestConstructor);
            Assert.Equal(MethodAttributes.Public, TestConstructor.Attributes);
            Assert.NotNull(TestConstructor.Generator);
            Assert.Null(TestConstructor.Name);
            Assert.Equal(1, TestConstructor.Parameters.Count);
            Assert.Null(TestConstructor.ReturnType);
            Assert.NotNull(TestConstructor.This);
        }

        [Test]
        public void CreateDefaultConstructor()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            IMethodBuilder TestConstructor = TestType.CreateDefaultConstructor();
            Assert.NotNull(TestConstructor);
            Assert.Equal(MethodAttributes.Public, TestConstructor.Attributes);
            Assert.Null(TestConstructor.Generator);
            Assert.Null(TestConstructor.Name);
            Assert.Equal(1, TestConstructor.Parameters.Count);
            Assert.Null(TestConstructor.ReturnType);
            Assert.NotNull(TestConstructor.This);
        }
    }
}
