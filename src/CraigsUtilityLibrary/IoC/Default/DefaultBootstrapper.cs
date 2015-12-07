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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.IoC.BaseClasses;
using Utilities.IoC.Default.Interfaces;
using Utilities.IoC.Default.TypeBuilders;
using Utilities.IoC.Interfaces;

namespace Utilities.IoC.Default
{
    /// <summary>
    /// Default bootstrapper if one isn't found
    /// </summary>
    public class DefaultBootstrapper : BootstrapperBase<IDictionary<Tuple<Type, string>, ITypeBuilder>>, IScope
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="types">The types.</param>
        public DefaultBootstrapper(IEnumerable<Assembly> assemblies, IEnumerable<Type> types)
            : base(assemblies, types)
        {
            _AppContainer = new ConcurrentDictionary<Tuple<Type, string>, ITypeBuilder>();
            if (GenericRegisterMethod == null)
            {
                GenericRegisterMethod = GetType().GetMethods().First(x => x.Name == "Register" && x.GetGenericArguments().Count() == 2);
                GenericResolveMethod = GetType().GetMethods().First(x => x.Name == "Resolve" && x.IsGenericMethod && x.GetParameters().Length == 1);
                GenericResolveAllMethod = GetType().GetMethod("ResolveAll", new Type[] { });
                Register<IScope, DefaultBootstrapper>();
                Register<IServiceScope, DefaultBootstrapper>();
                Register<IBootstrapper, DefaultBootstrapper>();
                Register<DefaultBootstrapper, DefaultBootstrapper>();
                Register<IServiceScopeFactory, ServiceScopeFactory>();
                Register<IScopeFactory, ServiceScopeFactory>();
                Register<ServiceScopeFactory, ServiceScopeFactory>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper" /> class.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        public DefaultBootstrapper(DefaultBootstrapper bootstrapper)
            : this(bootstrapper.Assemblies, bootstrapper.Types)
        {
            Parent = bootstrapper;
            foreach (var Key in bootstrapper.AppContainer)
            {
                _AppContainer.AddOrUpdate(Key.Key,
                                x => Key.Value.Copy(),
                                (x, y) => Key.Value.Copy());
            }
        }

        /// <summary>
        /// Name of the bootstrapper
        /// </summary>
        public override string Name => "Default bootstrapper";

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        public IServiceProvider ServiceProvider => this;

        /// <summary>
        /// App container
        /// </summary>
        protected override IDictionary<Tuple<Type, string>, ITypeBuilder> AppContainer => _AppContainer;

        /// <summary>
        /// Gets or sets the generic register method.
        /// </summary>
        /// <value>The generic register method.</value>
        private static MethodInfo GenericRegisterMethod { get; set; }

        /// <summary>
        /// Gets or sets the generic resolve all method.
        /// </summary>
        /// <value>The generic resolve all method.</value>
        private static MethodInfo GenericResolveAllMethod { get; set; }

        /// <summary>
        /// Gets or sets the generic resolve method.
        /// </summary>
        /// <value>The generic resolve method.</value>
        private static MethodInfo GenericResolveMethod { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        private DefaultBootstrapper Parent { get; set; }

        /// <summary>
        /// The application container
        /// </summary>
        private ConcurrentDictionary<Tuple<Type, string>, ITypeBuilder> _AppContainer = null;

        /// <summary>
        /// Creates a new sub scope.
        /// </summary>
        /// <returns>The new scope</returns>
        public IServiceScope CreateScope()
        {
            return new DefaultBootstrapper(this);
        }

        /// <summary>
        /// Registers an object
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="objectToRegister">The object to register.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// This
        /// </returns>
        public override IBootstrapper Register<T>(T objectToRegister, ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
        {
            return Register(x => objectToRegister, lifeTime, name);
        }

        /// <summary>
        /// Registers an object
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// This
        /// </returns>
        public override IBootstrapper Register<T>(ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
        {
            return Register<T, T>(lifeTime, name);
        }

        /// <summary>
        /// Registers two types together
        /// </summary>
        /// <typeparam name="T1">Interface/base class</typeparam>
        /// <typeparam name="T2">Implementation</typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>This</returns>
        public override IBootstrapper Register<T1, T2>(ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
        {
            Type Type = typeof(T2);
            return Register(x =>
            {
                ConstructorInfo Constructor = FindConstructor(Type);
                if (Constructor != null)
                {
                    return (T1)Activator.CreateInstance(Type, GetParameters(Constructor).ToArray());
                }
                return null;
            }, lifeTime, name);
        }

        /// <summary>
        /// Registers a function with a type
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="name">The name.</param>
        /// <returns>This</returns>
        public override IBootstrapper Register<T>(Func<IServiceProvider, T> function, ServiceLifetime lifeTime = ServiceLifetime.Transient, string name = "")
        {
            var Key = new Tuple<Type, string>(typeof(T), name);
            ITypeBuilder Value = GetTypeBuilder(function, lifeTime);
            _AppContainer.AddOrUpdate(Key,
                x => Value,
                (x, y) => Value);
            return this;
        }

        /// <summary>
        /// Registers all objects of a certain type with the bootstrapper
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <returns>This</returns>
        public override IBootstrapper RegisterAll<T>(ServiceLifetime lifeTime = ServiceLifetime.Transient)
        {
            foreach (Type Type in Types.Where(x => IsOfType(x, typeof(T))
                                                    && x.GetTypeInfo().IsClass
                                                    && !x.GetTypeInfo().IsAbstract
                                                    && !x.GetTypeInfo().ContainsGenericParameters))
            {
                GenericRegisterMethod.MakeGenericMethod(typeof(T), Type)
                    .Invoke(this, new object[] { lifeTime, Types.Count == 1 ? "" : Type.FullName });
                GenericRegisterMethod.MakeGenericMethod(Type, Type)
                    .Invoke(this, new object[] { lifeTime, "" });
            }
            return this;
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// Object of the type specified
        /// </returns>
        public override T Resolve<T>(T defaultObject = default(T))
        {
            return (T)Resolve(typeof(T), "", defaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// Object of the type specified
        /// </returns>
        public override T Resolve<T>(string name, T defaultObject = default(T))
        {
            return (T)Resolve(typeof(T), name, defaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// Object of the type specified
        /// </returns>
        public override object Resolve(Type objectType, object defaultObject = null)
        {
            return Resolve(objectType, "", defaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultObject">The default object.</param>
        /// <returns>
        /// Object of the type specified
        /// </returns>
        public override object Resolve(Type objectType, string name, object defaultObject = null)
        {
            try
            {
                var Key = new Tuple<Type, string>(objectType, name);
                ITypeBuilder Builder = null;
                return _AppContainer.TryGetValue(Key, out Builder) ? Builder.Create(this) : defaultObject;
            }
            catch { return defaultObject; }
        }

        /// <summary>
        /// Resolves all objects of the type specified
        /// </summary>
        /// <typeparam name="T">Type of objects to return</typeparam>
        /// <returns>An IEnumerable containing all objects of the type specified</returns>
        public override IEnumerable<T> ResolveAll<T>()
        {
            return ResolveAll(typeof(T)).Select(x => (T)x);
        }

        /// <summary>
        /// Resolves all objects of the type specified
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// An IEnumerable containing all objects of the type specified
        /// </returns>
        public override IEnumerable<object> ResolveAll(Type objectType)
        {
            var ReturnValues = new ConcurrentBag<object>();
            foreach (Tuple<Type, string> Key in _AppContainer.Keys.Where(x => x.Item1 == objectType))
            {
                ReturnValues.Add(Resolve(Key.Item1, Key.Item2, null));
            }
            return ReturnValues;
        }

        /// <summary>
        /// Converts the bootstrapper to a string
        /// </summary>
        /// <returns>String version of the bootstrapper</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (Tuple<Type, string> Key in AppContainer.Keys)
            {
                Builder.AppendLine(AppContainer[Key].ToString());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="managed">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool managed)
        {
            if (_AppContainer != null)
            {
                IEnumerable<ITypeBuilder> ItemsToDispose = null;
                if (Parent == null)
                    ItemsToDispose = _AppContainer.Values.Reverse();
                else
                    ItemsToDispose = _AppContainer.Values.Where(x => x as IScopedTypeBuilder != null).Reverse();
                foreach (IDisposable Item in ItemsToDispose)
                {
                    Item.Dispose();
                }
                _AppContainer.Clear();
                _AppContainer = null;
            }
            base.Dispose(managed);
        }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <typeparam name="T">Object type it should return</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <returns>The type builder based on the lifetime value.</returns>
        private static ITypeBuilder GetTypeBuilder<T>(Func<IServiceProvider, T> function, ServiceLifetime lifeTime)
        {
            if (lifeTime == ServiceLifetime.Transient)
                return new TransientTypeBuilder<T>(function);
            if (lifeTime == ServiceLifetime.Scoped)
                return new ScopedTypeBuilder<T>(function);
            return new SingletonTypeBuilder<T>(function);
        }

        /// <summary>
        /// Finds the constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The constructor that should be used
        /// </returns>
        private ConstructorInfo FindConstructor(Type type)
        {
            if (type == null)
                return null;
            ConstructorInfo[] Constructors = type.GetConstructors();
            ConstructorInfo Constructor = null;
            foreach (ConstructorInfo TempConstructor in Constructors.OrderByDescending(x => x.GetParameters().Length))
            {
                bool Found = true;
                foreach (ParameterInfo Parameter in TempConstructor.GetParameters())
                {
                    Type ParameterType = Parameter.ParameterType;
                    if (Parameter.ParameterType.GetInterfaces().Contains(typeof(IEnumerable)) && Parameter.ParameterType.GetTypeInfo().IsGenericType)
                    {
                        ParameterType = ParameterType.GetGenericArguments().First();
                        if (!AppContainer.Keys.Any(x => x.Item1 == ParameterType))
                        {
                            Found = false;
                            break;
                        }
                    }
                    else if (!AppContainer.Keys.Contains(new Tuple<Type, string>(ParameterType, "")))
                    {
                        Found = false;
                        break;
                    }
                }
                if (Found)
                {
                    Constructor = TempConstructor;
                    break;
                }
            }
            return Constructor;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns>
        /// The parameters
        /// </returns>
        private List<object> GetParameters(ConstructorInfo constructor)
        {
            if (constructor == null)
                return new List<object>();
            var Params = new List<object>();
            foreach (ParameterInfo Parameter in constructor.GetParameters())
            {
                if (Parameter.ParameterType.GetInterfaces().Contains(typeof(IEnumerable)) && Parameter.ParameterType.GetTypeInfo().IsGenericType)
                {
                    Type GenericParamType = Parameter.ParameterType.GetGenericArguments().First();
                    Params.Add(GenericResolveAllMethod.MakeGenericMethod(GenericParamType).Invoke(this, new object[] { }));
                }
                else
                {
                    Params.Add(GenericResolveMethod.MakeGenericMethod(Parameter.ParameterType)
                                                   .Invoke(this, new object[] {
                                                                    Parameter.ParameterType.GetTypeInfo().IsValueType ? Activator.CreateInstance(Parameter.ParameterType) : null
                                                   }));
                }
            }
            return Params;
        }

        private bool IsOfType(Type x, Type type)
        {
            if (x == typeof(object) || x == null)
                return false;
            if (x == type || x.GetInterfaces().Any(y => y == type))
                return true;
            return IsOfType(x.GetTypeInfo().BaseType, type);
        }
    }
}