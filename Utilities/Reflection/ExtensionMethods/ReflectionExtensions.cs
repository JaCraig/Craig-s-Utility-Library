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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
using System.Diagnostics.Contracts;

#endregion

namespace Utilities.Reflection.ExtensionMethods
{
    /// <summary>
    /// Reflection oriented extensions
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Functions

        #region GetObjects

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
            if (Property.PropertyType == typeof(string))
                Value = Value.FormatToString(Format);
            //if(!Value.GetType().IsOfType(Property.PropertyType))
            //    Value=Convert.ChangeType(Value,Property.PropertyType);
            Property.SetValue(Object, Value.TryTo(Property.PropertyType, null), null);
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