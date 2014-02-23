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

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.AOP.Interfaces;
using Utilities.DataTypes.CodeGen;

#endregion Usings

namespace Utilities.DataTypes.AOP
{
    /// <summary>
    /// AOP interface manager
    /// </summary>
    public class Manager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager(Compiler Compiler)
        {
            Contract.Requires<ArgumentNullException>(Compiler != null, "Compiler");
            Manager.Compiler = Compiler;
            if (Aspects.Count == 0)
                Aspects.Add(AppDomain.CurrentDomain.GetAssemblies().Objects<IAspect>());
            Compiler.Classes.ForEach(x => Classes.Add(x.BaseType, x));
            if (Classes.Count == 0)
            {
                foreach (Type TempType in AppDomain.CurrentDomain.GetAssemblies()
                                                                 .Types()
                                                                 .Where(x => !x.ContainsGenericParameters
                                                                             && !x.IsAbstract
                                                                             && x.IsClass
                                                                             && x.IsPublic
                                                                             && !x.IsSealed
                                                                             && x.IsVisible
                                                                             && !x.IsCOMObject
                                                                             && x.HasDefaultConstructor()
                                                                             && !string.IsNullOrEmpty(x.Namespace)
                                                                             && !x.Namespace.StartsWith("system", StringComparison.CurrentCultureIgnoreCase)
                                                                             && !x.Namespace.StartsWith("microsoft", StringComparison.CurrentCultureIgnoreCase)
                                                                             && !x.Namespace.StartsWith("utilities", StringComparison.CurrentCultureIgnoreCase)))
                {
                    try
                    {
                        Setup(TempType);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the system's compiler
        /// </summary>
        protected static Compiler Compiler { get; private set; }

        /// <summary>
        /// The list of aspects that are being used
        /// </summary>
        private static ICollection<IAspect> Aspects = new List<IAspect>();

        /// <summary>
        /// Dictionary containing generated types and associates it with original type
        /// </summary>
        private static IDictionary<Type, Type> Classes = new Dictionary<Type, Type>();

        #endregion Properties

        #region Functions

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
            Aspects.ForEach(x => x.Setup(ReturnObject));
            return ReturnObject;
        }

        /// <summary>
        /// Sets up a type so it can be used in the system later
        /// </summary>
        /// <param name="Type">Type to set up</param>
        public virtual void Setup(Type Type)
        {
            if (Classes.ContainsKey(Type))
                return;

            List<Assembly> AssembliesUsing = new List<Assembly>();
            List<string> Usings = new List<string>();
            AssembliesUsing.Add(typeof(object).Assembly, typeof(System.Linq.Enumerable).Assembly);
            AssembliesUsing.AddIfUnique(Type.Assembly);
            AssembliesUsing.AddIfUnique(GetAssemblies(Type));
            Aspects.ForEach(x => AssembliesUsing.AddIfUnique(x.AssembliesUsing));
            Usings.Add("System");
            Usings.Add("System.Collections.Generic");
            Usings.Add("System.Linq");
            Usings.Add("System.Text");
            Usings.Add("System.Threading.Tasks");

            Aspects.ForEach(x => Usings.AddIfUnique(x.Usings));

            List<Type> Interfaces = new List<Type>();
            Aspects.ForEach(x => Interfaces.AddRange(x.InterfacesUsing == null ? new List<Type>() : x.InterfacesUsing));
            StringBuilder Builder = new StringBuilder();
            string Namespace = "CULGeneratedTypes.C" + Guid.NewGuid().ToString("N");
            Builder.AppendLineFormat(@"{0}
namespace {1}
{{
    public class {2} : {3}{4} {5}
    {{
", Usings.ToString(x => "using " + x + ";", "\r\n"),
 Namespace,
 Type.Name + "Derived",
 Type.FullName.Replace("+", "."),
 Interfaces.Count > 0 ? "," : "", Interfaces.ToString(x => x.Name));
            if (Type.HasDefaultConstructor())
            {
                Builder.AppendLineFormat(@"
        public {0}()
            :base()
        {{
            ",
               Type.Name + "Derived");
                Aspects.ForEach(x => Builder.AppendLine(x.SetupDefaultConstructor(Type)));
                Builder.AppendLineFormat(@"
        }}");
            }

            Aspects.ForEach(x => Builder.AppendLine(x.SetupInterfaces(Type)));

            Type TempType = Type;
            List<string> MethodsAlreadyDone = new List<string>();
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
                        AssembliesUsing.AddIfUnique(GetAssemblies(Property.PropertyType));
                        Builder.AppendLineFormat(@"
        public override {0} {1}
        {{
            get
            {{
                {2}
            }}
            set
            {{
                {3}
            }}
        }}",
                                                    Property.PropertyType.GetName(),
                                                    Property.Name,
                                                    SetupMethod(Type, GetMethodInfo, true),
                                                    SetupMethod(Type, SetMethodInfo, true));
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
                        AssembliesUsing.AddIfUnique(GetAssemblies(Property.PropertyType));
                        Builder.AppendLineFormat(@"
        public override {0} {1}
        {{
            get
            {{
                {2}
            }}
        }}",
                                                    Property.PropertyType.GetName(),
                                                    Property.Name,
                                                    SetupMethod(Type, GetMethodInfo, true));
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
                foreach (MethodInfo Method in TempType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    string MethodAttribute = "public";
                    if (!MethodsAlreadyDone.Contains(Method.Name)
                        && Method.IsVirtual
                        && !Method.IsFinal
                        && !Method.IsPrivate
                        && !Method.Name.StartsWith("add_", StringComparison.InvariantCultureIgnoreCase)
                        && !Method.Name.StartsWith("remove_", StringComparison.InvariantCultureIgnoreCase)
                        && !Method.IsGenericMethod)
                    {
                        AssembliesUsing.AddIfUnique(GetAssemblies(Method.ReturnType));
                        Method.GetParameters().ForEach(x => AssembliesUsing.AddIfUnique(GetAssemblies(x.ParameterType)));
                        string Static = Method.IsStatic ? "static " : "";
                        Builder.AppendLineFormat(@"
        {4} override {0} {1}({2})
        {{
            {3}
        }}",
                                                    Static + Method.ReturnType.GetName(),
                                                    Method.Name,
                                                    Method.GetParameters().ToString(x => (x.IsOut ? "out " : "") + x.ParameterType.GetName() + " " + x.Name),
                                                    SetupMethod(Type, Method, false),
                                                    MethodAttribute);
                        MethodsAlreadyDone.Add(Method.Name);
                    }
                }
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            Builder.AppendLine(@"   }
}");

            Manager.Classes.Add(Type, Manager.Compiler.CreateClass(Namespace + "." + Type.Name + "Derived", Builder.ToString(), Usings, AssembliesUsing.ToArray()));
        }

        private static Assembly[] GetAssemblies(Type Type)
        {
            List<Assembly> Types = new List<Assembly>();
            Type TempType = Type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssemblies(x)));
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }

        private static string SetupMethod(Type Type, MethodInfo MethodInfo, bool IsProperty)
        {
            if (MethodInfo == null)
                return "";
            StringBuilder Builder = new StringBuilder();
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

        #endregion Functions
    }
}