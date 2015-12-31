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
using Utilities.IoC.Default.BaseClasses;
using Utilities.IoC.Default.Interfaces;

namespace Utilities.IoC.Default.TypeBuilders
{
    /// <summary>
    /// Transient Type builder
    /// </summary>
    /// <typeparam name="T">Type this builder creates</typeparam>
    public class TransientTypeBuilder<T> : TypeBuilderBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientTypeBuilder{T}"/> class.
        /// </summary>
        /// <param name="implementation">Implementation</param>
        /// <param name="lifeTime">The life time of the object</param>
        public TransientTypeBuilder(Func<IServiceProvider, T> implementation)
            : base(implementation)
        {
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>A copy of this instance.</returns>
        public override ITypeBuilder Copy()
        {
            return new TransientTypeBuilder<T>(Implementation);
        }

        /// <summary>
        /// Creates the object
        /// </summary>
        /// <returns>The created object</returns>
        public override object Create(IServiceProvider provider)
        {
            return Implementation(provider);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
        }
    }
}