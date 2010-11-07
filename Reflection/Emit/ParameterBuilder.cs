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
using System.Reflection;
using System.Reflection.Emit;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Used to define a parameter
    /// </summary>
    public class ParameterBuilder : IVariable
    {
        #region Constructor
        
        public ParameterBuilder(Type ParameterType,int Number)
        {
            if (Number == 0)
            {
                this.Name = "value";
                this.Number = 1;
                this.ParameterType = ParameterType;
                return;
            }
            this.Name = "Parameter" + Number.ToString();
            this.Number = Number;
            this.ParameterType = ParameterType;
        }

        #endregion

        #region Properties

        public string Name { get; protected set; }

        /// <summary>
        /// Order in the parameter list
        /// </summary>
        public int Number { get; protected set; }

        /// <summary>
        /// Parameter type
        /// </summary>
        public Type ParameterType { get; protected set; }

        #endregion

        #region Functions

        public void Load(ILGenerator Generator)
        {
            Generator.Emit(OpCodes.Ldarg, Number);
        }

        public void Save(ILGenerator Generator)
        {
            Generator.Emit(OpCodes.Starg, Number);
        }

        public string GetDefinition()
        {
            return Reflection.GetTypeName(ParameterType) + " " + Name;
        }

        #endregion

        #region Overridden Function

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}