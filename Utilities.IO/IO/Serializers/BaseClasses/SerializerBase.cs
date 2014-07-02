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
using Utilities.IO.Serializers.Interfaces;

namespace Utilities.IO.Serializers.BaseClasses
{
    /// <summary>
    /// Serializer base class
    /// </summary>
    /// <typeparam name="T">Serialized data type</typeparam>
    public abstract class SerializerBase<T> : ISerializer<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected SerializerBase()
        {
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public abstract string ContentType { get; }

        /// <summary>
        /// Common file type (extension)
        /// </summary>
        public abstract string FileType { get; }

        /// <summary>
        /// Name of the serializer
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Return type
        /// </summary>
        public Type ReturnType { get { return typeof(T); } }

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public abstract object Deserialize(Type ObjectType, T Data);

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public abstract T Serialize(Type ObjectType, object Data);
    }
}