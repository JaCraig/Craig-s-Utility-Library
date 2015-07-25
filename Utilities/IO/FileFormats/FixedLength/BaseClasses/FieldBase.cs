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

using Utilities.IO.FileFormats.FixedLength.Interfaces;

namespace Utilities.IO.FileFormats.FixedLength.BaseClasses
{
    /// <summary>
    /// Field base class
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    public abstract class FieldBase<T> : IField<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected FieldBase()
        {
        }

        /// <summary>
        /// Length of the field
        /// </summary>
        public virtual int Length { get; set; }

        /// <summary>
        /// Value of the field
        /// </summary>
        public virtual T Value { get; set; }

        /// <summary>
        /// Parses the field
        /// </summary>
        /// <param name="Value">Value to place in the field</param>
        /// <param name="Length">Length of the field</param>
        /// <param name="FillerCharacter">Filler character to use</param>
        public abstract void Parse(string Value, int Length = -1, string FillerCharacter = " ");

        /// <summary>
        /// Converts the field to a string
        /// </summary>
        /// <returns>The field as a string</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}