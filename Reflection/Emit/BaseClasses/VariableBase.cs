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
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.Emit.BaseClasses
{
    /// <summary>
    /// Variable base class
    /// </summary>
    public class VariableBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public VariableBase()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Variable name
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Variable data type
        /// </summary>
        public virtual Type DataType { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Assigns the value to the variable
        /// </summary>
        /// <param name="Value">Value to assign</param>
        public virtual void Assign(object Value)
        {
            MethodBase.CurrentMethod.Assign(this, Value);
        }

        /// <summary>
        /// Loads the variable onto the stack
        /// </summary>
        /// <param name="Generator">IL Generator</param>
        public virtual void Load(System.Reflection.Emit.ILGenerator Generator)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the top item from the stack onto the variable
        /// </summary>
        /// <param name="Generator">IL Generator</param>
        public virtual void Save(System.Reflection.Emit.ILGenerator Generator)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the definition of the variable
        /// </summary>
        /// <returns></returns>
        public virtual string GetDefinition()
        {
            return Reflection.GetTypeName(DataType) + " " + Name;
        }

        /// <summary>
        /// Calls a method on this variable
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Parameters">Parameters sent in</param>
        /// <returns>Variable returned by the function (if one exists, null otherwise)</returns>
        public virtual VariableBase Call(string MethodName, object[] Parameters=null)
        {
            if (string.IsNullOrEmpty(MethodName))
                throw new ArgumentNullException("MethodName");
            List<Type> ParameterTypes=new List<Type>();
            if (Parameters != null)
            {
                foreach (object Parameter in Parameters)
                {
                    if (Parameter is VariableBase)
                        ParameterTypes.Add(((VariableBase)Parameter).DataType);
                    else
                        ParameterTypes.Add(Parameter.GetType());
                }
            }
            return Call(DataType.GetMethod(MethodName, ParameterTypes.ToArray()), Parameters);
        }

        /// <summary>
        /// Calls a method on this variable
        /// </summary>
        /// <param name="Method">Method</param>
        /// <param name="Parameters">Parameters sent in</param>
        /// <returns>Variable returned by the function (if one exists, null otherwise)</returns>
        public virtual VariableBase Call(MethodBuilder Method, object[] Parameters = null)
        {
            if (Method==null)
                throw new ArgumentNullException("Method");
            return Call(Method.Builder, Parameters);
        }

        /// <summary>
        /// Calls a method on this variable
        /// </summary>
        /// <param name="Method">Method</param>
        /// <param name="Parameters">Parameters sent in</param>
        /// <returns>Variable returned by the function (if one exists, null otherwise)</returns>
        public virtual VariableBase Call(System.Reflection.Emit.MethodBuilder Method, object[] Parameters = null)
        {
            if (Method == null)
                throw new ArgumentNullException("Method");
            return MethodBase.CurrentMethod.Call(this, Method, Parameters);
        }

        /// <summary>
        /// Calls a method on this variable
        /// </summary>
        /// <param name="Method">Method</param>
        /// <param name="Parameters">Parameters sent in</param>
        /// <returns>Variable returned by the function (if one exists, null otherwise)</returns>
        public virtual VariableBase Call(System.Reflection.MethodInfo Method, object[] Parameters = null)
        {
            if (Method == null)
                throw new ArgumentNullException("Method");
            return MethodBase.CurrentMethod.Call(this, Method, Parameters);
        }

        /// <summary>
        /// Calls a method on this variable
        /// </summary>
        /// <param name="Method">Method</param>
        /// <param name="Parameters">Parameters sent in</param>
        /// <returns>Variable returned by the function (if one exists, null otherwise)</returns>
        public virtual void Call(System.Reflection.ConstructorInfo Method, object[] Parameters = null)
        {
            if (Method == null)
                throw new ArgumentNullException("Method");
            MethodBase.CurrentMethod.Call(this, Method, Parameters);
        }

        /// <summary>
        /// Calls a method on this variable
        /// </summary>
        /// <param name="Method">Method</param>
        /// <param name="Parameters">Parameters sent in</param>
        /// <returns>Variable returned by the function (if one exists, null otherwise)</returns>
        public virtual VariableBase Call(IMethodBuilder Method, object[] Parameters = null)
        {
            if (Method == null)
                throw new ArgumentNullException("Method");
            return Call((MethodBuilder)Method, Parameters);
        }

        #endregion

        #region Operator Functions

        public static VariableBase operator +(VariableBase Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Add(Left, Right);
        }

        public static VariableBase operator -(VariableBase Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Subtract(Left, Right);
        }

        public static VariableBase operator *(VariableBase Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Multiply(Left, Right);
        }

        public static VariableBase operator /(VariableBase Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Divide(Left, Right);
        }

        public static VariableBase operator %(VariableBase Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Modulo(Left, Right);
        }

        public static VariableBase operator +(VariableBase Left, object Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Add(Left, Right);
        }

        public static VariableBase operator -(VariableBase Left, object Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Subtract(Left, Right);
        }

        public static VariableBase operator *(VariableBase Left, object Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Multiply(Left, Right);
        }

        public static VariableBase operator /(VariableBase Left, object Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Divide(Left, Right);
        }

        public static VariableBase operator %(VariableBase Left, object Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Modulo(Left, Right);
        }

        public static VariableBase operator +(object Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Add(Left, Right);
        }

        public static VariableBase operator -(object Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Subtract(Left, Right);
        }

        public static VariableBase operator *(object Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Multiply(Left, Right);
        }

        public static VariableBase operator /(object Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Divide(Left, Right);
        }

        public static VariableBase operator %(object Left, VariableBase Right)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            return MethodBase.CurrentMethod.Modulo(Left, Right);
        }

        public static VariableBase operator ++(VariableBase Left)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            Left.Assign(MethodBase.CurrentMethod.Add(Left, 1));
            return Left;
        }

        public static VariableBase operator --(VariableBase Left)
        {
            if (MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            Left.Assign(MethodBase.CurrentMethod.Subtract(Left, 1));
            return Left;
        }

        #endregion
    }
}