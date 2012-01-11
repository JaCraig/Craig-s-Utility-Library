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
using System.Reflection.Emit;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Commands;
using Utilities.Reflection.Emit.Enums;
#endregion

namespace Utilities.Reflection.Emit.Interfaces
{
    /// <summary>
    /// Interface for methods
    /// </summary>
    public interface IMethodBuilder
    {
        #region Functions

        /// <summary>
        /// Sets the method as the current method
        /// </summary>
        void SetCurrentMethod();

        /// <summary>
        /// Defines a local variable
        /// </summary>
        /// <param name="Name">Name of the local variable</param>
        /// <param name="LocalType">The Type of the local variable</param>
        /// <returns>The LocalBuilder associated with the variable</returns>
        VariableBase CreateLocal(string Name, Type LocalType);

        /// <summary>
        /// Constant value
        /// </summary>
        /// <param name="Value">Value of the constant</param>
        /// <returns>The ConstantBuilder associated with the variable</returns>
        VariableBase CreateConstant(object Value);

        /// <summary>
        /// Creates new object
        /// </summary>
        /// <param name="Constructor">Constructor</param>
        /// <param name="Variables">Variables to send to the constructor</param>
        VariableBase NewObj(ConstructorInfo Constructor, object[] Variables=null);

        /// <summary>
        /// Creates new object
        /// </summary>
        /// <param name="ObjectType">object type</param>
        /// <param name="Variables">Variables to send to the constructor</param>
        VariableBase NewObj(Type ObjectType, object[] Variables=null);

        /// <summary>
        /// Assigns the value to the left hand side variable
        /// </summary>
        /// <param name="LeftHandSide">Left hand side variable</param>
        /// <param name="Value">Value to store (may be constant or VariableBase object)</param>
        void Assign(VariableBase LeftHandSide, object Value);

        /// <summary>
        /// Returns a specified value
        /// </summary>
        /// <param name="ReturnValue">Variable to return</param>
        void Return(object ReturnValue);

        /// <summary>
        /// Returns from the method (used if void is the return type)
        /// </summary>
        void Return();

        /// <summary>
        /// Calls a function on an object
        /// </summary>
        /// <param name="ObjectCallingOn">Object calling on</param>
        /// <param name="MethodCalling">Method calling</param>
        /// <param name="Parameters">Parameters sending</param>
        /// <returns>The return value</returns>
        VariableBase Call(VariableBase ObjectCallingOn, MethodInfo MethodCalling, object[] Parameters);

        /// <summary>
        /// Calls a function on an object
        /// </summary>
        /// <param name="ObjectCallingOn">Object calling on</param>
        /// <param name="MethodCalling">Method calling</param>
        /// <param name="Parameters">Parameters sending</param>
        /// <returns>The return value</returns>
        void Call(VariableBase ObjectCallingOn, ConstructorInfo MethodCalling, object[] Parameters);

        /// <summary>
        /// Defines an if statement
        /// </summary>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="LeftHandSide">Left hand side of the if statement</param>
        /// <param name="RightHandSide">Right hand side of the if statement</param>
        /// <returns>The if command</returns>
        If If(VariableBase LeftHandSide, Comparison ComparisonType, VariableBase RightHandSide);

        /// <summary>
        /// Defines the end of an if statement
        /// </summary>
        /// <param name="IfCommand">If command</param>
        void EndIf(If IfCommand);

        /// <summary>
        /// Defines a while statement
        /// </summary>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="LeftHandSide">Left hand side of the while statement</param>
        /// <param name="RightHandSide">Right hand side of the while statement</param>
        /// <returns>The while command</returns>
        While While(VariableBase LeftHandSide, Comparison ComparisonType, VariableBase RightHandSide);

        /// <summary>
        /// Defines the end of a while statement
        /// </summary>
        /// <param name="WhileCommand">While command</param>
        void EndWhile(While WhileCommand);

        /// <summary>
        /// Adds two variables and returns the result
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The result</returns>
        VariableBase Add(object LeftHandSide, object RightHandSide);

        /// <summary>
        /// Subtracts two variables and returns the result
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The result</returns>
        VariableBase Subtract(object LeftHandSide, object RightHandSide);

        /// <summary>
        /// Multiplies two variables and returns the result
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The result</returns>
        VariableBase Multiply(object LeftHandSide, object RightHandSide);

        /// <summary>
        /// Divides two variables and returns the result
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The result</returns>
        VariableBase Divide(object LeftHandSide, object RightHandSide);

        /// <summary>
        /// Mods (%) two variables and returns the result
        /// </summary>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        /// <returns>The result</returns>
        VariableBase Modulo(object LeftHandSide, object RightHandSide);

        /// <summary>
        /// Starts a try block
        /// </summary>
        Utilities.Reflection.Emit.Commands.Try Try();

        /// <summary>
        /// Ends a try block and starts a catch block
        /// </summary>
        /// <param name="ExceptionType">Exception type</param>
        Utilities.Reflection.Emit.Commands.Catch Catch(Type ExceptionType);

        /// <summary>
        /// Ends a try/catch block
        /// </summary>
        void EndTry();

        /// <summary>
        /// Boxes a value
        /// </summary>
        /// <param name="Value">Value to box</param>
        /// <returns>The resulting boxed variable</returns>
        VariableBase Box(object Value);

        /// <summary>
        /// Unboxes a value
        /// </summary>
        /// <param name="Value">Value to unbox</param>
        /// <param name="ValueType">Type to unbox to</param>
        /// <returns>The resulting unboxed variable</returns>
        VariableBase UnBox(VariableBase Value,Type ValueType);

        /// <summary>
        /// Casts an object to another type
        /// </summary>
        /// <param name="Value">Value to cast</param>
        /// <param name="ValueType">Value type to cast to</param>
        /// <returns>The resulting casted value</returns>
        VariableBase Cast(VariableBase Value, Type ValueType);

        /// <summary>
        /// Throws an exception
        /// </summary>
        /// <param name="Exception">Exception to throw</param>
        void Throw(VariableBase Exception);

        #endregion

        #region Properties

        /// <summary>
        /// Method name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return type
        /// </summary>
        Type ReturnType { get; }

        /// <summary>
        /// Parameters
        /// </summary>
        List<ParameterBuilder> Parameters { get; }

        /// <summary>
        /// Attributes for the method
        /// </summary>
        System.Reflection.MethodAttributes Attributes { get; }

        /// <summary>
        /// IL generator for this method
        /// </summary>
        ILGenerator Generator { get; }

        /// <summary>
        /// Returns the this object for this object
        /// </summary>
        VariableBase This { get; }

        #endregion
    }
}