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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.AOP.Generators;
using Utilities.DataTypes.AOP.Interfaces;
using Utilities.DataTypes.CodeGen;

namespace Utilities.DataTypes.AOP
{
    /// <summary>
    /// AOP interface manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Compiler">The compiler.</param>
        /// <param name="Aspects">The aspects.</param>
        /// <param name="Modules">The modules.</param>
        public Manager(Compiler Compiler, IEnumerable<IAspect> Aspects, IEnumerable<IAOPModule> Modules)
        {
            Contract.Requires<ArgumentNullException>(Compiler != null, "Compiler");
            Contract.Requires<ArgumentNullException>(Compiler.Classes != null, "Compiler.Classes");
            Contract.Requires<ArgumentNullException>(Aspects != null, "Aspects");
            Contract.Requires<ArgumentNullException>(Modules != null, "Modules");
            Manager.Compiler = Compiler;
            if (Manager.Aspects.Count == 0)
                Manager.Aspects.Add(Aspects);
            Compiler.Classes.ForEachParallel(x => Classes.AddOrUpdate(x.BaseType, y => x, (y, z) => x));
            Modules.ForEachParallel(x => x.Setup(this));
        }

        /// <summary>
        /// Gets the system's compiler
        /// </summary>
        protected static Compiler Compiler { get; private set; }

        /// <summary>
        /// The list of aspects that are being used
        /// </summary>
        private static readonly ConcurrentBag<IAspect> Aspects = new ConcurrentBag<IAspect>();

        /// <summary>
        /// Dictionary containing generated types and associates it with original type
        /// </summary>
        private static ConcurrentDictionary<Type, Type> Classes = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Creates an object of the specified base type, registering the type if necessary
        /// </summary>
        /// <typeparam name="T">The base type</typeparam>
        /// <returns>Returns an object of the specified base type</returns>
        public virtual T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        /// <summary>
        /// Creates an object of the specified base type, registering the type if necessary
        /// </summary>
        /// <param name="BaseType">The base type</param>
        /// <returns>Returns an object of the specified base type</returns>
        public virtual object Create(Type BaseType)
        {
            if (!Classes.ContainsKey(BaseType))
                Setup(BaseType);
            object ReturnObject = Classes[BaseType].Assembly.CreateInstance(Classes[BaseType].FullName);
            if (Classes[BaseType] != BaseType)
                Aspects.ForEach(x => x.Setup(ReturnObject));
            return ReturnObject;
        }

        /// <summary>
        /// Sets up all types from the assembly that it can
        /// </summary>
        /// <param name="Assembly">Assembly to set up</param>
        public virtual void Setup(params Assembly[] Assembly)
        {
            Contract.Requires<ArgumentNullException>(Assembly != null, "Assembly");
            Setup(FilterTypesToSetup(Assembly.Types()));
        }

        /// <summary>
        /// Sets up a type so it can be used in the system later
        /// </summary>
        /// <param name="types">The types.</param>
        public virtual void Setup(params Type[] types)
        {
            IEnumerable<Type> TempTypes = FilterTypesToSetup(types);
            var AssembliesUsing = new List<Assembly>();
            AssembliesUsing.Add(typeof(object).Assembly, typeof(System.Linq.Enumerable).Assembly);
            Aspects.ForEach(x => AssembliesUsing.AddIfUnique(x.AssembliesUsing));

            var Usings = new List<string>();
            Usings.Add("System");
            Usings.Add("System.Collections.Generic");
            Usings.Add("System.Linq");
            Usings.Add("System.Text");
            Usings.Add("System.Threading.Tasks");
            Aspects.ForEach(x => Usings.AddIfUnique(x.Usings));

            var Interfaces = new List<Type>();
            Aspects.ForEach(x => Interfaces.AddRange(x.InterfacesUsing ?? new List<Type>()));

            var Builder = new StringBuilder();

            foreach (Type Type in TempTypes)
            {
                AssembliesUsing.AddIfUnique(Type.Assembly);
                AssembliesUsing.AddIfUnique(GetAssemblies(Type));

                string Namespace = "CULGeneratedTypes.C" + Guid.NewGuid().ToString("N");
                string ClassName = Type.Name + "Derived";
                Builder.AppendLine(Setup(Type, Namespace, ClassName, Usings, Interfaces, AssembliesUsing));
            }
            try
            {
                IEnumerable<Type> Types = Manager.Compiler.Create(Builder.ToString(), Usings, AssembliesUsing.ToArray());
                foreach (Type Type in TempTypes)
                {
                    Manager.Classes.AddOrUpdate(Type,
                        Types.FirstOrDefault(x => x.Is(Type)),
                        (x, y) => x);
                }
            }
            catch (Exception)
            {
                foreach (Type Type in TempTypes)
                {
                    Manager.Classes.AddOrUpdate(Type,
                        Type,
                        (x, y) => x);
                }
            }
        }

        /// <summary>
        /// Outputs manager info as a string
        /// </summary>
        /// <returns>String version of the manager</returns>
        public override string ToString()
        {
            return "AOP registered classes: " + Classes.Keys.ToString(x => x.Name) + "\r\n";
        }

        /// <summary>
        /// Determines whether this instance can setup the specified types.
        /// </summary>
        /// <param name="enumerable">The list of types</param>
        /// <returns>The types that can be set up</returns>
        private static Type[] FilterTypesToSetup(IEnumerable<Type> enumerable)
        {
            Contract.Requires<ArgumentNullException>(enumerable != null);
            return enumerable.Where(x => !Classes.ContainsKey(x)
                                && !x.ContainsGenericParameters
                                && (x.IsPublic || x.IsNestedPublic)
                                && !x.IsSealed
                                && x.IsVisible
                                && !x.IsCOMObject
                                && !string.IsNullOrEmpty(x.Namespace))
                          .ToArray();
        }

        private static Assembly[] GetAssemblies(Type Type)
        {
            var Types = new List<Assembly>();
            Type TempType = Type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly.GetReferencedAssemblies().ForEach(x =>
                {
                    try
                    {
                        return Assembly.Load(x);
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(x => x != null));
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x)));
                TempType.GetEvents().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.EventHandlerType)));
                TempType.GetFields().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.FieldType)));
                TempType.GetProperties().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.PropertyType)));
                TempType.GetMethods().ForEach(x =>
                {
                    Types.AddIfUnique(GetAssembliesSimple(x.ReturnType));
                    x.GetParameters().ForEach(y => Types.AddIfUnique(GetAssembliesSimple(y.ParameterType)));
                });
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }

        private static Assembly[] GetAssembliesSimple(Type Type)
        {
            var Types = new List<Assembly>();
            Type TempType = Type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x)));
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }

        /// <summary>
        /// Setups the specified type.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Namespace">The namespace.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="Usings">The usings.</param>
        /// <param name="Interfaces">The interfaces.</param>
        /// <param name="AssembliesUsing">The assemblies using.</param>
        /// <returns></returns>
        private static string Setup(Type Type, string Namespace,
            string ClassName, List<string> Usings,
            List<Type> Interfaces, List<Assembly> AssembliesUsing)
        {
            if (Type.IsInterface)
                return new ClassGenerator(Type, Aspects).Generate(Namespace, ClassName, Usings, Interfaces, AssembliesUsing);
            if (Type.IsAbstract)
                return new ClassGenerator(Type, Aspects).Generate(Namespace, ClassName, Usings, Interfaces, AssembliesUsing);
            return new ClassGenerator(Type, Aspects).Generate(Namespace, ClassName, Usings, Interfaces, AssembliesUsing);
        }

        private static string SetupAbstract(Type type, string @namespace, string className, List<string> usings, List<Type> interfaces, List<Assembly> assembliesUsing)
        {
            return "";
        }

        private static string SetupClass(Type type, string @namespace, string className, List<string> usings, List<Type> interfaces, List<Assembly> assembliesUsing)
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat(@"namespace {1}
{{
    {0}

    public class {2} : {3}{4} {5}
    {{
", usings.ToString(x => "using " + x + ";", "\r\n"),
 @namespace,
 className,
 type.FullName.Replace("+", "."),
 interfaces.Count > 0 ? "," : "", interfaces.ToString(x => x.Name));
            if (type.HasDefaultConstructor())
            {
                //Builder.AppendLine(new ConstructorGenerator(type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First(x => x.GetParameters().Length == 0)).Generate(assembliesUsing, Aspects));
            }

            Aspects.ForEach(x => Builder.AppendLine(x.SetupInterfaces(type)));

            Type TempType = type;
            var MethodsAlreadyDone = new List<string>();
            while (TempType != null)
            {
                foreach (PropertyInfo Property in TempType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    MethodInfo GetMethodInfo = Property.GetGetMethod();
                    MethodInfo SetMethodInfo = Property.GetSetMethod();
                    if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                        && !MethodsAlreadyDone.Contains("set_" + Property.Name)
                        && GetMethodInfo != null
                        && GetMethodInfo.IsVirtual
                        && SetMethodInfo != null
                        && SetMethodInfo.IsPublic
                        && !GetMethodInfo.IsFinal
                        && Property.GetIndexParameters().Length == 0)
                    {
                        Builder.AppendLine(new PropertyGenerator(Property).Generate(assembliesUsing, Aspects));
                        MethodsAlreadyDone.Add(GetMethodInfo.Name);
                        MethodsAlreadyDone.Add(SetMethodInfo.Name);
                    }
                    else if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                        && GetMethodInfo != null
                        && GetMethodInfo.IsVirtual
                        && SetMethodInfo == null
                        && !GetMethodInfo.IsFinal
                        && Property.GetIndexParameters().Length == 0)
                    {
                        Builder.AppendLine(new PropertyGenerator(Property).Generate(assembliesUsing, Aspects));
                        MethodsAlreadyDone.Add(GetMethodInfo.Name);
                    }
                    else
                    {
                        if (GetMethodInfo != null)
                            MethodsAlreadyDone.Add(GetMethodInfo.Name);
                        if (SetMethodInfo != null)
                            MethodsAlreadyDone.Add(SetMethodInfo.Name);
                    }
                }
                foreach (MethodInfo Method in TempType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                                                        .Where(x => !MethodsAlreadyDone.Contains(x.Name)
                                                            && x.IsVirtual
                                                            && !x.IsFinal
                                                            && !x.IsPrivate
                                                            && !x.Name.StartsWith("add_", StringComparison.InvariantCultureIgnoreCase)
                                                            && !x.Name.StartsWith("remove_", StringComparison.InvariantCultureIgnoreCase)
                                                            && !x.IsGenericMethod))
                {
                    Builder.AppendLine(new MethodGenerator(Method).Generate(assembliesUsing, Aspects));
                    MethodsAlreadyDone.Add(Method.Name);
                }
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            Builder.AppendLine(@"   }
}");
            return Builder.ToString();
        }

        private static string SetupInterface(Type type, string @namespace, string className, List<string> usings, List<Type> interfaces, List<Assembly> assembliesUsing)
        {
            return "";
        }

        private static string SetupMethod(Type Type, MethodInfo MethodInfo, bool IsProperty)
        {
            if (MethodInfo == null)
                return "";
            var Builder = new StringBuilder();
            string BaseMethodName = MethodInfo.Name.Replace("get_", "").Replace("set_", "");
            string ReturnValue = MethodInfo.ReturnType != typeof(void) ? "FinalReturnValue" : "";
            string BaseCall = "";
            if (IsProperty)
                BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName : ReturnValue + "=base." + BaseMethodName;
            else
                BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName + "(" : ReturnValue + "=base." + BaseMethodName + "(";
            ParameterInfo[] Parameters = MethodInfo.GetParameters();
            if (IsProperty)
            {
                BaseCall += Parameters.Length > 0 ? "=" + Parameters.ToString(x => x.Name) + ";" : ";";
            }
            else
            {
                BaseCall += Parameters.Length > 0 ? Parameters.ToString(x => (x.IsOut ? "out " : "") + x.Name) : "";
                BaseCall += ");\r\n";
            }
            Builder.AppendLineFormat(@"
                try
                {{
                    {0}
                    {1}
                    {2}
                    {3}
                    {4}
                }}
                catch(Exception CaughtException)
                {{
                    {5}
                    throw;
                }}",
                MethodInfo.ReturnType != typeof(void) ? MethodInfo.ReturnType.GetName() + " " + ReturnValue + ";" : "",
                Aspects.ForEach(x => x.SetupStartMethod(MethodInfo, Type)).ToString(x => x, "\r\n"),
                BaseCall,
                Aspects.ForEach(x => x.SetupEndMethod(MethodInfo, Type, ReturnValue)).ToString(x => x, "\r\n"),
                string.IsNullOrEmpty(ReturnValue) ? "" : "return " + ReturnValue + ";",
                Aspects.ForEach(x => x.SetupExceptionMethod(MethodInfo, Type)).ToString(x => x, "\r\n"));
            return Builder.ToString();
        }
    }
}