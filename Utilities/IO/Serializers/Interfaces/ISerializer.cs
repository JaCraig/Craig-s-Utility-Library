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

namespace Utilities.IO.Serializers.Interfaces
{
    /// <summary>
    /// Serializer interface
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Content type associated with this serializer (MIME type)
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// File ending associated with the serializer
        /// </summary>
        string FileType { get; }

        /// <summary>
        /// Name of the serializer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return type for the serialized data
        /// </summary>
        Type ReturnType { get; }
    }

    /// <summary>
    /// Serializer interface
    /// </summary>
    /// <typeparam name="T">Object type returned</typeparam>
    public interface ISerializer<T> : ISerializer
    {
        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        object Deserialize(Type ObjectType, T Data);

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        T Serialize(Type ObjectType, object Data);
    }
}