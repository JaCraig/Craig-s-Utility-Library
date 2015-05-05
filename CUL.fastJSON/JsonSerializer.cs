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

using System;
using System.Web;
using Utilities.DataTypes;
using Utilities.IO.Serializers.BaseClasses;

namespace CUL.Serialization
{
    /// <summary>
    /// fastJSON based Json serializer
    /// </summary>
    public class JsonSerializer : SerializerBase<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonSerializer()
            : base()
        {
            fastJSON.JSON.Parameters.EnableAnonymousTypes = true;
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType { get { return "application/json"; } }

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType { get { return ".json"; } }

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get { return "fastJSON"; } }

        /// <summary>
        /// Deserializes the data to the specified object type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The object</returns>
        public override object Deserialize(Type ObjectType, string Data)
        {
            if (string.IsNullOrEmpty(Data) || ObjectType == null)
                return null;
            return fastJSON.JSON.ToObject(Data, ObjectType);
        }

        /// <summary>
        /// Serializes the object to a JSON string
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Object</param>
        /// <returns>The object as a JSON string</returns>
        public override string Serialize(Type ObjectType, object Data)
        {
            if (Data == null || ObjectType == null)
                return "";
            string ReturnValue = fastJSON.JSON.ToJSON(Data);
            HttpRequest Request = HttpContext.Current.Chain(x => x.Request);
            if (Request != null &&
                (!string.IsNullOrEmpty(Request.QueryString["callback"]) ||
                !string.IsNullOrEmpty(Request.QueryString["jsonp"])))
            {
                string Callback = Request.QueryString["callback"] ?? Request.QueryString["jsonp"];
                ReturnValue = string.Format("{0}({1});", Callback, ReturnValue);
            }
            return ReturnValue;
        }
    }
}