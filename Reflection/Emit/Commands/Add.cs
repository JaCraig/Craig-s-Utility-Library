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
using System.Reflection.Emit;
using System.Text;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Adds two variables
    /// </summary>
    public class Add : CommandBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LeftHandSide">Left variable</param>
        /// <param name="RightHandSide">Right variable</param>
        public Add(object LeftHandSide, object RightHandSide)
            : base()
        {
            Utilities.Reflection.Emit.BaseClasses.MethodBase Method = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod;
            if (!(LeftHandSide is VariableBase))
                this.LeftHandSide = Method.CreateConstant(LeftHandSide);
            else
                this.LeftHandSide = (VariableBase)LeftHandSide;
            if (!(RightHandSide is VariableBase))
                this.RightHandSide = Method.CreateConstant(RightHandSide);
            else
                this.RightHandSide = (VariableBase)RightHandSide;
            Result = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateLocal("AddLocalResult" + Utilities.Reflection.Emit.BaseClasses.MethodBase.ObjectCounter.ToString(),
                this.LeftHandSide.DataType);
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

        #endregion

        #region Functions

        public override void Setup()
        {
            ILGenerator Generator = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator;
            if (LeftHandSide is FieldBuilder || LeftHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            LeftHandSide.Load(Generator);
            if (RightHandSide is FieldBuilder || RightHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            RightHandSide.Load(Generator);
            if (LeftHandSide.DataType != RightHandSide.DataType)
            {
                if (ConversionOpCodes.ContainsKey(LeftHandSide.DataType))
                {
                    Generator.Emit(ConversionOpCodes[LeftHandSide.DataType]);
                }
            }
            Generator.Emit(OpCodes.Add);
            Result.Save(Generator);
        }

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(Result).Append("=").Append(LeftHandSide).Append("+").Append(RightHandSide).Append(";\n");
            return Output.ToString();
        }

        #endregion
    }
}