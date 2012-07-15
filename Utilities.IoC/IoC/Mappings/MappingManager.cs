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
using System.Reflection;
using Utilities.IoC.Mappings.BaseClasses;
using Utilities.IoC.Mappings.Interfaces;
using Utilities.IoC.Mappings.Internal_Classes;
using Utilities.IoC.Providers;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.IoC.Mappings
{
    /// <summary>
    /// Mapping manager
    /// </summary>
    public class MappingManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MappingManager(ProviderManager ProviderManager)
        {
            Modules = new List<IModule>();
            Mappings = new List<IMapping>();
            this.ProviderManager = ProviderManager;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Scans the assembly for mapping modules and creates them
        /// </summary>
        /// <param name="ModuleAssembly"></param>
        public void Setup(Assembly ModuleAssembly)
        {
            IEnumerable<Type> Modules = ModuleAssembly.GetTypes(typeof(IModule));
            List<IModule> TempModules = new List<IModule>();
            foreach (Type Module in Modules)
            {
                if (!Module.IsInterface && !Module.IsAbstract)
                    TempModules.Add((IModule)Module.Assembly.CreateInstance(Module.FullName));
            }
            foreach (IModule Module in TempModules)
            {
                Module.Manager = this;
                Module.Setup();
            }
            this.Modules.AddRange(TempModules);
        }

        /// <summary>
        /// Creates a mapping object
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <returns>a mapping object</returns>
        public IMapping CreateMapping(Type ServiceType)
        {
            IMapping Mapping = new Mapping(ServiceType, ProviderManager, this);
            Mappings.Add(Mapping);
            return Mapping;
        }

        /// <summary>
        /// Creates a mapping object
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <returns>A mapping object</returns>
        public IMapping CreateMapping(Type ServiceType, Type AttributeType)
        {
            IMapping Mapping = new Mapping(ServiceType, AttributeType, ProviderManager, this);
            Mappings.Add(Mapping);
            return Mapping;
        }

        /// <summary>
        /// Gets the mapping that matches this service type
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <returns>The mapping associated with this service type</returns>
        public IMapping GetMapping(Type ServiceType)
        {
            MappingKey Key = new MappingKey(ServiceType, null, ProviderManager, this);
            return Mappings.Find(x => x.Equals(Key));
        }

        /// <summary>
        /// Gets the mapping that matches this service type and attribute type
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <returns>The mapping associated with this service type and attribute type</returns>
        public IMapping GetMapping(Type ServiceType, Type AttributeType)
        {
            MappingKey Key = new MappingKey(ServiceType, AttributeType, ProviderManager, this);
            return Mappings.Find(x => x.Equals(Key));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Modules listing
        /// </summary>
        protected virtual List<IModule> Modules { get; set; }

        /// <summary>
        /// Mapping listing
        /// </summary>
        protected virtual List<IMapping> Mappings { get; set; }

        /// <summary>
        /// Provider manager
        /// </summary>
        protected virtual ProviderManager ProviderManager { get; set; }

        #endregion
    }
}
