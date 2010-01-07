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
                    ParameterInfo []Parameters = Property.GetIndexParameters();
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
                Builder.Append(DumpProperties(Assembly)+"<br /><br />");
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
            foreach(Assembly TempAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(TempAssembly.GetName().Name.Equals(Name,StringComparison.InvariantCultureIgnoreCase))
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
        public static object MakeShallowCopy(object Object,bool SimpleTypesOnly)
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
        public static object MakeShallowCopyInheritedClass(Type DerivedType,object Object, bool SimpleTypesOnly)
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
        public static System.Collections.Generic.List<Type> GetTypes(string AssemblyLocation, string Interface,out Assembly Assembly)
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
            ListMapping<Assembly, Type> ReturnList = new ListMapping<Assembly,Type>();
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
                if (Field.IsPublic)
                {
                    Field.SetValue(ClassInstance, Field.GetValue(Object));
                }
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
            Type FieldType = Field.FieldType;
            if(Field.FieldType.FullName.StartsWith("System.Collections.Generic.List", StringComparison.CurrentCultureIgnoreCase))
            {
                FieldType=Field.FieldType.GetGenericArguments()[0];
            }

            if (FieldType.FullName.StartsWith("System"))
            {
                SetField(Field, ClassInstance, Object);
            }
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
            Type PropertyType = Property.PropertyType;
            if (Property.PropertyType.FullName.StartsWith("System.Collections.Generic.List", StringComparison.CurrentCultureIgnoreCase))
            {
                PropertyType = Property.PropertyType.GetGenericArguments()[0];
            }

            if (PropertyType.FullName.StartsWith("System"))
            {
                SetProperty(Property, ClassInstance, Object);
            }
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
                if (Property.GetSetMethod() != null && Property.GetGetMethod() != null)
                {
                    Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
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

        #endregion
    }
}
