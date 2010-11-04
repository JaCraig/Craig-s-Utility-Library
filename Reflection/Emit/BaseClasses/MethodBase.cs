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
using Utilities.Reflection.Emit.Commands;
using System.Reflection.Emit;
#endregion

namespace Utilities.Reflection.Emit.BaseClasses
{
    /// <summary>
    /// Method base class
    /// </summary>
    public class MethodBase : IMethodBuilder
    {
        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MethodBase()
        {
            Commands = new List<ICommand>();
        }

        #endregion

        #region Functions

        public LocalBuilder CreateLocal(string Name, Type LocalType)
        {
            return new LocalBuilder(this, Name, LocalType);
        }

        public ConstantBuilder CreateConstant(object Value)
        {
            return new ConstantBuilder(Value);
        }

        public IVariable NewObj(ConstructorInfo Constructor, List<IVariable> Variables)
        {
            Commands.Add(new NewObj(Constructor, Variables, Generator));
            return new ObjectBuilder(this, Constructor.DeclaringType);
        }

        public void Save(IVariable LeftHandSide, object Value)
        {
            if (LeftHandSide == null)
                throw new ArgumentNullException("LeftHandSide");
            if (LeftHandSide is FieldBuilder || LeftHandSide is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            if (Value is IVariable)
            {
                ((IVariable)Value).Load(Generator);
            }
            else
            {
                ConstantBuilder Constant = new ConstantBuilder(Value);
                Constant.Load(Generator);
            }
            LeftHandSide.Save(Generator);
        }

        public void Return(object ReturnValue)
        {
            if (this.ReturnType == typeof(void) || this.ReturnType == null)
            {
                Generator.Emit(OpCodes.Ret);
                return;
            }
            if (ReturnValue == null)
            {
                ConstantBuilder Constant = new ConstantBuilder(ReturnValue);
                Constant.Load(Generator);
                Generator.Emit(OpCodes.Ret);
                return;
            }
            if (ReturnValue is FieldBuilder || ReturnValue is IPropertyBuilder)
                Generator.Emit(OpCodes.Ldarg_0);
            if (ReturnValue is IVariable)
            {
                ((IVariable)ReturnValue).Load(Generator);
            }
            else
            {
                ConstantBuilder Constant = new ConstantBuilder(ReturnValue);
                Constant.Load(Generator);
            }
            Generator.Emit(OpCodes.Ret);
        }

        #endregion

        #region Properties

        public string Name { get; protected set; }
        public Type ReturnType { get; protected set; }
        public List<Type> ParameterTypes { get; protected set; }
        public System.Reflection.MethodAttributes Attributes { get; protected set; }
        public System.Reflection.Emit.ILGenerator Generator { get; protected set; }

        protected List<ICommand> Commands { get; set; }

        #endregion
    }
}