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
using System.Linq;
using System.Text;
using Batman.Core.Bootstrapper.Interfaces;

using Utilities.DataTypes.ExtensionMethods;
using Batman.Core.Logging.BaseClasses;
using Utilities.IO.Logging.Enums;
using Batman.Core.Logging;
using System.IO;
using Batman.Core.Tasks;
using Batman.Core.Tasks.Enums;
using Batman.Core.FileSystem;
using Batman.Core.Communication;
using Batman.Core.Profiling.Interfaces;
using Batman.Core.Serialization.Interfaces;
using System.Web.Mvc;
#endregion

namespace Batman.Core.Serialization
{
    /// <summary>
    /// Manager in charge of serializing data
    /// </summary>
    public class SerializationManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializationManager()
        {
            Serializers = new Dictionary<string, ISerializer>();
            foreach (ISerializer Serializer in AppDomain.CurrentDomain.GetAssemblies().Objects<ISerializer>())
            {
                Serializers.Add(Serializer.ContentType, Serializer);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Serializers
        /// </summary>
        public IDictionary<string, ISerializer> Serializers { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type</param>
        /// <returns>The serialized object as a string</returns>
        public ActionResult Serialize<T>(T Object, string ContentType = "application/json")
        {
            return Serialize(Object, typeof(T), ContentType);
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type</param>
        /// <returns>The serialized object as a string</returns>
        public ActionResult Serialize(object Object, Type ObjectType, string ContentType = "application/json")
        {
            if (!Serializers.ContainsKey(ContentType))
                return null;
            return Serializers[ContentType].Serialize(ObjectType, Object);
        }

        /// <summary>
        /// Outputs information about the serializers the system is using
        /// </summary>
        /// <returns>String version of the object</returns>
        public override string ToString()
        {
            return new StringBuilder().AppendLineFormat(Serializers.ToString(x => x.Value.Name))
                                      .ToString();
        }

        #endregion
    }
}