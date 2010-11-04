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
    /// Helper class for defining a constant value
    /// </summary>
    public class ConstantBuilder : IVariable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Value of the constant</param>
        public ConstantBuilder(object Value)
        {
            this.Value = Value;
            if (Value != null)
            {
                this.Type = Value.GetType();
                return;
            }
            this.Type = null;
        }

        #endregion

        #region Functions

        public void Load(ILGenerator Generator)
        {
            if (this.Value == null)
            {
                Generator.Emit(OpCodes.Ldnull);
            }
            else if (this.Type == typeof(Int32))
            {
                Generator.Emit(OpCodes.Ldc_I4, (Int32)Value);
            }
            else if (this.Type == typeof(Int64))
            {
                Generator.Emit(OpCodes.Ldc_I8, (Int64)Value);
            }
            else if (this.Type == typeof(float))
            {
                Generator.Emit(OpCodes.Ldc_R4, (float)Value);
            }
            else if (this.Type == typeof(double))
            {
                Generator.Emit(OpCodes.Ldc_R8, (double)Value);
            }
            else if (this.Type == typeof(string))
            {
                Generator.Emit(OpCodes.Ldstr, (string)Value);
            }
        }

        public void Save(ILGenerator Generator)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Local type
        /// </summary>
        public Type Type { get; protected set; }

        /// <summary>
        /// Value of the constant
        /// </summary>
        public object Value { get; protected set; }

        public string Name { get { return ""; } }

        #endregion
    }
}
