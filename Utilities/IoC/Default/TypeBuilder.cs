/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using Utilities.IoC.Default.Interfaces;

namespace Utilities.IoC.Default
{
    /// <summary>
    /// Type builder
    /// </summary>
    /// <typeparam name="T">Type this builder creates</typeparam>
    public class TypeBuilder<T> : ITypeBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TypeBuilder(Func<T> Implementation)
        {
            this.Implementation = Implementation;
            this.ReturnType = typeof(T);
        }

        /// <summary>
        /// Return type of the implementation
        /// </summary>
        public Type ReturnType { get; set; }

        /// <summary>
        /// Implementation used to create the type
        /// </summary>
        protected Func<T> Implementation { get; set; }

        /// <summary>
        /// Creates the object
        /// </summary>
        /// <returns>The created object</returns>
        public object Create()
        {
            return Implementation();
        }

        /// <summary>
        /// Outputs the string version of whatever object the builder holds
        /// </summary>
        /// <returns>The string version of the object this holds</returns>
        public override string ToString()
        {
            return Implementation().ToString();
        }
    }
}