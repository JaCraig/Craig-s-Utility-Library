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
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Call command
    /// </summary>
    public class Call : CommandBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ObjectCallingOn">Object calling on</param>
        /// <param name="Method">Method builder</param>
        /// <param name="MethodCalling">Method calling on the object</param>
        /// <param name="Parameters">List of parameters to send in</param>
        public Call(IMethodBuilder Method, VariableBase ObjectCallingOn, MethodInfo MethodCalling, object[] Parameters)
            : base()
        {
            this.ObjectCallingOn = ObjectCallingOn;
            this.MethodCalling = MethodCalling;
            this.MethodCallingFrom = Method;
            if (MethodCalling.ReturnType != null && MethodCalling.ReturnType != typeof(void))
            {
                Result = Method.CreateLocal(MethodCalling.Name + "ReturnObject"+Utilities.Reflection.Emit.BaseClasses.MethodBase.ObjectCounter.ToString(), MethodCalling.ReturnType);
            }
            if (Parameters != null)
            {
                this.Parameters = new VariableBase[Parameters.Length];
                for (int x = 0; x < Parameters.Length; ++x)
                {
                    if (Parameters[x] is VariableBase)
                        this.Parameters[x] = (VariableBase)Parameters[x];
                    else
                        this.Parameters[x] = MethodCallingFrom.CreateConstant(Parameters[x]);
                }
            }
            else
            {
                this.Parameters = null;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ObjectCallingOn">Object calling on</param>
        /// <param name="Method">Method builder</param>
        /// <param name="MethodCalling">Method calling on the object</param>
        /// <param name="Parameters">List of parameters to send in</param>
        public Call(IMethodBuilder Method, VariableBase ObjectCallingOn, ConstructorInfo MethodCalling, object[] Parameters)
            : base()
        {
            this.ObjectCallingOn = ObjectCallingOn;
            this.ConstructorCalling = MethodCalling;
            this.MethodCallingFrom = Method;
            if (Parameters != null)
            {
                this.Parameters = new VariableBase[Parameters.Length];
                for (int x = 0; x < Parameters.Length; ++x)
                {
                    if (Parameters[x] is VariableBase)
                        this.Parameters[x] = (VariableBase)Parameters[x];
                    else
                        this.Parameters[x] = MethodCallingFrom.CreateConstant(Parameters[x]);
                }
            }
            else
            {
                this.Parameters = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Object calling on
        /// </summary>
        protected virtual VariableBase ObjectCallingOn { get; set; }

        /// <summary>
        /// Method calling
        /// </summary>
        protected virtual MethodInfo MethodCalling { get; set; }

        /// <summary>
        /// Method calling
        /// </summary>
        protected virtual ConstructorInfo ConstructorCalling { get; set; }

        /// <summary>
        /// Parameters sent in
        /// </summary>
        protected virtual VariableBase[] Parameters { get; set; }

        /// <summary>
        /// Method calling from
        /// </summary>
        protected virtual IMethodBuilder MethodCallingFrom { get; set; }

        #endregion

        #region Functions

        public override void Setup()
        {
            if (ObjectCallingOn != null)
            {
                if (ObjectCallingOn is FieldBuilder || ObjectCallingOn is IPropertyBuilder)
                    MethodCallingFrom.Generator.Emit(OpCodes.Ldarg_0);
                ObjectCallingOn.Load(MethodCallingFrom.Generator);
            }
            if (Parameters != null)
            {
                foreach (VariableBase Parameter in this.Parameters)
                {
                    if (Parameter is FieldBuilder || Parameter is IPropertyBuilder)
                        MethodCallingFrom.Generator.Emit(OpCodes.Ldarg_0);
                    Parameter.Load(MethodCallingFrom.Generator);
                }
            }
            OpCode OpCodeUsing = OpCodes.Callvirt;
            if (MethodCalling != null)
            {
                if (!MethodCalling.IsVirtual||
                    (ObjectCallingOn.Name=="this"&& MethodCalling.Name==MethodCallingFrom.Name))
                    OpCodeUsing = OpCodes.Call;
                MethodCallingFrom.Generator.EmitCall(OpCodeUsing, MethodCalling, null);
                if (MethodCalling.ReturnType != null && MethodCalling.ReturnType != typeof(void))
                {
                    Result.Save(MethodCallingFrom.Generator);
                }
            }
            else if (ConstructorCalling != null)
            {
                if (!ConstructorCalling.IsVirtual||
                    (ObjectCallingOn.Name == "this" && MethodCalling.Name == MethodCallingFrom.Name))
                    OpCodeUsing = OpCodes.Call;
                MethodCallingFrom.Generator.Emit(OpCodeUsing, ConstructorCalling);
            }
        }

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            if (Result != null)
            {
                Output.Append(Result).Append(" = ");
            }
            if(ObjectCallingOn!=null)
                Output.Append(ObjectCallingOn).Append(".");
            if (ObjectCallingOn.Name == "this" && MethodCallingFrom.Name == MethodCalling.Name)
            {
                Output.Append("base").Append("(");
            }
            else
            {
                Output.Append(MethodCalling.Name).Append("(");
            }
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