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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.IoC.Mappings.BaseClasses;
using Utilities.IoC.Providers;
#endregion

namespace Utilities.IoC.Mappings.Internal_Classes
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public class Mapping : MappingKey
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="MappingManager">Mapping manager</param>
        /// <param name="ProviderManager">Provider manager</param>
        public Mapping(Type ServiceType, ProviderManager ProviderManager, MappingManager MappingManager)
            : base(ServiceType, null, ProviderManager, MappingManager)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <param name="MappingManager">Mapping manager</param>
        /// <param name="ProviderManager">Provider manager</param>
        public Mapping(Type ServiceType, Type AttributeType, ProviderManager ProviderManager, MappingManager MappingManager)
            : base(ServiceType, AttributeType, ProviderManager, MappingManager)
        {
        }

        #endregion
    }
}
