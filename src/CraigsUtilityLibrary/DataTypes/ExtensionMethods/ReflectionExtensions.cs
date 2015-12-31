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

using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Version info
    /// </summary>
    public enum VersionInfo
    {
        /// <summary>
        /// Short version
        /// </summary>
        ShortVersion = 1,

        /// <summary>
        /// Long version
        /// </summary>
        LongVersion = 2
    }

    /// <summary>
    /// Reflection oriented extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="provider">Attribute provider</param>
        /// <param name="inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>Attribute specified if it exists</returns>
        public static T Attribute<T>(this MemberInfo provider, bool inherit = true) where T : Attribute
        {
            if (provider == null)
                return default(T);
            if (provider.IsDefined(typeof(T), inherit))
            {
                T[] Attributes = provider.Attributes<T>(inherit);
                if (Attributes.Length > 0)
                    return Attributes[0];
            }
            return default(T);
        }

        /// <summary>
        /// Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="provider">Attribute provider</param>
        /// <param name="inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>Array of attributes</returns>
        public static T[] Attributes<T>(this MemberInfo provider, bool inherit = true) where T : Attribute
        {
            if (provider == null)
                return new T[0];
            return provider.IsDefined(typeof(T), inherit) ? provider.GetCustomAttributes(typeof(T), inherit).ToArray(x => (T)x) : new T[0];
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType Call<ReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject == null)
                throw new ArgumentNullException(nameof(inputObject));
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));
            if (inputVariables == null)
                inputVariables = new object[0];
            Type ObjectType = inputObject.GetType();
            Type[] MethodInputTypes = new Type[inputVariables.Length];
            for (int x = 0; x < inputVariables.Length; ++x)
                MethodInputTypes[x] = inputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            return (ReturnType)Method.Invoke(inputObject, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <typeparam name="GenericType1">Generic method type 1</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType Call<GenericType1, ReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject == null)
                throw new ArgumentNullException(nameof(inputObject));
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));
            if (inputVariables == null)
                inputVariables = new object[0];
            Type ObjectType = inputObject.GetType();
            Type[] MethodInputTypes = new Type[inputVariables.Length];
            for (int x = 0; x < inputVariables.Length; ++x)
                MethodInputTypes[x] = inputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            Method = Method.MakeGenericMethod(typeof(GenericType1));
            return inputObject.Call<ReturnType>(Method, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <typeparam name="GenericType1">Generic method type 1</typeparam>
        /// <typeparam name="GenericType2">Generic method type 2</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType Call<GenericType1, GenericType2, ReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject == null)
                throw new ArgumentNullException(nameof(inputObject));
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));
            if (inputVariables == null)
                inputVariables = new object[0];
            Type ObjectType = inputObject.GetType();
            Type[] MethodInputTypes = new Type[inputVariables.Length];
            for (int x = 0; x < inputVariables.Length; ++x)
                MethodInputTypes[x] = inputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            Method = Method.MakeGenericMethod(typeof(GenericType1), typeof(GenericType2));
            return inputObject.Call<ReturnType>(Method, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <typeparam name="GenericType1">Generic method type 1</typeparam>
        /// <typeparam name="GenericType2">Generic method type 2</typeparam>
        /// <typeparam name="GenericType3">Generic method type 3</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType Call<GenericType1, GenericType2, GenericType3, ReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject == null)
                throw new ArgumentNullException(nameof(inputObject));
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));
            if (inputVariables == null)
                inputVariables = new object[0];
            Type ObjectType = inputObject.GetType();
            Type[] MethodInputTypes = new Type[inputVariables.Length];
            for (int x = 0; x < inputVariables.Length; ++x)
                MethodInputTypes[x] = inputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            Method = Method.MakeGenericMethod(typeof(GenericType1), typeof(GenericType2), typeof(GenericType3));
            return inputObject.Call<ReturnType>(Method, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType Call<ReturnType>(this object inputObject, MethodInfo method, params object[] inputVariables)
        {
            if (inputObject == null)
                throw new ArgumentNullException(nameof(inputObject));
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (inputVariables == null)
                inputVariables = new object[0];
            return (ReturnType)method.Invoke(inputObject, inputVariables);
        }

        /// <summary>
        /// Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static ClassType Create<ClassType>(this Type type, params object[] args)
        {
            if (type == null)
                return default(ClassType);
            return (ClassType)type.Create(args);
        }

        /// <summary>
        /// Creates an instance of the type
        /// </summary>
        /// <param name="type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object Create(this Type type, params object[] args)
        {
            if (type == null)
                return null;
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Creates an instance of the types and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<ClassType> Create<ClassType>(this IEnumerable<Type> types, params object[] args)
        {
            if (types == null || types.Count() == 0)
                yield break;
            foreach (var Type in types)
                yield return Type.Create<ClassType>(args);
        }

        /// <summary>
        /// Creates an instance of the types specified
        /// </summary>
        /// <param name="types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<object> Create(this IEnumerable<Type> types, params object[] args)
        {
            if (types == null || types.Count() == 0)
                yield break;
            foreach (var Type in types)
                yield return Type.Create(args);
        }

        /// <summary>
        /// Returns the type's name (Actual C# name, not the funky version from the Name property)
        /// </summary>
        /// <param name="objectType">Type to get the name of</param>
        /// <returns>string name of the type</returns>
        public static string GetName(this Type objectType)
        {
            if (objectType == null)
                return "";
            var Output = new StringBuilder();
            if (objectType.Name == "Void")
            {
                Output.Append("void");
            }
            else
            {
                Output.Append(objectType.DeclaringType == null ? objectType.Namespace : objectType.DeclaringType.GetName())
                    .Append(".");
                if (objectType.Name.Contains("`"))
                {
                    Type[] GenericTypes = objectType.GetGenericArguments();
                    Output.Append(objectType.Name.Remove(objectType.Name.IndexOf("`", StringComparison.OrdinalIgnoreCase)))
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
                    Output.Append(objectType.Name);
                }
            }
            return Output.ToString().Replace("&", "");
        }

        /// <summary>
        /// Determines if the type has a default constructor
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null)
                return false;
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                        .Any(x => x.GetParameters().Length == 0);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="inputObject">Object</param>
        /// <param name="type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this object inputObject, Type type)
        {
            if (inputObject == null || type == null)
                return false;
            return inputObject.GetType().Is(type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this Type objectType, Type type)
        {
            if (objectType == null || type == null)
                return false;
            if (type == typeof(object))
                return true;
            if (type == objectType || objectType.GetInterfaces().Any(x => x == type))
                return true;
            if (objectType.GetTypeInfo().BaseType == null)
                return false;
            return objectType.GetTypeInfo().BaseType.Is(type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="inputObject">Object</param>
        /// <typeparam name="BaseObjectType">Base object type</typeparam>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is<BaseObjectType>(this object inputObject)
        {
            if (inputObject == null)
                return false;
            return inputObject.Is(typeof(BaseObjectType));
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <typeparam name="BaseObjectType">Base object type</typeparam>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is<BaseObjectType>(this Type objectType)
        {
            if (objectType == null)
                return false;
            return objectType.Is(typeof(BaseObjectType));
        }

        /// <summary>
        /// Loads an assembly by its name
        /// </summary>
        /// <param name="name">Name of the assembly to return</param>
        /// <returns>The assembly specified if it exists</returns>
        public static Assembly Load(this AssemblyName name)
        {
            if (name == null)
                return null;
            try
            {
                return PlatformServices.Default.AssemblyLoadContextAccessor.Default.Load(name);
            }
            catch (BadImageFormatException) { return null; }
        }

        /// <summary>
        /// Makes a shallow copy of the object
        /// </summary>
        /// <param name="inputObject">Object to copy</param>
        /// <param name="simpleTypesOnly">
        /// If true, it only copies simple types (no classes, only items like int, string, etc.),
        /// false copies everything.
        /// </param>
        /// <returns>A copy of the object</returns>
        public static T MakeShallowCopy<T>(this T inputObject, bool simpleTypesOnly = false)
        {
            if (Equals(inputObject, default(T)))
                return default(T);
            Type ObjectType = inputObject.GetType();
            T ClassInstance = ObjectType.Create<T>();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                if (Property.CanRead
                        && Property.CanWrite
                        && simpleTypesOnly
                        && Property.PropertyType.GetTypeInfo().IsValueType)
                    Property.SetValue(ClassInstance, Property.GetValue(inputObject, null), null);
                else if (!simpleTypesOnly
                            && Property.CanRead
                            && Property.CanWrite)
                    Property.SetValue(ClassInstance, Property.GetValue(inputObject, null), null);
            }

            foreach (FieldInfo Field in ObjectType.GetFields())
            {
                if (simpleTypesOnly && Field.IsPublic)
                    Field.SetValue(ClassInstance, Field.GetValue(inputObject));
                else if (!simpleTypesOnly && Field.IsPublic)
                    Field.SetValue(ClassInstance, Field.GetValue(inputObject));
            }

            return ClassInstance;
        }

        /// <summary>
        /// Goes through a list of types and determines if they're marked with a specific attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="types">Types to check</param>
        /// <param name="inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>The list of types that are marked with an attribute</returns>
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> types, bool inherit = true)
            where T : Attribute
        {
            if (types == null)
                return null;
            return types.Where(x => x.GetTypeInfo().IsDefined(typeof(T), inherit) && !x.GetTypeInfo().IsAbstract);
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within an assembly that are of the
        /// specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="assembly">Assembly to search within</param>
        /// <param name="args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> Objects<ClassType>(this Assembly assembly, params object[] args)
        {
            if (assembly == null)
                return new List<ClassType>();
            return assembly.Types<ClassType>().Where(x => !x.GetTypeInfo().ContainsGenericParameters).Create<ClassType>(args);
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a group of assemblies that are
        /// of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="assemblies">Assemblies to search within</param>
        /// <param name="args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> Objects<ClassType>(this IEnumerable<Assembly> assemblies, params object[] args)
        {
            if (assemblies == null || assemblies.Count() == 0)
                yield break;
            foreach (var Assembly in assemblies)
                foreach (var Object in Assembly.Objects<ClassType>(args))
                    yield return Object;
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="inputObject">The object to get the property of</param>
        /// <param name="property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object Property(this object inputObject, PropertyInfo property)
        {
            if (inputObject == null || property == null)
                return null;
            return property.GetValue(inputObject, null);
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="inputObject">The object to get the property of</param>
        /// <param name="property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object Property(this object inputObject, string property)
        {
            if (inputObject == null || string.IsNullOrEmpty(property))
                return null;
            string[] Properties = property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = inputObject;
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
            if (DestinationProperty == null)
                throw new NullReferenceException("PropertyInfo can't be null");
            return TempObject.Property(DestinationProperty);
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="inputObject">The object to set the property of</param>
        /// <param name="property">The property to set</param>
        /// <param name="value">Value to set the property to</param>
        /// <param name="format">Allows for formatting if the destination is a string</param>
        public static object Property(this object inputObject, PropertyInfo property, object value, string format = "")
        {
            if (inputObject == null)
                return null;
            if (property == null || value == null)
                return inputObject;
            if (property.PropertyType == typeof(string))
                value = value.FormatToString(format);
            property.SetValue(inputObject, value.To(property.PropertyType, null), null);
            return inputObject;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="inputObject">The object to set the property of</param>
        /// <param name="property">The property to set</param>
        /// <param name="value">Value to set the property to</param>
        /// <param name="format">Allows for formatting if the destination is a string</param>
        public static object Property(this object inputObject, string property, object value, string format = "")
        {
            if (inputObject == null)
                return null;
            if (string.IsNullOrEmpty(property) || value == null)
                return inputObject;
            string[] Properties = property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = inputObject;
            Type TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return inputObject;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            if (DestinationProperty == null)
                throw new NullReferenceException("PropertyInfo can't be null");
            TempObject.Property(DestinationProperty, value, format);
            return inputObject;
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, DataType>> PropertyGetter<ClassType, DataType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.PropertyType.Is(typeof(DataType)))
                throw new ArgumentException("Property is not of the type specified");
            if (!property.DeclaringType.Is(typeof(ClassType)) && !typeof(ClassType).Is(property.DeclaringType))
                throw new ArgumentException("Property is not from the declaring class type specified");
            ParameterExpression ObjectInstance = Expression.Parameter(property.DeclaringType, "x");
            MemberExpression PropertyGet = Expression.Property(ObjectInstance, property);
            if (property.PropertyType != typeof(DataType))
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
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, object>> PropertyGetter<ClassType>(this PropertyInfo property)
        {
            return property.PropertyGetter<ClassType, object>();
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string PropertyName(this LambdaExpression expression)
        {
            if (expression == null)
                return "";
            if (expression.Body is UnaryExpression && expression.Body.NodeType == ExpressionType.Convert)
            {
                var Temp = (MemberExpression)((UnaryExpression)expression.Body).Operand;
                return Temp.Expression.PropertyName() + Temp.Member.Name;
            }
            if (!(expression.Body is MemberExpression))
                throw new ArgumentException("Expression.Body is not a MemberExpression");
            return ((MemberExpression)expression.Body).Expression.PropertyName() + ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string PropertyName(this Expression expression)
        {
            var TempExpression = expression as MemberExpression;
            if (TempExpression == null)
                return "";
            return TempExpression.Expression.PropertyName() + TempExpression.Member.Name + ".";
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, DataType>> PropertySetter<ClassType, DataType>(this LambdaExpression property)//Expression<Func<ClassType, DataType>> Property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            string PropertyName = property.PropertyName();
            string[] SplitName = PropertyName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (SplitName.Length == 0)
                return null;
            PropertyInfo PropertyInfo = typeof(ClassType).GetProperty(SplitName[0]);
            ParameterExpression ObjectInstance = Expression.Parameter(PropertyInfo.DeclaringType, "x");
            ParameterExpression PropertySet = Expression.Parameter(typeof(DataType), "y");
            ConstantExpression DefaultConstant = Expression.Constant(((object)null).To(PropertyInfo.PropertyType, null), PropertyInfo.PropertyType);
            MethodCallExpression SetterCall = null;
            MemberExpression PropertyGet = null;
            if (SplitName.Length > 1)
            {
                PropertyGet = Expression.Property(ObjectInstance, PropertyInfo);
                for (int x = 1; x < SplitName.Length - 1; ++x)
                {
                    PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[x]);
                    if (PropertyInfo == null)
                        throw new NullReferenceException("PropertyInfo can't be null");
                    PropertyGet = Expression.Property(PropertyGet, PropertyInfo);
                }
                PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[SplitName.Length - 1]);
            }
            MethodInfo SetMethod = PropertyInfo.GetSetMethod();
            if (SetMethod != null)
            {
                if (PropertyInfo.PropertyType != typeof(DataType))
                {
                    MethodInfo ConversionMethod = typeof(TypeConversionExtensions).GetMethods().FirstOrDefault(x => x.ContainsGenericParameters
                        && x.GetGenericArguments().Length == 2
                        && x.Name == "To"
                        && x.GetParameters().Length == 2);
                    ConversionMethod = ConversionMethod.MakeGenericMethod(typeof(DataType), PropertyInfo.PropertyType);
                    MethodCallExpression Convert = Expression.Call(ConversionMethod, PropertySet, DefaultConstant);
                    SetterCall = PropertyGet == null ? Expression.Call(ObjectInstance, SetMethod, Convert) : Expression.Call(PropertyGet, SetMethod, Convert);
                    return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
                }
                SetterCall = PropertyGet == null ? Expression.Call(ObjectInstance, SetMethod, PropertySet) : Expression.Call(PropertyGet, SetMethod, PropertySet);
            }
            else
                return Expression.Lambda<Action<ClassType, DataType>>(Expression.Empty(), ObjectInstance, PropertySet);
            return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, object>> PropertySetter<ClassType>(this LambdaExpression property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            return property.PropertySetter<ClassType, object>();
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="inputObject">object who contains the property</param>
        /// <param name="propertyPath">
        /// Path of the property (ex: Prop1.Prop2.Prop3 would be the Prop1 of the source object,
        /// which then has a Prop2 on it, which in turn has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type PropertyType(this object inputObject, string propertyPath)
        {
            if (inputObject == null || string.IsNullOrEmpty(propertyPath))
                return null;
            return inputObject.GetType().PropertyType(propertyPath);
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="propertyPath">
        /// Path of the property (ex: Prop1.Prop2.Prop3 would be the Prop1 of the source object,
        /// which then has a Prop2 on it, which in turn has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type PropertyType(this Type objectType, string propertyPath)
        {
            if (objectType == null || string.IsNullOrEmpty(propertyPath))
                return null;
            string[] SourceProperties = propertyPath.Split(new string[] { "." }, StringSplitOptions.None);
            PropertyInfo PropertyInfo = null;
            for (int x = 0; x < SourceProperties.Length; ++x)
            {
                PropertyInfo = objectType.GetProperty(SourceProperties[x]);
                objectType = PropertyInfo.PropertyType;
            }
            return objectType;
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="assembly">Assembly to get version information from</param>
        /// <param name="infoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString(this Assembly assembly, VersionInfo infoType)
        {
            if (assembly == null)
                return "";
            if (infoType.HasFlag(VersionInfo.ShortVersion))
            {
                Version Version = assembly.GetName().Version;
                return Version.Major + "." + Version.Minor;
            }
            else
            {
                return assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="assemblies">Assemblies to get version information from</param>
        /// <param name="infoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString(this IEnumerable<Assembly> assemblies, VersionInfo infoType)
        {
            if (assemblies == null || assemblies.Count() == 0)
                return "";
            var Builder = new StringBuilder();
            assemblies.OrderBy(x => x.FullName).ForEach<Assembly>(x => Builder.AppendLine(x.GetName().Name + ": " + x.ToString(infoType)));
            return Builder.ToString();
        }

        /// <summary>
        /// Gets assembly information for all currently loaded assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to dump information from</param>
        /// <param name="htmlOutput">Should HTML output be used</param>
        /// <returns>An HTML formatted string containing the assembly information</returns>
        public static string ToString(this IEnumerable<Assembly> assemblies, bool htmlOutput)
        {
            if (assemblies == null || assemblies.Count() == 0)
                return "";
            var Builder = new StringBuilder();
            Builder.Append(htmlOutput ? "<strong>Assembly Information</strong><br />" : "Assembly Information\r\n");
            assemblies.ForEach<Assembly>(x => Builder.Append(x.ToString(htmlOutput)));
            return Builder.ToString();
        }

        /// <summary>
        /// Dumps the property names and current values from an object
        /// </summary>
        /// <param name="inputObject">Object to dunp</param>
        /// <param name="htmlOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        public static string ToString(this object inputObject, bool htmlOutput)
        {
            if (inputObject == null)
                return "";
            var TempValue = new StringBuilder();
            TempValue.Append(htmlOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            Type ObjectType = inputObject.GetType();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                TempValue.Append(htmlOutput ? "<tr><td>" : Environment.NewLine).Append(Property.Name).Append(htmlOutput ? "</td><td>" : "\t\t\t\t");
                ParameterInfo[] Parameters = Property.GetIndexParameters();
                if (Property.CanRead && Parameters.Length == 0)
                {
                    try
                    {
                        object Value = Property.GetValue(inputObject, null);
                        TempValue.Append(Value == null ? "null" : Value.ToString());
                    }
                    catch { }
                }
                TempValue.Append(htmlOutput ? "</td></tr>" : "");
            }
            TempValue.Append(htmlOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Dumps the properties names and current values from an object type (used for static classes)
        /// </summary>
        /// <param name="objectType">Object type to dunp</param>
        /// <param name="htmlOutput">Should this be output as an HTML string</param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string ToString(this Type objectType, bool htmlOutput)
        {
            if (objectType == null)
                return "";
            var TempValue = new StringBuilder();
            TempValue.Append(htmlOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            PropertyInfo[] Properties = objectType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                TempValue.Append(htmlOutput ? "<tr><td>" : System.Environment.NewLine).Append(Property.Name).Append(htmlOutput ? "</td><td>" : "\t\t\t\t");
                if (Property.CanRead && Property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        TempValue.Append(Property.GetValue(null, null) == null ? "null" : Property.GetValue(null, null).ToString());
                    }
                    catch { }
                }
                TempValue.Append(htmlOutput ? "</td></tr>" : "");
            }
            TempValue.Append(htmlOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assembly">Assembly to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types<BaseType>(this Assembly assembly)
        {
            if (assembly == null)
                return new List<Type>();
            return assembly.Types(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assembly">Assembly to check</param>
        /// <param name="baseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types(this Assembly assembly, Type baseType)
        {
            if (assembly == null || baseType == null)
                return new List<Type>();
            try
            {
                return assembly.GetTypes().Where(x => x.Is(baseType) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract);
            }
            catch { return new List<Type>(); }
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assemblies">Assemblies to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types<BaseType>(this IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null || assemblies.Count() == 0)
                return new List<Type>();
            return assemblies.Types(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assemblies">Assemblies to check</param>
        /// <param name="baseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types(this IEnumerable<Assembly> assemblies, Type baseType)
        {
            if (assemblies == null || assemblies.Count() == 0 || baseType == null)
                yield break;
            foreach (var Assembly in assemblies)
            {
                foreach (var Type in Assembly.Types(baseType))
                {
                    yield return Type;
                }
            }
        }

        /// <summary>
        /// Gets a list of types in the assemblies specified
        /// </summary>
        /// <param name="assemblies">Assemblies to check</param>
        /// <returns>List of types</returns>
        public static IEnumerable<Type> Types(this IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null || assemblies.Count() == 0)
                yield break;
            foreach (var Assembly in assemblies)
            {
                foreach (var Type in Assembly.GetTypes())
                {
                    yield return Type;
                }
            }
        }
    }
}