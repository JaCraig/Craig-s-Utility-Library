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
using Utilities.IoC.Mappings;
using Utilities.IoC.Providers.Scope;
#endregion

namespace Utilities.IoC.Providers.Interfaces
{
    /// <summary>
    /// Provider interface
    /// </summary>
    public interface IProvider
    {
        #region Functions

        /// <summary>
        /// Creates an implementation based off of a type
        /// </summary>
        /// <param name="ImplementationType">Implementation type</param>
        /// <param name="MappingManager">Mapping manager</param>
        /// <returns>an implementation class</returns>
        IImplementation CreateImplementation(Type ImplementationType, MappingManager MappingManager);

        /// <summary>
        /// Creates an implementation that uses a specified function
        /// </summary>
        /// <param name="Implementation">Implementation delegate</param>
        /// <returns>An implementation class</returns>
        IImplementation CreateImplementation<ImplementationType>(Func<ImplementationType> Implementation);

        /// <summary>
        /// Creates an implementation based off an existing implementation
        /// </summary>
        /// <param name="Implementation">Implementation class</param>
        /// <param name="MappingManager">Mapping manager</param>
        /// <returns>Potentially a new implementation class (if appropriate)</returns>
        IImplementation CreateImplementation(IImplementation Implementation, MappingManager MappingManager);

        #endregion

        #region Properties

        /// <summary>
        /// Provider scope
        /// </summary>
        BaseScope ProviderScope { get; }

        #endregion
    }
}
