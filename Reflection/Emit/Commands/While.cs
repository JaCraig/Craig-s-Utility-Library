/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.Reflection.Emit.Enums;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// While command
    /// </summary>
    public class While : CommandBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        public While(Comparison ComparisonType, VariableBase LeftHandSide, VariableBase RightHandSide)
            : base()
        {
            ILGenerator Generator = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator;
            this.StartWhileLabel = Generator.DefineLabel();
            this.EndWhileLabel = Generator.DefineLabel();
            if (LeftHandSide != null)
                this.LeftHandSide = LeftHandSide;
            else
                this.LeftHandSide = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateConstant(null);
            if (RightHandSide != null)
                this.RightHandSide = RightHandSide;
            else
                this.RightHandSide = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateConstant(null);
            this.ComparisonType = ComparisonType;
        }

        #endregion

        #region Properties

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
        public virtual VariableBase LeftHandSide { get; set; }

        /// <summary>
        /// Right hand side of the comparison
        /// </summary>
        public virtual VariableBase RightHandSide { get; set; }

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
            Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.EndWhile(this);
        }

        public override void Setup()
        {
            ILGenerator Generator = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator;
            Generator.MarkLabel(StartWhileLabel);
            if (LeftHandSide is FieldBuilder || LeftHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            LeftHandSide.Load(Generator);
            if (RightHandSide is FieldBuilder || RightHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            RightHandSide.Load(Generator);
            if (ComparisonType == Comparison.Equal)
            {
                Generator.Emit(OpCodes.Ceq);
                Generator.Emit(OpCodes.Ldc_I4_0);
            }
            Generator.Emit(ComparisonOpCodes[ComparisonType], EndWhileLabel);
        }

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append("while(").Append(LeftHandSide)
                .Append(ComparisonTextEquivalent[ComparisonType]).Append(RightHandSide)
                .Append(")\n{\n");
            return Output.ToString();
        }

        #endregion
    }
}