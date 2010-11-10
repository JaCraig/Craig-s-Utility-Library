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

        public virtual IVariable CreateLocal(string Name, Type LocalType)
        {
            DefineLocal TempCommand = new DefineLocal(this, Name, LocalType);
            Commands.Add(TempCommand);
            return TempCommand.GetObject();
        }

        public virtual IVariable CreateConstant(object Value)
        {
            return new ConstantBuilder(Value);
        }

        public virtual IVariable NewObj(ConstructorInfo Constructor, List<IVariable> Variables)
        {
            NewObj TempCommand = new NewObj(this, Constructor, Variables, ObjectCounter, Generator);
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.GetObject();
        }

        public IVariable Call(IVariable ObjectCallingOn, MethodInfo MethodCalling, List<IVariable> Parameters)
        {
            Call TempCommand = new Call(this, ObjectCallingOn, MethodCalling, Parameters);
            Commands.Add(TempCommand);
            return TempCommand.GetObject();
        }

        public virtual void Assign(IVariable LeftHandSide, object Value)
        {
            Commands.Add(new Assign(LeftHandSide, Value, Generator));
        }

        public virtual void Return(object ReturnValue)
        {
            Commands.Add(new Return(ReturnType, ReturnValue, Generator));
        }

        public virtual void Return()
        {
            Commands.Add(new Return(ReturnType, null, Generator));
        }

        public If If(Enums.Comparison ComparisonType, IVariable LeftHandSide, IVariable RightHandSide)
        {
            Utilities.Reflection.Emit.Commands.If TempCommand = new If(this, ComparisonType, LeftHandSide, RightHandSide);
            Commands.Add(TempCommand);
            return TempCommand;
        }

        public void EndIf(If IfCommand)
        {
            EndIf TempCommand = new EndIf(this, IfCommand);
            Commands.Add(TempCommand);
        }

        public While While(Enums.Comparison ComparisonType, IVariable LeftHandSide, IVariable RightHandSide)
        {
            While TempCommand = new While(this, ComparisonType, LeftHandSide, RightHandSide);
            Commands.Add(TempCommand);
            return TempCommand;
        }

        public void EndWhile(While WhileCommand)
        {
            EndWhile TempCommand = new EndWhile(this, WhileCommand);
            Commands.Add(TempCommand);
        }

        #endregion

        #region Properties

        public virtual string Name { get; protected set; }
        public virtual Type ReturnType { get; protected set; }
        public virtual List<ParameterBuilder> Parameters { get; protected set; }
        public virtual System.Reflection.MethodAttributes Attributes { get; protected set; }
        public virtual System.Reflection.Emit.ILGenerator Generator { get; protected set; }

        public virtual List<ICommand> Commands { get; protected set; }
        protected static int ObjectCounter { get; set; }

        #endregion
    }
}