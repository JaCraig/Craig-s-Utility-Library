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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.IO.Serializers.BaseClasses;

namespace Utilities.IO.Serializers.Default
{
    /// <summary>
    /// JSON serializer
    /// </summary>
    public class JSONSerializer : SerializerBase<string>
    {
        /// <summary>
        /// JSONP regex filter
        /// </summary>
        private static Regex JsonPRegex = new Regex(@"[^\(]+\(([^\)]*)\);", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType => "application/json";

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType => ".json";

        /// <summary>
        /// Name
        /// </summary>
        public override string Name => "JSON";

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
            data = JsonPRegex.Replace(data, "$1");
            using (MemoryStream Stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                var Serializer = new DataContractJsonSerializer(objectType);
                return Serializer.ReadObject(Stream);
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
            string ReturnValue = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                var Serializer = new DataContractJsonSerializer(data.GetType());
                Serializer.WriteObject(Stream, data);
                Stream.Flush();
                ReturnValue = Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
                if (HttpContext.Current != null)
                {
                    HttpRequest Request = HttpContext.Current.Request;
                    if (!string.IsNullOrEmpty(Request.QueryString["callback"]) || !string.IsNullOrEmpty(Request.QueryString["jsonp"]))
                    {
                        string Callback = Request.QueryString["callback"] ?? Request.QueryString["jsonp"];
                        ReturnValue = string.Format(CultureInfo.InvariantCulture, "{0}({1});", Callback, ReturnValue);
                    }
                }
            }
            return ReturnValue;
        }
    }
}