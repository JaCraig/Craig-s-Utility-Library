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
using System.Reflection.Emit;
using Utilities.Reflection.Emit.BaseClasses;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Command for creating a new object
    /// </summary>
    public class NewObj : ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Method">Method created within</param>
        /// <param name="Constructor">Constructor to use</param>
        /// <param name="Variables">Variables sent to the constructor</param>
        /// <param name="ObjectCount">Object count</param>
        /// <param name="Generator">IL generator</param>
        public NewObj(IMethodBuilder Method, ConstructorInfo Constructor, object[] Parameters,
            int ObjectCount, ILGenerator Generator)
        {
            this.Constructor = Constructor;
            if (Parameters != null)
            {
                this.Parameters = new object[Parameters.Length];
                for (int x = 0; x < Parameters.Length; ++x)
                {
                    if (Parameters[x] is VariableBase)
                        this.Parameters[x] = Parameters[x];
                    else
                        this.Parameters[x] = Method.CreateConstant(Parameters[x]);
                }
                foreach (VariableBase Parameter in this.Parameters)
                {
                    if (Parameter is FieldBuilder || Parameter is IPropertyBuilder)
                        Method.Generator.Emit(OpCodes.Ldarg_0);
                    Parameter.Load(Method.Generator);
                }
            }
            Generator.Emit(OpCodes.Newobj, Constructor);
            TempObject = Method.CreateLocal("ObjLocal" + ObjectCount.ToString(), Constructor.DeclaringType);
            TempObject.Save(Method.Generator);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Constructor used
        /// </summary>
        protected virtual ConstructorInfo Constructor { get; set; }

        /// <summary>
        /// Method object is created within
        /// </summary>
        protected virtual IMethodBuilder Method { get; set; }

        /// <summary>
        /// Variables sent to the Constructor
        /// </summary>
        protected virtual object[] Parameters { get; set; }

        /// <summary>
        /// Temp object
        /// </summary>
        protected virtual VariableBase TempObject { get; set; }

        /// <summary>
        /// Object count
        /// </summary>
        protected virtual int ObjectCount { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the object that temporarily stores the new object
        /// </summary>
        /// <returns>The new object</returns>
        public virtual VariableBase GetObject()
        {
            return TempObject;
        }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(TempObject).Append(" = new ")
                .Append(Reflection.GetTypeName(Constructor.DeclaringType))
                .Append("(");
            string Seperator = "";
            if (Parameters != null)
            {
                foreach (VariableBase Variable in Parameters)
                {
                    Output.Append(Seperator).Append(Variable.ToString());
                    Seperator = ",";
                }
            }
            Output.Append(");\n");
            return Output.ToString();
        }

        #endregion
    }
}