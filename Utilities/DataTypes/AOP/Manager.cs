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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.AOP.Interfaces;
using Utilities.DataTypes.CodeGen;
using Utilities.DataTypes;
#endregion

namespace Utilities.DataTypes.AOP
{
    /// <summary>
    /// AOP interface manager
    /// </summary>
    public class AOPManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AOPManager()
        {
            Aspects.Add(AppDomain.CurrentDomain.GetAssemblies().Objects<IAspect>());
            Compiler.Classes.ForEach(x => Classes.Add(x.BaseType, x));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the system's compiler
        /// </summary>
        protected static Compiler Compiler { get { return IoC.Manager.Bootstrapper.Resolve<Compiler>(); } }

        /// <summary>
        /// Dictionary containing generated types and associates it with original type
        /// </summary>
        private static IDictionary<Type, Type> Classes = new Dictionary<Type, Type>();

        /// <summary>
        /// The list of aspects that are being used
        /// </summary>
        private static ICollection<IAspect> Aspects = new List<IAspect>();

        #endregion

        #region Functions

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
            Aspects.ForEach(x => AssembliesUsing.AddIfUnique(x.AssembliesUsing));
            Usings.Add("System");
            Usings.Add("System.Collections.Generic");
            Usings.Add("System.Linq");

            
            Aspects.ForEach(x => Usings.AddIfUnique(x.Usings));

            List<Type> Interfaces = new List<Type>();
            Aspects.ForEach(x => Interfaces.AddRange(x.InterfacesUsing == null ? new List<Type>() : x.InterfacesUsing));
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat(@"{0}
namespace {1}
{{
    public class {2} : {3}{4} {5}
    {{
", Usings.ToString(x => "using " + x + ";", "\r\n"),
 "CULGeneratedTypes", 
 Type.Name + "Derived", 
 Type.Namespace + "." + Type.Name, 
 Interfaces.Count > 0 ? "," : "", Interfaces.ToString(x => x.Name));

                Aspects.ForEach(x => Builder.AppendLine(x.SetupInterfaces(Type)));
            
                Type TempType = Type;
                List<string> MethodsAlreadyDone = new List<string>();
                while (TempType != null)
                {
                    foreach (PropertyInfo Property in TempType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                    {
                        MethodInfo GetMethodInfo = Property.GetGetMethod();
                        MethodInfo SetMethodInfo = Property.GetSetMethod();
                        if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                            && !MethodsAlreadyDone.Contains("set_" + Property.Name)
                            && GetMethodInfo != null
                            && GetMethodInfo.IsVirtual
                            && !GetMethodInfo.IsFinal)
                        {
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
                                                        SetupMethod(Type, GetMethodInfo),
                                                        SetupMethod(Type, SetMethodInfo));
                            MethodsAlreadyDone.Add(GetMethodInfo.Name);
                            MethodsAlreadyDone.Add(SetMethodInfo.Name);
                        }
                    }
                    foreach (MethodInfo Method in TempType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                    {
                        if (!MethodsAlreadyDone.Contains(Method.Name) && Method.IsVirtual && !Method.IsFinal)
                        {
                            string Static = Method.IsStatic ? "static " : "";
                            Builder.AppendLineFormat(@"
        public override {0} {1}({2})
        {{ 
            {3}
        }}",
                                                        Static + Method.ReturnType.GetName(),
                                                        Method.Name,
                                                        Method.GetParameters().ToString(x => x.ParameterType.GetName() + " " + x.Name),
                                                        SetupMethod(Type, Method));
                            MethodsAlreadyDone.Add(Method.Name);
                        }
                    }
                    TempType = TempType.BaseType;
                    if (TempType == typeof(object))
                        break;
                }
                Builder.AppendLine(@"   }
}");

                AOPManager.Classes.Add(Type, AOPManager.Compiler.CreateClass(Type.Name + "Derived", Builder.ToString(), Usings, AssembliesUsing.ToArray()));
        }

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


        private static string SetupMethod(Type Type, MethodInfo MethodInfo)
        {
            StringBuilder Builder = new StringBuilder();
            MethodInfo BaseMethod = Type.GetMethod(MethodInfo.Name);
            string BaseMethodName = MethodInfo.Name.Replace("get_", "").Replace("set_", "");
            string ReturnValue = MethodInfo.ReturnType != typeof(void) ? "FinalReturnValue" : "";
            string BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName + "(" : ReturnValue + "=base." + BaseMethodName + "(";
            ParameterInfo[] Parameters = MethodInfo.GetParameters();
            BaseCall += Parameters.Length > 0 ? Parameters.ToString(x => x.Name) : "";
            BaseCall += ");\r\n";
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

        #endregion
    }
}