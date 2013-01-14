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
    /// Used to define a parameter
    /// </summary>
    public class ParameterBuilder : VariableBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ParameterType">Parameter type</param>
        /// <param name="Number">Position in parameter order</param>
        public ParameterBuilder(Type ParameterType, int Number)
            : base()
        {
            if (Number == -1)
            {
                this.Name = "value";
                this.Number = 1;
                this.DataType = ParameterType;
                return;
            }
            else if (Number == 0)
            {
                this.Name = "this";
                this.Number = 0;
                this.DataType = null;
                return;
            }
            this.Name = "Parameter" + Number.ToString();
            this.Number = Number;
            this.DataType = ParameterType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order in the parameter list
        /// </summary>
        public virtual int Number { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Loads from the parameter
        /// </summary>
        /// <param name="Generator">IL Generator</param>
        public override void Load(ILGenerator Generator)
        {
            Generator.Emit(OpCodes.Ldarg, Number);
        }

        /// <summary>
        /// Saves to the parameter
        /// </summary>
        /// <param name="Generator">IL Generator</param>
        public override void Save(ILGenerator Generator)
        {
            Generator.Emit(OpCodes.Starg, Number);
        }

        #endregion

        #region Overridden Function

        /// <summary>
        /// Outputs the parameter as a string
        /// </summary>
        /// <returns>The parameter</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Operator Functions

        /// <summary>
        /// Increments by one
        /// </summary>
        /// <param name="Left">Parameter to increment</param>
        /// <returns>The parameter</returns>
        public static ParameterBuilder operator ++(ParameterBuilder Left)
        {
            if (Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod == null)
                throw new InvalidOperationException("Unsure which method is the current method");
            Left.Assign(Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Add(Left, 1));
            return Left;
        }

        /// <summary>
        /// Decrements by one
        /// </summary>
        /// <param name="Left">Parameter to decrement</param>
        /// <returns>The parameter</returns>
        public static ParameterBuilder operator --(ParameterBuilder Left)
        {
            if (Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod == null)
                throw new InvalidOperationException("Unsure which method is the current method");
            Left.Assign(Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Subtract(Left, 1));
            return Left;
        }

        #endregion
    }
}