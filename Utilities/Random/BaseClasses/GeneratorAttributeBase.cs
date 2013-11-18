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
using Utilities.Random.Interfaces;

#endregion

namespace Utilities.Random.BaseClasses
{
    /// <summary>
    /// Attribute base class for generators
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class GeneratorAttributeBase : System.Attribute,IGenerator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Minimum value</param>
        /// <param name="Max">Maximum value</param>
        protected GeneratorAttributeBase(object Min,object Max)
            : base()
        {
            this.Min = Min;
            this.Max = Max;
        }

        /// <summary>
        /// Minimum allowed
        /// </summary>
        public virtual object Min { get; protected set; }

        /// <summary>
        /// Maximum allowed
        /// </summary>
        public virtual object Max { get; protected set; }

        /// <summary>
        /// Generates next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public abstract object NextObj(System.Random Rand);
    }
}