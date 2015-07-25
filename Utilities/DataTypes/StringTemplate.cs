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
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Acts as a template for a string
    /// </summary>
    [Serializable]
    public class StringTemplate : Dictionary<string, string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Template">Template</param>
        /// <param name="KeyEnd">Ending signifier of a key</param>
        /// <param name="KeyStart">Starting signifier of a key</param>
        public StringTemplate(string Template, string KeyStart = "{", string KeyEnd = "}")
        {
            this.KeyStart = KeyStart;
            this.KeyEnd = KeyEnd;
            this.Template = Template;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Info">Serialization info</param>
        /// <param name="Context">Streaming context</param>
        protected StringTemplate(SerializationInfo Info, StreamingContext Context)
            : base(Info, Context)
        {
        }

        /// <summary>
        /// Ending signifier of a key
        /// </summary>
        public string KeyEnd { get; protected set; }

        /// <summary>
        /// Beginning signifier of a key
        /// </summary>
        public string KeyStart { get; protected set; }

        /// <summary>
        /// Template
        /// </summary>
        public string Template { get; protected set; }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(StringTemplate Value)
        {
            Contract.Requires<ArgumentNullException>(Value != null, "Value");
            return Value.ToString();
        }

        /// <summary>
        /// Implements the ISerializable interface and returns the data needed to serialize the
        /// dictionary instance
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Applies the key/values to the template and returns the resulting string
        /// </summary>
        /// <returns>The resulting string</returns>
        public override string ToString()
        {
            return Template.ToString(this.ToArray(x => new KeyValuePair<string, string>(KeyStart + x.Key + KeyEnd, x.Value)));
        }
    }
}