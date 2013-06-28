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
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.Serializers.Interfaces;
#endregion

namespace Utilities.IO.Serializers
{
    /// <summary>
    /// XML serializer
    /// </summary>
    public class XMLSerializer : ISerializer<string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="EncodingUsing">Encoding that the serializer should use (defaults to ASCII)</param>
        public XMLSerializer(Encoding EncodingUsing = null)
        {
            this.EncodingUsing = EncodingUsing.Check(new ASCIIEncoding());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Encoding that the serializer should use
        /// </summary>
        public virtual Encoding EncodingUsing { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="Object">Object to serialize</param>
        /// <returns>The serialized object</returns>
        public string Serialize(object Object)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer Serializer = new XmlSerializer(Object.GetType());
                Serializer.Serialize(Stream, Object);
                Stream.Flush();
                return EncodingUsing.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
        }

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The resulting object</returns>
        public object Deserialize(string Data, Type ObjectType)
        {
            if (string.IsNullOrEmpty(Data))
                return null;
            using (MemoryStream Stream = new MemoryStream(EncodingUsing.GetBytes(Data)))
            {
                XmlSerializer Serializer = new XmlSerializer(ObjectType);
                return Serializer.Deserialize(Stream);
            }
        }

        #endregion
    }
}