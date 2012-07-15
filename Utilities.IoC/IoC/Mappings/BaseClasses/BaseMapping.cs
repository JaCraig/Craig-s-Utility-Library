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
using Utilities.IoC.Providers.Scope;
using Utilities.IoC.Providers.Interfaces;
#endregion

namespace Utilities.IoC.Mappings.BaseClasses
{
    /// <summary>
    /// Base mapping
    /// </summary>
    public class BaseMapping : IMapping
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <param name="MappingManager">Mapping manager</param>
        /// <param name="ProviderManager">Provider manager</param>
        public BaseMapping(Type ServiceType, Type AttributeType, ProviderManager ProviderManager, MappingManager MappingManager)
        {
            this.ServiceType = ServiceType;
            this.AttributeType = AttributeType;
            this.ProviderManager = ProviderManager;
            this.MappingManager = MappingManager;
            this.Scope = new StandardScope();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Maps an item to an implementation type
        /// </summary>
        /// <typeparam name="ImplementationType">Implementation type</typeparam>
        /// <returns>The mapping object</returns>
        public virtual IMapping To<ImplementationType>()
        {
            return To(typeof(ImplementationType));
        }

        /// <summary>
        /// Maps an item to an implementation type
        /// </summary>
        /// <param name="ImplementationType">Implementation type</param>
        /// <returns>The mapping object</returns>
        public virtual IMapping To(Type ImplementationType)
        {
            Implementation = ProviderManager.GetProvider(Scope).CreateImplementation(ImplementationType, MappingManager);
            return this;
        }

        /// <summary>
        /// Maps an item to an implementation function
        /// </summary>
        /// <typeparam name="ImplementationType">Implementation type</typeparam>
        /// <param name="Implementation">Implementation function</param>
        /// <returns>The mapping object</returns>
        public virtual IMapping To<ImplementationType>(Func<ImplementationType> Implementation)
        {
            this.Implementation = ProviderManager.GetProvider(Scope).CreateImplementation(Implementation);
            return this;
        }

        /// <summary>
        /// Maps an item to an implementation class
        /// </summary>
        /// <param name="Implementation">Implementation class</param>
        /// <returns>The mapping object</returns>
        public virtual IMapping To(Providers.Interfaces.IImplementation Implementation)
        {
            this.Implementation = Implementation;
            return this;
        }

        /// <summary>
        /// Sets the scope for the mapping
        /// </summary>
        /// <param name="Scope">The scope to use</param>
        /// <returns>Mapping object</returns>
        public virtual IMapping SetScope(BaseScope Scope)
        {
            this.Scope = Scope;
            Implementation = ProviderManager.GetProvider(Scope).CreateImplementation(Implementation, MappingManager);
            return this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Service type
        /// </summary>
        public virtual Type ServiceType { get; protected set; }

        /// <summary>
        /// Attribute type
        /// </summary>
        public virtual Type AttributeType { get; protected set; }

        /// <summary>
        /// Scope
        /// </summary>
        public virtual BaseScope Scope { get; protected set; }

        /// <summary>
        /// Implementation
        /// </summary>
        public virtual IImplementation Implementation { get; protected set; }

        /// <summary>
        /// Provider manager
        /// </summary>
        protected virtual ProviderManager ProviderManager { get; set; }

        /// <summary>
        /// Mapping manager
        /// </summary>
        protected virtual MappingManager MappingManager { get; set; }

        #endregion
    }
}
