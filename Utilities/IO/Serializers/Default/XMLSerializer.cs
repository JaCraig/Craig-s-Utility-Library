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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.IO.Serializers.BaseClasses;
using Utilities.IO.Serializers.Interfaces;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO.Serializers.Default
{
    /// <summary>
    /// XML serializer
    /// </summary>
    public class XMLSerializer : SerializerBase<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XMLSerializer()
            : base()
        {
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType { get { return "text/xml"; } }

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType { get { return ".xml"; } }

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get { return "XML"; } }
        
        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public override string Serialize(Type ObjectType, object Data)
        {
            if (Data == null)
                return null;
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer Serializer = new XmlSerializer(ObjectType);
                Serializer.Serialize(Stream, Data);
                Stream.Flush();
                return Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
        }

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public override object Deserialize(Type ObjectType, string Data)
        {
            if (string.IsNullOrEmpty(Data))
                return null;
            using (MemoryStream Stream = new MemoryStream(Encoding.UTF8.GetBytes(Data)))
            {
                XmlSerializer Serializer = new XmlSerializer(ObjectType);
                return Serializer.Deserialize(Stream);
            }
        }
    }
}