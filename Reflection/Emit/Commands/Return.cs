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
using Utilities.Reflection.Emit.BaseClasses;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Return command
    /// </summary>
    public class Return:ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReturnType">Return type</param>
        /// <param name="ReturnValue">Return value</param>
        /// <param name="Generator">IL generator</param>
        public Return(Type ReturnType,object ReturnValue,ILGenerator Generator)
        {
            this.ReturnType = ReturnType;
            this.ReturnValue = ReturnValue;
            if (ReturnType == typeof(void) || ReturnType == null)
            {
                Generator.Emit(OpCodes.Ret);
                return;
            }
            if (ReturnValue == null)
            {
                ConstantBuilder Constant = new ConstantBuilder(ReturnValue);
                ReturnValue = Constant;
                Constant.Load(Generator);
                Generator.Emit(OpCodes.Ret);
                return;
            }
            if (ReturnValue is FieldBuilder || ReturnValue is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            if (ReturnValue is VariableBase)
            {
                ((VariableBase)ReturnValue).Load(Generator);
            }
            else
            {
                ConstantBuilder Constant = new ConstantBuilder(ReturnValue);
                Constant.Load(Generator);
            }
            Generator.Emit(OpCodes.Ret);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return type
        /// </summary>
        protected virtual Type ReturnType { get; set; }

        /// <summary>
        /// Return value
        /// </summary>
        protected virtual object ReturnValue { get; set; }

        #endregion

        #region Overridden Function

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            if (ReturnType != null && ReturnType != typeof(void))
            {
                Output.Append("return ").Append(ReturnValue.ToString()).Append(";\n");
            }
            return Output.ToString();
        }

        #endregion
    }
}
