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
using System.Collections.Generic;
using System.Reflection;
using Utilities.DataTypes.CodeGen.BaseClasses;

namespace Utilities.DataTypes.CodeGen
{
    /// <summary>
    /// Compiler
    /// </summary>
    public class Compiler : CompilerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Compiler()
            : base("CULGeneratedTypes")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyDirectory">Assembly directory</param>
        /// <param name="optimize">Optimize the assembly?</param>
        public Compiler(string assemblyName, string assemblyDirectory, bool optimize)
            : base(assemblyName, assemblyDirectory, optimize)
        {
        }

        /// <summary>
        /// Compiles the specified code and returns the types that are created
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="usings">The usings.</param>
        /// <param name="references">The references.</param>
        /// <returns>The list of types that are generated</returns>
        public IEnumerable<Type> Create(string code, IEnumerable<string> usings, params Assembly[] references)
        {
            return Add(code, usings, references);
        }

        /// <summary>
        /// Creates a class object
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="code">Code</param>
        /// <param name="references">Assembly references</param>
        /// <param name="usings">Namespace usings</param>
        /// <returns>Type generated</returns>
        public Type CreateClass(string className, string code, IEnumerable<string> usings, params Assembly[] references)
        {
            return Add(className, code, usings, references);
        }
    }
}