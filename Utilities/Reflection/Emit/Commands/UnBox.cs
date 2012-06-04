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
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Unboxes an object
    /// </summary>
    public class UnBox : CommandBase
    {
        #region Constructor

        /// <summary>
        /// Unboxes a value to a specified type
        /// </summary>
        /// <param name="Value">Value to unbox</param>
        /// <param name="ValueType">Value type</param>
        public UnBox(VariableBase Value, Type ValueType)
            : base()
        {
            this.Value = Value;
            this.ValueType = ValueType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value to unbox
        /// </summary>
        public virtual VariableBase Value { get; set; }

        /// <summary>
        /// Value type to unbox to
        /// </summary>
        public virtual Type ValueType { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the unbox statement
        /// </summary>
        public override void Setup()
        {
            if (!ValueType.IsValueType)
                throw new ArgumentException("ValueType is not a value type, unbox operations convert reference types to value types");
            Result = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateLocal("UnBoxResult" + Value.Name + Utilities.Reflection.Emit.BaseClasses.MethodBase.ObjectCounter.ToString(), ValueType);
            ILGenerator Generator = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator;
            if (Value is FieldBuilder || Value is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            Value.Load(Generator);
            Generator.Emit(OpCodes.Unbox_Any, ValueType);
            Result.Save(Generator);
        }

        /// <summary>
        /// Unbox statement as a string
        /// </summary>
        /// <returns>The unbox statement as a string</returns>
        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(Result).Append("=(").Append(ValueType.GetName())
                .Append(")").Append(Value).Append(";\n");
            return Output.ToString();
        }

        #endregion
    }
}