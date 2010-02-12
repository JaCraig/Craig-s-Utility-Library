/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using System.IO;
using System.Reflection;
using System.Text;
using Utilities.DataTypes;
using System.Linq.Expressions;
#endregion

namespace Utilities
{
    /// <summary>
    /// Utility class that handles various
    /// functions dealing with reflection.
    /// </summary>
    public static class Reflection
    {
        #region Public Static Functions
        /// <summary>
        /// Dumps the properties names and current values
        /// from an object
        /// </summary>
        /// <param name="Object">Object to dunp</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        public static string DumpProperties(object Object)
        {
            try
            {
                StringBuilder TempValue = new StringBuilder();
                TempValue.Append("<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>");
                Type ObjectType = Object.GetType();
                PropertyInfo[] Properties = ObjectType.GetProperties();
                foreach (PropertyInfo Property in Properties)
                {
                    TempValue.Append("<tr><td>" + Property.Name + "</td><td>");
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
                    TempValue.Append("</td></tr>");
                }
                TempValue.Append("</tbody></table>");
                return TempValue.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Dumps the properties names and current values
        /// from an object type (used for static classes)
        /// </summary>
        /// <param name="ObjectType">Object type to dunp</param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string DumpProperties(Type ObjectType)
        {
            try
            {
                StringBuilder TempValue = new StringBuilder();
                TempValue.Append("<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>");
                PropertyInfo[] Properties = ObjectType.GetProperties();
                foreach (PropertyInfo Property in Properties)
                {
                    TempValue.Append("<tr><td>" + Property.Name + "</td><td>");
                    if (Property.GetIndexParameters().Length == 0)
                    {
                        try
                        {
                            TempValue.Append(Property.GetValue(null, null) == null ? "null" : Property.GetValue(null, null).ToString());
                        }
                        catch { }
                    }
                    TempValue.Append("</td></tr>");
                }
                TempValue.Append("</tbody></table>");
                return TempValue.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Returns all assemblies and their properties
        /// </summary>
        /// <returns>An HTML formatted string that contains the assemblies and their information</returns>
        public static string DumpAllAssembliesAndProperties()
        {
            StringBuilder Builder = new StringBuilder();
            Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly Assembly in Assemblies)
            {
                Builder.Append("<strong>" + Assembly.GetName().Name + "</strong><br />");
                Builder.Append(DumpProperties(Assembly) + "<br /><br />");
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Gets an assembly by its name if it is currently loaded
        /// </summary>
        /// <param name="Name">Name of the assembly to return</param>
        /// <returns>The assembly specified if it exists, otherwise it returns null</returns>
        public static System.Reflection.Assembly GetLoadedAssembly(string Name)
        {
            foreach (Assembly TempAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (TempAssembly.GetName().Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return TempAssembly;
                }
            }
            return null;
        }

        /// <summary>
        /// Makes a shallow copy of the object
        /// </summary>
        /// <param name="Object">Object to copy</param>
        /// <param name="SimpleTypesOnly">If true, it only copies simple types (no classes, only items like int, string, etc.), false copies everything.</param>
        /// <returns>A copy of the object</returns>
        public static object MakeShallowCopy(object Object, bool SimpleTypesOnly)
        {
            Type ObjectType = Object.GetType();
            PropertyInfo[] Properties = ObjectType.GetProperties();
            FieldInfo[] Fields = ObjectType.GetFields();
            object ClassInstance = Activator.CreateInstance(ObjectType);

            foreach (PropertyInfo Property in Properties)
            {
                try
                {
                    if (SimpleTypesOnly)
                    {
                        SetPropertyifSimpleType(Property, ClassInstance, Object);
                    }
                    else
                    {
                        SetProperty(Property, ClassInstance, Object);
                    }
                }
                catch { }
            }

            foreach (FieldInfo Field in Fields)
            {
                try
                {
                    if (SimpleTypesOnly)
                    {
                        SetFieldifSimpleType(Field, ClassInstance, Object);
                    }
                    else
                    {
                        SetField(Field, ClassInstance, Object);
                    }
                }
                catch { }
            }

            return ClassInstance;
        }

        /// <summary>
        /// Makes a shallow copy of the object to a different class type (inherits from the original)
        /// </summary>
        /// <param name="DerivedType">Derived type</param>
        /// <param name="Object">Object to copy</param>
        /// <param name="SimpleTypesOnly">If true, it only copies simple types (no classes, only items like int, string, etc.), false copies everything.</param>
        /// <returns>A copy of the object</returns>
        public static object MakeShallowCopyInheritedClass(Type DerivedType, object Object, bool SimpleTypesOnly)
        {
            Type ObjectType = Object.GetType();
            Type ReturnedObjectType = DerivedType;
            PropertyInfo[] Properties = ObjectType.GetProperties();
            FieldInfo[] Fields = ObjectType.GetFields();
            object ClassInstance = Activator.CreateInstance(ReturnedObjectType);

            foreach (PropertyInfo Property in Properties)
            {
                try
                {
                    PropertyInfo ChildProperty = ReturnedObjectType.GetProperty(Property.Name);
                    if (ChildProperty != null)
                    {
                        if (SimpleTypesOnly)
                        {
                            SetPropertyifSimpleType(ChildProperty, Property, ClassInstance, Object);
                        }
                        else
                        {
                            SetProperty(ChildProperty, Property, ClassInstance, Object);
                        }
                    }
                }
                catch { }
            }

            foreach (FieldInfo Field in Fields)
            {
                try
                {
                    FieldInfo ChildField = ReturnedObjectType.GetField(Field.Name);
                    if (ChildField != null)
                    {
                        if (SimpleTypesOnly)
                        {
                            SetFieldifSimpleType(ChildField, Field, ClassInstance, Object);
                        }
                        else
                        {
                            SetField(ChildField, Field, ClassInstance, Object);
                        }
                    }
                }
                catch { }
            }

            return ClassInstance;
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <param name="Interface">Interface to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static System.Collections.Generic.List<Type> GetTypes(Assembly Assembly, string Interface)
        {
            System.Collections.Generic.List<Type> ReturnList = new System.Collections.Generic.List<Type>();
            if (Assembly != null)
            {
                Type[] Types = Assembly.GetTypes();
                foreach (Type Type in Types)
                {
                    if (Type.GetInterface(Interface, true) != null)
                    {
                        ReturnList.Add(Type);
                    }
                }
            }
            return ReturnList;
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="AssemblyLocation">Location of the DLL</param>
        /// <param name="Interface">Interface to look for</param>
        /// <param name="Assembly">The assembly holding the types</param>
        /// <returns>A list of types that use the interface</returns>
        public static System.Collections.Generic.List<Type> GetTypes(string AssemblyLocation, string Interface, out Assembly Assembly)
        {
            Assembly = Assembly.LoadFile(AssemblyLocation);
            return GetTypes(Assembly, Interface);
        }

        /// <summary>
        /// Gets a list of types based on an interface from all assemblies found in a directory
        /// </summary>
        /// <param name="AssemblyDirectory">Directory to search in</param>
        /// <param name="Interface">The interface to look for</param>
        /// <returns>A list mapping using the assembly as the key and a list of types</returns>
        public static ListMapping<Assembly, Type> GetTypesFromDirectory(string AssemblyDirectory, string Interface)
        {
            ListMapping<Assembly, Type> ReturnList = new ListMapping<Assembly, Type>();
            System.Collections.Generic.List<Assembly> Assemblies = GetAssembliesFromDirectory(AssemblyDirectory);
            foreach (Assembly Assembly in Assemblies)
            {
                Type[] Types = Assembly.GetTypes();
                foreach (Type Type in Types)
                {
                    if (Type.GetInterface(Interface, true) != null)
                    {
                        ReturnList.Add(Assembly, Type);
                    }
                }
            }
            return ReturnList;
        }

        /// <summary>
        /// Gets a list of assemblies from a directory
        /// </summary>
        /// <param name="Directory">The directory to search in</param>
        /// <returns>List of assemblies in the directory</returns>
        public static System.Collections.Generic.List<Assembly> GetAssembliesFromDirectory(string Directory)
        {
            System.Collections.Generic.List<Assembly> ReturnList = new System.Collections.Generic.List<Assembly>();
            System.Collections.Generic.List<FileInfo> Files = FileManager.FileList(Directory);
            foreach (FileInfo File in Files)
            {
                if (File.Extension.Equals(".dll", StringComparison.CurrentCultureIgnoreCase))
                {
                    ReturnList.Add(Assembly.LoadFile(File.FullName));
                }
            }
            return ReturnList;
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        public static object CallMethod(string MethodName, object Object,params object[] InputVariables)
        {
            if(string.IsNullOrEmpty(MethodName)||Object==null)
                return null;
            Type ObjectType = Object.GetType();
            MethodInfo Method = null;
            if (InputVariables != null)
            {
                Type[] MethodInputTypes = new Type[InputVariables.Length];
                for (int x = 0; x < InputVariables.Length; ++x)
                {
                    MethodInputTypes[x] = InputVariables[x].GetType();
                }
                Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
                if (Method != null)
                {
                    return Method.Invoke(Object, InputVariables);
                }
            }
            Method = ObjectType.GetMethod(MethodName);
            if (Method != null)
            {
                return Method.Invoke(Object, null);
            }
            return null;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="SourceValue">The source value</param>
        /// <param name="DestinationObject">The destination object</param>
        /// <param name="DestinationPropertyInfo">The destination property info</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static void SetValue(object SourceValue, object DestinationObject,
            PropertyInfo DestinationPropertyInfo, string Format)
        {
            if (DestinationObject == null || DestinationPropertyInfo == null)
                return;
            Type DestinationPropertyType = DestinationPropertyInfo.PropertyType;
            DestinationPropertyInfo.SetValue(DestinationObject,
                Parse(SourceValue, DestinationPropertyType, Format),
                null);
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="SourceValue">The source value</param>
        /// <param name="DestinationObject">The destination object</param>
        /// <param name="PropertyPath">The path to the property (ex: MyProp.SubProp.FinalProp
        /// would look at the MyProp on the destination object, then find it's SubProp,
        /// and finally copy the SourceValue to the FinalProp property on the destination
        /// object)</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static void SetValue(object SourceValue, object DestinationObject, 
            string PropertyPath, string Format)
        {
            string[] Splitter = { "." };
            string[] DestinationProperties = PropertyPath.Split(Splitter, StringSplitOptions.None);
            object TempDestinationProperty = DestinationObject;
            Type DestinationPropertyType = DestinationObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < DestinationProperties.Length - 1; ++x)
            {
                DestinationProperty = DestinationPropertyType.GetProperty(DestinationProperties[x]);
                DestinationPropertyType=DestinationProperty.PropertyType;
                TempDestinationProperty = DestinationProperty.GetValue(TempDestinationProperty, null);
                if (TempDestinationProperty == null)
                    return;
            }
            DestinationProperty = DestinationPropertyType.GetProperty(DestinationProperties[DestinationProperties.Length - 1]);
            SetValue(SourceValue, TempDestinationProperty, DestinationProperty, Format);
        }

        /// <summary>
        /// Gets the name of the property held within the expression
        /// </summary>
        /// <typeparam name="T">The type of object used in the expression</typeparam>
        /// <param name="Expression">The expression</param>
        /// <returns>A string containing the name of the property</returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> Expression)
        {
            if (Expression == null)
                return "";
            string Name = "";
            if (Expression.Body.NodeType == ExpressionType.Convert)
            {
                Name = Expression.Body.ToString().Replace("Convert(", "").Replace(")", "");
                Name = Name.Remove(0, Name.IndexOf(".") + 1);
            }
            else
            {
                Name = Expression.Body.ToString();
                Name = Name.Remove(0, Name.IndexOf(".") + 1);
            }
            return Name;
        }

        /// <summary>
        /// Gets a property's value
        /// </summary>
        /// <param name="SourceObject">object who contains the property</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The value contained in the property or null if the property can not
        /// be reached</returns>
        public static object GetPropertyValue(object SourceObject, string PropertyPath)
        {
            if (SourceObject == null||string.IsNullOrEmpty(PropertyPath))
                return null;
            string[] Splitter = { "." };
            string[] SourceProperties = PropertyPath.Split(Splitter, StringSplitOptions.None);
            object TempSourceProperty = SourceObject;
            Type PropertyType = SourceObject.GetType();
            for (int x = 0; x < SourceProperties.Length; ++x)
            {
                PropertyInfo SourcePropertyInfo = PropertyType.GetProperty(SourceProperties[x]);
                if (SourcePropertyInfo == null)
                    return null;
                TempSourceProperty = SourcePropertyInfo.GetValue(TempSourceProperty, null);
                if (TempSourceProperty == null)
                    return null;
                PropertyType = SourcePropertyInfo.PropertyType;
            }
            return TempSourceProperty;
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="SourceObject">object who contains the property</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(object SourceObject, string PropertyPath)
        {
            if (SourceObject == null||string.IsNullOrEmpty(PropertyPath))
                return null;
            string[] Splitter = { "." };
            string[] SourceProperties = PropertyPath.Split(Splitter, StringSplitOptions.None);
            object TempSourceProperty = SourceObject;
            Type PropertyType = SourceObject.GetType();
            PropertyInfo PropertyInfo = null;
            for (int x = 0; x < SourceProperties.Length; ++x)
            {
                PropertyInfo = PropertyType.GetProperty(SourceProperties[x]);
                PropertyType = PropertyInfo.PropertyType;
            }
            return PropertyType;
        }

        #endregion

        #region Private Static Functions

        /// <summary>
        /// Copies a field value
        /// </summary>
        /// <param name="Field">Field object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetField(FieldInfo Field, object ClassInstance, object Object)
        {
            try
            {
                SetField(Field, Field, ClassInstance, Object);
            }
            catch { }
        }

        /// <summary>
        /// Copies a field value
        /// </summary>
        /// <param name="Field">Field object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetFieldifSimpleType(FieldInfo Field, object ClassInstance, object Object)
        {
            try
            {
                SetFieldifSimpleType(Field, Field, ClassInstance, Object);
            }
            catch { }
        }

        /// <summary>
        /// Copies a field value
        /// </summary>
        /// <param name="ChildField">Child field object</param>
        /// <param name="Field">Field object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetField(FieldInfo ChildField, FieldInfo Field, object ClassInstance, object Object)
        {
            try
            {
                if (Field.IsPublic && ChildField.IsPublic)
                {
                    ChildField.SetValue(ClassInstance, Field.GetValue(Object));
                }
            }
            catch { }
        }

        /// <summary>
        /// Copies a field value
        /// </summary>
        /// <param name="ChildField">Child field object</param>
        /// <param name="Field">Field object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetFieldifSimpleType(FieldInfo ChildField, FieldInfo Field, object ClassInstance, object Object)
        {
            Type FieldType = Field.FieldType;
            if (Field.FieldType.FullName.StartsWith("System.Collections.Generic.List", StringComparison.CurrentCultureIgnoreCase))
            {
                FieldType = Field.FieldType.GetGenericArguments()[0];
            }

            if (FieldType.FullName.StartsWith("System"))
            {
                SetField(ChildField, Field, ClassInstance, Object);
            }
        }

        /// <summary>
        /// Copies a property value
        /// </summary>
        /// <param name="Property">Property object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetPropertyifSimpleType(PropertyInfo Property, object ClassInstance, object Object)
        {
            try
            {
                SetPropertyifSimpleType(Property, Property, ClassInstance, Object);
            }
            catch { }
        }

        /// <summary>
        /// Copies a property value
        /// </summary>
        /// <param name="Property">Property object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetProperty(PropertyInfo Property, object ClassInstance, object Object)
        {
            try
            {
                SetProperty(Property, Property, ClassInstance, Object);
            }
            catch { }
        }

        /// <summary>
        /// Copies a property value
        /// </summary>
        /// <param name="ChildProperty">Child property object</param>
        /// <param name="Property">Property object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetProperty(PropertyInfo ChildProperty, PropertyInfo Property, object ClassInstance, object Object)
        {
            try
            {
                if (ChildProperty.GetSetMethod() != null && Property.GetGetMethod() != null)
                {
                    ChildProperty.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                }
            }
            catch { }
        }

        /// <summary>
        /// Copies a property value
        /// </summary>
        /// <param name="ChildProperty">Child property object</param>
        /// <param name="Property">Property object</param>
        /// <param name="ClassInstance">Class to copy to</param>
        /// <param name="Object">Class to copy from</param>
        private static void SetPropertyifSimpleType(PropertyInfo ChildProperty, PropertyInfo Property, object ClassInstance, object Object)
        {
            Type PropertyType = Property.PropertyType;
            if (Property.PropertyType.FullName.StartsWith("System.Collections.Generic.List", StringComparison.CurrentCultureIgnoreCase))
            {
                PropertyType = Property.PropertyType.GetGenericArguments()[0];
            }

            if (PropertyType.FullName.StartsWith("System"))
            {
                SetProperty(ChildProperty, Property, ClassInstance, Object);
            }
        }


        /// <summary>
        /// Parses the object and turns it into the requested output type
        /// </summary>
        /// <param name="Input">Input object</param>
        /// <param name="OutputType">Output type</param>
        /// <returns>An object with the requested output type</returns>
        internal static object Parse(object Input, Type OutputType)
        {
            return Parse(Input, OutputType, "");
        }

        /// <summary>
        /// Parses the string into the requested output type
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="OutputType">Output type</param>
        /// <returns>An object with the requested output type</returns>
        private static object Parse(string Input, Type OutputType)
        {
            try
            {
                if (string.IsNullOrEmpty(Input))
                    return null;
                return Parse(Input, OutputType, "");
            }
            catch { throw; }
        }

        /// <summary>
        /// Parses the object into the requested output type
        /// </summary>
        /// <param name="Input">Input object</param>
        /// <param name="OutputType">Output object</param>
        /// <param name="Format">format string (may be overridded if the conversion 
        /// involves a floating point value to "f0")</param>
        /// <returns>The object converted to the specified output type</returns>
        private static object Parse(object Input, Type OutputType,string Format)
        {
            try
            {
                if (Input==null || OutputType == null)
                    return null;
                Type InputType = Input.GetType();
                if (InputType == OutputType)
                {
                    return Input;
                }
                else if (OutputType == typeof(string))
                {
                    return StringHelper.FormatToString(Input, Format);
                }
                else
                {
                    return CallMethod("Parse", OutputType.Assembly.CreateInstance(OutputType.FullName), StringHelper.FormatToString(Input, DiscoverFormatString(InputType, OutputType, Format)));
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Used to find the format string to use
        /// </summary>
        /// <param name="InputType">Input type</param>
        /// <param name="OutputType">Output type</param>
        /// <param name="FormatString">the string format</param>
        /// <returns>The format string to use</returns>
        private static string DiscoverFormatString(Type InputType,
            Type OutputType,string FormatString)
        {
            if (InputType == OutputType
                || InputType == typeof(string)
                || OutputType == typeof(string))
                return FormatString;
            if (InputType == typeof(float)
                || InputType == typeof(double)
                || InputType == typeof(decimal)
                || OutputType == typeof(float)
                || OutputType == typeof(double)
                || OutputType == typeof(decimal))
                return "f0";
            return FormatString;
        }
        #endregion
    }
}