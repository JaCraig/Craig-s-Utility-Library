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
#endregion

namespace Utilities.IoC.Mappings.BaseClasses
{
    /// <summary>
    /// Base module class
    /// </summary>
    public abstract class BaseModule : IModule
    {
        #region Functions

        /// <summary>
        /// Sets up the module
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Creates a mapping using a specific service type
        /// </summary>
        /// <typeparam name="ServiceType">Service type</typeparam>
        /// <returns>A mapping object</returns>
        public virtual IMapping Map<ServiceType>()
        {
            return Map(typeof(ServiceType));
        }

        /// <summary>
        /// Creates a mapping using a specific service type and attribute type
        /// </summary>
        /// <typeparam name="ServiceType">Service type</typeparam>
        /// <typeparam name="AttributeType">Attribute type</typeparam>
        /// <returns>A mapping object</returns>
        public virtual IMapping Map<ServiceType, AttributeType>()
        {
            return Map(typeof(ServiceType), typeof(AttributeType));
        }

        /// <summary>
        /// Creates a mapping using a specific service type
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <returns>A mapping object</returns>
        public virtual IMapping Map(Type ServiceType, Type AttributeType = null)
        {
            return Manager.CreateMapping(ServiceType, AttributeType);
        }

        #endregion

        #region Properties

        public virtual MappingManager Manager { get; set; }

        #endregion
    }
}
