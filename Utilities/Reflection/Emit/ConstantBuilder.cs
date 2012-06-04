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

#region Usings
using System;
using System.Reflection.Emit;
using Utilities.Reflection.Emit.BaseClasses;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining a constant value
    /// </summary>
    public class ConstantBuilder : VariableBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Value of the constant</param>
        public ConstantBuilder(object Value)
            : base()
        {
            this.Value = Value;
            if (Value != null)
            {
                this.DataType = Value.GetType();
                return;
            }
            this.DataType = null;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Saves the constant
        /// </summary>
        /// <param name="Generator">IL Generator</param>
        public override void Save(ILGenerator Generator)
        {
        }

        /// <summary>
        /// Loads the constant
        /// </summary>
        /// <param name="Generator">IL Generator</param>
        public override void Load(ILGenerator Generator)
        {
            if (this.Value == null)
            {
                Generator.Emit(OpCodes.Ldnull);
            }
            else if (this.DataType == typeof(Int32))
            {
                Generator.Emit(OpCodes.Ldc_I4, (Int32)Value);
            }
            else if (this.DataType == typeof(Int64))
            {
                Generator.Emit(OpCodes.Ldc_I8, (Int64)Value);
            }
            else if (this.DataType == typeof(float))
            {
                Generator.Emit(OpCodes.Ldc_R4, (float)Value);
            }
            else if (this.DataType == typeof(double))
            {
                Generator.Emit(OpCodes.Ldc_R8, (double)Value);
            }
            else if (this.DataType == typeof(string))
            {
                Generator.Emit(OpCodes.Ldstr, (string)Value);
            }
            else if(this.DataType==typeof(bool))
            {
                if((bool)Value)
                {
                    Generator.Emit(OpCodes.Ldc_I4_1);
                }
                else
                {
                    Generator.Emit(OpCodes.Ldc_I4_0);
                }
            }
        }

        /// <summary>
        /// Get the definition of the constant
        /// </summary>
        /// <returns>The definition of the constant</returns>
        public override string GetDefinition()
        {
            return Value.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value of the constant
        /// </summary>
        public virtual object Value { get; protected set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// The definition of the constant as a string
        /// </summary>
        /// <returns>The constant as a string</returns>
        public override string ToString()
        {
            if (this.DataType == typeof(string))
            {
                return "\"" + Value.ToString() + "\"";
            }
            return Value.ToString();
        }

        #endregion
    }
}