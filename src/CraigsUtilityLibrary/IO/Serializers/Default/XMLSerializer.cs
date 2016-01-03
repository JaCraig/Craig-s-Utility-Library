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
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Utilities.IO.Serializers.BaseClasses;
using Utilities.IoC.Interfaces;

namespace Utilities.IO.Serializers.Default
{
    /// <summary>
    /// XML serializer
    /// </summary>
    public class XMLSerializer : SerializerBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XMLSerializer"/> class.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        public XMLSerializer(IBootstrapper bootstrapper)
            : base(bootstrapper)
        {
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType => "text/xml";

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType => ".xml";

        /// <summary>
        /// Name
        /// </summary>
        public override string Name => "XML";

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public override object Deserialize(Type objectType, string data)
        {
            if (string.IsNullOrEmpty(data) || objectType == null)
                return null;
            using (MemoryStream Stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                var Serializer = new XmlSerializer(objectType);
                return Serializer.Deserialize(Stream);
            }
        }

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public override string Serialize(Type objectType, object data)
        {
            if (data == null || objectType == null)
                return null;
            using (MemoryStream Stream = new MemoryStream())
            {
                var Serializer = new XmlSerializer(objectType);
                Serializer.Serialize(Stream, data);
                Stream.Flush();
                return Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
        }
    }
}