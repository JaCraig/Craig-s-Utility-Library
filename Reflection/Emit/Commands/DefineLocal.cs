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
using Utilities.Reflection.Emit.Interfaces;
using System.Reflection;
using Utilities.Reflection.Emit.Commands;
using System.Reflection.Emit;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Defines a local variable
    /// </summary>
    public class DefineLocal:ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="Name">Local object name</param>
        /// <param name="LocalType">Local type</param>
        public DefineLocal(IMethodBuilder Method,string Name,Type LocalType)
        {
            TempObject = new LocalBuilder(Method, Name, LocalType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Temp object
        /// </summary>
        protected virtual IVariable TempObject { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the object that temporarily stores the new object
        /// </summary>
        /// <returns>The new object</returns>
        public virtual IVariable GetObject()
        {
            return TempObject;
        }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(TempObject.GetDefinition()).Append(";\n");
            return Output.ToString();
        }

        #endregion
    }
}
