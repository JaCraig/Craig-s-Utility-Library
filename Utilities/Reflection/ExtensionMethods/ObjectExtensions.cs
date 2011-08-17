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
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
#endregion

namespace Utilities.Reflection.ExtensionMethods
{
    /// <summary>
    /// Object extensions
    /// </summary>
    public static class ObjectExtensions
    {
        #region Functions

        #region CallMethod

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType CallMethod<ReturnType>(this object Object, string MethodName, params object[] InputVariables)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(MethodName))
                throw new ArgumentNullException("MethodName");
            Type ObjectType = Object.GetType();
            MethodInfo Method = null;
            if (InputVariables != null)
            {
                Type[] MethodInputTypes = new Type[InputVariables.Length];
                for (int x = 0; x < InputVariables.Length; ++x)
                    MethodInputTypes[x] = InputVariables[x].GetType();
                Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            }
            Method = ObjectType.GetMethod(MethodName);
            if (Method == null)
                throw new NullReferenceException("Could not find method " + MethodName + " with the appropriate input variables.");
            return (ReturnType)Method.Invoke(Object, InputVariables);
        }

        #endregion

        #region IsIEnumerable

        /// <summary>
        /// Simple function to determine if an item is an IEnumerable
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsIEnumerable(this Type ObjectType)
        {
            return ObjectType.IsOfType(typeof(IEnumerable));
        }

        #endregion

        #region IsOfType

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this object Object, Type Type)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Type == null)
                throw new ArgumentNullException("Type");
            return Object.GetType().IsOfType(Type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this Type ObjectType, Type Type)
        {
            if (ObjectType == null)
                return false;
            if (Type == null)
                throw new ArgumentNullException("Type");
            if (Type == ObjectType || Type.GetInterface(Type.Name, true) != null)
                return true;
            if (ObjectType.BaseType == null)
                return false;
            return ObjectType.BaseType.IsOfType(Type);
        }

        #endregion

        #endregion
    }
}