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

using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.IoC.Interfaces;

namespace Utilities.IoC.BaseClasses
{
    /// <summary>
    /// Bootstrapper base class
    /// </summary>
    /// <typeparam name="Container">The actual IoC object</typeparam>
    public abstract class BootstrapperBase<Container> : IBootstrapper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="types">The types.</param>
        protected BootstrapperBase(IEnumerable<Assembly> assemblies, IEnumerable<Type> types)
        {
            Assemblies = assemblies.ToList();
            Types = types.ToList();
        }

        /// <summary>
        /// Name of the bootstrapper
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The IoC container
        /// </summary>
        protected abstract Container AppContainer { get; }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        protected List<Assembly> Assemblies { get; set; }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        protected List<Type> Types { get; private set; }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>This</returns>
        public IBootstrapper AddAssembly(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                return this;
            foreach (Assembly Assembly in assemblies)
            {
                if (!Assemblies.Contains(Assembly))
                {
                    try
                    {
                        Types.AddRange(Assembly.GetTypes());
                        Assemblies.Add(Assembly);
                    }
                    catch (ReflectionTypeLoadException) { }
                }
            }
            return this;
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The service specified</returns>
        public object GetService(Type serviceType)
        {
            return Resolve(serviceType, "", null);
        }

        /// <summary>
        /// Registers an object with the bootstrapper
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="objectToRegister">The object to register.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// This
        /// </returns>
        public abstract IBootstrapper Register<T>(T objectToRegister, ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
            where T : class;

        /// <summary>
        /// Registers a type with the default constructor
        /// </summary>
        /// <typeparam name="T">Object type to register</typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// This
        /// </returns>
        public abstract IBootstrapper Register<T>(ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
            where T : class, new();

        /// <summary>
        /// Registers a type with the default constructor of a child class
        /// </summary>
        /// <typeparam name="T1">Base class/interface type</typeparam>
        /// <typeparam name="T2">Child class type</typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// This
        /// </returns>
        public abstract IBootstrapper Register<T1, T2>(ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
            where T2 : class, T1
            where T1 : class;

        /// <summary>
        /// Registers a type with a function
        /// </summary>
        /// <typeparam name="T">Type that the function returns</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// This
        /// </returns>
        public abstract IBootstrapper Register<T>(Func<IServiceProvider, T> function, ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
            where T : class;

        /// <summary>
        /// Registers all objects of a certain type with the bootstrapper
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <returns>
        /// This
        /// </returns>
        public abstract IBootstrapper RegisterAll<T>(ServiceLifetime lifeTime = ServiceLifetime.Transient)
            where T : class;

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public abstract T Resolve<T>(T defaultObject = default(T))
            where T : class;

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public abstract T Resolve<T>(string name, T defaultObject = default(T))
            where T : class;

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public abstract object Resolve(Type objectType, object defaultObject = null);

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public abstract object Resolve(Type objectType, string name, object defaultObject = null);

        /// <summary>
        /// Resolves the objects based on the type specified
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>A list of objects of the specified type</returns>
        public abstract IEnumerable<T> ResolveAll<T>()
            where T : class;

        /// <summary>
        /// Resolves all objects based on the type specified
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// A list of objects of the specified type
        /// </returns>
        public abstract IEnumerable<object> ResolveAll(Type objectType);

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~BootstrapperBase()
        {
            Dispose(false);
        }
    }
}