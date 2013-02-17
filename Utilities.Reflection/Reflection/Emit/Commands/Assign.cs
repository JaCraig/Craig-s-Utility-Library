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
using System.Text;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Assignment command
    /// </summary>
    public class Assign : CommandBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="Value">Value to store</param>
        public Assign(VariableBase LeftHandSide, object Value)
            : base()
        {
            if (LeftHandSide == null)
                throw new ArgumentNullException("LeftHandSide");
            this.LeftHandSide = LeftHandSide;
            VariableBase TempValue = Value as VariableBase;
            this.RightHandSide = TempValue == null ? Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateConstant(Value) : TempValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Left hand side of the assignment
        /// </summary>
        protected virtual VariableBase LeftHandSide { get; set; }

        /// <summary>
        /// Value to assign
        /// </summary>
        protected virtual VariableBase RightHandSide { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the command
        /// </summary>
        public override void Setup()
        {
            ILGenerator Generator = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator;
            if (RightHandSide.DataType.IsValueType 
                && !LeftHandSide.DataType.IsValueType)
            {
                RightHandSide = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Box(RightHandSide);
            }
            else if (!RightHandSide.DataType.IsValueType 
                && LeftHandSide.DataType.IsValueType)
            {
                RightHandSide = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.UnBox(RightHandSide, LeftHandSide.DataType);
            }
            else if (!RightHandSide.DataType.IsValueType 
                && !LeftHandSide.DataType.IsValueType
                && RightHandSide.DataType != LeftHandSide.DataType)
            {
                RightHandSide = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Cast(RightHandSide, LeftHandSide.DataType);
            }
            if (LeftHandSide is FieldBuilder || LeftHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            if (RightHandSide is FieldBuilder || RightHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            RightHandSide.Load(Generator);
            if (RightHandSide.DataType != LeftHandSide.DataType)
            {
                if (ConversionOpCodes.ContainsKey(LeftHandSide.DataType))
                {
                    Generator.Emit(ConversionOpCodes[LeftHandSide.DataType]);
                }
            }
            LeftHandSide.Save(Generator);
        }

        /// <summary>
        /// Converts the command to a string
        /// </summary>
        /// <returns>The string version of the command</returns>
        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(LeftHandSide).Append("=").Append(RightHandSide).Append(";\n");
            return Output.ToString();
        }

        #endregion
    }
}