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
using Utilities.Reflection.Emit.Enums;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    public class While:ICommand
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        public While(IMethodBuilder Method, Comparison ComparisonType, IVariable LeftHandSide, IVariable RightHandSide)
        {
            this.Method = Method;
            this.StartWhileLabel = Method.Generator.DefineLabel();
            this.EndWhileLabel = Method.Generator.DefineLabel();
            this.LeftHandSide = LeftHandSide;
            this.RightHandSide = RightHandSide;
            this.ComparisonType = ComparisonType;
            Method.Generator.MarkLabel(StartWhileLabel);
            if (LeftHandSide is FieldBuilder || LeftHandSide is IPropertyBuilder)
                Method.Generator.Emit(OpCodes.Ldarg_0);
            LeftHandSide.Load(Method.Generator);
            if (RightHandSide is FieldBuilder || RightHandSide is IPropertyBuilder)
                Method.Generator.Emit(OpCodes.Ldarg_0);
            RightHandSide.Load(Method.Generator);
            if (ComparisonType == Comparison.Equal)
            {
                Method.Generator.Emit(OpCodes.Ceq);
                Method.Generator.Emit(OpCodes.Ldc_I4_0);
                Method.Generator.Emit(OpCodes.Beq, EndWhileLabel);
            }
            else if (ComparisonType == Comparison.GreaterThan)
            {
                Method.Generator.Emit(OpCodes.Ble, EndWhileLabel);
            }
            else if (ComparisonType == Comparison.GreaterThenOrEqual)
            {
                Method.Generator.Emit(OpCodes.Blt, EndWhileLabel);
            }
            else if (ComparisonType == Comparison.LessThan)
            {
                Method.Generator.Emit(OpCodes.Bge, EndWhileLabel);
            }
            else if (ComparisonType == Comparison.LessThanOrEqual)
            {
                Method.Generator.Emit(OpCodes.Bgt, EndWhileLabel);
            }
            else if (ComparisonType == Comparison.NotEqual)
            {
                Method.Generator.Emit(OpCodes.Beq, EndWhileLabel);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Method builder
        /// </summary>
        protected virtual IMethodBuilder Method { get; set; }

        /// <summary>
        /// Start while label
        /// </summary>
        public virtual Label StartWhileLabel { get; set; }

        /// <summary>
        /// End while label
        /// </summary>
        public virtual Label EndWhileLabel { get; set; }

        /// <summary>
        /// Left hand side of the comparison
        /// </summary>
        public virtual IVariable LeftHandSide { get; set; }

        /// <summary>
        /// Right hand side of the comparison
        /// </summary>
        public virtual IVariable RightHandSide { get; set; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public virtual Comparison ComparisonType { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Ends the while statement
        /// </summary>
        public virtual void EndWhile()
        {
            Method.EndWhile(this);
        }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append("while(");
            Output.Append(LeftHandSide);
            if (ComparisonType == Comparison.Equal)
            {
                Output.Append("==");
            }
            else if (ComparisonType == Comparison.GreaterThan)
            {
                Output.Append(">");
            }
            else if (ComparisonType == Comparison.GreaterThenOrEqual)
            {
                Output.Append(">=");
            }
            else if (ComparisonType == Comparison.LessThan)
            {
                Output.Append("<");
            }
            else if (ComparisonType == Comparison.LessThanOrEqual)
            {
                Output.Append("<=");
            }
            else if (ComparisonType == Comparison.NotEqual)
            {
                Output.Append("!=");
            }
            Output.Append(RightHandSide).Append(")\n{\n");
            return Output.ToString();
        }

        #endregion
    }
}
