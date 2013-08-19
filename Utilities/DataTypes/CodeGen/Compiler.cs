/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Utilities.DataTypes;
using Utilities.DataTypes.CodeGen.BaseClasses;
#endregion

namespace Utilities.DataTypes.CodeGen
{
    /// <summary>
    /// Compiler
    /// </summary>
    public class Compiler : CompilerBase
    {
        #region Constructor

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
        /// <param name="AssemblyName">Assembly name</param>
        /// <param name="AssemblyDirectory">Assembly directory</param>
        /// <param name="Optimize">Optimize the assembly?</param>
        public Compiler(string AssemblyName, string AssemblyDirectory, bool Optimize)
            : base(AssemblyName, AssemblyDirectory, Optimize)
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Creates a class object
        /// </summary>
        /// <param name="ClassName">Class name</param>
        /// <param name="Code">Code</param>
        /// <param name="References">Assembly references</param>
        /// <param name="Usings">Namespace usings</param>
        /// <returns>Type generated</returns>
        public Type CreateClass(string ClassName, string Code, IEnumerable<string> Usings, params Assembly[] References)
        {
            return Add(ClassName, Code, Usings, References);
        }

        #endregion
    }
}