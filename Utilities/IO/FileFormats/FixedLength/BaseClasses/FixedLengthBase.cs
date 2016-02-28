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

using System.Text;
using Utilities.IO.FileFormats.BaseClasses;
using Utilities.IO.FileFormats.FixedLength.Interfaces;

namespace Utilities.IO.FileFormats.FixedLength.BaseClasses
{
    /// <summary>
    /// Parses and creates a fixed length file
    /// </summary>
    /// <typeparam name="ObjectType">Object Type</typeparam>
    /// <typeparam name="FieldType">Field Type</typeparam>
    public abstract class FixedLengthBase<ObjectType, FieldType> : StringListFormatBase<ObjectType, IRecord<FieldType>>
        where ObjectType : FixedLengthBase<ObjectType, FieldType>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected FixedLengthBase()
        {
        }

        /// <summary>
        /// Parses the string into fields
        /// </summary>
        /// <param name="Value">The string value</param>
        /// <param name="Length">Max length for the record</param>
        public abstract void Parse(string Value, int Length = -1);

        /// <summary>
        /// Converts the file to a string
        /// </summary>
        /// <returns>The file as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (IRecord<FieldType> Record in Records)
                Builder.Append(Record.ToString());
            return Builder.ToString();
        }

        /// <summary>
        /// Loads the data into the object
        /// </summary>
        /// <param name="Data">The data to load</param>
        protected override void LoadFromData(string Data)
        {
            Parse(Data);
        }
    }
}