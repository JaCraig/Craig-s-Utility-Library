/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Collections;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using Utilities.Reflection.AOP.Interfaces;
using Utilities.Reflection.Emit.Interfaces;
using Utilities.Reflection.AOP.EventArgs;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit;
using Utilities.Reflection.ExtensionMethods;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Reflection.AOP
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
        /// <param name="AspectLocation">Aspect DLL location (optional)</param>
        /// <param name="AssemblyDirectory">Directory to save the generated types (optional)</param>
        /// <param name="AssemblyName">Assembly name to save the generated types as (optional)</param>
        /// <param name="RegenerateAssembly">Should this assembly be regenerated if found? (optional)</param>
        public AOPManager(string AspectLocation = "", string AssemblyDirectory = "", string AssemblyName = "Aspects", bool RegenerateAssembly = false)
        {
            this.AssemblyDirectory = AssemblyDirectory;
            this.AssemblyName = AssemblyName;
            this.RegenerateAssembly = RegenerateAssembly;
            if (!string.IsNullOrEmpty(AspectLocation))
            {
                if (AspectLocation.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                    Aspects.AddRange(new AssemblyName(AspectLocation).Load().GetObjects<IAspect>());
                else if (new DirectoryInfo(AspectLocation).Exists)
                    Aspects.AddRange(new DirectoryInfo(AspectLocation).GetObjects<IAspect>());
                else
                    Aspects.AddRange(new AssemblyName(AspectLocation).Load().GetObjects<IAspect>());
            }
            if (AssemblyBuilder != null)
                return;
            if (string.IsNullOrEmpty(AssemblyDirectory)
                || !new FileInfo(AssemblyDirectory + AssemblyName + ".dll").Exists
                || RegenerateAssembly)
            {
                AssemblyBuilder = new Utilities.Reflection.Emit.Assembly(AssemblyName, AssemblyDirectory);
            }
            else
            {
                new AssemblyName(AssemblyDirectory + AssemblyName + ".dll").Load()
                                                                           .GetTypes()
                                                                           .ForEach(x => Classes.Add(x.BaseType, x));
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Adds an aspect to the manager (only needed if loading aspects manually)
        /// </summary>
        /// <param name="Aspect">Aspect to load</param>
        public virtual void AddAspect(IAspect Aspect)
        {
            Aspects.AddIfUnique(Aspect);
        }

        /// <summary>
        /// Saves the assembly to the directory
        /// </summary>
        public virtual void Save()
        {
            if (!string.IsNullOrEmpty(AssemblyDirectory)
                && (!new FileInfo(AssemblyDirectory + AssemblyName + ".dll").Exists
                || RegenerateAssembly))
            {
                AssemblyBuilder.Create();
            }
        }

        /// <summary>
        /// Sets up a type so it can be used in the system later
        /// </summary>
        /// <param name="Type">Type to set up</param>
        public virtual void Setup(Type Type)
        {
            if (Classes.ContainsKey(Type))
                return;
            if (new FileInfo(AssemblyDirectory + AssemblyName + ".dll").Exists
                && !RegenerateAssembly)
                throw new ArgumentException("Type specified not found and can't be generated due to being in 'GoDaddy' mode. Delete already generated DLL to add new types or set RegenerateAssembly to true.");
            List<Type> Interfaces = new List<Type>();
            Aspects.ForEach(x => Interfaces.AddRange(x.InterfacesUsing == null ? new List<Type>() : x.InterfacesUsing));
            Interfaces.Add(typeof(IEvents));
            Utilities.Reflection.Emit.TypeBuilder Builder = AssemblyBuilder.CreateType(AssemblyName + "." + Type.Name + "Derived",
                            System.Reflection.TypeAttributes.Class | System.Reflection.TypeAttributes.Public,
                            Type,
                            Interfaces);
            {
                IPropertyBuilder AspectusStarting = Builder.CreateDefaultProperty("Aspectus_Starting", typeof(EventHandler<Starting>));
                IPropertyBuilder AspectusEnding = Builder.CreateDefaultProperty("Aspectus_Ending", typeof(EventHandler<Ending>));
                IPropertyBuilder AspectusException = Builder.CreateDefaultProperty("Aspectus_Exception", typeof(EventHandler<Utilities.Reflection.AOP.EventArgs.Exception>));

                Aspects.ForEach(x => x.SetupInterfaces(Builder));

                Type TempType = Type;
                List<string> MethodsAlreadyDone = new List<string>();
                while (TempType != null)
                {
                    foreach (PropertyInfo Property in TempType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                    {
                        MethodInfo GetMethodInfo = Property.GetGetMethod();
                        if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                            && !MethodsAlreadyDone.Contains("set_" + Property.Name)
                            && GetMethodInfo != null
                            && GetMethodInfo.IsVirtual
                            && !GetMethodInfo.IsFinal)
                        {
                            IPropertyBuilder OverrideProperty = Builder.CreateProperty(Property.Name, Property.PropertyType,
                                System.Reflection.PropertyAttributes.SpecialName,
                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual);
                            {
                                IMethodBuilder Get = OverrideProperty.GetMethod;
                                {
                                    SetupMethod(Type, Get, AspectusStarting, AspectusEnding, AspectusException, null);
                                    MethodsAlreadyDone.Add(Get.Name);
                                }
                                IMethodBuilder Set = OverrideProperty.SetMethod;
                                {
                                    SetupMethod(Type, Set, AspectusStarting, AspectusEnding, AspectusException, null);
                                    MethodsAlreadyDone.Add(Set.Name);
                                }
                            }
                        }
                    }
                    foreach (MethodInfo Method in TempType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                    {
                        if (!MethodsAlreadyDone.Contains(Method.Name) && Method.IsVirtual && !Method.IsFinal)
                        {
                            MethodAttributes MethodAttribute = MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Public;
                            if (Method.IsStatic)
                                MethodAttribute |= MethodAttributes.Static;
                            List<Type> ParameterTypes = new List<Type>();
                            Method.GetParameters().ForEach(x => ParameterTypes.Add(x.ParameterType));
                            IMethodBuilder OverrideMethod = Builder.CreateMethod(Method.Name,
                                MethodAttribute,
                                Method.ReturnType,
                                ParameterTypes);
                            SetupMethod(Type, OverrideMethod, AspectusStarting, AspectusEnding, AspectusException, Method);
                            MethodsAlreadyDone.Add(Method.Name);
                        }
                    }

                    TempType = TempType.BaseType;
                    if (TempType == typeof(object))
                        break;
                }

                Classes.Add(Type, Builder.Create());
            }
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

        #endregion

        #region Private Functions

        private void SetupMethod(Type BaseType, IMethodBuilder Method, IPropertyBuilder AspectusStarting,
            IPropertyBuilder AspectusEnding, IPropertyBuilder AspectusException, MethodInfo BaseMethod)
        {
            if (BaseMethod == null)
                BaseMethod = BaseType.GetMethod(Method.Name);
            Method.SetCurrentMethod();
            System.Reflection.Emit.Label EndLabel = Method.Generator.DefineLabel();
            VariableBase ReturnValue = Method.ReturnType != typeof(void) ? Method.CreateLocal("FinalReturnValue", Method.ReturnType) : null;
            Utilities.Reflection.Emit.Commands.Try Try = Method.Try();
            {
                SetupStart(Method, EndLabel, ReturnValue, AspectusStarting);
                Aspects.ForEach(x => x.SetupStartMethod(Method, BaseType));
                List<ParameterBuilder> Parameters = new List<ParameterBuilder>();
                Method.Parameters.For(1, Method.Parameters.Count - 1, x => Parameters.Add(x));
                if (Method.ReturnType != typeof(void) && BaseMethod != null)
                    ReturnValue.Assign(Method.This.Call(BaseMethod, Parameters.ToArray()));
                else if (BaseMethod != null)
                    Method.This.Call(BaseMethod, Parameters.ToArray());
                SetupEnd(Method, ReturnValue, AspectusEnding);
                Aspects.ForEach(x => x.SetupEndMethod(Method, BaseType, ReturnValue));
                Method.Generator.MarkLabel(EndLabel);
            }
            Utilities.Reflection.Emit.Commands.Catch Catch = Try.StartCatchBlock(typeof(System.Exception));
            {
                SetupException(Method, Catch, AspectusException);
                Aspects.ForEach(x => x.SetupExceptionMethod(Method, BaseType));
                Catch.Rethrow();
            }
            Try.EndTryBlock();

            if (Method.ReturnType != typeof(void))
                Method.Return(ReturnValue);
            else
                Method.Return();
        }

        private void SetupException(IMethodBuilder Method, Utilities.Reflection.Emit.Commands.Catch Catch, IPropertyBuilder AspectusException)
        {
            VariableBase ExceptionArgs = Method.NewObj(typeof(Utilities.Reflection.AOP.EventArgs.Exception).GetConstructor(new Type[0]));
            ExceptionArgs.Call(typeof(Utilities.Reflection.AOP.EventArgs.Exception).GetProperty("InternalException").GetSetMethod(), new object[] { Catch.Exception });
            VariableBase IEventsThis = Method.Cast(Method.This, typeof(IEvents));
            Type EventHelperType = typeof(Utilities.Events.EventHelper);
            MethodInfo[] Methods = EventHelperType.GetMethods()
                                                  .Where<MethodInfo>(x => x.GetParameters().Length == 3)
                                                  .ToArray();
            MethodInfo TempMethod = Methods.Length > 0 ? Methods[0] : null;
            TempMethod = TempMethod.MakeGenericMethod(new Type[] { typeof(Utilities.Reflection.AOP.EventArgs.Exception) });
            Method.Call(null, TempMethod, new object[] { AspectusException, IEventsThis, ExceptionArgs });
        }

        private void SetupEnd(IMethodBuilder Method, VariableBase ReturnValue, IPropertyBuilder AspectusEnding)
        {
            VariableBase EndingArgs = Method.NewObj(typeof(Ending).GetConstructor(new Type[0]));
            EndingArgs.Call(typeof(Ending).GetProperty("MethodName").GetSetMethod(), new object[] { Method.Name });
            if (Method.ReturnType != typeof(void) && ReturnValue.DataType != null && ReturnValue.DataType.IsValueType)
                EndingArgs.Call(typeof(Ending).GetProperty("ReturnValue").GetSetMethod(), new object[] { Method.Box(ReturnValue) });
            else if (Method.ReturnType != typeof(void))
                EndingArgs.Call(typeof(Ending).GetProperty("ReturnValue").GetSetMethod(), new object[] { ReturnValue });
            VariableBase ParameterList = EndingArgs.Call(typeof(Ending).GetProperty("Parameters").GetGetMethod());
            for (int x = 1; x < Method.Parameters.Count; ++x)
            {
                if (Method.Parameters[x].DataType != null && Method.Parameters[x].DataType.IsValueType)
                    ParameterList.Call(typeof(List<object>).GetMethod("Add"), new object[] { Method.Box(Method.Parameters[x]) });
                else
                    ParameterList.Call(typeof(List<object>).GetMethod("Add"), new object[] { Method.Parameters[x] });
            }

            VariableBase IEventsThis = Method.Cast(Method.This, typeof(IEvents));
            Type EventHelperType = typeof(Utilities.Events.EventHelper);
            MethodInfo[] Methods = EventHelperType.GetMethods()
                                                  .Where<MethodInfo>(x => x.GetParameters().Length == 3)
                                                  .ToArray();
            MethodInfo TempMethod = Methods.Length > 0 ? Methods[0] : null;
            TempMethod = TempMethod.MakeGenericMethod(new Type[] { typeof(Ending) });
            Method.Call(null, TempMethod, new object[] { AspectusEnding, IEventsThis, EndingArgs });
            if (Method.ReturnType != typeof(void))
            {
                VariableBase TempReturnValue = EndingArgs.Call(typeof(Ending).GetProperty("ReturnValue").GetGetMethod());
                VariableBase TempNull = Method.CreateLocal("TempNull", typeof(object));
                Utilities.Reflection.Emit.Commands.If If = Method.If(TempReturnValue, Utilities.Reflection.Emit.Enums.Comparison.NotEqual, TempNull);
                {
                    ReturnValue.Assign(TempReturnValue);
                }
                Method.SetCurrentMethod();
                If.EndIf();
            }
        }

        private void SetupStart(IMethodBuilder Method, System.Reflection.Emit.Label EndLabel,
            VariableBase ReturnValue, IPropertyBuilder AspectusStarting)
        {
            VariableBase StartingArgs = Method.NewObj(typeof(Starting).GetConstructor(new Type[0]));
            StartingArgs.Call(typeof(Starting).GetProperty("MethodName").GetSetMethod(), new object[] { Method.Name });

            VariableBase ParameterList = StartingArgs.Call(typeof(Starting).GetProperty("Parameters").GetGetMethod());
            for (int x = 1; x < Method.Parameters.Count; ++x)
            {
                if (Method.Parameters[x].DataType != null && Method.Parameters[x].DataType.IsValueType)
                    ParameterList.Call(typeof(List<object>).GetMethod("Add"), new object[] { Method.Box(Method.Parameters[x]) });
                else
                    ParameterList.Call(typeof(List<object>).GetMethod("Add"), new object[] { Method.Parameters[x] });
            }

            VariableBase IEventsThis = Method.Cast(Method.This, typeof(IEvents));
            Type EventHelperType = typeof(Utilities.Events.EventHelper);
            MethodInfo[] Methods = EventHelperType.GetMethods()
                                                  .Where<MethodInfo>(x => x.GetParameters().Length == 3)
                                                  .ToArray();
            MethodInfo TempMethod = Methods.Length > 0 ? Methods[0] : null;
            TempMethod = TempMethod.MakeGenericMethod(new Type[] { typeof(Starting) });
            Method.Call(null, TempMethod, new object[] { AspectusStarting, IEventsThis, StartingArgs });
            if (Method.ReturnType != typeof(void))
            {
                VariableBase TempReturnValue = StartingArgs.Call(typeof(Starting).GetProperty("ReturnValue").GetGetMethod());
                VariableBase TempNull = Method.CreateLocal("TempNull", typeof(object));
                Utilities.Reflection.Emit.Commands.If If = Method.If(TempReturnValue, Utilities.Reflection.Emit.Enums.Comparison.NotEqual, TempNull);
                {
                    ReturnValue.Assign(TempReturnValue);
                    Method.Generator.Emit(System.Reflection.Emit.OpCodes.Br, EndLabel);
                }
                Method.SetCurrentMethod();
                If.EndIf();
            }
        }

        #endregion

        #region Properties/Fields

        /// <summary>
        /// Assembly containing generated types
        /// </summary>
        protected static Utilities.Reflection.Emit.Assembly AssemblyBuilder { get; set; }

        /// <summary>
        /// Dictionary containing generated types and associates it with original type
        /// </summary>
        protected static Dictionary<Type, Type> Classes = new Dictionary<Type, Type>();

        /// <summary>
        /// The list of aspects that are being used
        /// </summary>
        protected static List<IAspect> Aspects = new List<IAspect>();

        /// <summary>
        /// Assembly directory
        /// </summary>
        protected virtual string AssemblyDirectory { get; set; }

        /// <summary>
        /// Assembly name
        /// </summary>
        protected virtual string AssemblyName { get; set; }

        /// <summary>
        /// Determines if the assembly needs to be regenerated
        /// </summary>
        protected virtual bool RegenerateAssembly { get; set; }

        #endregion
    }
}