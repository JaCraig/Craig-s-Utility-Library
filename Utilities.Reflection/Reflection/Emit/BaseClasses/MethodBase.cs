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
using System.Reflection;
using Utilities.Reflection.Emit.Commands;
using Utilities.Reflection.Emit.Interfaces;

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
            SetCurrentMethod();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Creates a local variable
        /// </summary>
        /// <param name="Name">Name of the local</param>
        /// <param name="LocalType">Object type</param>
        /// <returns>The variable</returns>
        public virtual VariableBase CreateLocal(string Name, Type LocalType)
        {
            SetCurrentMethod();
            DefineLocal TempCommand = new DefineLocal(Name, LocalType);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            return TempCommand.Result;
        }

        /// <summary>
        /// Creates a constant
        /// </summary>
        /// <param name="Value">Constant value</param>
        /// <returns>The constant</returns>
        public virtual VariableBase CreateConstant(object Value)
        {
            SetCurrentMethod();
            return new ConstantBuilder(Value);
        }

        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="Constructor">Constructor to use</param>
        /// <param name="Variables">Variables to use</param>
        /// <returns>The new object</returns>
        public virtual VariableBase NewObj(ConstructorInfo Constructor, object[] Variables = null)
        {
            SetCurrentMethod();
            NewObj TempCommand = new NewObj(Constructor, Variables);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Variables">Variables to use</param>
        /// <returns>The new object</returns>
        public virtual VariableBase NewObj(Type ObjectType, object[] Variables = null)
        {
            SetCurrentMethod();
            List<Type> VariableTypes = new List<Type>();
            if (Variables != null)
            {
                foreach (object Variable in Variables)
                {
                    if (Variable is VariableBase)
                        VariableTypes.Add(((VariableBase)Variable).DataType);
                    else
                        VariableTypes.Add(Variable.GetType());
                }
            }
            ConstructorInfo Constructor = ObjectType.GetConstructor(VariableTypes.ToArray());
            return NewObj(Constructor, Variables);
        }

        /// <summary>
        /// Calls a method
        /// </summary>
        /// <param name="ObjectCallingOn">Object to call the method on</param>
        /// <param name="MethodCalling">Method to call</param>
        /// <param name="Parameters">Parameters to use</param>
        /// <returns>The result of the method call</returns>
        public virtual VariableBase Call(VariableBase ObjectCallingOn, MethodInfo MethodCalling, object[] Parameters)
        {
            SetCurrentMethod();
            Call TempCommand = new Call(this, ObjectCallingOn, MethodCalling, Parameters);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Calls a constructor
        /// </summary>
        /// <param name="ObjectCallingOn">Object to call the constructor on</param>
        /// <param name="MethodCalling">Constructor to call</param>
        /// <param name="Parameters">Parameters to use</param>
        public virtual void Call(VariableBase ObjectCallingOn, ConstructorInfo MethodCalling, object[] Parameters)
        {
            SetCurrentMethod();
            Call TempCommand = new Call(this, ObjectCallingOn, MethodCalling, Parameters);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
        }

        /// <summary>
        /// Assigns a value to a variable
        /// </summary>
        /// <param name="LeftHandSide">Variable to assign to</param>
        /// <param name="Value">Value to assign</param>
        public virtual void Assign(VariableBase LeftHandSide, object Value)
        {
            SetCurrentMethod();
            Assign TempCommand = new Assign(LeftHandSide, Value);
            TempCommand.Setup();
            Commands.Add(TempCommand);
        }

        /// <summary>
        /// Returns a value back from the method
        /// </summary>
        /// <param name="ReturnValue">Value to return</param>
        public virtual void Return(object ReturnValue)
        {
            Return TempCommand = new Return(ReturnType, ReturnValue);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            CurrentMethod = null;
        }

        /// <summary>
        /// Returns from the method
        /// </summary>
        public virtual void Return()
        {
            Return(null);
        }

        /// <summary>
        /// Creates an if statement
        /// </summary>
        /// <param name="LeftHandSide">Left hand side variable</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="RightHandSide">Right hand side variable</param>
        /// <returns>The if object</returns>
        public virtual If If(VariableBase LeftHandSide, Enums.Comparison ComparisonType, VariableBase RightHandSide)
        {
            SetCurrentMethod();
            Utilities.Reflection.Emit.Commands.If TempCommand = new If(ComparisonType, LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            return TempCommand;
        }

        /// <summary>
        /// Ends an if statement
        /// </summary>
        /// <param name="IfCommand">If command to end</param>
        public virtual void EndIf(If IfCommand)
        {
            SetCurrentMethod();
            EndIf TempCommand = new EndIf(IfCommand);
            TempCommand.Setup();
            Commands.Add(TempCommand);
        }

        /// <summary>
        /// Creates a while statement
        /// </summary>
        /// <param name="LeftHandSide">Left hand side variable</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="RightHandSide">Right hand side variable</param>
        /// <returns>The while object</returns>
        public virtual While While(VariableBase LeftHandSide, Enums.Comparison ComparisonType, VariableBase RightHandSide)
        {
            SetCurrentMethod();
            While TempCommand = new While(ComparisonType, LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            return TempCommand;
        }

        /// <summary>
        /// Ends a while statement
        /// </summary>
        /// <param name="WhileCommand">While statement to end</param>
        public virtual void EndWhile(While WhileCommand)
        {
            SetCurrentMethod();
            EndWhile TempCommand = new EndWhile(WhileCommand);
            TempCommand.Setup();
            Commands.Add(TempCommand);
        }

        /// <summary>
        /// Adds two objects
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Add(object LeftHandSide, object RightHandSide)
        {
            SetCurrentMethod();
            Add TempCommand = new Add(LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Subtracts two objects
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Subtract(object LeftHandSide, object RightHandSide)
        {
            SetCurrentMethod();
            Subtract TempCommand = new Subtract(LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Multiply two objects
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Multiply(object LeftHandSide, object RightHandSide)
        {
            SetCurrentMethod();
            Multiply TempCommand = new Multiply(LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Divides two objects
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Divide(object LeftHandSide, object RightHandSide)
        {
            SetCurrentMethod();
            Divide TempCommand = new Divide(LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Modulo operator
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Modulo(object LeftHandSide, object RightHandSide)
        {
            SetCurrentMethod();
            Modulo TempCommand = new Modulo(LeftHandSide, RightHandSide);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Starts a try block
        /// </summary>
        /// <returns>The resulting try block</returns>
        public virtual Utilities.Reflection.Emit.Commands.Try Try()
        {
            Utilities.Reflection.Emit.Commands.Try TempCommand = new Utilities.Reflection.Emit.Commands.Try();
            TempCommand.Setup();
            Commands.Add(TempCommand);
            return TempCommand;
        }

        /// <summary>
        /// Starts a catch block
        /// </summary>
        /// <param name="ExceptionType">Exception type to catch</param>
        /// <returns>The resulting catch block</returns>
        public virtual Utilities.Reflection.Emit.Commands.Catch Catch(Type ExceptionType)
        {
            Utilities.Reflection.Emit.Commands.Catch TempCommand = new Utilities.Reflection.Emit.Commands.Catch(ExceptionType);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            TempCommand.Exception = CreateLocal("ExceptionLocal" + ObjectCounter.ToString(), ExceptionType);
            TempCommand.Exception.Save(Generator);
            ++ObjectCounter;
            return TempCommand;
        }

        /// <summary>
        /// Ends a try block
        /// </summary>
        public virtual void EndTry()
        {
            EndTry TempCommand = new Emit.Commands.EndTry();
            TempCommand.Setup();
            Commands.Add(TempCommand);
        }

        /// <summary>
        /// Boxes an object
        /// </summary>
        /// <param name="Value">Variable to box</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Box(object Value)
        {
            Box TempCommand = new Box(Value);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Unboxes an object
        /// </summary>
        /// <param name="Value">Value to unbox</param>
        /// <param name="ValueType">Type to unbox to</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase UnBox(VariableBase Value, Type ValueType)
        {
            UnBox TempCommand = new UnBox(Value, ValueType);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Casts an object as a specific type
        /// </summary>
        /// <param name="Value">Value to cast</param>
        /// <param name="ValueType">Type to cast to</param>
        /// <returns>The resulting variable</returns>
        public virtual VariableBase Cast(VariableBase Value, Type ValueType)
        {
            Cast TempCommand = new Cast(Value, ValueType);
            TempCommand.Setup();
            Commands.Add(TempCommand);
            ++ObjectCounter;
            return TempCommand.Result;
        }

        /// <summary>
        /// Throws an exception
        /// </summary>
        /// <param name="Exception">Exception to throw</param>
        public virtual void Throw(VariableBase Exception)
        {
            Throw TempCommand = new Throw(Exception);
            TempCommand.Setup();
            Commands.Add(TempCommand);
        }

        /// <summary>
        /// Sets the current method to this
        /// </summary>
        public virtual void SetCurrentMethod()
        {
            CurrentMethod = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the method
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Return type
        /// </summary>
        public virtual Type ReturnType { get; protected set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public virtual List<ParameterBuilder> Parameters { get; protected set; }

        /// <summary>
        /// Attributes for the method
        /// </summary>
        public virtual System.Reflection.MethodAttributes Attributes { get; protected set; }

        /// <summary>
        /// IL Generator for the method
        /// </summary>
        public virtual System.Reflection.Emit.ILGenerator Generator { get; protected set; }

        /// <summary>
        /// Current method
        /// </summary>
        public static MethodBase CurrentMethod { get; protected set; }

        /// <summary>
        /// Commands used in the method
        /// </summary>
        public virtual List<ICommand> Commands { get; protected set; }

        /// <summary>
        /// Object counter
        /// </summary>
        public static int ObjectCounter { get; set; }

        /// <summary>
        /// The this object
        /// </summary>
        public virtual VariableBase This
        {
            get
            {
                return Parameters[0];
            }
        }

        #endregion
    }
}