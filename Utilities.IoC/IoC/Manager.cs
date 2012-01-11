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
using Utilities.IoC.Providers;
using Utilities.IoC.Mappings;
using Utilities.IoC.Mappings.Interfaces;
using System.Reflection;
using System.IO;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.IoC
{
    /// <summary>
    /// Manager class
    /// </summary>
    public class Manager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            if (ProviderManager.IsNull())
                ProviderManager = new ProviderManager();
            if (MappingManager.IsNull())
                MappingManager = new MappingManager(ProviderManager);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Loads all mapping modules found within the assembly
        /// </summary>
        /// <param name="ModuleAssembly">Module assembly</param>
        public void Setup(Assembly ModuleAssembly)
        {
            ProviderManager.Setup(ModuleAssembly);
            MappingManager.Setup(ModuleAssembly);
        }

        /// <summary>
        /// Loads all mapping modules found within the assemblies
        /// </summary>
        /// <param name="ModuleAssemblies">Module assemblies</param>
        public void Setup(IEnumerable<Assembly> ModuleAssemblies)
        {
            ModuleAssemblies.ForEach(x => Setup(x));
        }

        /// <summary>
        /// Loads all mapping modules found within a specific directory
        /// </summary>
        /// <param name="Directory">Directory to scan for modules</param>
        /// <param name="ScanSubDirectories">Determines if sub directories should be scanned</param>
        public void Setup(string Directory, bool ScanSubDirectories = true)
        {
            Setup(new DirectoryInfo(Directory).LoadAssemblies(ScanSubDirectories));
        }

        /// <summary>
        /// Creates an object of the specified type
        /// </summary>
        /// <typeparam name="ServiceType">Service type</typeparam>
        /// <returns>An object of the specified type</returns>
        public ServiceType Get<ServiceType>()
        {
            return (ServiceType)Get(typeof(ServiceType));
        }

        /// <summary>
        /// Creates an object of the specified type associated with the attribute type
        /// </summary>
        /// <typeparam name="ServiceType">Service type</typeparam>
        /// <typeparam name="AttributeType">Attribute type</typeparam>
        /// <returns>An object of the specified type</returns>
        public ServiceType Get<ServiceType, AttributeType>()
        {
            return (ServiceType)Get(typeof(ServiceType), typeof(AttributeType));
        }

        /// <summary>
        /// Creates an object of the specified type
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <returns>An object of the specified type</returns>
        public object Get(Type ServiceType)
        {
            IMapping Mapping = MappingManager.GetMapping(ServiceType);
            if (Mapping.IsNull())
                throw new ArgumentException("ServiceType not found in mappings");
            return Mapping.Implementation.Create();
        }

        /// <summary>
        /// Creates an object of the specified type associated with the attribute type
        /// </summary>
        /// <param name="ServiceType">Service type</param>
        /// <param name="AttributeType">Attribute type</param>
        /// <returns>An object of the specified type</returns>
        public object Get(Type ServiceType, Type AttributeType)
        {
            IMapping Mapping = MappingManager.GetMapping(ServiceType, AttributeType);
            if (Mapping.IsNull())
                throw new ArgumentException("ServiceType not found in mappings");
            return Mapping.Implementation.Create();
        }

        #endregion

        #region Properties

        protected static ProviderManager ProviderManager { get; set; }
        protected static MappingManager MappingManager { get; set; }

        #endregion
    }
}