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
using Utilities.Reflection.Emit.BaseClasses;

namespace UnitTests.Reflection.Emit
{
    public class DefaultPropertyBuilder
    {
        [Test]
        public void Create()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.Interfaces.IPropertyBuilder Property = TestType.CreateDefaultProperty("Property1", typeof(int));
            Assert.NotNull(Property);
            Assert.Equal(typeof(int), Property.DataType);
            Assert.Equal("Property1", Property.Name);
            Assert.Equal(PropertyAttributes.SpecialName, Property.Attributes);
            Assert.NotNull(Property.GetMethod);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, Property.GetMethodAttributes);
            Assert.NotNull(Property.SetMethod);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.Virtual, Property.SetMethodAttributes);
            Assert.NotNull(Property.Builder);
        }

        [Test]
        public void GetDefinition()
        {
            Utilities.Reflection.Emit.Assembly Assembly = new Utilities.Reflection.Emit.Assembly("TestAssembly");
            Utilities.Reflection.Emit.TypeBuilder TestType = Assembly.CreateType("TestType");
            Utilities.Reflection.Emit.Interfaces.IPropertyBuilder Property = TestType.CreateDefaultProperty("Property1", typeof(int));
            Assert.NotEmpty(Property.GetDefinition());
        }
    }
}
