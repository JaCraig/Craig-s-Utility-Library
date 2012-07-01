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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.IO;
using Utilities.IO.ExtensionMethods;
using Utilities.DataTypes.ExtensionMethods;
using System.Linq.Expressions;
#endregion

namespace Utilities.Reflection.ExtensionMethods
{
    /// <summary>
    /// Reflection oriented extensions
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Functions

        #region CallMethod

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType CallMethod<ReturnType>(this object Object, string MethodName, params object[] InputVariables)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(MethodName))
                throw new ArgumentNullException("MethodName");
            if (InputVariables == null)
                InputVariables = new object[0];
            Type ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new NullReferenceException("Could not find method " + MethodName + " with the appropriate input variables.");
            return (ReturnType)Method.Invoke(Object, InputVariables);
        }

        #endregion

        #region CreateInstance

        /// <summary>
        /// Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static ClassType CreateInstance<ClassType>(this Type Type,params object[] args)
        {
            if (Type == null)
                throw new ArgumentNullException("Type");
            return (ClassType)Type.CreateInstance(args);
        }

        /// <summary>
        /// Creates an instance of the type
        /// </summary>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object CreateInstance(this Type Type, params object[] args)
        {
            if (Type == null)
                throw new ArgumentNullException("Type");
            return Activator.CreateInstance(Type, args);
        }

        #endregion

        #region DumpProperties

        /// <summary>
        /// Dumps the property names and current values from an object
        /// </summary>
        /// <param name="Object">Object to dunp</param>
        /// <param name="HTMLOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        public static string DumpProperties(this object Object, bool HTMLOutput = true)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            StringBuilder TempValue = new StringBuilder();
            TempValue.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            Type ObjectType = Object.GetType();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                TempValue.Append(HTMLOutput ? "<tr><td>" : "").Append(Property.Name).Append(HTMLOutput ? "</td><td>" : "\t\t\t\t");
                ParameterInfo[] Parameters = Property.GetIndexParameters();
                if (Property.CanRead && Parameters.Length == 0)
                {
                    try
                    {
                        object Value = Property.GetValue(Object, null);
                        TempValue.Append(Value == null ? "null" : Value.ToString());
                    }
                    catch { }
                }
                TempValue.Append(HTMLOutput ? "</td></tr>" : "");
            }
            TempValue.Append(HTMLOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Dumps the properties names and current values
        /// from an object type (used for static classes)
        /// </summary>
        /// <param name="ObjectType">Object type to dunp</param>
        /// <param name="HTMLOutput">Should this be output as an HTML string</param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string DumpProperties(this Type ObjectType, bool HTMLOutput = true)
        {
            if (ObjectType == null)
                throw new ArgumentNullException("ObjectType");
            StringBuilder TempValue = new StringBuilder();
            TempValue.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            PropertyInfo[] Properties = ObjectType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                TempValue.Append(HTMLOutput ? "<tr><td>" : "").Append(Property.Name).Append(HTMLOutput ? "</td><td>" : "\t\t\t\t");
                if (Property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        TempValue.Append(Property.GetValue(null, null) == null ? "null" : Property.GetValue(null, null).ToString());
                    }
                    catch { }
                }
                TempValue.Append(HTMLOutput ? "</td></tr>" : "");
            }
            TempValue.Append(HTMLOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        #endregion

        #region GetAttribute

        /// <summary>
        /// Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Attribute specified if it exists</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider Provider, bool Inherit=true) where T : Attribute
        {
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.GetAttributes<T>(Inherit)[0] : default(T);
        }

        #endregion

        #region GetAttributes

        /// <summary>
        /// Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Array of attributes</returns>
        public static T[] GetAttributes<T>(this ICustomAttributeProvider Provider, bool Inherit=true) where T:Attribute
        {
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.GetCustomAttributes(typeof(T), Inherit).ToArray(x => (T)x) : new T[0];
        }

        #endregion

        #region GetName

        /// <summary>
        /// Returns the type's name (Actual C# name, not the funky version from
        /// the Name property)
        /// </summary>
        /// <param name="ObjectType">Type to get the name of</param>
        /// <returns>string name of the type</returns>
        public static string GetName(this Type ObjectType)
        {
            if (ObjectType == null)
                throw new ArgumentNullException("ObjectType");
            StringBuilder Output = new StringBuilder();
            if (ObjectType.Name == "Void")
            {
                Output.Append("void");
            }
            else
            {
                if (ObjectType.Name.Contains("`"))
                {
                    Type[] GenericTypes = ObjectType.GetGenericArguments();
                    Output.Append(ObjectType.Name.Remove(ObjectType.Name.IndexOf("`")))
                        .Append("<");
                    string Seperator = "";
                    foreach (Type GenericType in GenericTypes)
                    {
                        Output.Append(Seperator).Append(GenericType.GetName());
                        Seperator = ",";
                    }
                    Output.Append(">");
                }
                else
                {
                    Output.Append(ObjectType.Name);
                }
            }
            return Output.ToString();
        }

        #endregion

        #region GetObjects

        /// <summary>
        /// Returns an instance of all classes that it finds within an assembly
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assembly">Assembly to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> GetObjects<ClassType>(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            System.Collections.Generic.List<ClassType> ReturnValues = new System.Collections.Generic.List<ClassType>();
            foreach (Type Type in Assembly.GetTypes<ClassType>().Where(x => !x.ContainsGenericParameters))
                ReturnValues.Add(Type.CreateInstance<ClassType>());
            return ReturnValues;
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a group of assemblies
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assemblies">Assemblies to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> GetObjects<ClassType>(this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            List<ClassType> ReturnValues = new List<ClassType>();
            foreach (Assembly Assembly in Assemblies)
                ReturnValues.AddRange(Assembly.GetObjects<ClassType>());
            return ReturnValues;
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a directory
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Directory">Directory to search within</param>
        /// <param name="Recursive">Should this be recursive</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> GetObjects<ClassType>(this DirectoryInfo Directory, bool Recursive = false)
        {
            if (Directory == null)
                throw new ArgumentNullException("Directory");
            return Directory.LoadAssemblies(Recursive).GetObjects<ClassType>();
        }

        #endregion

        #region GetProperty

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object GetProperty(this object Object, PropertyInfo Property)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Property == null)
                throw new ArgumentNullException("Property");
            return Property.GetValue(Object, null);
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object GetProperty(this object Object, string Property)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(Property))
                throw new ArgumentNullException("Property");
            string[] Properties = Property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = Object;
            Type TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return null;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            return TempObject.GetProperty(DestinationProperty);
        }

        #endregion

        #region GetPropertyGetter

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, DataType>> GetPropertyGetter<ClassType, DataType>(this PropertyInfo Property)
        {
            if (!IsOfType(Property.PropertyType, typeof(DataType)))
                throw new ArgumentException("Property is not of the type specified");
            if (!IsOfType(Property.DeclaringType, typeof(ClassType)))
                throw new ArgumentException("Property is not from the declaring class type specified");
            ParameterExpression ObjectInstance = Expression.Parameter(Property.DeclaringType, "x");
            MemberExpression PropertyGet = Expression.Property(ObjectInstance, Property);
            if (Property.PropertyType != typeof(DataType))
            {
                UnaryExpression Convert = Expression.Convert(PropertyGet, typeof(DataType));
                return Expression.Lambda<Func<ClassType, DataType>>(Convert, ObjectInstance);
            }
            return Expression.Lambda<Func<ClassType, DataType>>(PropertyGet, ObjectInstance);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, object>> GetPropertyGetter<ClassType>(this PropertyInfo Property)
        {
            return Property.GetPropertyGetter<ClassType, object>();
        }

        #endregion

        #region GetPropertyName

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type of the property</typeparam>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName<ClassType, DataType>(this Expression<Func<ClassType, DataType>> Expression)
        {
            if (Expression.Body is UnaryExpression && Expression.Body.NodeType == ExpressionType.Convert)
            {
                MemberExpression Temp = (MemberExpression)((UnaryExpression)Expression.Body).Operand;
                return GetPropertyName(Temp.Expression) + Temp.Member.Name;
            }
            if (!(Expression.Body is MemberExpression))
                throw new ArgumentException("Expression.Body is not a MemberExpression");
            return ((MemberExpression)Expression.Body).Expression.GetPropertyName() + ((MemberExpression)Expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName(this Expression Expression)
        {
            if (!(Expression is MemberExpression))
                return "";
            return ((MemberExpression)Expression).Expression.GetPropertyName() + ((MemberExpression)Expression).Member.Name + ".";
        }

        #endregion

        #region GetPropertyType

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="Object">object who contains the property</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(this object Object, string PropertyPath)
        {
            if (Object == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            return Object.GetType().GetPropertyType(PropertyPath);
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(this Type ObjectType, string PropertyPath)
        {
            if (ObjectType == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            string[] SourceProperties = PropertyPath.Split(new string[] { "." }, StringSplitOptions.None);
            PropertyInfo PropertyInfo = null;
            for (int x = 0; x < SourceProperties.Length; ++x)
            {
                PropertyInfo = ObjectType.GetProperty(SourceProperties[x]);
                ObjectType = PropertyInfo.PropertyType;
            }
            return ObjectType;
        }

        #endregion

        #region GetPropertySetter

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, DataType>> GetPropertySetter<ClassType, DataType>(this Expression<Func<ClassType, DataType>> Property)
        {
            if (Property == null)
                throw new ArgumentNullException("Property");
            string PropertyName = Property.GetPropertyName();
            string[] SplitName = PropertyName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo PropertyInfo = typeof(ClassType).GetProperty(SplitName[0]);
            ParameterExpression ObjectInstance = Expression.Parameter(PropertyInfo.DeclaringType, "x");
            ParameterExpression PropertySet = Expression.Parameter(typeof(DataType), "y");
            MethodCallExpression SetterCall = null;
            MemberExpression PropertyGet = null;
            if (SplitName.Length > 1)
            {
                PropertyGet = Expression.Property(ObjectInstance, PropertyInfo);
                for (int x = 1; x < SplitName.Length - 1; ++x)
                {
                    PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[x]);
                    PropertyGet = Expression.Property(PropertyGet, PropertyInfo);
                }
                PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[SplitName.Length - 1]);
            }
            if (PropertyInfo.PropertyType != typeof(DataType))
            {
                UnaryExpression Convert = Expression.Convert(PropertySet, PropertyInfo.PropertyType);
                if (PropertyGet == null)
                    SetterCall = Expression.Call(ObjectInstance, PropertyInfo.GetSetMethod(), Convert);
                else
                    SetterCall = Expression.Call(PropertyGet, PropertyInfo.GetSetMethod(), Convert);
                return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
            }
            if (PropertyGet == null)
                SetterCall = Expression.Call(ObjectInstance, PropertyInfo.GetSetMethod(), PropertySet);
            else
                SetterCall = Expression.Call(PropertyGet, PropertyInfo.GetSetMethod(), PropertySet);
            return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, object>> GetPropertySetter<ClassType>(this Expression<Func<ClassType, object>> Property)
        {
            return Property.GetPropertySetter<ClassType, object>();
        }

        #endregion

        #region GetTypes

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes<BaseType>(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            return Assembly.GetTypes(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes(this Assembly Assembly,Type BaseType)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            if (BaseType == null)
                throw new ArgumentNullException("BaseType");
            return Assembly.GetTypes().Where(x => x.IsOfType(BaseType) && x.IsClass && !x.IsAbstract);
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes<BaseType>(this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            return Assemblies.GetTypes(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> Assemblies, Type BaseType)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            if (BaseType == null)
                throw new ArgumentNullException("BaseType");
            List<Type> ReturnValues = new List<Type>();
            Assemblies.ForEach(y => ReturnValues.AddRange(y.GetTypes(BaseType)));
            return ReturnValues;
        }

        #endregion

        #region HasDefaultConstructor

        /// <summary>
        /// Determines if the type has a default constructor
        /// </summary>
        /// <param name="Type">Type to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public static bool HasDefaultConstructor(this Type Type)
        {
            Type.ThrowIfNull("Type");
            return Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                        .Any(x => x.GetParameters().Length == 0);
        }

        #endregion

        #region IsIEnumerable

        /// <summary>
        /// Simple function to determine if an item is an IEnumerable
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsIEnumerable(this Type ObjectType)
        {
            return ObjectType.IsOfType(typeof(IEnumerable));
        }

        #endregion

        #region IsOfType

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this object Object, Type Type)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Type == null)
                throw new ArgumentNullException("Type");
            return Object.GetType().IsOfType(Type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this Type ObjectType, Type Type)
        {
            if (ObjectType == null)
                return false;
            if (Type == null)
                throw new ArgumentNullException("Type");
            if (Type == ObjectType || ObjectType.GetInterfaces().Any(x => x == Type))
                return true;
            if (ObjectType.BaseType == null)
                return false;
            return ObjectType.BaseType.IsOfType(Type);
        }

        #endregion

        #region Load

        /// <summary>
        /// Loads an assembly by its name
        /// </summary>
        /// <param name="Name">Name of the assembly to return</param>
        /// <returns>The assembly specified if it exists</returns>
        public static System.Reflection.Assembly Load(this AssemblyName Name)
        {
            Name.ThrowIfNull("Name");
            return AppDomain.CurrentDomain.Load(Name);
        }

        #endregion

        #region LoadAssemblies

        /// <summary>
        /// Loads assemblies within a directory and returns them in an array.
        /// </summary>
        /// <param name="Directory">The directory to search in</param>
        /// <param name="Recursive">Determines whether to search recursively or not</param>
        /// <returns>Array of assemblies in the directory</returns>
        public static IEnumerable<Assembly> LoadAssemblies(this DirectoryInfo Directory, bool Recursive = false)
        {
            foreach (FileInfo File in Directory.GetFiles("*.dll", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                yield return AssemblyName.GetAssemblyName(File.FullName).Load();
        }

        #endregion

        #region MarkedWith

        /// <summary>
        /// Goes through a list of types and determines if they're marked with a specific attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Types">Types to check</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>The list of types that are marked with an attribute</returns>
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> Types, bool Inherit = true) where T : Attribute
        {
            if(Types==null)
                return null;
            return Types.Where(x => x.IsDefined(typeof(T), Inherit) && !x.IsAbstract);
        }

        #endregion

        #region MakeShallowCopy

        /// <summary>
        /// Makes a shallow copy of the object
        /// </summary>
        /// <param name="Object">Object to copy</param>
        /// <param name="SimpleTypesOnly">If true, it only copies simple types (no classes, only items like int, string, etc.), false copies everything.</param>
        /// <returns>A copy of the object</returns>
        public static T MakeShallowCopy<T>(this T Object, bool SimpleTypesOnly=false)
        {
            if (Object == null)
                return default(T);
            Type ObjectType = Object.GetType();
            T ClassInstance = ObjectType.CreateInstance<T>();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                try
                {
                        if (Property.CanRead 
                                && Property.CanWrite
                                && SimpleTypesOnly 
                                && Property.PropertyType.IsValueType)
                            Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                        else if (!SimpleTypesOnly
                                    &&Property.CanRead 
                                    && Property.CanWrite)
                            Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                }
                catch { }
            }

            foreach (FieldInfo Field in ObjectType.GetFields())
            {
                try
                {
                    if (SimpleTypesOnly&&Field.IsPublic)
                        Field.SetValue(ClassInstance, Field.GetValue(Object));
                    else if(!SimpleTypesOnly&&Field.IsPublic)
                        Field.SetValue(ClassInstance, Field.GetValue(Object));
                }
                catch { }
            }

            return ClassInstance;
        }

        #endregion

        #region SetProperty

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object SetProperty(this object Object, PropertyInfo Property, object Value, string Format = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Property == null)
                throw new ArgumentNullException("Property");
            if (Value == null)
                throw new ArgumentNullException("Value");
            if(Property.PropertyType==typeof(string))
                Value=Value.FormatToString(Format);
            if(!Value.GetType().IsOfType(Property.PropertyType))
                Value=Convert.ChangeType(Value,Property.PropertyType);
            Property.SetValue(Object, Value, null);
            return Object;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object SetProperty(this object Object, string Property, object Value, string Format = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(Property))
                throw new ArgumentNullException("Property");
            if (Value == null)
                throw new ArgumentNullException("Value");
            string[] Properties = Property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = Object;
            Type TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return Object;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            TempObject.SetProperty(DestinationProperty, Value, Format);
            return Object;
        }

        #endregion

        #region ToLongVersionString

        /// <summary>
        /// Gets the long version of the version information
        /// </summary>
        /// <param name="Assembly">Assembly to get version information from</param>
        /// <returns>The long version of the version information</returns>
        public static string ToLongVersionString(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            return Assembly.GetName().Version.ToString();
        }

        #endregion

        #region ToShortVersionString

        /// <summary>
        /// Gets the short version of the version information
        /// </summary>
        /// <param name="Assembly">Assembly to get version information from</param>
        /// <returns>The short version of the version information</returns>
        public static string ToShortVersionString(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            Version VersionInfo=Assembly.GetName().Version;
            return VersionInfo.Major + "." + VersionInfo.Minor;
        }

        #endregion

        #endregion
    }
}