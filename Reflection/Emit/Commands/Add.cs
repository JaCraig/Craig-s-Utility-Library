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
    /// Adds two variables
    /// </summary>
    public class Add:ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="LeftHandSide">Left variable</param>
        /// <param name="RightHandSide">Right variable</param>
        /// <param name="ObjectCount">Object counter</param>
        public Add(IMethodBuilder Method,object LeftHandSide,object RightHandSide,int ObjectCount)
        {
            if (!(LeftHandSide is VariableBase))
            {
                this.LeftHandSide = Method.CreateConstant(LeftHandSide);
            }
            else
            {
                this.LeftHandSide = (VariableBase)LeftHandSide;
            }
            if (!(RightHandSide is VariableBase))
            {
                this.RightHandSide = Method.CreateConstant(RightHandSide);
            }
            else
            {
                this.RightHandSide = (VariableBase)RightHandSide;
            }
            this.Method = Method;
            Result = this.Method.CreateLocal("AddLocalResult" + ObjectCount.ToString(), this.LeftHandSide.DataType);
            if (this.LeftHandSide is FieldBuilder || this.LeftHandSide is IPropertyBuilder)
                Method.Generator.Emit(OpCodes.Ldarg_0);
            this.LeftHandSide.Load(Method.Generator);
            if (this.RightHandSide is FieldBuilder || this.RightHandSide is IPropertyBuilder)
                Method.Generator.Emit(OpCodes.Ldarg_0);
            this.RightHandSide.Load(Method.Generator);
            Method.Generator.Emit(OpCodes.Add);
            Result.Save(Method.Generator);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Left hand side of the addition
        /// </summary>
        protected virtual VariableBase LeftHandSide { get; set; }

        /// <summary>
        /// Right hand side of the addition
        /// </summary>
        protected virtual VariableBase RightHandSide { get; set; }

        /// <summary>
        /// Value assigned to
        /// </summary>
        public virtual VariableBase Result { get; set; }

        /// <summary>
        /// Method builder
        /// </summary>
        protected virtual IMethodBuilder Method { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(Result).Append("=").Append(LeftHandSide).Append("+").Append(RightHandSide).Append(";\n");
            return Output.ToString();
        }

        #endregion
    }
}
