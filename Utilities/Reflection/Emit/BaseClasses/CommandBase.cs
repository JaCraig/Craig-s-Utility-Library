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
using System.Collections.Generic;
using System.Reflection.Emit;
using Utilities.Reflection.Emit.Enums;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit.BaseClasses
{
    /// <summary>
    /// Command base class
    /// </summary>
    public abstract class CommandBase:ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandBase()
        {
            SetupOpCodes();
        }

        #endregion

        #region Functions

        private void SetupOpCodes()
        {
            if (ConversionOpCodes == null)
            {
                ConversionOpCodes = new Dictionary<Type, OpCode>();
                ConversionOpCodes.Add(typeof(int), OpCodes.Conv_I4);
                ConversionOpCodes.Add(typeof(Int64), OpCodes.Conv_I8);
                ConversionOpCodes.Add(typeof(float), OpCodes.Conv_R4);
                ConversionOpCodes.Add(typeof(double), OpCodes.Conv_R8);
                ConversionOpCodes.Add(typeof(uint), OpCodes.Conv_U4);
                ConversionOpCodes.Add(typeof(UInt64), OpCodes.Conv_U8);
            }
            if (ComparisonOpCodes == null)
            {
                ComparisonOpCodes = new Dictionary<Comparison, OpCode>();
                ComparisonOpCodes.Add(Comparison.Equal, OpCodes.Beq);
                ComparisonOpCodes.Add(Comparison.GreaterThan, OpCodes.Ble);
                ComparisonOpCodes.Add(Comparison.GreaterThenOrEqual, OpCodes.Blt);
                ComparisonOpCodes.Add(Comparison.LessThan, OpCodes.Bge);
                ComparisonOpCodes.Add(Comparison.LessThanOrEqual, OpCodes.Bgt);
                ComparisonOpCodes.Add(Comparison.NotEqual, OpCodes.Beq);
            }
            if (ComparisonTextEquivalent == null)
            {
                ComparisonTextEquivalent = new Dictionary<Comparison, string>();
                ComparisonTextEquivalent.Add(Comparison.Equal, "==");
                ComparisonTextEquivalent.Add(Comparison.GreaterThan, ">");
                ComparisonTextEquivalent.Add(Comparison.GreaterThenOrEqual, ">=");
                ComparisonTextEquivalent.Add(Comparison.LessThan, "<");
                ComparisonTextEquivalent.Add(Comparison.LessThanOrEqual, "<=");
                ComparisonTextEquivalent.Add(Comparison.NotEqual, "!=");
            }
        }

        /// <summary>
        /// Sets up the command
        /// </summary>
        public abstract void Setup();

        #endregion

        #region Properties

        /// <summary>
        /// Used to store conversion opcodes
        /// </summary>
        protected static Dictionary<Type, OpCode> ConversionOpCodes { get; set; }

        /// <summary>
        /// Used to store comparison opcodes
        /// </summary>
        protected static Dictionary<Comparison, OpCode> ComparisonOpCodes { get; set; }

        /// <summary>
        /// Used to store text equivalent of comparison types
        /// </summary>
        protected static Dictionary<Comparison, string> ComparisonTextEquivalent { get; set; }

        /// <summary>
        /// Return value (set to null if not used by the command)
        /// </summary>
        public virtual VariableBase Result { get; set; }

        #endregion
    }
}
