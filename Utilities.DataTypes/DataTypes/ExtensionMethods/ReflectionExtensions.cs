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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Reflection oriented extensions
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Functions

        #region Attribute

        /// <summary>
        /// Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Attribute specified if it exists</returns>
        public static T Attribute<T>(this ICustomAttributeProvider Provider, bool Inherit = true) where T : Attribute
        {
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.Attributes<T>(Inherit)[0] : default(T);
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Array of attributes</returns>
        public static T[] Attributes<T>(this ICustomAttributeProvider Provider, bool Inherit = true) where T : Attribute
        {
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.GetCustomAttributes(typeof(T), Inherit).ToArray(x => (T)x) : new T[0];
        }

        #endregion

        #region Call

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType Call<ReturnType>(this object Object, string MethodName, params object[] InputVariables)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(MethodName), "MethodName");
            if (InputVariables == null)
                InputVariables = new object[0];
            Type ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + MethodName + " with the appropriate input variables.");
            return (ReturnType)Method.Invoke(Object, InputVariables);
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static ClassType Create<ClassType>(this Type Type,params object[] args)
        {
            Contract.Requires<ArgumentNullException>(Type != null, "Type");
            return (ClassType)Type.Create(args);
        }

        /// <summary>
        /// Creates an instance of the type
        /// </summary>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object Create(this Type Type, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(Type != null, "Type");
            return Activator.CreateInstance(Type, args);
        }

        /// <summary>
        /// Creates an instance of the types and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="Types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<ClassType> Create<ClassType>(this IEnumerable<Type> Types, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(Types != null, "Type");
            return Types.ForEach(x => x.Create<ClassType>(args));
        }

        /// <summary>
        /// Creates an instance of the types specified
        /// </summary>
        /// <param name="Types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<object> Create(this IEnumerable<Type> Types, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(Types != null, "Type");
            return Types.ForEach(x => x.Create(args));
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
            Contract.Requires<ArgumentNullException>(ObjectType != null, "ObjectType");
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
                    Output.Append(ObjectType.Name.Remove(ObjectType.Name.IndexOf("`", StringComparison.InvariantCulture)))
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
        
        #region HasDefaultConstructor

        /// <summary>
        /// Determines if the type has a default constructor
        /// </summary>
        /// <param name="Type">Type to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public static bool HasDefaultConstructor(this Type Type)
        {
            Contract.Requires<ArgumentNullException>(Type != null, "Type");
            return Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                        .Any(x => x.GetParameters().Length == 0);
        }

        #endregion

        #region Is

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this object Object, Type Type)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            Contract.Requires<ArgumentNullException>(Type != null, "Type");
            return Object.GetType().Is(Type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this Type ObjectType, Type Type)
        {
            Contract.Requires<ArgumentNullException>(Type != null, "Type");
            if (ObjectType == null)
                return false;
            if (Type == ObjectType || ObjectType.GetInterfaces().Any(x => x == Type))
                return true;
            if (ObjectType.BaseType == null)
                return false;
            return ObjectType.BaseType.Is(Type);
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
            Contract.Requires<ArgumentNullException>(Name != null, "Name");
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
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> Types, bool Inherit = true) 
            where T : Attribute
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static T MakeShallowCopy<T>(this T Object, bool SimpleTypesOnly=false)
        {
            if (Object == null)
                return default(T);
            Type ObjectType = Object.GetType();
            T ClassInstance = ObjectType.Create<T>();
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

        #region Objects

        /// <summary>
        /// Returns an instance of all classes that it finds within an assembly
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assembly">Assembly to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> Objects<ClassType>(this Assembly Assembly)
        {
            Contract.Requires<ArgumentNullException>(Assembly != null, "Assembly");
            return Assembly.Types<ClassType>().Where(x => !x.ContainsGenericParameters).Create<ClassType>();
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a group of assemblies
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assemblies">Assemblies to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> Objects<ClassType>(this IEnumerable<Assembly> Assemblies)
        {
            Contract.Requires<ArgumentNullException>(Assemblies != null, "Assemblies");
            List<ClassType> ReturnValues = new List<ClassType>();
            foreach (Assembly Assembly in Assemblies)
                ReturnValues.AddRange(Assembly.Objects<ClassType>());
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
        public static IEnumerable<ClassType> Objects<ClassType>(this DirectoryInfo Directory, bool Recursive = false)
        {
            Contract.Requires<ArgumentNullException>(Directory != null, "Directory");
            return Directory.LoadAssemblies(Recursive).Objects<ClassType>();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object Property(this object Object, PropertyInfo Property)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            Contract.Requires<ArgumentNullException>(Property != null, "Property");
            return Property.GetValue(Object, null);
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object Property(this object Object, string Property)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Property), "Property");
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
            return TempObject.Property(DestinationProperty);
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object Property(this object Object, PropertyInfo Property, object Value, string Format = "")
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            Contract.Requires<ArgumentNullException>(Property != null, "Property");
            Contract.Requires<ArgumentNullException>(Value != null, "Value");
            if (Property.PropertyType == typeof(string))
                Value = Value.FormatToString(Format);
            Property.SetValue(Object, Value.To(Property.PropertyType, null), null);
            return Object;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object Property(this object Object, string Property, object Value, string Format = "")
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Property), "Property");
            Contract.Requires<ArgumentNullException>(Value != null, "Value");
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
            TempObject.Property(DestinationProperty, Value, Format);
            return Object;
        }

        #endregion

        #region PropertyGetter

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, DataType>> PropertyGetter<ClassType, DataType>(this PropertyInfo Property)
        {
            if (!Property.PropertyType.Is(typeof(DataType)))
                throw new ArgumentException("Property is not of the type specified");
            if (!Property.DeclaringType.Is(typeof(ClassType)) && !typeof(ClassType).Is(Property.DeclaringType))
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
        public static Expression<Func<ClassType, object>> PropertyGetter<ClassType>(this PropertyInfo Property)
        {
            return Property.PropertyGetter<ClassType, object>();
        }

        #endregion

        #region PropertyName

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string PropertyName(this LambdaExpression Expression)
        {
            if (Expression.Body is UnaryExpression && Expression.Body.NodeType == ExpressionType.Convert)
            {
                MemberExpression Temp = (MemberExpression)((UnaryExpression)Expression.Body).Operand;
                return Temp.Expression.PropertyName() + Temp.Member.Name;
            }
            if (!(Expression.Body is MemberExpression))
                throw new ArgumentException("Expression.Body is not a MemberExpression");
            return ((MemberExpression)Expression.Body).Expression.PropertyName() + ((MemberExpression)Expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string PropertyName(this Expression Expression)
        {
            MemberExpression TempExpression = Expression as MemberExpression;
            if (TempExpression == null)
                return "";
            return TempExpression.Expression.PropertyName() + TempExpression.Member.Name + ".";
        }

        #endregion

        #region PropertySetter

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, DataType>> PropertySetter<ClassType, DataType>(this Expression<Func<ClassType, DataType>> Property)
        {
            Contract.Requires<ArgumentNullException>(Property != null, "Property");
            string PropertyName = Property.PropertyName();
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
        public static Expression<Action<ClassType, object>> PropertySetter<ClassType>(this Expression<Func<ClassType, object>> Property)
        {
            return Property.PropertySetter<ClassType, object>();
        }

        #endregion

        #region PropertyType

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="Object">object who contains the property</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type PropertyType(this object Object, string PropertyPath)
        {
            if (Object == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            return Object.GetType().PropertyType(PropertyPath);
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type PropertyType(this Type ObjectType, string PropertyPath)
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
        
        #region ToString

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="Assembly">Assembly to get version information from</param>
        /// <param name="InfoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString(this Assembly Assembly, VersionInfo InfoType)
        {
            Contract.Requires<ArgumentNullException>(Assembly != null, "Assembly");
            if (InfoType.HasFlag(VersionInfo.ShortVersion))
            {
                Version Version = Assembly.GetName().Version;
                return Version.Major + "." + Version.Minor;
            }
            else
            {
                return Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="Assemblies">Assemblies to get version information from</param>
        /// <param name="InfoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString(this IEnumerable<Assembly> Assemblies, VersionInfo InfoType)
        {
            StringBuilder Builder = new StringBuilder();
            Assemblies.ForEach<Assembly>(x => Builder.AppendLine(x.GetName().Name + ": " + x.ToString(InfoType)));
            return Builder.ToString();
        }

        /// <summary>
        /// Gets assembly information for all currently loaded assemblies
        /// </summary>
        /// <param name="Assemblies">Assemblies to dump information from</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>An HTML formatted string containing the assembly information</returns>
        public static string ToString(this IEnumerable<Assembly> Assemblies, bool HTMLOutput)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append(HTMLOutput ? "<strong>Assembly Information</strong><br />" : "Assembly Information\r\n");
            Assemblies.ForEach<Assembly>(x => Builder.Append(x.ToString(HTMLOutput)));
            return Builder.ToString();
        }

        /// <summary>
        /// Dumps the property names and current values from an object
        /// </summary>
        /// <param name="Object">Object to dunp</param>
        /// <param name="HTMLOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string ToString(this object Object, bool HTMLOutput)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string ToString(this Type ObjectType, bool HTMLOutput)
        {
            Contract.Requires<ArgumentNullException>(ObjectType != null, "ObjectType");
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

        #region Types

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types<BaseType>(this Assembly Assembly)
        {
            Contract.Requires<ArgumentNullException>(Assembly != null, "Assembly");
            return Assembly.Types(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types(this Assembly Assembly, Type BaseType)
        {
            Contract.Requires<ArgumentNullException>(Assembly != null, "Assembly");
            Contract.Requires<ArgumentNullException>(BaseType != null, "BaseType");
            return Assembly.GetTypes().Where(x => x.Is(BaseType) && x.IsClass && !x.IsAbstract);
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types<BaseType>(this IEnumerable<Assembly> Assemblies)
        {
            Contract.Requires<ArgumentNullException>(Assemblies != null, "Assemblies");
            return Assemblies.Types(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types(this IEnumerable<Assembly> Assemblies, Type BaseType)
        {
            Contract.Requires<ArgumentNullException>(Assemblies != null, "Assemblies");
            Contract.Requires<ArgumentNullException>(BaseType != null, "BaseType");
            List<Type> ReturnValues = new List<Type>();
            Assemblies.ForEach(y => ReturnValues.AddRange(y.Types(BaseType)));
            return ReturnValues;
        }

        #endregion

        #endregion
    }

    #region Enums

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

    #endregion
}