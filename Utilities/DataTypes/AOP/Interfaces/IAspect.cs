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
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Interfaces;
#endregion

namespace Utilities.Reflection.AOP.Interfaces
{
    /// <summary>
    /// Aspect interface
    /// </summary>
    public interface IAspect
    {
        #region Functions

        /// <summary>
        /// Used to insert IL code at the beginning of the method
        /// </summary>
        /// <param name="Method">Overridding Method</param>
        /// <param name="BaseType">Base type</param>
        void SetupStartMethod(IMethodBuilder Method, Type BaseType);

        /// <summary>
        /// Used to insert IL code at the end of the method
        /// </summary>
        /// <param name="Method">Overridding Method</param>
        /// <param name="BaseType">Base type</param>
        /// <param name="ReturnValue">Local holder for the value returned by the function</param>
        void SetupEndMethod(IMethodBuilder Method, Type BaseType, VariableBase ReturnValue);

        /// <summary>
        /// Used to insert IL code within the catch portion of the try/catch portion of the method
        /// </summary>
        /// <param name="Method">Overridding Method</param>
        /// <param name="BaseType">Base type</param>
        void SetupExceptionMethod(IMethodBuilder Method, Type BaseType);

        /// <summary>
        /// Used to hook into the object once it has been created
        /// </summary>
        /// <param name="Object">Object created by the system</param>
        void Setup(object Object);

        /// <summary>
        /// Used to set up any interfaces, extra fields, methods, etc. prior to overridding any methods.
        /// </summary>
        /// <param name="TypeBuilder">Type builder object</param>
        void SetupInterfaces(Utilities.Reflection.Emit.TypeBuilder TypeBuilder);

        #endregion

        #region Properties

        /// <summary>
        /// List of interfaces that need to be injected by this aspect
        /// </summary>
        ICollection<Type> InterfacesUsing { get; }

        #endregion
    }
}
