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
using Utilities.IoC.Mappings.Interfaces;
using Utilities.IoC.Providers;
#endregion

namespace Utilities.IoC.Mappings.BaseClasses
{
    /// <summary>
    /// Mapping key
    /// </summary>
    public class MappingKey : BaseMapping
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <param name="ProviderManager">Provider manager</param>
        /// <param name="MappingManager">Mapping manager</param>
        public MappingKey(Type ServiceType, Type AttributeType, ProviderManager ProviderManager, MappingManager MappingManager)
            : base(ServiceType, AttributeType, ProviderManager, MappingManager)
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Determines if the mapping keys are equal
        /// </summary>
        /// <param name="obj">The object to check against</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is IMapping))
                return false;
            IMapping ObjectMapping = (IMapping)obj;
            return ObjectMapping.AttributeType == AttributeType
                && ObjectMapping.ServiceType == ServiceType;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns>The hash code for this object</returns>
        public override int GetHashCode()
        {
            int AttributeHash = AttributeType == null ? 1 : AttributeType.GetHashCode();
            int ServiceHash = ServiceType == null ? 1 : ServiceType.GetHashCode();
            return ServiceHash * AttributeHash;
        }

        #endregion
    }
}
