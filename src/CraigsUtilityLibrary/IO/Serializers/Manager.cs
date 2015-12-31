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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.Serializers.Interfaces;

namespace Utilities.IO.Serializers
{
    /// <summary>
    /// Serialization manager class
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        public Manager(IEnumerable<ISerializer> serializers)
        {
            serializers = serializers ?? new List<ISerializer>();
            Serializers = serializers.Where(x => !x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase))
                                          .ToDictionary(x => x.ContentType);
            serializers.Where(x => x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase))
                       .ForEach(x =>
                       {
                           if (!Serializers.ContainsKey(x.ContentType))
                               Serializers.Add(x.ContentType, x);
                       });
        }

        /// <summary>
        /// Serializers
        /// </summary>
        protected IDictionary<string, ISerializer> Serializers { get; private set; }

        /// <summary>
        /// Determines if the system can serialize/deserialize the content type
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanSerialize(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;
            return Serializers.ContainsKey(contentType.Split(';')[0]);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <typeparam name="R">Return object type</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public R Deserialize<T, R>(T data, string contentType = "application/json")
        {
            if (string.IsNullOrEmpty(contentType))
                return default(R);
            return (R)Deserialize<T>(data, typeof(R), contentType);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="objectType">Object type requested</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize<T>(T data, Type objectType, string contentType = "application/json")
        {
            if (string.IsNullOrEmpty(contentType) || objectType == null)
                return null;
            contentType = contentType.Split(';')[0];
            if (!Serializers.ContainsKey(contentType) || Serializers[contentType].ReturnType != typeof(T))
                return null;
            return ((ISerializer<T>)Serializers[contentType]).Deserialize(objectType, data);
        }

        /// <summary>
        /// File type to content type
        /// </summary>
        /// <param name="fileType">File type</param>
        /// <returns>Content type</returns>
        public string FileTypeToContentType(string fileType)
        {
            return Serializers.FirstOrDefault(x => string.Equals(x.Value.FileType, fileType, StringComparison.OrdinalIgnoreCase))
                              .Chain(x => x.Value)
                              .Chain(x => x.ContentType, "");
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <typeparam name="R">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public R Serialize<T, R>(T data, string contentType = "application/json")
        {
            if (string.IsNullOrEmpty(contentType))
                return default(R);
            return Serialize<R>(data, typeof(T), contentType);
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Object to serialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <typeparam name="T">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public T Serialize<T>(object data, Type objectType, string contentType = "application/json")
        {
            if (string.IsNullOrEmpty(contentType) || objectType == null)
                return default(T);
            contentType = contentType.Split(';')[0];
            if (!Serializers.ContainsKey(contentType) || Serializers[contentType].ReturnType != typeof(T))
                return default(T);
            return ((ISerializer<T>)Serializers[contentType]).Serialize(objectType, data);
        }

        /// <summary>
        /// Outputs information about the serializers the system is using
        /// </summary>
        /// <returns>String version of the object</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.Append("Serializers: ").AppendLine(Serializers.ToString(x => x.Value.Name));
            return Builder.ToString();
        }
    }
}