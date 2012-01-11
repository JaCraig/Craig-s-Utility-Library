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

#endregion

namespace Utilities.Reflection.Emit.Interfaces
{
    /// <summary>
    /// Interface for properties
    /// </summary>
    public interface IPropertyBuilder
    {
        #region Properties

        /// <summary>
        /// Property name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Data type
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// Property builder
        /// </summary>
        System.Reflection.Emit.PropertyBuilder Builder { get; }

        /// <summary>
        /// Attributes for the property
        /// </summary>
        System.Reflection.PropertyAttributes Attributes { get; }

        /// <summary>
        /// Attributes for the get method
        /// </summary>
        System.Reflection.MethodAttributes GetMethodAttributes { get; }

        /// <summary>
        /// Attributes for the set method
        /// </summary>
        System.Reflection.MethodAttributes SetMethodAttributes { get; }

        /// <summary>
        /// Method builder for the get method
        /// </summary>
        MethodBuilder GetMethod { get; }

        /// <summary>
        /// Method builder for the set method
        /// </summary>
        MethodBuilder SetMethod { get; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the definition of the variable
        /// </summary>
        /// <returns>string representation of the variable definition</returns>
        string GetDefinition();

        #endregion
    }
}