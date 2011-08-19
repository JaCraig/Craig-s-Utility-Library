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
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Interfaces;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Command for creating a new object
    /// </summary>
    public class NewObj : CommandBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Constructor">Constructor to use</param>
        /// <param name="Parameters">Variables sent to the constructor</param>
        public NewObj(ConstructorInfo Constructor, object[] Parameters)
            : base()
        {
            this.Constructor = Constructor;
            if (Parameters != null)
            {
                this.Parameters = new VariableBase[Parameters.Length];
                for (int x = 0; x < Parameters.Length; ++x)
                {
                    if (Parameters[x] is VariableBase)
                        this.Parameters[x] = (VariableBase)Parameters[x];
                    else
                        this.Parameters[x] = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateConstant(Parameters[x]);
                }
            }
            Result = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.CreateLocal("ObjLocal" + Utilities.Reflection.Emit.BaseClasses.MethodBase.ObjectCounter.ToString(),
                Constructor.DeclaringType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Constructor used
        /// </summary>
        protected virtual ConstructorInfo Constructor { get; set; }

        /// <summary>
        /// Variables sent to the Constructor
        /// </summary>
        protected virtual VariableBase[] Parameters { get; set; }

        #endregion

        #region Functions

        public override void Setup()
        {
            ILGenerator Generator = Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator;
            if (Parameters != null)
            {
                foreach (VariableBase Parameter in Parameters)
                {
                    if (Parameter is FieldBuilder || Parameter is IPropertyBuilder)
                        Generator.Emit(OpCodes.Ldarg_0);
                    Parameter.Load(Generator);
                }
            }
            Generator.Emit(OpCodes.Newobj, Constructor);
            Result.Save(Generator);
        }

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(Result).Append(" = new ")
                .Append(Constructor.DeclaringType.GetName())
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