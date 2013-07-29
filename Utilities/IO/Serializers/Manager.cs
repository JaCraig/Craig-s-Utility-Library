/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Threading.Tasks;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.IO.Serializers.Interfaces;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO.Serializers
{
    /// <summary>
    /// Serialization manager class
    /// </summary>
    public class Manager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            Serializers = new Dictionary<string, ISerializer>();
            foreach (Assembly Assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type FileSystem in Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ISerializer))
                                                                        && x.IsClass
                                                                        && !x.IsAbstract
                                                                        && !x.ContainsGenericParameters
                                                                        && !x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase)))
                {
                    ISerializer Serializer = (ISerializer)Activator.CreateInstance(FileSystem);
                    if (!Serializers.ContainsKey(Serializer.ContentType))
                        Serializers.Add(Serializer.ContentType, Serializer);
                }
            }
            foreach (Assembly Assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type FileSystem in Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ISerializer))
                                                                        && x.IsClass
                                                                        && !x.IsAbstract
                                                                        && !x.ContainsGenericParameters
                                                                        && x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase)))
                {
                    ISerializer Serializer = (ISerializer)Activator.CreateInstance(FileSystem);
                    if (!Serializers.ContainsKey(Serializer.ContentType))
                        Serializers.Add(Serializer.ContentType, Serializer);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Serializers
        /// </summary>
        protected IDictionary<string, ISerializer> Serializers { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <typeparam name="R">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public R Serialize<T, R>(T Object, string ContentType = "application/json")
        {
            return Serialize<R>(Object, typeof(T), ContentType);
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <typeparam name="T">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public T Serialize<T>(object Object, Type ObjectType, string ContentType = "application/json")
        {
            if (!Serializers.ContainsKey(ContentType) || Serializers[ContentType].ReturnType != typeof(T))
                return default(T);
            return ((ISerializer<T>)Serializers[ContentType]).Serialize(ObjectType, Object);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <typeparam name="R">Return object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public R Deserialize<T, R>(T Data, string ContentType = "application/json")
        {
            return (R)Deserialize<T>(Data, typeof(R), ContentType);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ObjectType">Object type requested</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize<T>(T Data, Type ObjectType, string ContentType = "application/json")
        {
            if (!Serializers.ContainsKey(ContentType) || Serializers[ContentType].ReturnType != typeof(T))
                return null;
            return ((ISerializer<T>)Serializers[ContentType]).Deserialize(ObjectType, Data);
        }

        /// <summary>
        /// Outputs information about the serializers the system is using
        /// </summary>
        /// <returns>String version of the object</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            foreach (string Key in Serializers.Keys)
            {
                Builder.AppendLine(Serializers[Key].Name);
            }
            return Builder.ToString();
        }

        #endregion
    }
}
