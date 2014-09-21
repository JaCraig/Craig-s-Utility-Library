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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;

namespace Utilities.IoC
{
    /// <summary>
    /// IoC manager class
    /// </summary>
    public class Manager : IDisposable
    {
        private static Manager _Instance = new Manager();

        private static object Temp = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        protected Manager()
        {
            IEnumerable<FileInfo> Files = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.TopDirectoryOnly)
                                                                                              .Where(x => !x.Name.Equals("CULGeneratedTypes.dll", StringComparison.InvariantCultureIgnoreCase));
            List<Assembly> LoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            LoadAssemblies(LoadedAssemblies, Files.Select(x => AssemblyName.GetAssemblyName(x.FullName)).ToArray());
            IEnumerable<Assembly> StarterAssemblies = LoadedAssemblies.ToArray();
            foreach (Assembly Assembly in StarterAssemblies)
            {
                try
                {
                    LoadAssemblies(LoadedAssemblies, Assembly.GetReferencedAssemblies());
                }
                catch { }
            }
            FileInfo GeneratedFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\CULGeneratedTypes.dll");
            if (GeneratedFile.Exists
                && LoadedAssemblies.Where(x => !x.FullName.Contains("vshost32") && !x.IsDynamic)
                                   .All(x => new System.IO.FileInfo(x.Location).LastWriteTime <= GeneratedFile.LastWriteTime))
            {
                LoadAssemblies(LoadedAssemblies, AssemblyName.GetAssemblyName(GeneratedFile.FullName));
            }
            LoadedAssemblies = LoadedAssemblies.Distinct(new AssemblyComparer()).ToList();
            IEnumerable<Assembly> Assemblies = LoadedAssemblies.Where(x =>
            {
                try
                {
                    x.GetTypes();
                }
                catch (ReflectionTypeLoadException) { return false; }
                return true;
            });

            List<Type> Bootstrappers = new List<Type>();
            foreach (Assembly Assembly in Assemblies)
            {
                Bootstrappers.AddRange(Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IBootstrapper))
                                                                        && x.IsClass
                                                                        && !x.IsAbstract
                                                                        && !x.ContainsGenericParameters
                                                                        && !x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase)));
            }
            if (Bootstrappers.Count == 0)
            {
                Bootstrappers.Add(typeof(DefaultBootstrapper));
            }
            InternalBootstrapper = (IBootstrapper)Activator.CreateInstance(Bootstrappers[0], Assemblies);
            List<Type> Modules = new List<Type>();
            foreach (Assembly Assembly in Assemblies)
            {
                Modules.AddRange(Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IModule))
                                                                        && x.IsClass
                                                                        && !x.IsAbstract
                                                                        && !x.ContainsGenericParameters));
            }
            List<IModule> ModuleObjects = new List<IModule>();
            foreach (Type Module in Modules)
            {
                ModuleObjects.Add(((IModule)Activator.CreateInstance(Module)));
            }
            foreach (IModule Module in ModuleObjects.OrderBy(x => x.Order))
            {
                Module.Load(InternalBootstrapper);
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Manager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the instance of the manager
        /// </summary>
        public static IBootstrapper Bootstrapper
        {
            get
            {
                if (_Instance == null)
                {
                    lock (Temp)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Manager();
                        }
                    }
                }
                return _Instance.InternalBootstrapper;
            }
        }

        /// <summary>
        /// Bootstrapper object
        /// </summary>
        protected IBootstrapper InternalBootstrapper { get; private set; }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Displays information about the IoC container in string form
        /// </summary>
        /// <returns>Information about the IoC container</returns>
        public override string ToString()
        {
            return Bootstrapper.Name;
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected virtual void Dispose(bool Managed)
        {
            if (InternalBootstrapper != null)
            {
                InternalBootstrapper.Dispose();
                InternalBootstrapper = null;
            }
        }

        private void LoadAssemblies(List<Assembly> Assemblies, params AssemblyName[] assemblyName)
        {
            if (assemblyName == null)
                return;
            foreach (AssemblyName Name in assemblyName.Where(x => !x.FullName.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!Assemblies.Any(x => x.FullName == Name.FullName))
                {
                    Assembly TempAssembly = AppDomain.CurrentDomain.Load(Name);
                    Assemblies.Add(TempAssembly);
                    LoadAssemblies(Assemblies, TempAssembly.GetReferencedAssemblies());
                }
            }
        }

        /// <summary>
        /// Assembly comparer
        /// </summary>
        private class AssemblyComparer : IEqualityComparer<Assembly>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type System.Reflection.Assembly to compare.</param>
            /// <param name="y">The second object of type System.Reflection.Assembly to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(Assembly x, Assembly y)
            {
                return string.Equals(x.FullName, y.FullName, StringComparison.InvariantCultureIgnoreCase);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public int GetHashCode(Assembly obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}