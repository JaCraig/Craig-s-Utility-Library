/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining a method within a type
    /// </summary>
    public class MethodBuilder:IMethodBuilder
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the method</param>
        /// <param name="Attributes">Attributes for the method (public, private, etc.)</param>
        /// <param name="Parameters">Parameter types for the method</param>
        /// <param name="ReturnType">Return type for the method</param>
        public MethodBuilder(TypeBuilder TypeBuilder, string Name, 
            MethodAttributes Attributes,List<Type> Parameters, Type ReturnType)
        {
            if (TypeBuilder==null)
                throw new ArgumentNullException("TypeBuilder");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.Type = TypeBuilder;
            this.Attributes = Attributes;
            this.ReturnType = (ReturnType == null) ? typeof(void) : ReturnType;
            if (Parameters != null)
            {
                this.ParameterTypes = new List<Type>();
                this.ParameterTypes.AddRange(Parameters);
            }
            Setup();
        }

        #endregion

        #region Functions

        private void Setup()
        {
            if (Type == null)
                throw new NullReferenceException("No type is associated with this method");
            Builder = Type.Builder.DefineMethod(Name, Attributes, ReturnType,
                (ParameterTypes != null && ParameterTypes.Count > 0) ? ParameterTypes.ToArray() : System.Type.EmptyTypes);
            Generator = Builder.GetILGenerator();
        }

        public LocalBuilder CreateLocal(string Name, Type LocalType)
        {
            return new LocalBuilder(this, Name, LocalType);
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public Type ReturnType { get; private set; }
        public List<Type> ParameterTypes { get; private set; }
        public System.Reflection.MethodAttributes Attributes { get; private set; }
        public ILGenerator Generator { get; private set; }

        /// <summary>
        /// Method builder
        /// </summary>
        public System.Reflection.Emit.MethodBuilder Builder { get; private set; }

        private TypeBuilder Type { get; set; }

        #endregion
    }
}