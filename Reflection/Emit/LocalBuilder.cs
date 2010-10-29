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
    /// Helper class for defining a local variable
    /// </summary>
    public class LocalBuilder
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MethodBuilder">Method builder</param>
        /// <param name="Name">Name of the local</param>
        /// <param name="LocalType">Type of the local</param>
        public LocalBuilder(IMethodBuilder MethodBuilder, string Name, Type LocalType)
        {
            if (MethodBuilder == null)
                throw new ArgumentNullException("MethodBuilder");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.MethodBuilder = MethodBuilder;
            this.LocalType = LocalType;
            Setup();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the field
        /// </summary>
        private void Setup()
        {
            if (MethodBuilder == null)
                throw new NullReferenceException("No method is associated with this local variable");
            Builder = MethodBuilder.Generator.DeclareLocal(LocalType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Local name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Local builder
        /// </summary>
        public System.Reflection.Emit.LocalBuilder Builder { get; private set; }

        /// <summary>
        /// Local type
        /// </summary>
        public Type LocalType { get; private set; }

        private IMethodBuilder MethodBuilder { get; set; }

        #endregion
    }
}
