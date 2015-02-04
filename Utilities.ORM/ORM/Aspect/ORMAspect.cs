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
using System.Reflection;
using System.Reflection.Emit;
using Utilities.ORM.Aspect.Interfaces;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.Reflection.AOP.Interfaces;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Enums;
using Utilities.Reflection.Emit.Interfaces;
using Utilities.SQL.Interfaces;
using Utilities.SQL.ParameterTypes;

#endregion Usings

namespace Utilities.ORM.Aspect
{
    /// <summary>
    /// ORM Aspect (used internally)
    /// </summary>
    public class ORMAspect : IAspect
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ORMAspect(Utilities.DataTypes.ListMapping<Type, IMapping> Mappings)
        {
            this.InterfacesUsing = new List<Type>();
            this.InterfacesUsing.Add(typeof(IORMObject));
            ClassMappings = Mappings;
        }

        #endregion Constructor

        #region Functions

        /// <summary>
        /// Sets up an object
        /// </summary>
        /// <param name="Object">Object to set up</param>
        public void Setup(object Object)
        {
        }

        /// <summary>
        /// Sets up the code at the end of the method
        /// </summary>
        /// <param name="Method">Method builder to use</param>
        /// <param name="BaseType">Base type</param>
        /// <param name="ReturnValue">Return value (if there is one)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        public void SetupEndMethod(Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType, Reflection.Emit.BaseClasses.VariableBase ReturnValue)
        {
            Method.SetCurrentMethod();
            if (ClassMappings.ContainsKey(BaseType)
                && Method.Name.StartsWith("get_", StringComparison.InvariantCulture))
            {
                foreach (IMapping Mapping in ClassMappings[BaseType])
                {
                    string PropertyName = Method.Name.Replace("get_", "");
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Name == PropertyName);
                    if (Property != null)
                    {
                        if (Property is IManyToOne || Property is IMap)
                            SetupSingleProperty(Method, BaseType, ReturnValue, Property, Mapping);
                        else if (Property is IIEnumerableManyToOne || Property is IManyToMany)
                            SetupIEnumerableProperty(Method, BaseType, ReturnValue, Property, Mapping);
                        else if (Property is IListManyToMany || Property is IListManyToOne)
                            SetupListProperty(Method, BaseType, ReturnValue, Property, Mapping);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Sets up the exception method
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="BaseType">Base type</param>
        public void SetupExceptionMethod(Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType)
        {
        }

        /// <summary>
        /// Set up interfaces
        /// </summary>
        /// <param name="TypeBuilder">Type builder to use</param>
        public void SetupInterfaces(Reflection.Emit.TypeBuilder TypeBuilder)
        {
            Fields = new List<Reflection.Emit.FieldBuilder>();
            SessionField = CreateProperty(TypeBuilder, "Session0", typeof(Session));
            CreateConstructor(TypeBuilder);
            SetupFields(TypeBuilder);
        }

        /// <summary>
        /// Sets up the start method
        /// </summary>
        /// <param name="Method">Method builder to use</param>
        /// <param name="BaseType">Base type</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        public void SetupStartMethod(Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType)
        {
            Method.SetCurrentMethod();
            if (ClassMappings.ContainsKey(BaseType)
                && Method.Name.StartsWith("set_", StringComparison.InvariantCulture))
            {
                foreach (IMapping Mapping in ClassMappings[BaseType])
                {
                    string PropertyName = Method.Name.Replace("set_", "");
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Name == PropertyName);
                    if (Property != null)
                    {
                        Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
                        if (Field != null)
                            Field.Assign(Method.Parameters.ElementAt(1));
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the default constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        private static void CreateConstructor(Reflection.Emit.TypeBuilder TypeBuilder)
        {
            IMethodBuilder Constructor = TypeBuilder.CreateConstructor();
            {
                Constructor.SetCurrentMethod();
                Constructor.This.Call(TypeBuilder.BaseClass.GetConstructor(Type.EmptyTypes));
                Constructor.Return();
            }
        }

        /// <summary>
        /// Creates a default property
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the property</param>
        /// <param name="PropertyType">Property type</param>
        /// <returns>The property builder</returns>
        private static IPropertyBuilder CreateProperty(Reflection.Emit.TypeBuilder TypeBuilder, string Name, Type PropertyType)
        {
            return TypeBuilder.CreateDefaultProperty(Name, PropertyType, PropertyAttributes.SpecialName,
                MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Public,
                MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Public);
        }

        /// <summary>
        /// Sets up the fields needed to store the data for lazy loading
        /// </summary>
        /// <param name="TypeBuilder"></param>
        private void SetupFields(Reflection.Emit.TypeBuilder TypeBuilder)
        {
            if (ClassMappings.ContainsKey(TypeBuilder.BaseClass))
            {
                foreach (IMapping Mapping in ClassMappings[TypeBuilder.BaseClass])
                {
                    foreach (IProperty Property in Mapping.Properties)
                    {
                        if (Property is IManyToOne || Property is IMap)
                        {
                            if (Fields.FirstOrDefault(x => x.Name == Property.DerivedFieldName) == null)
                            {
                                Fields.Add(TypeBuilder.CreateField(Property.DerivedFieldName, Property.Type));
                                Fields.Add(TypeBuilder.CreateField(Property.DerivedFieldName + "_Loaded", typeof(bool)));
                            }
                        }
                        else if (Property is IIEnumerableManyToOne || Property is IManyToMany)
                        {
                            if (Fields.FirstOrDefault(x => x.Name == Property.DerivedFieldName) == null)
                            {
                                Fields.Add(TypeBuilder.CreateField(Property.DerivedFieldName, typeof(IEnumerable<>).MakeGenericType(Property.Type)));
                                Fields.Add(TypeBuilder.CreateField(Property.DerivedFieldName + "_Loaded", typeof(bool)));
                            }
                        }
                        else if (Property is IListManyToOne || Property is IListManyToMany)
                        {
                            if (Fields.FirstOrDefault(x => x.Name == Property.DerivedFieldName) == null)
                            {
                                Fields.Add(TypeBuilder.CreateField(Property.DerivedFieldName, typeof(List<>).MakeGenericType(Property.Type)));
                                Fields.Add(TypeBuilder.CreateField(Property.DerivedFieldName + "_Loaded", typeof(bool)));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets up a property (IEnumerable)
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="BaseType">Base type for the object</param>
        /// <param name="ReturnValue">Return value</param>
        /// <param name="Property">Property info</param>
        /// <param name="Mapping">Mapping info</param>
        private void SetupIEnumerableProperty(IMethodBuilder Method, Type BaseType, Reflection.Emit.BaseClasses.VariableBase ReturnValue, IProperty Property, IMapping Mapping)
        {
            Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
            Utilities.Reflection.Emit.FieldBuilder FieldLoaded = Fields.Find(x => x.Name == Property.DerivedFieldName + "_Loaded");
            Utilities.Reflection.Emit.Commands.If If1 = Method.If((VariableBase)SessionField, Comparison.NotEqual, null);
            {
                Utilities.Reflection.Emit.Commands.If If2 = Method.If(Field, Comparison.Equal, null);
                {
                    Utilities.Reflection.Emit.Commands.If If3 = Method.If(FieldLoaded, Comparison.Equal, Method.CreateConstant(false));
                    {
                        //Load data
                        VariableBase IDValue = Method.This.Call(BaseType.GetProperty(Mapping.IDProperty.Name).GetGetMethod());
                        VariableBase IDParameter = Method.NewObj(typeof(EqualParameter<>).MakeGenericType(Mapping.IDProperty.Type), new object[] { IDValue, "ID", "@" });
                        VariableBase PropertyList = Method.NewObj(typeof(List<IParameter>));
                        PropertyList.Call("Add", new object[] { IDParameter });
                        MethodInfo LoadPropertiesMethod = typeof(Session).GetMethod("LoadProperties");
                        LoadPropertiesMethod = LoadPropertiesMethod.MakeGenericMethod(new Type[] { BaseType, Field.DataType.GetGenericArguments()[0] });
                        VariableBase ReturnVal = ((VariableBase)SessionField).Call(LoadPropertiesMethod, new object[] { Method.This, Property.Name, PropertyList.Call("ToArray") });
                        Field.Assign(ReturnVal);
                        FieldLoaded.Assign(true);
                    }
                    If3.EndIf();
                }
                If2.EndIf();
                Utilities.Reflection.Emit.Commands.If If4 = Method.If(Field, Comparison.Equal, null);
                {
                    Field.Assign(Method.NewObj(typeof(List<>).MakeGenericType(Property.Type).GetConstructor(Type.EmptyTypes)));
                }
                If4.EndIf();
            }
            If1.EndIf();
            ReturnValue.Assign(Field);
        }

        /// <summary>
        /// Sets up a property (List)
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="BaseType">Base type for the object</param>
        /// <param name="ReturnValue">Return value</param>
        /// <param name="Property">Property info</param>
        /// <param name="Mapping">Mapping info</param>
        private void SetupListProperty(IMethodBuilder Method, Type BaseType, VariableBase ReturnValue, IProperty Property, IMapping Mapping)
        {
            Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
            Utilities.Reflection.Emit.FieldBuilder FieldLoaded = Fields.Find(x => x.Name == Property.DerivedFieldName + "_Loaded");
            Utilities.Reflection.Emit.Commands.If If1 = Method.If((VariableBase)SessionField, Comparison.NotEqual, null);
            {
                Utilities.Reflection.Emit.Commands.If If2 = Method.If(Field, Comparison.Equal, null);
                {
                    Utilities.Reflection.Emit.Commands.If If3 = Method.If(FieldLoaded, Comparison.Equal, Method.CreateConstant(false));
                    {
                        //Load data
                        VariableBase IDValue = Method.This.Call(BaseType.GetProperty(Mapping.IDProperty.Name).GetGetMethod());
                        VariableBase IDParameter = Method.NewObj(typeof(EqualParameter<>).MakeGenericType(Mapping.IDProperty.Type), new object[] { IDValue, "ID", "@" });
                        VariableBase PropertyList = Method.NewObj(typeof(List<IParameter>));
                        PropertyList.Call("Add", new object[] { IDParameter });
                        MethodInfo LoadPropertiesMethod = typeof(Session).GetMethod("LoadListProperties");
                        LoadPropertiesMethod = LoadPropertiesMethod.MakeGenericMethod(new Type[] { BaseType, Field.DataType.GetGenericArguments()[0] });
                        VariableBase ReturnVal = ((VariableBase)SessionField).Call(LoadPropertiesMethod, new object[] { Method.This, Property.Name, PropertyList.Call("ToArray") });
                        Field.Assign(ReturnVal);
                        FieldLoaded.Assign(true);
                    }
                    If3.EndIf();
                }
                If2.EndIf();
                PropertyInfo CountProperty = Field.DataType.GetProperty("Count");
                Utilities.Reflection.Emit.Commands.If If4 = Method.If(Field.Call(CountProperty.GetGetMethod()), Comparison.Equal, Method.CreateConstant(0));
                {
                    Utilities.Reflection.Emit.Commands.If If3 = Method.If(FieldLoaded, Comparison.Equal, Method.CreateConstant(false));
                    {
                        //Load data
                        VariableBase IDValue = Method.This.Call(BaseType.GetProperty(Mapping.IDProperty.Name).GetGetMethod());
                        VariableBase IDParameter = Method.NewObj(typeof(EqualParameter<>).MakeGenericType(Mapping.IDProperty.Type), new object[] { IDValue, "ID", "@" });
                        VariableBase PropertyList = Method.NewObj(typeof(List<IParameter>));
                        PropertyList.Call("Add", new object[] { IDParameter });
                        MethodInfo LoadPropertiesMethod = typeof(Session).GetMethod("LoadProperties");
                        LoadPropertiesMethod = LoadPropertiesMethod.MakeGenericMethod(new Type[] { BaseType, Field.DataType.GetGenericArguments()[0] });
                        VariableBase ReturnVal = ((VariableBase)SessionField).Call(LoadPropertiesMethod, new object[] { Method.This, Property.Name, PropertyList.Call("ToArray") });
                        Field.Assign(ReturnVal);
                        FieldLoaded.Assign(true);
                    }
                    If3.EndIf();
                }
                If4.EndIf();
                Utilities.Reflection.Emit.Commands.If If5 = Method.If(Field, Comparison.Equal, null);
                {
                    Field.Assign(Method.NewObj(typeof(List<>).MakeGenericType(Property.Type).GetConstructor(Type.EmptyTypes)));
                }
                If5.EndIf();
            }
            If1.EndIf();
            ReturnValue.Assign(Field);
        }

        /// <summary>
        /// Sets up a property (non IEnumerable)
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="BaseType">Base type for the object</param>
        /// <param name="ReturnValue">Return value</param>
        /// <param name="Property">Property info</param>
        /// <param name="Mapping">Mapping info</param>
        private void SetupSingleProperty(IMethodBuilder Method, Type BaseType, Reflection.Emit.BaseClasses.VariableBase ReturnValue, IProperty Property, IMapping Mapping)
        {
            Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
            Utilities.Reflection.Emit.FieldBuilder FieldLoaded = Fields.Find(x => x.Name == Property.DerivedFieldName + "_Loaded");
            Utilities.Reflection.Emit.Commands.If If1 = Method.If((VariableBase)SessionField, Comparison.NotEqual, null);
            {
                Utilities.Reflection.Emit.Commands.If If2 = Method.If(Field, Comparison.Equal, null);
                {
                    Utilities.Reflection.Emit.Commands.If If3 = Method.If(FieldLoaded, Comparison.Equal, Method.CreateConstant(false));
                    {
                        //Load Data
                        VariableBase IDValue = Method.This.Call(BaseType.GetProperty(Mapping.IDProperty.Name).GetGetMethod());
                        VariableBase IDParameter = Method.NewObj(typeof(EqualParameter<>).MakeGenericType(Mapping.IDProperty.Type), new object[] { IDValue, "ID", "@" });
                        VariableBase PropertyList = Method.NewObj(typeof(List<IParameter>));
                        PropertyList.Call("Add", new object[] { IDParameter });
                        MethodInfo LoadPropertyMethod = typeof(Session).GetMethod("LoadProperty");
                        LoadPropertyMethod = LoadPropertyMethod.MakeGenericMethod(new Type[] { BaseType, Field.DataType });
                        VariableBase ReturnVal = ((VariableBase)SessionField).Call(LoadPropertyMethod, new object[] { Method.This, Property.Name, PropertyList.Call("ToArray") });
                        Field.Assign(ReturnVal);
                        FieldLoaded.Assign(true);
                    }
                    If3.EndIf();
                }
                If2.EndIf();
            }
            If1.EndIf();
            ReturnValue.Assign(Field);
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Interfaces this aspect is using
        /// </summary>
        public ICollection<Type> InterfacesUsing { get; private set; }

        /// <summary>
        /// Class mappings
        /// </summary>
        private Utilities.DataTypes.ListMapping<Type, IMapping> ClassMappings { get; set; }

        /// <summary>
        /// Fields used for storing Map, ManyToOne, and ManyToMany properties
        /// </summary>
        private List<Utilities.Reflection.Emit.FieldBuilder> Fields { get; set; }

        /// <summary>
        /// Field to store the session object
        /// </summary>
        private IPropertyBuilder SessionField { get; set; }

        #endregion Properties
    }
}