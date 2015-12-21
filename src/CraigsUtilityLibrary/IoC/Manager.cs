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
    public static class Manager
    {
        /// <summary>
        /// Bootstrapper object
        /// </summary>
        public static IBootstrapper Bootstrapper { get; private set; }

        /// <summary>
        /// Creates the IoC Container
        /// </summary>
        /// <param name="descriptors">The service descriptors.</param>
        /// <param name="assemblies">The assemblies to scan for modules/types.</param>
        /// <returns>The resulting bootstrapper</returns>
        public static IBootstrapper CreateContainer(IEnumerable<ServiceDescriptor> descriptors, params Assembly[] assemblies)
        {
            assemblies = assemblies ?? new Assembly[0];
            var Assemblies = LoadAssemblies(assemblies);
            var LoadedTypes = Assemblies.SelectMany(x => x.ExportedTypes);
            Bootstrapper = GetBootstrapper(Assemblies, LoadedTypes);
            Bootstrapper.Register<IServiceProvider>(Bootstrapper, ServiceLifetime.Singleton);
            RegisterModules();
            RegisterServiceDescriptors(descriptors);
            return Bootstrapper;
        }

        private static IBootstrapper GetBootstrapper(IEnumerable<Assembly> Assemblies, IEnumerable<Type> LoadedTypes)
        {
            var Bootstrappers = LoadedTypes.Where(x => x.GetInterfaces().Contains(typeof(IBootstrapper))
                                                                                  && x.GetTypeInfo().IsClass
                                                                                  && !x.GetTypeInfo().IsAbstract
                                                                                  && !x.GetTypeInfo().ContainsGenericParameters
                                                                                  && !x.GetTypeInfo().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase))
                                                       .ToList();
            if (Bootstrappers.Count == 0)
                Bootstrappers.Add(typeof(DefaultBootstrapper));
            return (IBootstrapper)Activator.CreateInstance(Bootstrappers[0], Assemblies, LoadedTypes);
        }

        private static IEnumerable<Assembly> LoadAssemblies(Assembly[] assemblies)
        {
            var Assemblies = new List<Assembly>();
            Assemblies.AddRange(assemblies);
            if (Assemblies.Count == 0 || !Assemblies.Contains(typeof(Manager).GetTypeInfo().Assembly))
                Assemblies.Add(typeof(Manager).GetTypeInfo().Assembly);
            var GeneratedFile = new FileInfo(".\\CULGeneratedTypes.dll");
            if (GeneratedFile.Exists
                && !Assemblies.Any(x => new FileInfo(x.ManifestModule.FullyQualifiedName).LastWriteTime > GeneratedFile.LastWriteTime))
            {
                Assemblies.Add(Assembly.Load(new AssemblyName(GeneratedFile.FullName)));
            }
            return Assemblies;
        }

        private static void RegisterModules()
        {
            Bootstrapper.RegisterAll<IModule>();
            foreach (IModule Module in Bootstrapper.ResolveAll<IModule>().OrderBy(x => x.Order))
            {
                Module.Load(Bootstrapper);
            }
        }

        private static void RegisterServiceDescriptors(IEnumerable<ServiceDescriptor> descriptors)
        {
            var RegisterTypes = typeof(IBootstrapper).GetTypeInfo()
                                                                 .GetDeclaredMethods("Register")
                                                                 .First(x => x.GetGenericArguments().Count() == 2);
            var RegisterFunc = typeof(IBootstrapper).GetTypeInfo()
                                                    .GetDeclaredMethods("Register")
                                                     .First(x => x.GetGenericArguments().Count() == 1 &&
                                                            x.GetParameters().Count() == 3 &&
                                                            x.GetParameters()[0].ParameterType == typeof(Func<,>));
            var RegisterObj = typeof(IBootstrapper).GetTypeInfo()
                                                    .GetDeclaredMethods("Register")
                                                     .First(x => x.GetGenericArguments().Count() == 1 &&
                                                            x.GetParameters().Count() == 3 &&
                                                            x.GetParameters()[0].ParameterType != typeof(Func<,>));
            foreach (var item in descriptors)
            {
                if (item.ImplementationType != null)
                {
                    var serviceTypeInfo = item.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        var tempType = serviceTypeInfo.MakeGenericType(item.ServiceType);
                        var tempRegistration = RegisterTypes.MakeGenericMethod(item.ImplementationType, tempType);
                        tempRegistration.Invoke(Bootstrapper, new object[] { item.Lifetime, "" });
                    }
                    else
                    {
                        var tempRegistration = RegisterTypes.MakeGenericMethod(item.ImplementationType, item.ServiceType);
                        tempRegistration.Invoke(Bootstrapper, new object[] { item.Lifetime, "" });
                    }
                }
                else if (item.ImplementationFactory != null)
                {
                    var tempRegistration = RegisterFunc.MakeGenericMethod(item.ImplementationType, item.ServiceType);
                    tempRegistration.Invoke(Bootstrapper, new object[] { item.ImplementationFactory, item.Lifetime, "" });
                }
                else
                {
                    var tempRegistration = RegisterObj.MakeGenericMethod(item.ImplementationType, item.ServiceType);
                    tempRegistration.Invoke(Bootstrapper, new object[] { item.ImplementationInstance, item.Lifetime, "" });
                }
            }
        }
    }
}