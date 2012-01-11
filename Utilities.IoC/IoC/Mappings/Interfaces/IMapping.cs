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
using System.Linq;
using System.Text;
using Utilities.IoC.Providers.Scope;
using Utilities.IoC.Providers.Interfaces;
#endregion

namespace Utilities.IoC.Mappings.Interfaces
{
    /// <summary>
    /// Mapping interface
    /// </summary>
    public interface IMapping
    {
        #region Properties

        /// <summary>
        /// Service type
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// Attribute type
        /// </summary>
        Type AttributeType { get; }

        /// <summary>
        /// Scope used for the mapping
        /// </summary>
        BaseScope Scope { get; }

        /// <summary>
        /// Implementation used to create this mapping
        /// </summary>
        IImplementation Implementation { get; }

        #endregion

        #region Functions

        /// <summary>
        /// Maps the service type to the specified implementation type
        /// </summary>
        /// <typeparam name="ImplementationType">Implementation type</typeparam>
        /// <returns>this</returns>
        IMapping To<ImplementationType>();

        /// <summary>
        /// Maps the service type to the specified implementation type
        /// </summary>
        /// <param name="ImplementationType">Implementation type</param>
        /// <returns>this</returns>
        IMapping To(Type ImplementationType);

        /// <summary>
        /// Maps the service type to the specified implementation function
        /// </summary>
        /// <param name="Implementation">Function called when type is generated</param>
        /// <returns>this</returns>
        IMapping To<ImplementationType>(Func<ImplementationType> Implementation);

        /// <summary>
        /// Maps the service type to the specified implementation class
        /// </summary>
        /// <param name="Implementation">Implementation class</param>
        /// <returns>this</returns>
        IMapping To(IImplementation Implementation);

        /// <summary>
        /// Sets the scope of the mapping
        /// </summary>
        /// <param name="Scope">Scope to use</param>
        /// <returns>this</returns>
        IMapping SetScope(BaseScope Scope);

        #endregion
    }
}
