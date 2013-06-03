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
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.Serializers;
using Utilities.IO.Serializers.Interfaces;
#endregion

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    /// Serialization extensions
    /// </summary>
    public static class SerializationExtensions
    {
        #region Extension Methods
        
        #region Serialize

        /// <summary>
        /// Serializes the object using the specified serializer
        /// </summary>
        /// <param name="Object">Object to serialize</param>
        /// <param name="Serializer">Serializer to use (defaults to JSONSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <param name="FileLocation">File location to save to</param>
        /// <returns>The serialized object</returns>
        public static string Serialize(this object Object, ISerializer<string> Serializer = null, Encoding EncodingUsing = null, string FileLocation = "")
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            string Data = Serializer.Check(()=>new JSONSerializer(EncodingUsing)).Serialize(Object);
            if (!string.IsNullOrEmpty(FileLocation))
                FileLocation.Save(Data);
            return Data;
        }

        #endregion

        #region SerializeBinary

        /// <summary>
        /// Serializes the object using the specified serializer
        /// </summary>
        /// <param name="Object">Object to serialize</param>
        /// <param name="Serializer">Serializer to use (defaults to BinarySerializer)</param>
        /// <param name="FileLocation">File location to save to</param>
        /// <returns>The serialized object</returns>
        public static byte[] SerializeBinary(this object Object, ISerializer<byte[]> Serializer = null, string FileLocation = "")
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            byte[] Data = Serializer.Check(()=>new BinarySerializer()).Serialize(Object);
            if (!string.IsNullOrEmpty(FileLocation))
                FileLocation.Save(Data);
            return Data;
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <typeparam name="R">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="Serializer">Serializer to use (defaults to JSONSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)></param>
        /// <returns>The deserialized object</returns>
        public static R Deserialize<R>(this string Data, ISerializer<string> Serializer = null, Encoding EncodingUsing = null)
        {
            return string.IsNullOrEmpty(Data) ? default(R) : (R)Data.Deserialize(typeof(R), Serializer, EncodingUsing);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <typeparam name="R">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="Serializer">Serializer to use (defaults to XMLSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)></param>
        /// <returns>The deserialized object</returns>
        
        public static R Deserialize<R>(this XmlDocument Data, ISerializer<string> Serializer = null, Encoding EncodingUsing = null)
        {
            return (Data == null) ? default(R) : (R)Data.InnerXml.Deserialize(typeof(R), Serializer.Check(()=>new XMLSerializer(EncodingUsing)), EncodingUsing);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <typeparam name="R">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="Serializer">Serializer to use (defaults to BinarySerializer)</param>
        /// <returns>The deserialized object</returns>
        public static R Deserialize<R>(this byte[] Data, ISerializer<byte[]> Serializer = null)
        {
            return (Data==null) ? default(R) : (R)Data.Deserialize(typeof(R), Serializer);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <typeparam name="R">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="Serializer">Serializer to use (defaults to JSONSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)></param>
        /// <returns>The deserialized object</returns>
        public static R Deserialize<R>(this FileInfo Data, ISerializer<string> Serializer = null, Encoding EncodingUsing = null)
        {
            return (Data==null || !Data.Exists) ? default(R) : (R)Data.Read().Deserialize(typeof(R), Serializer, EncodingUsing);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="Serializer">Serializer to use (defaults to JSONSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)></param>
        /// <param name="ObjectType">Object type</param>
        /// <returns>The deserialized object</returns>
        public static object Deserialize(this FileInfo Data,Type ObjectType, ISerializer<string> Serializer = null, Encoding EncodingUsing = null)
        {
            return (Data==null || !Data.Exists) ? null : Data.Read().Deserialize(ObjectType, Serializer, EncodingUsing);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Serializer">Serializer to use (defaults to JSONSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)></param>
        /// <returns>The deserialized object</returns>
        public static object Deserialize(this string Data,Type ObjectType, ISerializer<string> Serializer = null, Encoding EncodingUsing = null)
        {
            return Serializer.Check(()=>new JSONSerializer(EncodingUsing)).Deserialize(Data, ObjectType);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Serializer">Serializer to use (defaults to BinarySerializer)</param>
        /// <returns>The deserialized object</returns>
        public static object Deserialize(this byte[] Data, Type ObjectType, ISerializer<byte[]> Serializer = null)
        {
            return Serializer.Check(()=>new BinarySerializer()).Deserialize(Data, ObjectType);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Serializer">Serializer to use (defaults to XMLSerializer)</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)></param>
        /// <returns>The deserialized object</returns>
        
        public static object Deserialize(this XmlDocument Data, Type ObjectType, ISerializer<string> Serializer = null, Encoding EncodingUsing = null)
        {
            return (Data == null) ? null : Data.InnerXml.Deserialize(ObjectType, Serializer.Check(()=>new XMLSerializer(EncodingUsing)), EncodingUsing);
        }

        #endregion

        #region DeserializeBinary

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Serializer">Serializer to use (defaults to BinarySerializer)</param>
        /// <returns>The deserialized object</returns>
        public static object DeserializeBinary(this FileInfo Data, Type ObjectType, ISerializer<byte[]> Serializer = null)
        {
            return (Data==null || !Data.Exists) ? null : Data.ReadBinary().Deserialize(ObjectType, Serializer);
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <typeparam name="R">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="Serializer">Serializer to use (defaults to BinarySerializer)</param>
        /// <returns>The deserialized object</returns>
        public static R DeserializeBinary<R>(this FileInfo Data, ISerializer<byte[]> Serializer = null)
        {
            return (Data==null || !Data.Exists) ? default(R) : (R)Data.ReadBinary().Deserialize(typeof(R), Serializer);
        }

        #endregion

        #endregion
    }
}
